using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CryptoJPEG
{
    public partial class MainJPEGForm : Form
    {
        public MainJPEGForm()
        {
            InitializeComponent();
            wbf = new WaitingBoxForm("Processing", "", this);
            this.AllowDrop = true;
            this.DragEnter += new DragEventHandler(Form1_DragEnter);
            this.DragDrop += new DragEventHandler(Form1_DragDrop);
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.None;

            if (tabControl1.SelectedIndex == 0)
                e.Effect = DragDropEffects.Copy;
            else if (tabControl1.SelectedIndex == 1)
                e.Effect = DragDropEffects.Copy;
            else if (tabControl1.SelectedIndex == 4)
                e.Effect = DragDropEffects.Copy;                                
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files == null) return;
            if (files.Length == 0) return;

            if (tabControl1.SelectedIndex == 0)
                DropFiles(files);
            else if (tabControl1.SelectedIndex == 1)
            {
                if (File.Exists(files[0]))
                    if (Path.GetExtension(files[0]).ToLower() == ".exif")
                        ReadRWExif(files[0]);
            }
            else if (tabControl1.SelectedIndex == 4)
                LoadExifFromFile(files[0]);
            return;            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Text += " v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            ReadDat();
        }

        private WaitingBoxForm wbf;

        public List<string> PARAM_OPERATIONS_LIST
        {
            get
            {
                return (new List<string>(new string[] { "NO", "Encrypt Small File to JPEG", "Decrypt Small File from JPEG", "Encrypt to ENX", "Decrypt from ENX", "JPEG EXIF Rewrite", "JPEG Enrypt Image", "JPEG Decrypt Image", "Batch Work", "Rename File" }));
            }
        }

        public string PARAM_FILE_FILTER
        {
            get
            {
                return listPARAMS.Items[0].SubItems[3].Text.Trim();
            }
            set
            {
                listPARAMS.Items[0].SubItems[3].Text = value;
            }
        }

        public string PARAM_DEFAULT_OPERATION
        {
            get
            {
                return listPARAMS.Items[1].SubItems[3].Text.Trim();
            }
            set
            {
                listPARAMS.Items[1].SubItems[3].Text = value;
            }
        }

        public string PARAM_JPEGFILE
        {
            get
            {
                return listPARAMS.Items[2].SubItems[3].Text.Trim();
            }
            set
            {
                listPARAMS.Items[2].SubItems[3].Text = value;
            }
        }

        public string PARAM_KEYWORD
        {
            get
            {
                return listPARAMS.Items[3].SubItems[3].Text.Trim();
            }
            set
            {
                listPARAMS.Items[3].SubItems[3].Text = value;
            }
        }

        public string PARAM_SUBFOLDERS
        {
            get
            {
                return listPARAMS.Items[4].SubItems[3].Text.Trim();
            }
            set
            {
                listPARAMS.Items[4].SubItems[3].Text = value;
            }
        }

        public string PARAM_JPEGFILE_FULL
        {
            get
            {
                string jpeg = PARAM_JPEGFILE;
                if (File.Exists(CD + @"\" + jpeg))
                    jpeg = CD + @"\" + jpeg;
                if (!File.Exists(jpeg))
                    jpeg = CD + @"\" + jpeg;
                if (!File.Exists(jpeg))
                    return "";
                return jpeg;
            }
        }

        public string GetFullPathForFileName(string fileName)
        {
            string file = fileName;
            if (File.Exists(CD + @"\" + file))
                file = CD + @"\" + file;
            if (!File.Exists(file))
                file = CD + @"\" + file;
            if (!File.Exists(file))
                return "";
            return file;
        }

        public static string CD
        {
            get
            {

                string fname = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.ToString();
                fname = fname.Replace("file:///", "");
                fname = fname.Replace("/", @"\");
                fname = fname.Substring(0, fname.LastIndexOf(@"\") + 1);
                return fname;
            }
        }

        private string WriteDataToJPEG(string fileName, string jpeg_file)
        {
            FileInfo fi = new FileInfo(fileName);
            Dictionary<string, object> exif_data = new Dictionary<string, object>();

            // Supported tags: DateTime, DocumentName, ImageDescription, Make, Model, Software, Artist, Copyright, 
            //                 MakerNote, UserComment, DateTimeOriginal, DateTimeDigitized, ImageUniqueID     

            CryptoEXIF.ExifInfo exi = new CryptoEXIF.ExifInfo();
            try
            {
                exi = CryptoEXIF.ExifInfo.ParseExifFile(fileName);
            }
            catch { };


            for (int i = 0; i < exRW.Items.Count; i++)
            {
                string txt = exRW.Items[i].SubItems[3].Text.Trim();
                if (txt.Length == 0) continue;
                if (txt == "{LastFileWriteTime}")
                    txt = fi.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");
                if (txt == "{FileName}")
                    txt = fi.Name;
                if (txt.StartsWith("{"))
                {
                    txt = exi[txt.Replace("{","").Replace("}",""),""];
                    if (String.IsNullOrEmpty(txt)) continue;
                };
                exif_data.Add(exRW.Items[i].SubItems[1].Text, txt);
            };

            //exif_data.Add("DateTime", fi.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss"));
            //exif_data.Add("DateTimeOriginal", fi.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss"));
            //exif_data.Add("DateTimeDigitized", fi.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss"));

            return CryptoJPEG.CryptoEXIF.WriteDataToJPEG(fileName, jpeg_file, exif_data);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = false;
            WriteDat();
        }

        public static string TimeLength(string text, int len)
        {
            text = text.Trim();
            if (text.Length <= len)
                return text;
            else
                return text.Substring(0, len);
        }

        private void WriteDat()
        {
            FileStream fs = new FileStream(CD + @"\JPEGUtils.dat", FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine("[PARAMETERS]");
            for (int i = 0; i < listPARAMS.Items.Count; i++)
            {
                string txt = listPARAMS.Items[i].SubItems[3].Text.Trim();
                if (txt.Length == 0) continue;
                if(listPARAMS.Items[i].SubItems[0].Text == "KEYWORD")
                    sw.WriteLine("PARAM:" + listPARAMS.Items[i].SubItems[0].Text + ":" + Base64CREncode(txt));
                else
                    sw.WriteLine("PARAM:" + listPARAMS.Items[i].SubItems[0].Text + ":" + txt);
            };
            sw.WriteLine("[REWRITE_EXIF]");
            // RW
            for (int i = 0; i < exRW.Items.Count; i++)
            {
                string txt = exRW.Items[i].SubItems[3].Text.Trim();
                if (txt.Length == 0) continue;
                sw.WriteLine("RW:" + exRW.Items[i].SubItems[1].Text + ":" + txt);
            };
            sw.WriteLine("[BATCH]");
            // BX
            for (int i = 0; i < todobatch.Items.Count; i++)
            {
                string txt = todobatch.Items[i].SubItems[3].Text.Trim();
                if (txt.Length == 0) continue;
                sw.WriteLine("BX:" + todobatch.Items[i].SubItems[0].Text + ":" + txt);
            };
            sw.Close();
            fs.Close();
        }

        private void WriteRWExif(string file)
        {
            FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine("[REWRITE_EXIF]");
            for (int i = 0; i < exRW.Items.Count; i++)
            {
                string txt = exRW.Items[i].SubItems[3].Text.Trim();
                if (txt.Length == 0) continue;
                sw.WriteLine("RW:" + exRW.Items[i].SubItems[1].Text + ":" + txt);
            };
            sw.Close();
            fs.Close();
        }

        private void ReadDat()
        {
            if (!File.Exists(CD + @"\JPEGUtils.dat")) return;
            FileStream fs = new FileStream(CD + @"\JPEGUtils.dat", FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine().Trim();
                if (line.Length <= 3) continue;
                if(line.IndexOf(":") < 0) continue;
                string[] kv = line.Split(new char[] { ':' }, 3);
                if (kv[0] == "PARAM")
                    for (int i = 0; i < listPARAMS.Items.Count; i++)
                    {
                        if (listPARAMS.Items[i].SubItems[0].Text == kv[1])
                        {
                            if (kv[1] == "KEYWORD")
                                listPARAMS.Items[i].SubItems[3].Text = Base64CRDecode(kv[2].Trim());
                            else
                                listPARAMS.Items[i].SubItems[3].Text = kv[2].Trim();
                        };
                    };
                if(kv[0] == "RW")
                    for (int i = 0; i < exRW.Items.Count; i++)
                    {
                        if (exRW.Items[i].SubItems[1].Text == kv[1])
                            exRW.Items[i].SubItems[3].Text = kv[2].Trim();
                    };
                if(kv[0] == "BX")
                    for (int i = 0; i < todobatch.Items.Count; i++)
                    {
                        if (todobatch.Items[i].SubItems[0].Text == kv[1])
                            todobatch.Items[i].SubItems[3].Text = kv[2].Trim();
                    };
            };            
            sr.Close();
            fs.Close();
        }

        private void ReadRWExif(string file)
        {
            FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine().Trim();
                if (line.Length <= 3) continue;
                if (line.IndexOf(":") < 0) continue;
                string[] kv = line.Split(new char[] { ':' }, 3);
                if (kv[0] == "RW")
                    for (int i = 0; i < exRW.Items.Count; i++)
                    {
                        if (exRW.Items[i].SubItems[1].Text == kv[1])
                            exRW.Items[i].SubItems[3].Text = kv[2].Trim();
                    };                
            };
            sr.Close();
            fs.Close();
        }

        private bool RewriteFileExif(string fileName)
        {
            FileInfo fi = new FileInfo(fileName);
            Dictionary<string, object> exif_data = new Dictionary<string, object>();

            // Supported tags: DateTime, DocumentName, ImageDescription, Make, Model, Software, Artist, Copyright, 
            //                 MakerNote, UserComment, DateTimeOriginal, DateTimeDigitized, ImageUniqueID     

            CryptoEXIF.ExifInfo exi = new CryptoEXIF.ExifInfo();
            try
            {
                exi = CryptoEXIF.ExifInfo.ParseExifFile(fileName);
            }
            catch { };


            for (int i = 0; i < exRW.Items.Count; i++)
            {
                string txt = exRW.Items[i].SubItems[3].Text.Trim();
                if (txt.Length == 0) continue;
                if (txt.StartsWith("{"))
                {
                    txt = exi[txt.Replace("{", "").Replace("}", ""), ""];
                    if (String.IsNullOrEmpty(txt)) continue;
                };
                exif_data.Add(exRW.Items[i].SubItems[1].Text, txt);
            };

            string tmp_file = Path.GetDirectoryName(fileName) + @"\__tmp__.tmp";
            bool isok = false;
            try
            {
                CryptoEXIF.ExifInfo.ExifRewrite(fileName, tmp_file, exif_data);
                File.Delete(fileName);
                File.Move(tmp_file, fileName);
                isok = true;
            }
            catch 
            { 
                if(File.Exists(tmp_file))
                    File.Delete(tmp_file);
            };
            return isok;
        }

        private void LoadExifFromFile(string fileName)
        {
            exif.Items.Clear();
            ListViewItem lvi = new ListViewItem("[[FILE]]");
            lvi.SubItems.Add(Path.GetFileName(fileName));
            lvi.SubItems.Add(fileName);
            exif.Items.Add(lvi);

            CryptoEXIF.ExifInfo exi = CryptoEXIF.ExifInfo.ParseExifFile(fileName);
            if ((exi != null) && (exi.Count > 0))
                for (int i = 0; i < exi.Count; i++)
                {
                    lvi = new ListViewItem("0x" + exi.Entries[i].entry_tag_number.ToString("X4"));
                    lvi.SubItems.Add(exi.Entries[i].entry_tag_name);
                    if (exi.Entries[i].entry_value != null)
                        lvi.SubItems.Add(exi.Entries[i].entry_text);
                    exif.Items.Add(lvi);
                };
        }

        private void exif_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (exif.SelectedItems.Count != 1) return;
            string txt = exif.SelectedItems[0].SubItems[2].Text;
            InputBox.Show("Tag value", exif.SelectedItems[0].SubItems[0].Text + " " + exif.SelectedItems[0].SubItems[1].Text + ":", txt);
        }

        private void exRW_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (exRW.SelectedItems.Count != 1) return;

            string txt = exRW.SelectedItems[0].SubItems[3].Text;

            if (exRW.SelectedItems[0].SubItems[2].Text.StartsWith("{"))
            {
                List<string> vals = new List<string>();
                vals.Add(exRW.SelectedItems[0].SubItems[2].Text);
                if (exRW.SelectedItems[0].SubItems[2].Text == "{Make}")
                    vals.Add("CORALHO");
                if (exRW.SelectedItems[0].SubItems[2].Text == "{Model}")
                    vals.Add("Y"+DateTime.Now.ToString("yy-M"));
                if (exRW.SelectedItems[0].SubItems[2].Text.StartsWith("{Date"))
                    vals.Add("{LastFileWriteTime}");
                if (vals.IndexOf(txt) < 0) vals.Add(txt);                
                if (InputBox.Show("Tag value", exRW.SelectedItems[0].SubItems[0].Text + " " + exRW.SelectedItems[0].SubItems[1].Text + ":", vals.ToArray(), ref txt, true) == DialogResult.OK)
                    exRW.SelectedItems[0].SubItems[3].Text = TimeLength(txt, 200);
                return;
            };

            if (txt == "") txt = exRW.SelectedItems[0].SubItems[2].Text;
            if (InputBox.Show("Tag value", exRW.SelectedItems[0].SubItems[0].Text + " " + exRW.SelectedItems[0].SubItems[1].Text + ":", ref txt) == DialogResult.OK)
                exRW.SelectedItems[0].SubItems[3].Text = TimeLength(txt, 200);
        }

        private void todobatch_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (todobatch.SelectedItems.Count != 1) return;
            
            if (todobatch.SelectedItems[0].SubItems[2].Text == "Yes or No (True or False)") // 2, 4, 5, 7
            {
                bool da = false;
                string text = todobatch.SelectedItems[0].SubItems[3].Text.Trim().ToLower();
                if ((text == "yes") || (text == "true"))
                    da = true;
                da = !da;
                todobatch.SelectedItems[0].SubItems[3].Text = da ? "Yes" : "No";
                return;
            };
            if (todobatch.SelectedIndices[0] == 0) // 0
            {
                int val = int.Parse(todobatch.SelectedItems[0].SubItems[3].Text);
                if (InputBox.Show("Change Parameter", todobatch.SelectedItems[0].SubItems[1].Text + ":", ref val, 0, 99999999) == DialogResult.OK)
                    todobatch.SelectedItems[0].SubItems[3].Text = val.ToString();
                return;

            };
            // 
            if (todobatch.SelectedIndices[0] == 3) // 3
            {
                string si = todobatch.SelectedItems[0].SubItems[3].Text;
                List<string> vals = new List<string>();                
                vals.Add("{FileNumber:0000}");
                vals.Add("{FileIndex:0000}");
                vals.Add("{FileName}");
                vals.Add("{FileModifiedDateTime:yyyyMMddHHmmss}");
                vals.Add("{FileCreatedDateTime:yyyyMMddHHmmss}");
                vals.Add("{FileNumber:0000} {Make} {FileModifiedDateTime:yyMMdd}");
                if (vals.IndexOf(si) < 0) vals.Add(si);
                if (InputBox.Show("Change Parameter", todobatch.SelectedItems[0].SubItems[1].Text + ":", vals.ToArray(), ref si, true) == DialogResult.OK)
                    todobatch.SelectedItems[0].SubItems[3].Text = si;
                return;

            };
            if (todobatch.SelectedIndices[0] == 5) // 5
            {
                string si = todobatch.SelectedItems[0].SubItems[3].Text;
                string[] sis = todobatch.SelectedItems[0].SubItems[2].Text.Split(new string[] { @"/" }, StringSplitOptions.None);
                if (InputBox.Show("Change Parameter", todobatch.SelectedItems[0].SubItems[1].Text + ":", sis, ref si) == DialogResult.OK)
                    todobatch.SelectedItems[0].SubItems[3].Text = si;
                return;
            };
            if ((todobatch.SelectedIndices[0] == 6) || (todobatch.SelectedIndices[0] == 7)) // 6, 7
            {
                int val = int.Parse(todobatch.SelectedItems[0].SubItems[3].Text);
                if (InputBox.Show("Change Parameter", todobatch.SelectedItems[0].SubItems[1].Text + ":", ref val, 8, 9999) == DialogResult.OK)
                    todobatch.SelectedItems[0].SubItems[3].Text = val.ToString();
                return;

            };
            if (todobatch.SelectedIndices[0] == 8) // 8
            {
                int val = int.Parse(todobatch.SelectedItems[0].SubItems[3].Text);
                if (InputBox.Show("Change Parameter", todobatch.SelectedItems[0].SubItems[1].Text + ":", ref val, 10, 100) == DialogResult.OK)
                    todobatch.SelectedItems[0].SubItems[3].Text = val.ToString();
                return;

            };
            if (todobatch.SelectedIndices[0] == 9) // 9
            {
                string si = todobatch.SelectedItems[0].SubItems[3].Text;
                string[] sis = todobatch.SelectedItems[0].SubItems[2].Text.Split(new string[] { @"/" }, StringSplitOptions.None);
                DialogResult dr = InputBox.Show("Change Parameter", todobatch.SelectedItems[0].SubItems[1].Text + ":", sis, ref si);
                if (dr == DialogResult.OK)
                {
                    todobatch.SelectedItems[0].SubItems[3].Text = si;
                    if (si == "Source") todobatch.Items[10].SubItems[3].Text = "";
                    if (si == "Subfolder") todobatch.Items[10].SubItems[3].Text = "Processed";
                    if (si == "Path")
                    {
                        todobatch.Items[10].SubItems[3].Text = "Processed";
                        todobatch.Items[10].Selected = true;
                        todobatch_MouseDoubleClick(sender, e);
                    };
                };
                return;
            };
            if (todobatch.SelectedIndices[0] == 10) // 10
            {
                if (todobatch.Items[9].SubItems[3].Text == "Path")
                {
                    FolderBrowserDialog fbd = new FolderBrowserDialog();
                    if (Directory.Exists(todobatch.SelectedItems[0].SubItems[3].Text))
                        fbd.SelectedPath = todobatch.SelectedItems[0].SubItems[3].Text;
                    if (fbd.ShowDialog() == DialogResult.OK)
                        todobatch.SelectedItems[0].SubItems[3].Text = fbd.SelectedPath.Trim('\\') + @"\";
                    fbd.Dispose();
                    return;

                }
                else if (todobatch.Items[9].SubItems[3].Text == "Source")
                    return;
            };
            if (todobatch.SelectedIndices[0] == 11) // 11
            {
                string si = todobatch.SelectedItems[0].SubItems[3].Text;
                string[] sis = new string[] { "No", "Lower Extention", "Lower Name", "Lower Full Name", "Upper Extention", "Upper Name", "Upper Full Name" };
                if (InputBox.Show("Change Parameter", todobatch.SelectedItems[0].SubItems[1].Text + ":", sis, ref si) == DialogResult.OK)
                    todobatch.SelectedItems[0].SubItems[3].Text = si;
                return;
            };
            if (todobatch.SelectedIndices[0] == 14) // 14
            {
                int val = int.Parse(todobatch.SelectedItems[0].SubItems[3].Text);
                if (InputBox.Show("Change Parameter", "Text Watermark Opacity:", ref val, 10, 100) == DialogResult.OK)
                    todobatch.SelectedItems[0].SubItems[3].Text = val.ToString();
                return;

            };
            if (todobatch.SelectedIndices[0] == 15) // 15
            {
                string si = todobatch.SelectedItems[0].SubItems[3].Text;
                string[] sis = new string[] { "0 - Center", "1 - Top Middle", "2 - Top Right", "3 - Right Middle", "4 - Bottom Right", "5 - Bottom Middle", "6 - Bottom Left", "7 - Left Middle", "8 - Top Left" };
                if (InputBox.Show("Change Parameter", "Text Watermark Position:", sis, ref si) == DialogResult.OK)
                    todobatch.SelectedItems[0].SubItems[3].Text = si;
                return;
            };
            if (todobatch.SelectedIndices[0] == 16) // 16
            {                
                string[] fonts = new string[] { "Arial" };
                using (System.Drawing.Text.InstalledFontCollection col = new System.Drawing.Text.InstalledFontCollection())
                {
                    fonts = new string[col.Families.Length];
                    for (int i = 0; i < col.Families.Length; i++)
                        fonts[i] = col.Families[i].Name;
                };
                string si = todobatch.SelectedItems[0].SubItems[3].Text;
                if (InputBox.Show("Change Parameter", "Text Watermark Font:", fonts, ref si) == DialogResult.OK)
                    todobatch.SelectedItems[0].SubItems[3].Text = si;
                return;
            };
            if (todobatch.SelectedIndices[0] == 17) // 17
            {
                string si = todobatch.SelectedItems[0].SubItems[3].Text;
                string msk = @"R^((\s+)|(\d{1,2}([%]{0,1}|(px)|(p))))$";
                if (InputBox.Show("Change Parameter", "Text Watermark Font Size:", ref si, msk) == DialogResult.OK)
                {
                    if (si.EndsWith("p")) si += "x";
                    if ((!si.EndsWith("px")) && (!si.EndsWith("%")))
                        si += "%";
                    todobatch.SelectedItems[0].SubItems[3].Text = si;
                };
                return;
            };
            if (todobatch.SelectedIndices[0] == 18) // 18
            {
                string si = todobatch.SelectedItems[0].SubItems[3].Text;
                Color c = RGBConverter(si);
                if (InputBox.QueryColorBox("Change Parameter", "Text Watermark Font Color:", ref c) == DialogResult.OK)
                    todobatch.SelectedItems[0].SubItems[3].Text = HexConverter(c);
                return;
            };
            if (todobatch.SelectedIndices[0] == 20) // 20
            {
                string si = todobatch.SelectedItems[0].SubItems[3].Text;
                if (InputBox.QueryFileBox("Change Parameter", "Watermark Image File:", ref si, "JPEG Files (*.jpg;*.jpeg)|*.jpg;*.jpeg") == DialogResult.OK)
                    todobatch.SelectedItems[0].SubItems[3].Text = si;
                return;
            };
            if (todobatch.SelectedIndices[0] == 21) // 21
            {
                int val = int.Parse(todobatch.SelectedItems[0].SubItems[3].Text);
                if (InputBox.Show("Change Parameter", "Watermark Image Opacity:", ref val, 10, 100) == DialogResult.OK)
                    todobatch.SelectedItems[0].SubItems[3].Text = val.ToString();
                return;
            };
            if (todobatch.SelectedIndices[0] == 22) // 22
            {
                string si = todobatch.SelectedItems[0].SubItems[3].Text;
                string[] sis = new string[] { "0 - Center", "1 - Top Middle", "2 - Top Right", "3 - Right Middle", "4 - Bottom Right", "5 - Bottom Middle", "6 - Bottom Left", "7 - Left Middle", "8 - Top Left" };
                if (InputBox.Show("Change Parameter", "Watermark Image Position:", sis, ref si) == DialogResult.OK)
                    todobatch.SelectedItems[0].SubItems[3].Text = si;
                return;
            };
            //

            string txt = todobatch.SelectedItems[0].SubItems[3].Text;
            if (txt == "") txt = todobatch.SelectedItems[0].SubItems[2].Text;
            if (InputBox.Show("Change Parameter", todobatch.SelectedItems[0].SubItems[1].Text + ":", ref txt) == DialogResult.OK)
                todobatch.SelectedItems[0].SubItems[3].Text = txt.Trim();
        }

        private static string[] pattern_files(string[] file_list, string file_pattern)
        {
            List<string> files = new List<string>();
            if ((file_list != null) && (file_list.Length > 0))
            {
                if ((file_pattern == "*.*") || (file_pattern == "*") || (file_pattern == ".*"))
                    files.AddRange(file_list);
                else
                {
                    Regex rg = new Regex(file_pattern, RegexOptions.IgnoreCase);
                    foreach (string file in file_list) if (rg.IsMatch(file)) files.Add(file);
                };
            };
            return files.ToArray();
        }                        

        private void todobatch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\r') return;
            todobatch_MouseDoubleClick(sender, null);
        }

        private void exRW_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\r') return;
            exRW_MouseDoubleClick(sender, null);
        }

        private bool ProceedENJ(string file, bool crypt)
        {
            FileInfo fi = new FileInfo(file);

            string tmp_file = Path.GetDirectoryName(file) + @"\__tmp__.tmp";
            bool isok = false;
            try
            {
                if (crypt)
                {
                    isok = ENDECRYPTER.Cryptj.EncryptFile(file, tmp_file, PARAM_KEYWORD, ENDECRYPTER.Cryptj.KEYSALT);
                }
                else
                {
                    isok = ENDECRYPTER.Cryptj.DecryptFile(file, tmp_file, PARAM_KEYWORD, ENDECRYPTER.Cryptj.KEYSALT);
                };
                if (isok)
                {
                    File.Delete(file);
                    File.Move(tmp_file, file);
                };
            }
            catch
            {
                if (File.Exists(tmp_file))
                    File.Delete(tmp_file);
            };
            return isok;
        }
       
        private static String HexConverter(Color c)
        {
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }

        private static Color RGBConverter(string hex)
        {
            Color rtn = Color.Black;
            try
            {
                return Color.FromArgb(
                    int.Parse(hex.Substring(1, 2),System.Globalization.NumberStyles.HexNumber),
                    int.Parse(hex.Substring(3, 2), System.Globalization.NumberStyles.HexNumber),
                    int.Parse(hex.Substring(5, 2), System.Globalization.NumberStyles.HexNumber));
            }
            catch (Exception ex)
            {
                //doing nothing
            }

            return rtn;
        }

        private static Bitmap ChangeOpacity(Image img, int opacityvalue)
        {
            Bitmap bmp = new Bitmap(img.Width, img.Height); // Determining Width and Height of Source Image
            Graphics graphics = Graphics.FromImage(bmp);
            System.Drawing.Imaging.ColorMatrix colormatrix = new System.Drawing.Imaging.ColorMatrix();
            colormatrix.Matrix33 = (float)(((float)opacityvalue) / 100.0);
            System.Drawing.Imaging.ImageAttributes imgAttribute = new System.Drawing.Imaging.ImageAttributes();
            imgAttribute.SetColorMatrix(colormatrix, System.Drawing.Imaging.ColorMatrixFlag.Default, System.Drawing.Imaging.ColorAdjustType.Bitmap);
            graphics.DrawImage(img, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, imgAttribute);
            graphics.Dispose();   // Releasing all resource used by graphics 
            return bmp;
        }

        public static string Base64CREncode(string plainText) 
        {
            if (String.IsNullOrEmpty(plainText)) return "";
            byte[] plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);            
            for(int i=0;i<plainTextBytes.Length;i++)
                plainTextBytes[i] = (byte)(plainTextBytes[i] + i);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64CRDecode(string base64EncodedData) 
        {
            if (String.IsNullOrEmpty(base64EncodedData)) return "";
            byte[] base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            for (int i = 0; i < base64EncodedBytes.Length; i++)
                base64EncodedBytes[i] = (byte)(base64EncodedBytes[i] - i);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        private void ctnt_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != ctnt) return;
            ctct.Text = ENDECRYPTER.Crypto.EncryptText(ctnt.Text.Trim(), PARAM_KEYWORD, ENDECRYPTER.Crypto.KEYSALT); 
        }

        private void ctct_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != ctct) return;
            ctnt.Text = ENDECRYPTER.Crypto.DecryptText(ctct.Text.Trim(), PARAM_KEYWORD, ENDECRYPTER.Crypto.KEYSALT); 
        }

        private void listPARAMS_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\r') return;
            listPARAMS_MouseDoubleClick(sender, null);
        }

        private void listPARAMS_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listPARAMS.SelectedIndices.Count != 1) return;

            if (listPARAMS.SelectedIndices[0] == 0)
            {
                string val = PARAM_FILE_FILTER;
                List<string> list = new List<string>();
                list.AddRange(new string[] { "*.*", "(.jpg$)|(.jpeg$)", "(.enx$)", "(.jpg$)|(.jpeg$)|(.enx$)", "(.jpg$)|(.jpeg$)|(.enx$)|(.mp4$)" });
                if(list.IndexOf(PARAM_FILE_FILTER) < 0) list.Insert(0, PARAM_FILE_FILTER);
                if (InputBox.Show("Change Parameter", "Default file filter to add files to the list:", list.ToArray(), ref val, true) == DialogResult.OK)
                    PARAM_FILE_FILTER = val;
            };
            if (listPARAMS.SelectedIndices[0] == 1)
            {
                string val = PARAM_DEFAULT_OPERATION;
                if (InputBox.Show("Change Parameter", "Default operation for new file in list:", PARAM_OPERATIONS_LIST.ToArray(), ref val, false) == DialogResult.OK)
                    PARAM_DEFAULT_OPERATION = val;
            };
            if (listPARAMS.SelectedIndices[0] == 2)
            {
                string val = PARAM_JPEGFILE;
                if (InputBox.QueryFileBox("Change Parameter", "JPEG Image file to use as template:", ref val, "JPEG Files (*.jpg;*.jpeg)|(*.jpg;*.jpeg;*.jpgx)") == DialogResult.OK)
                    PARAM_JPEGFILE = val;
            };
            if (listPARAMS.SelectedIndices[0] == 3)
            {
                string val = PARAM_KEYWORD;
                if (InputBox.Show("Change Parameter", "Keyword to use in crypt operations:", ref val) == DialogResult.OK)
                    PARAM_KEYWORD = val;
            };
            if (listPARAMS.SelectedIndices[0] == 4)
            {
                PARAM_SUBFOLDERS = PARAM_SUBFOLDERS == "No" ? "Yes" : "No";
            };
        }

        private void clearFileListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listFile.Items.Clear();
            ST_TTF.Text = "0";
        }

        private void addDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string dir = @"C:\My Documents\";
            if (InputBox.QueryDirectoryBox("Add Directory", "Path to Directory:", ref dir) == DialogResult.OK)
                DropFiles(new string[] { dir });
        }

        private void DropFiles(string[] fandd)
        {
            if (fandd == null) return;
            if (fandd.Length == 0) return;

            List<string> dd = new List<string>();
            List<string> ff = new List<string>();
            foreach (string fd in fandd)
            {
                if (File.Exists(fd))
                    ff.Add(fd);
                else if (Directory.Exists(fd))
                    dd.Add(fd);
            };
            if ((ff.Count == 0) && (dd.Count == 0)) return;
            wbf.Cancelable = true;
            wbf.CancelText = "Stop";
            wbf.Show("Processing", "Wait... Listing Files and Directories...", -1);
            try
            {
                DropFiles(ff.ToArray(), dd.ToArray(), listFile.Items.Count);
                wbf.Hide();
            }
            catch { }
        }

        public static string FormatFileSize(long size)
        {
            if (size < 1024) return String.Format("{0} B", size);
            if (size < 1024 * 1024) return String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0.0} KB", (float)size / 1024.0);
            if (size < 1024 * 1024 * 1024) return String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0.00} MB", (float)size / 1024.0 / 1024.0);
            return String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0.00} GB", (float)size / 1024.0 / 1024.0 / 1024.0);
        }

        private void DropFiles(string[] files, string[] dirs, int wasCount)
        {            
            List<string> dd = new List<string>();
            if ((dirs != null) && (dirs.Length > 0))
                foreach (string dir in dirs)
                    DropFiles(Directory.GetFiles(dir, "*.*", System.IO.SearchOption.TopDirectoryOnly), (PARAM_SUBFOLDERS == "Yes") ? Directory.GetDirectories(dir, "*.*", System.IO.SearchOption.TopDirectoryOnly) : null, wasCount);

            List<string> ff = new List<string>();
            if ((files != null) && (files.Length > 0))
                foreach (string file in files)
                {
                    if (File.Exists(file))
                        ff.Add(file);
                };


            ff = new List<string>(pattern_files(files, PARAM_FILE_FILTER));

            if (ff.Count == 0) return;
            
            foreach (string file in ff)
            {                
                FileInfo fi = new FileInfo(file);

                bool skip = false;

                if (listFile.Items.Count > 0)
                    for (int x = 0; x < listFile.Items.Count; x++)
                        if (listFile.Items[x].SubItems[9].Text == fi.FullName)
                        {
                            skip = true;
                            break;
                        };

                if (!skip)
                {
                    ListViewItem lvi = new ListViewItem((listFile.Items.Count + 1).ToString("00000")); //0
                    lvi.SubItems.Add(fi.Directory.ToString());//1
                    lvi.SubItems.Add(fi.Name);//2
                    lvi.SubItems.Add(FormatFileSize(fi.Length));//3
                    string op = PARAM_DEFAULT_OPERATION;
                    if (op != PARAM_OPERATIONS_LIST[0])
                    {
                        string ext = fi.Extension.ToLower();
                        if (ext == ".enx")
                            op = PARAM_OPERATIONS_LIST[4];
                        else if ((ext == ".jpg") || (ext == ".jpeg"))
                        {
                            if (op == PARAM_OPERATIONS_LIST[1]) op = PARAM_OPERATIONS_LIST[2];
                            if (op == PARAM_OPERATIONS_LIST[4]) op = PARAM_OPERATIONS_LIST[0];
                        }
                        else
                        {
                            if ((op == PARAM_OPERATIONS_LIST[1]) && (fi.Length > (40 * 1024))) op = PARAM_OPERATIONS_LIST[3];
                            if (op == PARAM_OPERATIONS_LIST[2]) op = PARAM_OPERATIONS_LIST[0];
                            if (op == PARAM_OPERATIONS_LIST[4]) op = PARAM_OPERATIONS_LIST[0];
                            if (op == PARAM_OPERATIONS_LIST[5]) op = PARAM_OPERATIONS_LIST[0];
                            if (op == PARAM_OPERATIONS_LIST[6]) op = PARAM_OPERATIONS_LIST[0];
                            if (op == PARAM_OPERATIONS_LIST[7]) op = PARAM_OPERATIONS_LIST[0];
                        };
                    };
                    lvi.SubItems.Add(op);//4
                    lvi.SubItems.Add("");//5
                    lvi.SubItems.Add("");//6
                    lvi.SubItems.Add("");//7
                    lvi.SubItems.Add("");//8
                    lvi.SubItems.Add(fi.FullName);//9
                    listFile.Items.Add(lvi);
                    lvi.EnsureVisible();
                    ST_TTF.Text = listFile.Items.Count.ToString();
                    wbf.Text = "Wait... Listing Files and Directories...\r\n"+
                        ".. already found " + (listFile.Items.Count - wasCount).ToString()+ " files";
                    if (wbf.Cancelled)
                    {
                        wbf.Hide();
                        throw new Exception("Cancelled");
                    };
                    Application.DoEvents();
                };
            };
        }

        private void addFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Add Files";
            ofd.Filter = "All Types (*.*)|*.*|JPEG Files (*.jpg;*.jpeg)|*.jpg;*.jpeg|ENX Files (*.enx)|*.enx";
            ofd.Multiselect = true;
            if (ofd.ShowDialog() == DialogResult.OK)
                DropFiles(ofd.FileNames);
            ofd.Dispose();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            removeFilesToolStripMenuItem.Enabled = listFile.SelectedIndices.Count > 0;
            removeFilesToolStripMenuItem.Text = listFile.SelectedIndices.Count == 1 ? "Remove File" : ("Remove " + listFile.SelectedIndices.Count.ToString() + " Files");
            clearFileListToolStripMenuItem.Enabled = listFile.Items.Count > 0;
            clearFileListToolStripMenuItem.Text = "Clear List (" + listFile.Items.Count.ToString() + " files)";
            processFilesToolStripMenuItem.Enabled = listFile.Items.Count > 0;
            processFilesToolStripMenuItem.Text = "Process " + listFile.Items.Count.ToString() + " files";
            removeNoOperationFilesToolStripMenuItem.Enabled = listFile.Items.Count > 0;
            removeCompletedToolStripMenuItem.Enabled = listFile.Items.Count > 0;
            clearStatusToolStripMenuItem.Enabled = listFile.Items.Count > 0;
            applyWorkToAllToolStripMenuItem.Enabled = listFile.Items.Count > 0;
            removeNotExistingToolStripMenuItem.Enabled = listFile.Items.Count > 0;
        }

        private void removeFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listFile.Items.Count == 0) return;
            for (int i = listFile.Items.Count - 1; i >= 0; i--)
                if (listFile.Items[i].Selected)
                    listFile.Items.RemoveAt(i);
            ST_TTF.Text = listFile.Items.Count.ToString();
        }

        private void listFile_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\r') return;
            listFile_MouseDoubleClick(sender, null);
        }

        private void listFile_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listFile.Items.Count == 0) return;
            if (listFile.SelectedIndices.Count != 1) return;

            List<string> oplist = PARAM_OPERATIONS_LIST;
            FileInfo fi = new FileInfo(listFile.SelectedItems[0].SubItems[9].Text);
            string ext = fi.Extension.ToLower();

            if (ext == ".enx")
            {
                oplist.Clear();
                oplist.Add("NO");
                oplist.Add("Decrypt from ENX");
                oplist.Add("Batch Work");
            }
            else if ((ext == ".jpg") || (ext == ".jpeg"))
            {
                oplist.RemoveAt(4);
                oplist.RemoveAt(1);
            }
            else
            {
                oplist.RemoveAt(7);
                oplist.RemoveAt(6);
                oplist.RemoveAt(5);
                oplist.RemoveAt(4);
                oplist.RemoveAt(2);
                if (fi.Length > (40 * 1024))
                    oplist.RemoveAt(1);
            };

            string val = listFile.SelectedItems[0].SubItems[4].Text;
            if (InputBox.Show("Change Parameter", "Default operation for new file in list:", oplist.ToArray(), ref val, false) == DialogResult.OK)
                listFile.SelectedItems[0].SubItems[4].Text = val;
        }

        private void removeNoOperationFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listFile.Items.Count == 0) return;
            for (int i = listFile.Items.Count - 1; i >= 0; i--)
                if (listFile.Items[i].SubItems[4].Text == "NO")
                    listFile.Items.RemoveAt(i);
            ST_TTF.Text = listFile.Items.Count.ToString();
        }

        private void processFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listFile.Items.Count == 0) return;
            wbf.Cancelable = true;
            wbf.CancelText = "STOP";
            wbf.Show("Processing Files", String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0.00%} ({1}/{2})", new object[] { 0, 0, listFile.Items.Count }), 0);
            ST_PROCESS.Text = "Loading...";
            DateTime Started= DateTime.Now;
            try
            {
                BATCHWORK BW = new BATCHWORK(todobatch, exRW);
                for (int z = 0; z < listFile.Items.Count; z++)
                {
                    listFile.Items[z].EnsureVisible();
                    if (wbf.Cancelled)
                    {
                        wbf.Hide();
                        throw new Exception("Cancelled");
                    };
                    listFile.Items[z].SubItems[5].Text = "";
                    listFile.Items[z].SubItems[6].Text = "";                    
                    string todo = listFile.Items[z].SubItems[4].Text;
                    if (todo == "NO")
                    {
                        listFile.Items[z].SubItems[6].Text = "NO";
                    };
                    string file = listFile.Items[z].SubItems[9].Text;
                    if (!File.Exists(file))
                    {
                        todo = "NO";
                        listFile.Items[z].SubItems[6].Text = "Error: File Not Found";
                    };
                    if (todo == "Encrypt Small File to JPEG")
                    {
                        if ((String.IsNullOrEmpty(PARAM_KEYWORD)) || (!File.Exists(PARAM_JPEGFILE_FULL)))
                        {
                            if(String.IsNullOrEmpty(PARAM_KEYWORD))
                                listFile.Items[z].SubItems[6].Text = "BAD KEYWORD";
                            else
                                listFile.Items[z].SubItems[6].Text = "No JPEG file found";
                        }
                        else
                        {
                            try
                            {
                                string resf_file = WriteDataToJPEG(file, PARAM_JPEGFILE_FULL);                                
                                if (String.IsNullOrEmpty(resf_file))
                                    listFile.Items[z].SubItems[6].Text = "Error: " + (CryptoJPEG.CryptoEXIF.LastError == null ? "" : CryptoJPEG.CryptoEXIF.LastError.Message.ToString());
                                else
                                    listFile.Items[z].SubItems[6].Text = "Enrypted to JPEG";
                                listFile.Items[z].SubItems[5].Text = Path.GetFileName(resf_file);
                            }
                            catch (Exception ex)
                            {
                                listFile.Items[z].SubItems[6].Text = "Error: " + ex.Message;
                            };
                        };
                    };
                    if (todo == "Decrypt Small File from JPEG")
                    {
                        if ((String.IsNullOrEmpty(PARAM_KEYWORD)) || (!File.Exists(PARAM_JPEGFILE_FULL)))
                        {
                            if (String.IsNullOrEmpty(PARAM_KEYWORD))
                                listFile.Items[z].SubItems[6].Text = "BAD KEYWORD";
                            else
                                listFile.Items[z].SubItems[6].Text = "No JPEG file found";
                        }
                        else
                        {
                            CryptoJPEG.CryptoEXIF.KEYWORD = PARAM_KEYWORD;                            
                            try
                            {
                                string resf_file = CryptoJPEG.CryptoEXIF.ReadDataFromJPEG(file);                                
                                if (String.IsNullOrEmpty(resf_file))
                                    listFile.Items[z].SubItems[6].Text = "Error: " + (CryptoJPEG.CryptoEXIF.LastError == null ? "" : CryptoJPEG.CryptoEXIF.LastError.Message.ToString());
                                else
                                    listFile.Items[z].SubItems[6].Text = "Decrypted from JPEG";
                                listFile.Items[z].SubItems[5].Text = Path.GetFileName(resf_file);
                            }
                            catch (Exception ex)
                            {
                                listFile.Items[z].SubItems[6].Text = "Error: " + ex.Message;
                            };
                        };
                    };
                    if (todo == "Encrypt to ENX")
                    {
                        try
                        {
                            string new_file = Path.GetDirectoryName(file).Trim('\\') + @"\" + Path.GetFileNameWithoutExtension(Path.GetFileName(file)) + ".enx";
                            if (ENDECRYPTER.Crypto.EncryptFile(file, new_file, PARAM_KEYWORD, ENDECRYPTER.Crypto.KEYSALT))
                            {
                                listFile.Items[z].SubItems[6].Text = "Crypted to ENX";
                                listFile.Items[z].SubItems[5].Text = Path.GetFileName(new_file);
                            }
                            else
                                listFile.Items[z].SubItems[6].Text = "Error";
                        }
                        catch (Exception ex)
                        {
                            listFile.Items[z].SubItems[6].Text = "Error: " + ex.Message;
                        };                       
                    };
                    if(todo == "Decrypt from ENX")
                    {
                        try
                        {
                            string new_file = "";
                            if (ENDECRYPTER.Crypto.DecryptFile(file, out new_file, PARAM_KEYWORD, ENDECRYPTER.Crypto.KEYSALT))
                            {
                                listFile.Items[z].SubItems[6].Text = "Decrypted from ENX";
                                listFile.Items[z].SubItems[5].Text = Path.GetFileName(new_file);
                            }
                            else
                                listFile.Items[z].SubItems[6].Text = "Error";
                        }
                        catch (Exception ex)
                        {
                            listFile.Items[z].SubItems[6].Text = "Error: " + ex.Message;
                        };    
                    };
                    if (todo == "JPEG EXIF Rewrite")
                    {
                        if(RewriteFileExif(file))
                            listFile.Items[z].SubItems[6].Text = "Exif Rewrited";
                        else
                            listFile.Items[z].SubItems[6].Text = "Error: " + (CryptoJPEG.CryptoEXIF.LastError == null ? "" : CryptoJPEG.CryptoEXIF.LastError.Message.ToString());

                    };
                    if (todo == "JPEG Enrypt Image")
                    {
                        if (ProceedENJ(file, true))
                            listFile.Items[z].SubItems[6].Text = "Image Crypted";
                        else
                            listFile.Items[z].SubItems[6].Text = "Error Crypt Image";
                    };
                    if (todo == "JPEG Decrypt Image")
                    {
                        if (ProceedENJ(file, false))
                            listFile.Items[z].SubItems[6].Text = "Image Decrypted";
                        else
                            listFile.Items[z].SubItems[6].Text = "Error Decrypt Image";
                    };
                    if (todo == "Batch Work")
                    {
                        string status;
                        string new_file;
                        bool new_filed;
                        if (BW.ProcessFile(file, z + 1, out status, out new_file, out new_filed))
                        {
                            listFile.Items[z].SubItems[6].Text = status;
                            listFile.Items[z].SubItems[5].Text = Path.GetFileName(new_file);
                        }
                        else
                            listFile.Items[z].SubItems[6].Text = "Error";
                    };
                    if (todo == "Rename File")
                    {
                        string status;
                        string new_file;
                        bool new_filed;
                        if (BW.RenameFile(file, z + 1, out status, out new_file, out new_filed))
                        {
                            listFile.Items[z].SubItems[6].Text = status;
                            listFile.Items[z].SubItems[5].Text = Path.GetFileName(new_file);
                        }
                        else
                            listFile.Items[z].SubItems[6].Text = "Error";
                    };
                    ST_PROCESS.Text = String.Format("{0}/{1} {2}", z + 1, listFile.Items.Count, listFile.Items[z].SubItems[2].Text);
                    Application.DoEvents();
                    TimeSpan span = DateTime.Now.Subtract(Started);
                    wbf.Show("Processing Files", String.Format(System.Globalization.CultureInfo.InvariantCulture, "Progress: {0:0.00%} ({1}/{2})\r\nElapsed: {3}", new object[] { (float)z / (float)listFile.Items.Count, z, listFile.Items.Count, span }), (int)((float)z / (float)listFile.Items.Count * 100.0));                    
                };
                ST_PROCESS.Text = "DONE";
                BW.Dispose();
            }
            catch (Exception ex)
            {
                ST_PROCESS.Text = "STOPPED";
            }
            finally
            {
                wbf.Hide();
            };
        }

        private void listFile_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 46)
                removeFilesToolStripMenuItem_Click(sender, null);
            if (e.KeyValue == 116)
                processFilesToolStripMenuItem_Click(sender, null);            

            if ((e.KeyValue == 37) || (e.KeyValue == 39))
            {
                if (listFile.SelectedItems.Count != 1) return;                
                List<string> vals = PARAM_OPERATIONS_LIST;
                int index = vals.IndexOf(listFile.SelectedItems[0].SubItems[4].Text);
                if (e.KeyValue == 37) index--;
                if (e.KeyValue == 39) index++;
                if (index < 0) index = vals.Count - 1;
                if (index >= vals.Count) index = 0;
                listFile.SelectedItems[0].SubItems[4].Text = vals[index];
                e.SuppressKeyPress = true;
                return;
            };
        }

        private void removeCompletedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listFile.Items.Count == 0) return;
            for (int i = listFile.Items.Count - 1; i >= 0; i--)
                if (listFile.Items[i].SubItems[6].Text != "")
                    listFile.Items.RemoveAt(i);
            ST_TTF.Text = listFile.Items.Count.ToString();
        }

        public class BATCHWORK
        {
            private string temp_file_name = MainJPEGForm.CD + @"\__tmp__.tmp";
            private ListView todobatch;
            private ListView exRW;
            private int start_index = 1;
            private bool unical_dir = false;
            private bool rename_files = false;
            private string rename_pattern = "";
            private bool rewrite_exif = false;
            private int resize = 0;
            private int im_wi = 1000;
            private int im_he = 1000;
            private int im_ql = 75;
            private int save_to = 0;
            private string save_path = "";
            private int change_ext = 0;
            private bool addWMT = false;
            private string addWMTtext = "PROTECTED";
            private int addWMTop = 50;
            private int addWMTpos = 0;
            private string addWMFont = "Arial";
            private string addWMSize = "14px";
            private Color addWMColor = Color.Black;
            private bool addWMI = false;
            private string addWMIFile = "";
            private int addWMIop = 50;
            private int addWMIpos = 0;
            private ImageMagick.MagickImage WatermarkImage = null;
            private string last_path = "";
            private int file_index = 1;

            public BATCHWORK(ListView todobatch, ListView exRW)
            {
                this.todobatch = todobatch;
                this.exRW = exRW;
                file_index = start_index = int.Parse(todobatch.Items[0].SubItems[3].Text);
                if ((todobatch.Items[1].SubItems[3].Text.ToLower().Trim() == "yes") || (todobatch.Items[1].SubItems[3].Text.ToLower().Trim() == "true"))
                    unical_dir = true;
                if ((todobatch.Items[2].SubItems[3].Text.ToLower().Trim() == "yes") || (todobatch.Items[2].SubItems[3].Text.ToLower().Trim() == "true"))
                    rename_files = true;
                rename_pattern = todobatch.Items[3].SubItems[3].Text.Trim();
                if ((todobatch.Items[4].SubItems[3].Text.ToLower().Trim() == "yes") || (todobatch.Items[4].SubItems[3].Text.ToLower().Trim() == "true"))
                    rewrite_exif = true;
                string text = todobatch.Items[5].SubItems[3].Text.Trim().ToLower();
                if (text == "resize") resize = 1;
                if (text == "reduce") resize = 2;
                if (text == "enlarge") resize = 3;
                im_wi = int.Parse(todobatch.Items[6].SubItems[3].Text.Trim());
                im_he = int.Parse(todobatch.Items[7].SubItems[3].Text.Trim());
                im_ql = int.Parse(todobatch.Items[8].SubItems[3].Text.Trim());
                if (todobatch.Items[9].SubItems[3].Text.Trim() == "Subfolder") save_to = 1;
                if (todobatch.Items[9].SubItems[3].Text.Trim() == "Path") save_to = 2;
                save_path = todobatch.Items[10].SubItems[3].Text.Trim();
                if (todobatch.Items[11].SubItems[3].Text.Trim() == "Lower Extention") change_ext = 1;
                if (todobatch.Items[11].SubItems[3].Text.Trim() == "Lower Name") change_ext = 2;
                if (todobatch.Items[11].SubItems[3].Text.Trim() == "Lower Full Name") change_ext = 3;
                if (todobatch.Items[11].SubItems[3].Text.Trim() == "Upper Extention") change_ext = 4;
                if (todobatch.Items[11].SubItems[3].Text.Trim() == "Upper Name") change_ext = 5;
                if (todobatch.Items[11].SubItems[3].Text.Trim() == "Upper Full Name") change_ext = 6;
                if ((todobatch.Items[12].SubItems[3].Text.ToLower().Trim() == "yes") || (todobatch.Items[12].SubItems[3].Text.ToLower().Trim() == "true"))
                    addWMT = true;
                addWMTtext = todobatch.Items[13].SubItems[3].Text.Trim();
                addWMT = addWMT && (!(String.IsNullOrEmpty(addWMTtext)));
                addWMTop = int.Parse(todobatch.Items[14].SubItems[3].Text.Trim());
                addWMTpos = int.Parse(todobatch.Items[15].SubItems[3].Text.Substring(0, 1));
                addWMFont = todobatch.Items[16].SubItems[3].Text.Trim();
                addWMSize = todobatch.Items[17].SubItems[3].Text.Trim();
                addWMColor = RGBConverter(todobatch.Items[18].SubItems[3].Text.Trim());
                if ((todobatch.Items[19].SubItems[3].Text.ToLower().Trim() == "yes") || (todobatch.Items[19].SubItems[3].Text.ToLower().Trim() == "true"))
                    addWMI = true;
                addWMIFile = todobatch.Items[20].SubItems[3].Text.Trim();
                addWMI = addWMI && File.Exists(addWMIFile);
                addWMIop = int.Parse(todobatch.Items[21].SubItems[3].Text.Trim());
                addWMIpos = int.Parse(todobatch.Items[22].SubItems[3].Text.Substring(0, 1));

                try
                {
                    if (addWMI)
                    {
                        WatermarkImage = new ImageMagick.MagickImage(addWMIFile);
                        if (addWMIop < 100)
                            WatermarkImage = new ImageMagick.MagickImage((Bitmap)ChangeOpacity(WatermarkImage.ToBitmap(), addWMIop));
                    };
                }
                catch
                {
                    addWMI = false;
                };
            }

            public void Dispose()
            {
                if (WatermarkImage != null)
                    WatermarkImage.Dispose();
            }

            public bool ProcessFile(string file, int fileNumber, out string status, out string new_file, out bool new_filed)
            {
                status = "analizing";
                new_filed = false;
                List<string> state = new List<string>();
                new_file = "";

                bool retval = false;
                try
                {
                    // Read File Info
                    FileInfo fi_origin = new FileInfo(file);
                    string file_origin = fi_origin.FullName;
                    string file_saveto = fi_origin.FullName;

                    // Reindex File Number
                    string curr_path = Path.GetDirectoryName(file_origin);
                    if (curr_path != last_path)
                    {
                        last_path = curr_path;
                        if (!unical_dir)
                            file_index = start_index;
                    };

                    // READ EXIF IF JPEG
                    CryptoEXIF.ExifInfo exif = new CryptoEXIF.ExifInfo();
                    if (IsJPG(file_origin))
                        try { exif = CryptoEXIF.ExifInfo.ParseExifFile(file_origin); }
                        catch { };
                    Application.DoEvents();

                    // If save to another path
                    string save_dir = save_path;
                    string rp = "";
                    try
                    {
                        rp = Path.GetPathRoot(save_dir);
                    }
                    catch { save_dir = "Processed"; };
                    if ((save_to == 1) || ((save_to == 2) && (rp == "")))
                    {
                        save_dir = fi_origin.Directory.ToString().Trim('\\') + @"\" + save_dir;
                        save_dir = save_dir.Trim('\\') + @"\";
                    };
                    if (save_to > 0)
                    {
                        try
                        {
                            if (rewrite_exif || rename_files || (resize > 0))
                                Directory.CreateDirectory(save_dir);
                        }
                        catch
                        {
                            status = "BAD PATH";
                            return false;
                        };
                        file_saveto = save_dir + fi_origin.Name;
                    };

                    // Formatting new file name                                
                    {
                        string renamed_file = rename_pattern;
                        MatchCollection mc = Regex.Matches(renamed_file, @"({[\w\:\s]+})", RegexOptions.IgnoreCase);
                        if (mc.Count > 0)
                            for (int x = 0; x < mc.Count; x++)
                            {
                                string gg = mc[x].Value;
                                string ttm = gg.Trim(new char[] { '{', '}' });
                                string[] spl = ttm.Split(new char[] { ':' }, 2);
                                string par = spl[0];
                                string val = "{0}";
                                if (spl.Length > 1)
                                    val = "{0:" + spl[1] + "}";
                                if (par == "FileNumber") val = String.Format(val, fileNumber);
                                if (par == "FileIndex") val = String.Format(val, file_index);
                                if (par == "FileName") val = String.Format(val, fi_origin.Name.Replace(fi_origin.Extension, ""));
                                if (par == "FileModifiedDateTime") val = String.Format(val, fi_origin.LastWriteTime);
                                if (par == "FileCreatedDateTime") val = String.Format(val, fi_origin.CreationTime);
                                if ((exif != null) && (exif.Count > 0))
                                    for (int y = 0; y < exif.Count; y++)
                                        if (par == exif.Entries[y].entry_tag_name)
                                        {
                                            DateTime dttp;
                                            long ltp;
                                            if (DateTime.TryParse(exif.Entries[y].entry_text, out dttp))
                                                val = String.Format(val, dttp);
                                            else if (long.TryParse(exif.Entries[y].entry_text, out ltp))
                                                val = String.Format(val, ltp);
                                            else
                                                val = String.Format(val, exif.Entries[y].entry_text);
                                        };
                                val = String.Format(val, "");
                                renamed_file = renamed_file.Replace(gg, val);
                            };

                        // 
                        renamed_file = renamed_file.Replace(":", "-");
                        while (renamed_file.IndexOf("  ") >= 0) renamed_file = renamed_file.Replace("  ", " ");
                        renamed_file = renamed_file.Trim();
                        string ext = fi_origin.Extension;
                        if ((change_ext == 1) || (change_ext == 3)) ext = ext.ToLower();
                        if ((change_ext == 2) || (change_ext == 3)) renamed_file = renamed_file.ToLower();
                        if ((change_ext == 4) || (change_ext == 6)) ext = ext.ToUpper();
                        if ((change_ext == 5) || (change_ext == 6)) renamed_file = renamed_file.ToUpper();
                        new_file = renamed_file = renamed_file + ext;
                        if (rename_files)
                            file_saveto = Path.GetDirectoryName(file_saveto).Trim('\\') + @"\" + renamed_file;
                    };

                    // delete if destination file is already exists
                    if (save_to > 0)
                    {
                        try
                        {
                            if (File.Exists(file_saveto))
                                File.Delete(file_saveto);
                        }
                        catch { };
                    };

                    if (IsJPG(file_origin))
                    {
                        // RESIZE or WATERMARK
                        if ((resize > 0) || addWMT || addWMI)
                        {
                            try
                            {
                                ImageMagick.MagickImage mi = new ImageMagick.MagickImage(file_origin);
                                Application.DoEvents();
                                byte need2save = 0;
                                if (resize > 0)
                                    if ((resize == 1) || ((resize == 2) && (!((mi.Width <= im_wi) && (mi.Height <= im_he)))) || ((resize == 3) && (!((mi.Width >= im_wi) || (mi.Height >= im_he)))))
                                    {
                                        mi.Resize(im_wi, im_he);
                                        need2save += 1;
                                    };
                                if (addWMT)
                                {
                                    using (Bitmap imageObj = mi.ToBitmap())
                                    {
                                        using (Graphics imageGraphics = Graphics.FromImage(imageObj))
                                        {
                                            float emSize = 12;
                                            if (addWMSize.EndsWith("%"))
                                                emSize = float.Parse(addWMSize.Remove(addWMSize.Length - 1)) * (float)mi.Height / 100;
                                            else
                                                emSize = float.Parse(addWMSize.Remove(addWMSize.Length - 2));
                                            Font f = new Font(addWMFont, emSize, FontStyle.Regular);
                                            SolidBrush sb = new SolidBrush(Color.FromArgb((int)(255 * addWMTop / 100), addWMColor));
                                            SizeF sf = imageGraphics.MeasureString(addWMTtext, f);
                                            if (addWMTpos == 0)
                                                imageGraphics.DrawString(addWMTtext, f, sb, mi.Width / 2 - sf.Width / 2, mi.Height / 2 - sf.Height / 2);
                                            if (addWMTpos == 1)
                                                imageGraphics.DrawString(addWMTtext, f, sb, mi.Width / 2 - sf.Width / 2, 5);
                                            if (addWMTpos == 2)
                                                imageGraphics.DrawString(addWMTtext, f, sb, mi.Width - sf.Width - 5, 5);
                                            if (addWMTpos == 3)
                                                imageGraphics.DrawString(addWMTtext, f, sb, mi.Width - sf.Width - 5, mi.Height / 2 - sf.Height / 2);
                                            if (addWMTpos == 4)
                                                imageGraphics.DrawString(addWMTtext, f, sb, mi.Width - sf.Width - 5, mi.Height - sf.Height - 5);
                                            if (addWMTpos == 5)
                                                imageGraphics.DrawString(addWMTtext, f, sb, mi.Width / 2 - sf.Width / 2, mi.Height - sf.Height - 5);
                                            if (addWMTpos == 6)
                                                imageGraphics.DrawString(addWMTtext, f, sb, 5, mi.Height - sf.Height - 5);
                                            if (addWMTpos == 7)
                                                imageGraphics.DrawString(addWMTtext, f, sb, 5, mi.Height / 2 - sf.Height / 2);
                                            if (addWMTpos == 8)
                                                imageGraphics.DrawString(addWMTtext, f, sb, 5, 5);

                                        };
                                        mi = new ImageMagick.MagickImage(imageObj);
                                        need2save += 10;
                                    };
                                };
                                if (addWMI)
                                {
                                    PointF point = new PointF(0, 0);

                                    if (addWMIpos == 0)
                                        point = new PointF(mi.Width / 2 - WatermarkImage.Width / 2, mi.Height / 2 - WatermarkImage.Height / 2);
                                    if (addWMIpos == 1)
                                        point = new PointF(mi.Width / 2 - WatermarkImage.Width / 2, 2);
                                    if (addWMIpos == 2)
                                        point = new PointF(mi.Width - WatermarkImage.Width - 2, 2);
                                    if (addWMIpos == 3)
                                        point = new PointF(mi.Width - WatermarkImage.Width - 2, mi.Height / 2 - WatermarkImage.Height / 2);
                                    if (addWMIpos == 4)
                                        point = new PointF(mi.Width - WatermarkImage.Width - 2, mi.Height - WatermarkImage.Height - 2);
                                    if (addWMIpos == 5)
                                        point = new PointF(mi.Width / 2 - WatermarkImage.Width / 2, mi.Height - WatermarkImage.Height - 2);
                                    if (addWMIpos == 6)
                                        point = new PointF(2, mi.Height - WatermarkImage.Height - 2);
                                    if (addWMIpos == 7)
                                        point = new PointF(2, mi.Height / 2 - WatermarkImage.Height / 2);
                                    if (addWMIpos == 8)
                                        point = new PointF(2, 2);

                                    mi.Composite(WatermarkImage, (int)point.X, (int)point.Y, ImageMagick.CompositeOperator.Over, "-channel a -evaluate multiply 0.5 +channel");
                                    need2save += 10;
                                };
                                if (need2save > 0)
                                {
                                    mi.Quality = im_ql;
                                    mi.Write(file_saveto);
                                    file_origin = file_saveto;
                                    if ((need2save % 10) == 1)
                                        state.Add("Image Resized");
                                    if (need2save > 1)
                                        state.Add("Watermarked");
                                }
                                else
                                {
                                    state.Add("No Need Resize");
                                };
                                mi.Dispose();
                            }
                            catch (Exception ex)
                            {
                                state.Add("Resize/Watermark Error");
                            };
                        }; // else state.Add("No need Resize");
                        Application.DoEvents();


                        // REWRITE EXIF
                        if (rewrite_exif)
                        {
                            Dictionary<string, object> exif_new = new Dictionary<string, object>();
                            for (int x = 0; x < exRW.Items.Count; x++)
                            {
                                string txt = exRW.Items[x].SubItems[3].Text.Trim();
                                if (txt.Length == 0) continue;
                                if (txt == "{LastFileWriteTime}")
                                    txt = fi_origin.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");
                                if (txt == "{FileName}")
                                    txt = fi_origin.Name;
                                if (txt.StartsWith("{"))
                                {
                                    txt = exif[txt.Replace("{", "").Replace("}", ""), ""];
                                    if (String.IsNullOrEmpty(txt)) continue;
                                };
                                exif_new.Add(exRW.Items[x].SubItems[1].Text, txt);
                            };
                            try
                            {
                                if (file_origin == file_saveto)
                                {
                                    CryptoEXIF.ExifInfo.ExifRewrite(file_origin, temp_file_name, exif_new);
                                    File.Delete(file_saveto);
                                    File.Move(temp_file_name, file_saveto);
                                }
                                else
                                {
                                    CryptoEXIF.ExifInfo.ExifRewrite(file_origin, file_saveto, exif_new);
                                    file_origin = file_saveto;
                                };
                                state.Add("Exif Rewrited");
                            }
                            catch (Exception ex)
                            {
                                state.Add("Exif RW Error");
                            };
                        }; // else state.Add("Exif RW No need");
                        Application.DoEvents();
                    }
                    else
                    {
                        //state.Add("Exif RW No JPEG");
                    };


                    // RENAME FILE //
                    if (rename_files)
                    {
                        if (String.Compare(fi_origin.FullName, file_saveto, true) == 0) // the same file
                        {
                            if (fi_origin.FullName == file_saveto)
                            {
                                state.Add("Skip rename");
                            }
                            else
                            {
                                File.Move(fi_origin.FullName, file_saveto);
                                state.Add("File Renamed");
                                new_filed = true;
                            };
                        }
                        else
                        {
                            if (File.Exists(file_saveto))
                            {
                                if (save_to > 0)
                                {
                                    state.Add("Saved to new file");
                                    new_filed = true;
                                }
                                else
                                {
                                    if (String.Compare(fi_origin.FullName, file_saveto, true) == 0)
                                    {
                                        File.Move(fi_origin.FullName, file_saveto);
                                        state.Add("File Renamed");
                                        new_filed = true;
                                    }
                                    else
                                    {
                                        try
                                        {
                                            File.Delete(fi_origin.FullName);
                                            state.Add("File Renamed");
                                            new_filed = true;
                                        }
                                        catch (Exception ex)
                                        {
                                            state.Add("File Copied");
                                        };
                                    };
                                };
                            }
                            else
                            {
                                if (save_to > 0)
                                {
                                    try
                                    {
                                        File.Copy(file_origin, file_saveto, true);
                                        state.Add("File Copied");
                                        new_filed = true;
                                    }
                                    catch (Exception ex)
                                    {
                                        state.Add("Copy Error");
                                    };
                                }
                                else
                                {
                                    try
                                    {
                                        File.Move(file_origin, file_saveto);
                                        state.Add("File Renamed");
                                        new_filed = true;
                                    }
                                    catch (Exception ex)
                                    {
                                        state.Add("Rename File Error");
                                    };
                                };
                            };
                        };
                    }; //else state.Add("No need rename");

                    file_index++;
                    retval = true;
                }
                catch (Exception ex)
                {
                    state.Add(ex.Message);
                    retval = false;
                }
                finally
                {                    

                };
                status = "";
                if(state.Count > 0)
                    foreach (string st in state)
                    {
                        if (status.Length > 0)
                            status += ", ";
                        status += st;
                    };
                return retval;
            }

            public bool RenameFile(string file, int fileNumber, out string status, out string new_file, out bool new_filed)
            {
                status = "analizing";
                new_filed = false;
                List<string> state = new List<string>();
                new_file = "";

                bool retval = false;
                try
                {
                    // Read File Info
                    FileInfo fi_origin = new FileInfo(file);
                    string file_origin = fi_origin.FullName;
                    string file_saveto = fi_origin.FullName;

                    // Reindex File Number
                    string curr_path = Path.GetDirectoryName(file_origin);
                    if (curr_path != last_path)
                    {
                        last_path = curr_path;
                        if (!unical_dir)
                            file_index = start_index;
                    };

                    // READ EXIF IF JPEG
                    CryptoEXIF.ExifInfo exif = new CryptoEXIF.ExifInfo();
                    if (IsJPG(file_origin))
                        try { exif = CryptoEXIF.ExifInfo.ParseExifFile(file_origin); }
                        catch { };
                    Application.DoEvents();

                    // If save to another path
                    string save_dir = save_path;
                    
                    // Formatting new file name                                
                    {
                        string renamed_file = rename_pattern;
                        MatchCollection mc = Regex.Matches(renamed_file, @"({[\w\:\s]+})", RegexOptions.IgnoreCase);
                        if (mc.Count > 0)
                            for (int x = 0; x < mc.Count; x++)
                            {
                                string gg = mc[x].Value;
                                string ttm = gg.Trim(new char[] { '{', '}' });
                                string[] spl = ttm.Split(new char[] { ':' }, 2);
                                string par = spl[0];
                                string val = "{0}";
                                if (spl.Length > 1)
                                    val = "{0:" + spl[1] + "}";
                                if (par == "FileNumber") val = String.Format(val, fileNumber);
                                if (par == "FileIndex") val = String.Format(val, file_index);
                                if (par == "FileName") val = String.Format(val, fi_origin.Name.Replace(fi_origin.Extension, ""));
                                if (par == "FileModifiedDateTime") val = String.Format(val, fi_origin.LastWriteTime);
                                if (par == "FileCreatedDateTime") val = String.Format(val, fi_origin.CreationTime);
                                if ((exif != null) && (exif.Count > 0))
                                    for (int y = 0; y < exif.Count; y++)
                                        if (par == exif.Entries[y].entry_tag_name)
                                        {
                                            DateTime dttp;
                                            long ltp;
                                            if (DateTime.TryParse(exif.Entries[y].entry_text, out dttp))
                                                val = String.Format(val, dttp);
                                            else if (long.TryParse(exif.Entries[y].entry_text, out ltp))
                                                val = String.Format(val, ltp);
                                            else
                                                val = String.Format(val, exif.Entries[y].entry_text);
                                        };
                                val = String.Format(val, "");
                                renamed_file = renamed_file.Replace(gg, val);
                            };

                        // 
                        renamed_file = renamed_file.Replace(":", "-");
                        while (renamed_file.IndexOf("  ") >= 0) renamed_file = renamed_file.Replace("  ", " ");
                        renamed_file = renamed_file.Trim();
                        string ext = fi_origin.Extension;
                        if ((change_ext == 1) || (change_ext == 3)) ext = ext.ToLower();
                        if ((change_ext == 2) || (change_ext == 3)) renamed_file = renamed_file.ToLower();
                        if ((change_ext == 4) || (change_ext == 6)) ext = ext.ToUpper();
                        if ((change_ext == 5) || (change_ext == 6)) renamed_file = renamed_file.ToUpper();
                        new_file = renamed_file = renamed_file + ext;
                        if (true)
                            file_saveto = Path.GetDirectoryName(file_saveto).Trim('\\') + @"\" + renamed_file;
                    };                    

                    // RENAME FILE //
                    if (true)
                    {
                        if (String.Compare(fi_origin.FullName, file_saveto, true) == 0) // the same file
                        {
                            if (fi_origin.FullName == file_saveto)
                            {
                                state.Add("Skip rename");
                            }
                            else
                            {
                                File.Move(fi_origin.FullName, file_saveto);
                                state.Add("File Renamed");
                                new_filed = true;
                            };
                        }
                        else
                        {
                            if (File.Exists(file_saveto))
                            {
                                if (save_to > 0)
                                {
                                    state.Add("Saved to new file");
                                    new_filed = true;
                                }
                                else
                                {
                                    if (String.Compare(fi_origin.FullName, file_saveto, true) == 0)
                                    {
                                        File.Move(fi_origin.FullName, file_saveto);
                                        state.Add("File Renamed");
                                        new_filed = true;
                                    }
                                    else
                                    {
                                        try
                                        {
                                            File.Delete(fi_origin.FullName);
                                            state.Add("File Renamed");
                                            new_filed = true;
                                        }
                                        catch (Exception ex)
                                        {
                                            state.Add("File Copied");
                                        };
                                    };
                                };
                            }
                            else
                            {
                                if (save_to > 0)
                                {
                                    try
                                    {
                                        File.Copy(file_origin, file_saveto, true);
                                        state.Add("File Copied");
                                        new_filed = true;
                                    }
                                    catch (Exception ex)
                                    {
                                        state.Add("Copy Error");
                                    };
                                }
                                else
                                {
                                    try
                                    {
                                        File.Move(file_origin, file_saveto);
                                        state.Add("File Renamed");
                                        new_filed = true;
                                    }
                                    catch (Exception ex)
                                    {
                                        state.Add("Rename File Error");
                                    };
                                };
                            };
                        };
                    }; //else state.Add("No need rename");

                    file_index++;
                    retval = true;
                }
                catch (Exception ex)
                {
                    state.Add(ex.Message);
                    retval = false;
                }
                finally
                {

                };
                status = "";
                if (state.Count > 0)
                    foreach (string st in state)
                    {
                        if (status.Length > 0)
                            status += ", ";
                        status += st;
                    };
                return retval;
            }

            private bool IsJPG(string filename)
            {
                filename = filename.ToLower();
                filename = Path.GetExtension(filename);
                if (filename == ".jpg") return true;
                if (filename == ".jpeg") return true;
                return false;
            }
        }

        private void clearStatusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listFile.Items.Count == 0) return;
            for (int i = listFile.Items.Count - 1; i >= 0; i--)
                listFile.Items[i].SubItems[6].Text = "";
        }

        private void applyWorkToAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listFile.Items.Count == 0) return;

            string val = "NO";
            if (InputBox.Show("Change Parameter", "Default operation for new file in list:", PARAM_OPERATIONS_LIST.ToArray(), ref val, false) != DialogResult.OK)
                return;
            for (int i = listFile.Items.Count - 1; i >= 0; i--)
                listFile.Items[i].SubItems[4].Text = val;
        }

        private void loadExifDataFromFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Exif Data (*.exif)|*.exif";
            if(ofd.ShowDialog() == DialogResult.OK)
                ReadRWExif(ofd.FileName);
            ofd.Dispose();
        }

        private void saveExifDataToFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Exif Data (*.exif)|*.exif";
            if (sfd.ShowDialog() == DialogResult.OK)
                WriteRWExif(sfd.FileName);
            sfd.Dispose();
        }

        private void removeNotExistingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listFile.Items.Count == 0) return;
            for (int i = listFile.Items.Count - 1; i >= 0; i--)
                if (!File.Exists(listFile.Items[i].SubItems[9].Text))
                    listFile.Items.RemoveAt(i);
            ST_TTF.Text = listFile.Items.Count.ToString();
        }

        private void listPARAMS_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            const int TEXT_OFFSET = 1;

            ListView listView = (ListView)sender;

            if((e.Item.Text == "KEYWORD") && (e.ColumnIndex == 3))
            {
                Rectangle rowBounds = e.SubItem.Bounds;
                Rectangle labelBounds = e.Item.GetBounds(ItemBoundsPortion.Label);
                int leftMargin = labelBounds.Left - TEXT_OFFSET;
                Rectangle bounds = new Rectangle(rowBounds.Left + leftMargin, rowBounds.Top, e.ColumnIndex == 0 ? labelBounds.Width : (rowBounds.Width - leftMargin - TEXT_OFFSET), rowBounds.Height);
                string text = "";
                if (!String.IsNullOrEmpty(e.SubItem.Text))
                    for (int i = 0; i < e.SubItem.Text.Length; i++)
                        text += "*";
                if (listView.Focused && e.Item.Selected)
                {
                    e.Graphics.FillRectangle(SystemBrushes.Highlight, rowBounds);
                    e.Graphics.DrawString(text, listView.Font, SystemBrushes.HighlightText, bounds);
                }
                else
                {
                    e.Graphics.FillRectangle(SystemBrushes.Window, rowBounds);
                    e.Graphics.DrawString(text, listView.Font, SystemBrushes.WindowText, bounds);
                };
            }
            else if((e.Item.Text == "DEFOP") && (e.ColumnIndex == 3))
            {
                Rectangle rowBounds = e.SubItem.Bounds;
                Rectangle labelBounds = e.Item.GetBounds(ItemBoundsPortion.Label);
                int leftMargin = labelBounds.Left - TEXT_OFFSET;
                Rectangle bounds = new Rectangle(rowBounds.Left + leftMargin, rowBounds.Top, e.ColumnIndex == 0 ? labelBounds.Width : (rowBounds.Width - leftMargin - TEXT_OFFSET), rowBounds.Height);
                string text = e.SubItem.Text;
                string work = "";
                if (text == "Batch Work")
                {                    
                    if (todobatch.Items[2].SubItems[3].Text == "Yes")
                        work += "Rename";
                    if (todobatch.Items[4].SubItems[3].Text == "Yes")
                    {
                        if (work.Length > 0) work += ", ";
                        work += "Exif Rewrite";
                    };
                    if (todobatch.Items[5].SubItems[3].Text != "No")
                    {
                        if (work.Length > 0) work += ", ";
                        work += todobatch.Items[5].SubItems[3].Text;
                    };
                    if (todobatch.Items[12].SubItems[3].Text == "Yes")
                    {
                        if (work.Length > 0) work += ", ";
                        work += "Add Text Watermark";
                    };
                    if (todobatch.Items[19].SubItems[3].Text == "Yes")
                    {
                        if (work.Length > 0) work += ", ";
                        work += "Add Image Watermark";
                    };
                    work = " (" + work + ")";
                };
                if (text == "Rename File")
                {
                    work += "`" + todobatch.Items[3].SubItems[3].Text + "`";
                };
                if (listView.Focused && e.Item.Selected)
                {
                    e.Graphics.FillRectangle(SystemBrushes.Highlight, rowBounds);
                    e.Graphics.DrawString(text, listView.Font, SystemBrushes.HighlightText, bounds);
                    if (work.Length > 0)
                    {
                        SizeF sz = e.Graphics.MeasureString(text, listView.Font);
                        SizeF sy = e.Graphics.MeasureString(work, listView.Font);
                        bounds = new Rectangle(bounds.Left - 2 + (int)sz.Width, bounds.Top, (int)sy.Width + 4, bounds.Height);
                        e.Graphics.DrawString(work, listView.Font, Brushes.Lime, bounds);
                    };
                }
                else
                {
                    e.Graphics.FillRectangle(SystemBrushes.Window, rowBounds);
                    e.Graphics.DrawString(text, listView.Font, SystemBrushes.WindowText, bounds);
                    if (work.Length > 0)
                    {
                        SizeF sz = e.Graphics.MeasureString(text, listView.Font);
                        SizeF sy = e.Graphics.MeasureString(work, listView.Font);
                        bounds = new Rectangle(bounds.Left - 2 + (int)sz.Width, bounds.Top, (int)sy.Width + 4, bounds.Height);
                        e.Graphics.DrawString(work, listView.Font, Brushes.Blue, bounds);
                    };
                };
            }
            else if((e.Item.Text == "JPEGFILE") && (e.ColumnIndex == 3))
            {
                Rectangle rowBounds = e.SubItem.Bounds;
                Rectangle labelBounds = e.Item.GetBounds(ItemBoundsPortion.Label);
                int leftMargin = labelBounds.Left - TEXT_OFFSET;
                Rectangle bounds = new Rectangle(rowBounds.Left + leftMargin, rowBounds.Top, e.ColumnIndex == 0 ? labelBounds.Width : (rowBounds.Width - leftMargin - TEXT_OFFSET), rowBounds.Height);
                if (listView.Focused && e.Item.Selected)
                {
                    if (File.Exists(PARAM_JPEGFILE_FULL))
                    {
                        e.Graphics.FillRectangle(SystemBrushes.Highlight, rowBounds);
                        e.Graphics.DrawString(e.SubItem.Text, listView.Font, SystemBrushes.HighlightText, bounds);
                    }
                    else
                    {
                        e.Graphics.FillRectangle(Brushes.Red, rowBounds);
                        if (String.IsNullOrEmpty(PARAM_JPEGFILE))
                            e.Graphics.DrawString(e.SubItem.Text, new Font(listView.Font, FontStyle.Bold), Brushes.White, bounds);
                        else
                            e.Graphics.DrawString(e.SubItem.Text + " - File Not Found", new Font(listView.Font, FontStyle.Bold), Brushes.White, bounds);
                    };
                }
                else
                {
                    if (File.Exists(PARAM_JPEGFILE_FULL))
                    {
                        e.Graphics.FillRectangle(SystemBrushes.Window, rowBounds);
                        e.Graphics.DrawString(e.SubItem.Text, listView.Font, Brushes.Green, bounds);
                    }
                    else
                    {
                        e.Graphics.FillRectangle(SystemBrushes.Window, rowBounds);
                        if (String.IsNullOrEmpty(PARAM_JPEGFILE))
                            e.Graphics.DrawString(e.SubItem.Text, new Font(listView.Font, FontStyle.Bold), Brushes.Red, bounds);
                        else
                            e.Graphics.DrawString(e.SubItem.Text + " - File Not Found", new Font(listView.Font, FontStyle.Bold), Brushes.Red, bounds);
                    };
                };
            }
            else
                e.DrawDefault = true;
        }

        private void todobatch_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            ListView listView = (ListView)sender;
            e.DrawDefault = false;
            // DEFAULT
            {                
                Rectangle rowBounds = e.SubItem.Bounds;
                Rectangle labelBounds = e.Item.GetBounds(ItemBoundsPortion.Label);
                int leftMargin = labelBounds.Left - 1;
                Rectangle bounds = new Rectangle(rowBounds.Left + leftMargin, rowBounds.Top, e.ColumnIndex == 0 ? labelBounds.Width : (rowBounds.Width - leftMargin - 1), rowBounds.Height);
                Font f = listView.Font;
                if ((e.ColumnIndex == 3) && ((e.Item.Text == "RENAME") || (e.Item.Text == "RWEXIF") || (e.Item.Text == "RESIZE") || (e.Item.Text == "TXTWMADD") || (e.Item.Text == "IMGWMADD")))
                    f = new Font(f, FontStyle.Bold);
                if ((e.ColumnIndex == 3) && (e.Item.Text == "SAVETOSUB")  && (e.SubItem.Text == "Source"))
                    f = new Font(f, FontStyle.Bold);
                if (listView.Focused && e.Item.Selected)
                {
                    e.Graphics.FillRectangle(SystemBrushes.Highlight, rowBounds);
                    e.Graphics.DrawString(e.SubItem.Text, f, Brushes.White, bounds);
                }
                else
                {
                    e.Graphics.FillRectangle(new SolidBrush(e.Item.BackColor), rowBounds);
                    e.Graphics.DrawString(e.SubItem.Text, f, new SolidBrush(e.Item.ForeColor), bounds);
                };
            };

            if (e.ColumnIndex != 3)
                return;

            if (e.Item.Text == "TXTWMCOLOR")
            {
                Rectangle rowBounds = e.SubItem.Bounds;
                Rectangle labelBounds = e.Item.GetBounds(ItemBoundsPortion.Label);
                int leftMargin = labelBounds.Left + 15;
                Rectangle bounds = new Rectangle(rowBounds.Left + leftMargin, rowBounds.Top, e.ColumnIndex == 0 ? labelBounds.Width : (rowBounds.Width - leftMargin + 10), rowBounds.Height);
                Rectangle box = new Rectangle(rowBounds.Left + 5, rowBounds.Top + 1, rowBounds.Height - 3, rowBounds.Height - 3);
                if (listView.Focused && e.Item.Selected)
                {
                    e.Graphics.FillRectangle(SystemBrushes.Highlight, rowBounds);
                    e.Graphics.FillRectangle(new SolidBrush(RGBConverter(e.SubItem.Text)), box);
                    e.Graphics.DrawRectangle(new Pen(SystemBrushes.HighlightText), box);
                    e.Graphics.DrawString(e.SubItem.Text, listView.Font, SystemBrushes.HighlightText, bounds);
                }
                else
                {
                    e.Graphics.FillRectangle(SystemBrushes.Window, rowBounds);
                    e.Graphics.FillRectangle(new SolidBrush(RGBConverter(e.SubItem.Text)), box);
                    e.Graphics.DrawString(e.SubItem.Text, listView.Font, SystemBrushes.WindowText, bounds);
                };
                return;
            };
            if ((e.Item.Text == "TXTWMPOS") || (e.Item.Text == "IMGWMPOS"))
            {
                Rectangle rowBounds = e.SubItem.Bounds;
                Rectangle labelBounds = e.Item.GetBounds(ItemBoundsPortion.Label);
                int leftMargin = labelBounds.Left + 15;
                Rectangle bounds = new Rectangle(rowBounds.Left + leftMargin, rowBounds.Top, e.ColumnIndex == 0 ? labelBounds.Width : (rowBounds.Width - leftMargin + 10), rowBounds.Height);
                Rectangle box = new Rectangle(rowBounds.Left + 5, rowBounds.Top + 1, rowBounds.Height - 3, rowBounds.Height - 3);
                int pos = int.Parse(e.SubItem.Text.Substring(0, 1));
                Rectangle smb = new Rectangle(box.Left + box.Width / 2 - 1, box.Top + box.Height / 2 - 1, 4, 4);
                if ((pos == 8) || (pos == 1) || (pos == 2)) smb.Y = box.Y + 2;
                if ((pos == 2) || (pos == 3) || (pos == 4)) smb.X = box.Right - 5;
                if ((pos == 4) || (pos == 5) || (pos == 6)) smb.Y = box.Bottom - 5;
                if ((pos == 6) || (pos == 7) || (pos == 8)) smb.X = box.Left + 2;
                if (listView.Focused && e.Item.Selected)
                {
                    e.Graphics.FillRectangle(SystemBrushes.Highlight, rowBounds);
                    e.Graphics.DrawRectangle(new Pen(SystemBrushes.HighlightText), box);
                    e.Graphics.FillRectangle(SystemBrushes.HighlightText, smb);
                    e.Graphics.DrawString(e.SubItem.Text, listView.Font, SystemBrushes.HighlightText, bounds);
                }
                else
                {
                    e.Graphics.FillRectangle(SystemBrushes.Window, rowBounds);
                    e.Graphics.DrawRectangle(new Pen(SystemBrushes.WindowText), box);
                    e.Graphics.FillRectangle(SystemBrushes.WindowText, smb);
                    e.Graphics.DrawString(e.SubItem.Text, listView.Font, SystemBrushes.WindowText, bounds);
                };
                return;
            };
            if (e.Item.Text == "TXTWMFONT")
            {
                Rectangle rowBounds = e.SubItem.Bounds;
                Rectangle labelBounds = e.Item.GetBounds(ItemBoundsPortion.Label);
                int leftMargin = labelBounds.Left - 1;
                Rectangle bounds = new Rectangle(rowBounds.Left + leftMargin, rowBounds.Top, e.ColumnIndex == 0 ? labelBounds.Width : (rowBounds.Width - leftMargin - 1), rowBounds.Height);
                Font f = new Font(e.SubItem.Text, listView.Font.Size);
                if (listView.Focused && e.Item.Selected)
                {
                    e.Graphics.FillRectangle(SystemBrushes.Highlight, rowBounds);
                    e.Graphics.DrawString(e.SubItem.Text, f, Brushes.White, bounds);
                }
                else
                {
                    e.Graphics.FillRectangle(SystemBrushes.Window, rowBounds);
                    e.Graphics.DrawString(e.SubItem.Text, f, SystemBrushes.WindowText, bounds);
                };
                return;
            };
            if (e.Item.Text == "TXTWMTEXT")
            {
                Rectangle rowBounds = e.SubItem.Bounds;
                Rectangle labelBounds = e.Item.GetBounds(ItemBoundsPortion.Label);
                int leftMargin = labelBounds.Left - 1;
                Rectangle bounds = new Rectangle(rowBounds.Left + leftMargin, rowBounds.Top, e.ColumnIndex == 0 ? labelBounds.Width : (rowBounds.Width - leftMargin - 1), rowBounds.Height);
                Font f = new Font(todobatch.Items[16].SubItems[3].Text, listView.Font.Size);
                if (listView.Focused && e.Item.Selected)
                {
                    e.Graphics.FillRectangle(SystemBrushes.Highlight, rowBounds);
                    e.Graphics.DrawString(e.SubItem.Text, f, Brushes.White, bounds);
                }
                else
                {
                    e.Graphics.FillRectangle(SystemBrushes.Window, rowBounds);
                    e.Graphics.DrawString(e.SubItem.Text, f, SystemBrushes.WindowText, bounds);
                };
                return;
            };
            if (e.Item.Text == "IMGWMFILE")
            {
                Rectangle rowBounds = e.SubItem.Bounds;
                Rectangle labelBounds = e.Item.GetBounds(ItemBoundsPortion.Label);
                int leftMargin = labelBounds.Left - 1;
                Rectangle bounds = new Rectangle(rowBounds.Left + leftMargin, rowBounds.Top, e.ColumnIndex == 0 ? labelBounds.Width : (rowBounds.Width - leftMargin - 1), rowBounds.Height);
                if (listView.Focused && e.Item.Selected)
                {
                    if (File.Exists(GetFullPathForFileName(e.SubItem.Text)))
                    {
                        e.Graphics.FillRectangle(SystemBrushes.Highlight, rowBounds);
                        e.Graphics.DrawString(e.SubItem.Text, listView.Font, SystemBrushes.HighlightText, bounds);
                    }
                    else
                    {
                        e.Graphics.FillRectangle(Brushes.Red, rowBounds);
                        if (String.IsNullOrEmpty(e.SubItem.Text))
                            e.Graphics.DrawString(e.SubItem.Text, new Font(listView.Font, FontStyle.Bold), Brushes.White, bounds);
                        else
                            e.Graphics.DrawString(e.SubItem.Text + " - File Not Found", new Font(listView.Font, FontStyle.Bold), Brushes.White, bounds);
                    };
                }
                else
                {
                    if (File.Exists(GetFullPathForFileName(e.SubItem.Text)))
                    {
                        e.Graphics.FillRectangle(SystemBrushes.Window, rowBounds);
                        e.Graphics.DrawString(e.SubItem.Text, listView.Font, Brushes.Green, bounds);
                    }
                    else
                    {
                        e.Graphics.FillRectangle(SystemBrushes.Window, rowBounds);
                        if (String.IsNullOrEmpty(e.SubItem.Text))
                            e.Graphics.DrawString(e.SubItem.Text, new Font(listView.Font, FontStyle.Bold), Brushes.Red, bounds);
                        else
                            e.Graphics.DrawString(e.SubItem.Text + " - File Not Found", new Font(listView.Font, FontStyle.Bold), Brushes.Red, bounds);
                    };
                };
                return;
            };
        }

        private void listPARAMS_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;            
        }

        private void todobatch_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void todobatch_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            //e.DrawDefault = true;
            //if ((e.Item.Text == "TXTWMCOLOR") || 
            //    (e.Item.Text == "TXTWMFONT") || 
            //    (e.Item.Text == "TXTWMTEXT") ||
            //    (e.Item.Text == "TXTWMPOS") ||
            //    (e.Item.Text == "IMGWMPOS"))
            //    e.DrawDefault = false;            
        }

        private void listPARAMS_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            if ((e.Item.Text == "KEYWORD") || (e.Item.Text == "JPEGFILE") || (e.Item.Text == "DEFOP"))
                e.DrawDefault = false;
            else
                e.DrawDefault = true;
        }

        private void listFile_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void listFile_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            const int TEXT_OFFSET = 1;

            ListView listView = (ListView)sender;
            if (e.ColumnIndex == 1)
            {
                e.DrawDefault = false;

                Rectangle rowBounds = e.SubItem.Bounds;
                Rectangle labelBounds = e.Item.GetBounds(ItemBoundsPortion.Label);
                int leftMargin = labelBounds.Left - TEXT_OFFSET;
                Rectangle bounds = new Rectangle(rowBounds.Left + leftMargin, rowBounds.Top, e.ColumnIndex == 0 ? labelBounds.Width : (rowBounds.Width - leftMargin - TEXT_OFFSET), rowBounds.Height);

                string text = "";
                if(!String.IsNullOrEmpty(e.SubItem.Text))
                    for (int i = e.SubItem.Text.Length - 1; i >= 0; i--)
                    {                        
                        SizeF sz = e.Graphics.MeasureString(e.SubItem.Text[i] + text, listView.Font);
                        if (sz.Width > bounds.Width) break;
                        text = e.SubItem.Text[i] + text;
                    };

                if (listView.Focused && e.Item.Selected)
                {
                    e.Graphics.FillRectangle(SystemBrushes.Highlight, rowBounds);
                    e.Graphics.DrawString(text, listView.Font, SystemBrushes.HighlightText, bounds);
                }
                else
                {
                    e.Graphics.FillRectangle(SystemBrushes.Window, rowBounds);
                    e.Graphics.DrawString(text, listView.Font, SystemBrushes.WindowText, bounds);
                };
            }
            else if (e.ColumnIndex == 4)
            {
                Rectangle rowBounds = e.SubItem.Bounds;
                Rectangle labelBounds = e.Item.GetBounds(ItemBoundsPortion.Label);
                int leftMargin = labelBounds.Left - TEXT_OFFSET;
                Rectangle bounds = new Rectangle(rowBounds.Left + leftMargin, rowBounds.Top, e.ColumnIndex == 0 ? labelBounds.Width : (rowBounds.Width - leftMargin - TEXT_OFFSET), rowBounds.Height);
                string text = e.SubItem.Text;
                int worki = 0;
                string work = "";
                if (text == "Batch Work")
                {
                    if (todobatch.Items[2].SubItems[3].Text == "Yes") worki++;
                    if (todobatch.Items[4].SubItems[3].Text == "Yes") worki++;
                    if (todobatch.Items[5].SubItems[3].Text != "No") worki++;
                    if (todobatch.Items[12].SubItems[3].Text == "Yes") worki++;
                    if (todobatch.Items[19].SubItems[3].Text == "Yes") worki++;
                    work = ": " + worki.ToString();
                };                
                if (listView.Focused && e.Item.Selected)
                {
                    e.Graphics.FillRectangle(SystemBrushes.Highlight, rowBounds);
                    e.Graphics.DrawString(text, listView.Font, SystemBrushes.HighlightText, bounds);
                    if (work.Length > 0)
                    {
                        SizeF sz = e.Graphics.MeasureString(text, listView.Font);
                        SizeF sy = e.Graphics.MeasureString(work, listView.Font);
                        bounds = new Rectangle(bounds.Left - 4 + (int)sz.Width, bounds.Top, (int)sy.Width + 4, bounds.Height);
                        e.Graphics.DrawString(work, listView.Font, Brushes.Lime, bounds);
                    };
                }
                else
                {
                    e.Graphics.FillRectangle(SystemBrushes.Window, rowBounds);
                    e.Graphics.DrawString(text, listView.Font, SystemBrushes.WindowText, bounds);
                    if (work.Length > 0)
                    {
                        SizeF sz = e.Graphics.MeasureString(text, listView.Font);
                        SizeF sy = e.Graphics.MeasureString(work, listView.Font);
                        bounds = new Rectangle(bounds.Left - 4 + (int)sz.Width, bounds.Top, (int)sy.Width + 4, bounds.Height);
                        e.Graphics.DrawString(work, listView.Font, Brushes.Blue, bounds);
                    };
                };
            }
            else if (e.ColumnIndex == 6)
            {
                e.DrawDefault = false;

                Rectangle rowBounds = e.SubItem.Bounds;
                Rectangle labelBounds = e.Item.GetBounds(ItemBoundsPortion.Label);
                int leftMargin = labelBounds.Left - TEXT_OFFSET;
                Rectangle bounds = new Rectangle(rowBounds.Left + leftMargin, rowBounds.Top, e.ColumnIndex == 0 ? labelBounds.Width : (rowBounds.Width - leftMargin - TEXT_OFFSET), rowBounds.Height);

                if (listView.Focused && e.Item.Selected)
                {
                    Brush bb = SystemBrushes.Highlight;
                    Brush fb = SystemBrushes.HighlightText;
                    if (e.SubItem.Text.IndexOf("Error") >= 0)
                    {
                        bb = Brushes.Red;
                        fb = Brushes.White;
                    };
                    e.Graphics.FillRectangle(bb, rowBounds);
                    e.Graphics.DrawString(e.SubItem.Text, listView.Font, fb, bounds);
                }
                else
                {
                    Brush bb = SystemBrushes.Window;
                    Brush fb = SystemBrushes.WindowText;
                    if (e.SubItem.Text.IndexOf("Error") >= 0)
                    {
                        bb = Brushes.White;
                        fb = Brushes.Red;
                    };
                    e.Graphics.FillRectangle(bb, rowBounds);
                    e.Graphics.DrawString(e.SubItem.Text, listView.Font, fb, bounds);
                };
            }
            else
                e.DrawDefault = true;
        }

        private void todobatch_KeyDown(object sender, KeyEventArgs e)
        {
            if (todobatch.SelectedItems.Count != 1) return;
            int si = todobatch.SelectedIndices[0];

            if ((e.KeyValue == 37) || (e.KeyValue == 39))
            {
                if ((si == 0))
                {
                    int val = int.Parse(todobatch.SelectedItems[0].SubItems[3].Text);
                    if (e.KeyValue == 37) val--;
                    if (e.KeyValue == 39) val++;
                    if (val < 0) val = 0;
                    if (val > 99999999) val = 99999999;
                    todobatch.SelectedItems[0].SubItems[3].Text = val.ToString();
                    return;
                };
                if ((si == 1) || (si == 2) || (si == 4) || (si == 12) || (si == 19))
                {
                    todobatch.SelectedItems[0].SubItems[3].Text = (todobatch.SelectedItems[0].SubItems[3].Text == "No") ? "Yes" : "No";
                    return;
                };
                if ((si == 5))
                {
                    List<string> vals = new List<string>(new string[] { "No", "Resize", "Reduce", "Enlarge" });
                    int index = vals.IndexOf(todobatch.SelectedItems[0].SubItems[3].Text);
                    if (e.KeyValue == 37) index--;
                    if (e.KeyValue == 39) index++;
                    if (index < 0) index = vals.Count - 1;
                    if (index >= vals.Count) index = 0;
                    todobatch.SelectedItems[0].SubItems[3].Text = vals[index];
                    return;
                };
                if ((si == 6) || (si == 7))
                {
                    int val = int.Parse(todobatch.SelectedItems[0].SubItems[3].Text);
                    if (e.KeyValue == 37) val--;
                    if (e.KeyValue == 39) val++;
                    if (val < 8) val = 8;
                    if (val > 9999) val = 9999;
                    todobatch.SelectedItems[0].SubItems[3].Text = val.ToString();
                    return;
                };
                if ((si == 8) || (si == 14) || (si == 21))
                {
                    int val = int.Parse(todobatch.SelectedItems[0].SubItems[3].Text);
                    if (e.KeyValue == 37) val--;
                    if (e.KeyValue == 39) val++;
                    if (val < 10) val = 10;
                    if (val > 100) val = 100;
                    todobatch.SelectedItems[0].SubItems[3].Text = val.ToString();
                    return;
                };
                if ((si == 9))
                {
                    List<string> vals = new List<string>(new string[] { "Source", "Subfolder", "Path" });
                    int index = vals.IndexOf(todobatch.SelectedItems[0].SubItems[3].Text);
                    if (e.KeyValue == 37) index--;
                    if (e.KeyValue == 39) index++;
                    if (index < 0) index = vals.Count - 1;
                    if (index >= vals.Count) index = 0;
                    todobatch.SelectedItems[0].SubItems[3].Text = vals[index];
                    return;
                };
                if ((si == 11))
                {
                    List<string> vals = new List<string>(new string[] { "No", "Lower Extention", "Lower Name", "Lower Full Name", "Upper Extention", "Upper Name", "Upper Full Name" });
                    int index = vals.IndexOf(todobatch.SelectedItems[0].SubItems[3].Text);
                    if (e.KeyValue == 37) index--;
                    if (e.KeyValue == 39) index++;
                    if (index < 0) index = vals.Count - 1;
                    if (index >= vals.Count) index = 0;
                    todobatch.SelectedItems[0].SubItems[3].Text = vals[index];
                    return;
                };
                if ((si == 15) || (si == 22))
                {
                    List<string> vals = new List<string>(new string[] { "0 - Center", "1 - Top Middle", "2 - Top Right", "3 - Right Middle", "4 - Bottom Right", "5 - Bottom Middle", "6 - Bottom Left", "7 - Left Middle", "8 - Top Left" });
                    int index = vals.IndexOf(todobatch.SelectedItems[0].SubItems[3].Text);
                    if (e.KeyValue == 37) index--;
                    if (e.KeyValue == 39) index++;
                    if (index < 0) index = vals.Count - 1;
                    if (index >= vals.Count) index = 0;
                    todobatch.SelectedItems[0].SubItems[3].Text = vals[index];
                    return;
                };
                if ((si == 17))
                {
                    string txt = todobatch.SelectedItems[0].SubItems[3].Text;
                    int val = 0;
                    if (txt.EndsWith("px"))
                    {
                        val = int.Parse(txt.Substring(0, txt.Length - 2));
                        txt = "px";
                    }
                    else
                    {
                        val = int.Parse(txt.Substring(0, txt.Length - 1));
                        txt = "%";
                    };
                    if (e.KeyValue == 37) val--;
                    if (e.KeyValue == 39) val++;
                    if (txt == "%")
                    {
                        if (val < 1) { val = 100; txt = "px"; };
                        if (val > 100) { val = 4; txt = "px"; };
                    }
                    else
                    {
                        if (val < 4) { val = 100; txt = "%"; };
                        if (val > 100) { val = 1; txt = "%"; };
                    };
                    todobatch.SelectedItems[0].SubItems[3].Text = val.ToString() + txt;
                    return;
                };
            };
        }

        private void listPARAMS_KeyDown(object sender, KeyEventArgs e)
        {
            if (listPARAMS.SelectedItems.Count != 1) return;
            int si = listPARAMS.SelectedIndices[0];

            if ((e.KeyValue == 37) || (e.KeyValue == 39))
            {
                if ((si == 4))
                {
                    listPARAMS.SelectedItems[0].SubItems[3].Text = (listPARAMS.SelectedItems[0].SubItems[3].Text == "No") ? "Yes" : "No";
                    return;
                };
                if ((si == 1))
                {
                    List<string> vals = PARAM_OPERATIONS_LIST;
                    int index = vals.IndexOf(listPARAMS.SelectedItems[0].SubItems[3].Text);
                    if (e.KeyValue == 37) index--;
                    if (e.KeyValue == 39) index++;
                    if (index < 0) index = vals.Count - 1;
                    if (index >= vals.Count) index = 0;
                    listPARAMS.SelectedItems[0].SubItems[3].Text = vals[index];
                    return;
                };                
            };
        }       
    }
}