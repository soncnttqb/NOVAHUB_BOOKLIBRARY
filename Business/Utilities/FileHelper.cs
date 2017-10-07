using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Business.Utilities
{
    public static class FileHelper
    {
        public static string TempFolderPath { get { return ConfigurationManager.AppSettings[Constants.ConfigKey.TempFolder].ToString(); } }
        public static string FilterExtension { get { return ConfigurationManager.AppSettings[Constants.ConfigKey.FilterExtension].ToString(); } }
        public static bool IsValidExtensionFile(string extension)
        {
            List<string> listExtension = ConfigurationManager.AppSettings[Constants.ConfigKey.Extensions].Split(new char[] { '|' }).Select(x => x.ToUpper()).ToList();
            return listExtension.Contains(extension.ToUpper());
        }
        public static void CreateFolderIfNotExist(string folderPath)
        {
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
        }
        public static void DeleteFile(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
        /// <summary>
        /// Copy image from bytes to filePath
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="filePath"></param>
        public static void CopyImage(byte[] bytes, string filePath)
        {
            if (bytes == null) return;

            DeleteFile(filePath);

            using (FileStream _fileStream = new FileStream(filePath, System.IO.FileMode.Create, System.IO.FileAccess.Write))
            {
                _fileStream.Write(bytes, 0, bytes.Length);
            }
        }

        public static void LoadImage(string filePath, PictureBox pictureBox)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                pictureBox.Image = Image.FromStream(fs);
            }
        }

        public static byte[] GetByteFromFile(string filePath)
        {
            byte[] bytes;
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                bytes = new byte[fs.Length];
                fs.Read(bytes, 0, System.Convert.ToInt32(fs.Length));
            }
            return bytes;
        }
    }
}
