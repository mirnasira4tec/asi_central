using asi.asicentral.util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model
{
    public class FileModel : IComparable
    {
        public string Name { get; set; }
        public string Extension { get; set; }
        public FileType Type { get; set; }
        public string FullPath { get; set; }
        public string RelativePath { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public DateTime Accessed { get; set; }


        public enum FileType
        {
            Folder,
            File
        };

        /// <summary>
        /// Constructor for a folder/directory
        /// </summary>
        /// <param name="dirInfo"></param>
        /// <param name="basePath"></param>
        public FileModel(DirectoryInfo dirInfo, string basePath = null)
        {
            Name = dirInfo.Name;
            FullPath = FileSystemHelper.DecodePath(dirInfo.FullName);
            RelativePath = basePath == null ? FullPath : FullPath.Substring(basePath.Length);
            Type = FileType.Folder;
            Created = dirInfo.CreationTime;
            Modified = dirInfo.LastWriteTime;
            Accessed = dirInfo.LastAccessTime;
        }

        /// <summary>
        /// Constructor for a file
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <param name="basePath"></param>
        public FileModel(FileInfo fileInfo, string basePath = null)
        {
            Name = fileInfo.Name;
            Extension = fileInfo.Extension;
            FullPath = FileSystemHelper.DecodePath(fileInfo.FullName);
            RelativePath = basePath == null ? FullPath : FullPath.Substring(basePath.Length);
            Type = FileType.File;
            Created = fileInfo.CreationTime;
            Modified = fileInfo.LastWriteTime;
            Accessed = fileInfo.LastAccessTime;
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is FileModel)
                return FullPath == ((FileModel)obj).FullPath;
            else
                return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return FullPath.GetHashCode();
        }

        public override string ToString()
        {
            return RelativePath;
        }

        /// <summary>
        /// Allows comparing the objects so the list of files can be sorted
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            if (obj == null) return -1;
            else
            {
                FileModel fileObj = (FileModel) obj;
                if (fileObj.Type == FileType.Folder || Type == FileType.Folder)
                {
                    if (fileObj.Type == Type) return FullPath.CompareTo(fileObj.FullPath);
                    else if (Type == FileType.Folder) return -1;
                    else return 1;
                        
                }
                else
                {
                    return FullPath.CompareTo(fileObj.FullPath);
                }
            }
        }
    }
}
