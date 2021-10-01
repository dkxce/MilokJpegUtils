using System;
using System.Collections.Generic;
using System.Windows.Forms;

// info EXIF
//
// https://sno.phy.queensu.ca/~phil/exiftool/TagNames/EXIF.html#Flash
// https://www.media.mit.edu/pia/Research/deepview/exif.html
//

namespace CryptoJPEG
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string im = MainJPEGForm.CD + @"\Magick.NET-Q8-AnyCPU.dll";
            if (!System.IO.File.Exists(im))
            {
                System.IO.FileStream fs = new System.IO.FileStream(im, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                fs.Write(global::JPEGUtils.Properties.Resources.Magick_NET_Q8_AnyCPU, 0, global::JPEGUtils.Properties.Resources.Magick_NET_Q8_AnyCPU.Length);
                fs.Close();
            };

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainJPEGForm());
        }
    }
}