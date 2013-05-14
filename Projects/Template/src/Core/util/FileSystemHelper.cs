using asi.asicentral.model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.util
{
    public class FileSystemHelper
    {
        /// <summary>
        /// Decodes files path (unc) to consistent nomenclature
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string DecodePath(string path)
        {
            return path != null ? path.Replace("\\", "/") : path;
        }

        public static IList<FileModel> GetFiles(string path, string basePath = null)
        {
            List<FileModel> fileList = new List<FileModel>();
            if (!string.IsNullOrEmpty(path))
            {
                string[] dirs = Directory.GetDirectories(path, "*.*", SearchOption.TopDirectoryOnly);
                foreach (string dir in dirs)
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(dir);
                    if (!dirInfo.Attributes.HasFlag(FileAttributes.Hidden))
                        fileList.Add(new FileModel(dirInfo, basePath));
                }
                string[] files = Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly);
                foreach (string file in files)
                {
                    FileInfo fileInfo = new FileInfo(file);
                    if (!fileInfo.Attributes.HasFlag(FileAttributes.Hidden))
                        fileList.Add(new FileModel(fileInfo, basePath));
                }
                //sort the list
                fileList.Sort();
                if (basePath != null && basePath != path)
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(path + "/..");
                    FileModel up = new FileModel(dirInfo, basePath);
                    up.Name = "..";
                    fileList.Insert(0, up);
                }
            }
            return fileList;
        }
    }
}
