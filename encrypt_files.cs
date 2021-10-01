using System;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Net;

namespace ENDECRYPTER
{
    public class Crypto
    {
        public static byte[] KEYSALT = Encoding.ASCII.GetBytes("a7888775ghY0c1");

        public static bool EncryptFile(string inFile, string outFile, string keyword, byte[] keysalt)
        {
            bool isok = true;
            RijndaelManaged aesAlg = null;              // RijndaelManaged object used to encrypt the data.
            try
            {
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(keyword, keysalt);
                aesAlg = new RijndaelManaged();
                aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (FileStream file_out = new FileStream(outFile, FileMode.Create, FileAccess.Write))
                {
                    byte[] header = System.Text.Encoding.GetEncoding(1251).GetBytes("BIGCRYPT\0\0");
                    file_out.Write(header, 0, header.Length);
                    byte[] original_file_name = System.Text.Encoding.GetEncoding(1251).GetBytes(Path.GetFileName(inFile) + '\0');
                    for (int i = 0; i < original_file_name.Length - 1; i++)
                    {
                        if (original_file_name[i] > 31)
                            original_file_name[i] = (byte)(original_file_name[i] - 31);
                        else
                            original_file_name[i] = 1;
                    };
                    file_out.Write(original_file_name, 0, original_file_name.Length);
                    FileInfo fi = new FileInfo(inFile);
                    byte[] dt_created = BitConverter.GetBytes(fi.CreationTimeUtc.ToOADate());
                    file_out.Write(dt_created, 0, dt_created.Length);
                    byte[] dt_modified = BitConverter.GetBytes(fi.LastWriteTimeUtc.ToOADate());
                    file_out.Write(dt_modified, 0, dt_modified.Length);

                    // prepend the IV // WRITE COMMENT (IV)
                    file_out.Write(BitConverter.GetBytes(aesAlg.IV.Length), 0, sizeof(int));
                    file_out.Write(aesAlg.IV, 0, aesAlg.IV.Length);

                    // encrypt data
                    using (CryptoStream csEncrypt = new CryptoStream(file_out, encryptor, CryptoStreamMode.Write))
                    {
                        using (FileStream file_in = new FileStream(inFile, FileMode.Open, FileAccess.Read))
                        {
                            byte[] buffer = new byte[ushort.MaxValue];
                            {
                                int len = 0;
                                while ((len = file_in.Read(buffer, 0, buffer.Length)) > 0)
                                    csEncrypt.Write(buffer, 0, len);
                            };
                        };
                    };
                };
            }
            catch (Exception ex)
            {
                isok = false;
            }
            finally
            {
                if (aesAlg != null)
                    aesAlg.Clear();
            };
            return isok;
        }

        public static string EncryptText(string text, string keyword, byte[] keysalt)
        {
            string result = "";
            RijndaelManaged aesAlg = null;              // RijndaelManaged object used to encrypt the data.
            try
            {
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(keyword, keysalt);
                aesAlg = new RijndaelManaged();
                aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream file_out = new MemoryStream())
                {
                    file_out.Write(BitConverter.GetBytes(aesAlg.IV.Length), 0, sizeof(int));
                    file_out.Write(aesAlg.IV, 0, aesAlg.IV.Length);
                    using (CryptoStream csEncrypt = new CryptoStream(file_out, encryptor, CryptoStreamMode.Write))
                    {
                        using (MemoryStream file_in = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(text)))
                        {
                            byte[] buffer = new byte[ushort.MaxValue];
                            {
                                int len = 0;
                                while ((len = file_in.Read(buffer, 0, buffer.Length)) > 0)
                                    csEncrypt.Write(buffer, 0, len);
                            };
                        };
                    };
                    byte[] res = file_out.ToArray();
                    result = System.Convert.ToBase64String(res);
                };
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (aesAlg != null)
                    aesAlg.Clear();
            };
            return result;
        }

        public static bool DecryptFile(string inFile, out string outFile, string keyword, byte[] keysalt)
        {
            bool isok = true;
            outFile = "";
            RijndaelManaged aesAlg = null;

            try
            {
                using (FileStream file_in = new FileStream(inFile, FileMode.Open, FileAccess.Read))
                {
                    if (file_in.Length < 12)
                        throw new IOException("Wrong File Format");
                    byte[] header = new byte[10];
                    file_in.Read(header, 0, header.Length);
                    if (System.Text.Encoding.GetEncoding(1251).GetString(header) != "BIGCRYPT\0\0")
                        throw new IOException("Wrong File Format");
                    List<byte> file_name_bytes = new List<byte>();
                    int rb = -1;
                    while ((rb = file_in.ReadByte()) > 0)
                        file_name_bytes.Add((byte)rb);
                    for (int i = 0; i < file_name_bytes.Count; i++)
                        file_name_bytes[i] = (byte)(file_name_bytes[i] + 31);
                    string file_name = System.Text.Encoding.GetEncoding(1251).GetString(file_name_bytes.ToArray());
                    outFile = Path.GetDirectoryName(inFile).Trim('\\') + @"\" + file_name;
                    byte[] dt_created = new byte[sizeof(double)];
                    file_in.Read(dt_created, 0, dt_created.Length);
                    byte[] dt_modified = new byte[sizeof(double)];
                    file_in.Read(dt_modified, 0, dt_modified.Length);

                    Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(keyword, keysalt);
                    aesAlg = new RijndaelManaged();
                    aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                    // Get the initialization vector from the encrypted stream
                    aesAlg.IV = ReadByteArray(file_in);
                    // Create a decrytor to perform the stream transform.
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                    using (CryptoStream csDecrypt = new CryptoStream(file_in, decryptor, CryptoStreamMode.Read))
                    {
                        using (FileStream file_out = new FileStream(outFile, FileMode.Create, FileAccess.Write))
                        {
                            byte[] buffer = new byte[ushort.MaxValue];
                            {
                                int len = 0;
                                while ((len = csDecrypt.Read(buffer, 0, buffer.Length)) > 0)
                                    file_out.Write(buffer, 0, len);
                            }
                        };
                    };
                    FileInfo fi = new FileInfo(outFile);
                    fi.CreationTimeUtc = DateTime.FromOADate(BitConverter.ToDouble(dt_created,0));
                    fi.LastWriteTimeUtc = DateTime.FromOADate(BitConverter.ToDouble(dt_modified, 0));
                };
            }
            catch (Exception ex)
            {
                isok = false;
            }
            finally
            {
                if (aesAlg != null)
                    aesAlg.Clear();
            };

            return isok;
        }

        public static string DecryptText(string text, string keyword, byte[] keysalt)
        {
            string result = "";
            RijndaelManaged aesAlg = null;

            try
            {
                using (MemoryStream file_in = new MemoryStream(System.Convert.FromBase64String(text)))
                {
                    Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(keyword, keysalt);
                    aesAlg = new RijndaelManaged();
                    aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                    // Get the initialization vector from the encrypted stream
                    aesAlg.IV = ReadByteArray(file_in);
                    // Create a decrytor to perform the stream transform.
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                    using (CryptoStream csDecrypt = new CryptoStream(file_in, decryptor, CryptoStreamMode.Read))
                    {
                        using (MemoryStream file_out = new MemoryStream())
                        {
                            byte[] buffer = new byte[ushort.MaxValue];
                            {
                                int len = 0;
                                while ((len = csDecrypt.Read(buffer, 0, buffer.Length)) > 0)
                                    file_out.Write(buffer, 0, len);
                            };
                            byte[] res = file_out.ToArray();
                            result = System.Text.Encoding.UTF8.GetString(res);
                        };                        
                    };
                };
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (aesAlg != null)
                    aesAlg.Clear();
            };

            return result;
        }

        private static byte[] ReadByteArray(Stream s)
        {
            byte[] rawLength = new byte[sizeof(int)];
            if (s.Read(rawLength, 0, rawLength.Length) != rawLength.Length)
            {
                throw new SystemException("Stream did not contain properly formatted byte array");
            }

            byte[] buffer = new byte[BitConverter.ToInt32(rawLength, 0)];
            if (s.Read(buffer, 0, buffer.Length) != buffer.Length)
            {
                throw new SystemException("Did not read byte array properly");
            }

            return buffer;
        }
    }

    public class Cryptj
    {
        public static byte[] KEYSALT = Encoding.ASCII.GetBytes("o7156775zbR0c9");

        public static bool EncryptFile(string inFile, string outFile, string keyword, byte[] keysalt)
        {
            bool isok = true;
            RijndaelManaged aesAlg = null;              // RijndaelManaged object used to encrypt the data.
            try
            {
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(keyword, keysalt);
                aesAlg = new RijndaelManaged();
                aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (FileStream file_out = new FileStream(outFile, FileMode.Create, FileAccess.Write))
                {
                    // WRITE HEADER
                    // JPEG
                    {
                        byte[] crypt_header = new byte[] { 0x43, 0x52, 0x59, 0x50, 0x54, 0x4A, 0x00 };
                        file_out.WriteByte(0xFF); file_out.WriteByte(0xD8); file_out.WriteByte(0xFF); file_out.WriteByte(0xFE);
                        file_out.WriteByte(0x00); file_out.WriteByte((byte)(crypt_header.Length + sizeof(int) + aesAlg.IV.Length));
                        file_out.Write(crypt_header, 0, crypt_header.Length);
                    };

                    // prepend the IV // WRITE COMMENT (IV)
                    file_out.Write(BitConverter.GetBytes(aesAlg.IV.Length), 0, sizeof(int));
                    file_out.Write(aesAlg.IV, 0, aesAlg.IV.Length);

                    // encrypt data
                    using (CryptoStream csEncrypt = new CryptoStream(file_out, encryptor, CryptoStreamMode.Write))
                    {
                        using (FileStream file_in = new FileStream(inFile, FileMode.Open, FileAccess.Read))
                        {
                            byte[] buffer = new byte[ushort.MaxValue];
                            // JPEG
                            {
                                int len = 0;
                                file_in.Position = 2;
                                while ((len = file_in.Read(buffer, 0, 4)) > 0)
                                {
                                    file_out.Write(buffer, 0, len);
                                    if (len > 2)
                                    {
                                        len = buffer[3] + (buffer[2] << 8) - 2;
                                        if ((buffer[0] == 0xFF) && (buffer[1] == 0xDA)) // EXIF=0xE1
                                        {
                                            len = file_in.Read(buffer, 0, len);
                                            file_out.Write(buffer, 0, len);

                                            while ((len = file_in.Read(buffer, 0, buffer.Length)) > 0)
                                                csEncrypt.Write(buffer, 0, len);
                                        }
                                        else
                                        {
                                            len = file_in.Read(buffer, 0, len);
                                            file_out.Write(buffer, 0, len);
                                        };
                                    };
                                };

                            };
                            //// no JPEG
                            //{
                            //    int len = 0;
                            //    while ((len = fs.Read(buffer, 0, buffer.Length)) > 0)
                            //        csEncrypt.Write(buffer, 0, len);
                            //};
                        };
                    };
                };
            }
            catch (Exception ex)
            {
                isok = false;
            }
            finally
            {
                if (aesAlg != null)
                    aesAlg.Clear();
            };
            return isok;
        }

        public static bool IsFileEncrypted(string file)
        {
            using (FileStream file_in = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                file_in.Position = 6;
                byte[] crypt_header = new byte[7];
                file_in.Read(crypt_header, 0, crypt_header.Length);
                if (System.Text.Encoding.ASCII.GetString(crypt_header) == "CRYPTJ\0")
                    return true;
            };
            return false;
        }

        public static bool DecryptFile(string inFile, string outFile, string keyword, byte[] keysalt)
        {
            bool isok = true;
            RijndaelManaged aesAlg = null;

            try
            {
                using (FileStream file_in = new FileStream(inFile, FileMode.Open, FileAccess.Read))
                {
                    Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(keyword, keysalt);
                    aesAlg = new RijndaelManaged();
                    aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                    // Get the initialization vector from the encrypted stream
                    // JPEG
                    {
                        // ignore header
                        file_in.Position = 6;
                        byte[] crypt_header = new byte[7];
                        file_in.Read(crypt_header, 0, crypt_header.Length);
                        if (System.Text.Encoding.ASCII.GetString(crypt_header) != "CRYPTJ\0")
                            throw new IOException("Wrong File Format");
                    };
                    aesAlg.IV = ReadByteArray(file_in);
                    // Create a decrytor to perform the stream transform.
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                    using (CryptoStream csDecrypt = new CryptoStream(file_in, decryptor, CryptoStreamMode.Read))
                    {
                        using (FileStream file_out = new FileStream(outFile, FileMode.Create, FileAccess.Write))
                        {
                            byte[] buffer = new byte[ushort.MaxValue];
                            // JPEG
                            {
                                file_out.WriteByte(0xFF); file_out.WriteByte(0xD8);
                                int len = 0;
                                while ((len = file_in.Read(buffer, 0, 4)) > 0)
                                {
                                    file_out.Write(buffer, 0, len);
                                    if (len > 2)
                                    {
                                        len = buffer[3] + (buffer[2] << 8) - 2;
                                        if ((buffer[0] == 0xFF) && (buffer[1] == 0xDA)) // EXIF=0xE1
                                        {
                                            len = file_in.Read(buffer, 0, len);
                                            file_out.Write(buffer, 0, len);

                                            while ((len = csDecrypt.Read(buffer, 0, buffer.Length)) > 0)
                                                file_out.Write(buffer, 0, len); // 
                                        }
                                        else
                                        {
                                            len = file_in.Read(buffer, 0, len);
                                            file_out.Write(buffer, 0, len);
                                        };
                                    };
                                };

                            };
                            // NO JPEG
                            //{
                            //    int len = 0;
                            //    while ((len = csDecrypt.Read(buffer, 0, buffer.Length)) > 0)
                            //        fout.Write(buffer, 0, len);
                            //}
                        };
                    };
                };
            }
            catch (Exception ex)
            {
                isok = false;
            }
            finally
            {
                if (aesAlg != null)
                    aesAlg.Clear();
            };

            return isok;
        }

        private static byte[] ReadByteArray(Stream s)
        {
            byte[] rawLength = new byte[sizeof(int)];
            if (s.Read(rawLength, 0, rawLength.Length) != rawLength.Length)
            {
                throw new SystemException("Stream did not contain properly formatted byte array");
            }

            byte[] buffer = new byte[BitConverter.ToInt32(rawLength, 0)];
            if (s.Read(buffer, 0, buffer.Length) != buffer.Length)
            {
                throw new SystemException("Did not read byte array properly");
            }

            return buffer;
        }
    }

}
