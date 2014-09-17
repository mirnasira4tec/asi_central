using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageResize
{
	class Program
	{
		private static void Main(string[] args)
		{
			var original = "D:\\temp\\images-resize\\original";
			var resized = "D:\\temp\\images-resize\\Media RESIZED\\";
			var destination = "D:\\temp\\images-resize\\New\\";
			var count = WalkDirectoryTree(new DirectoryInfo(original), original, resized, destination);
			Console.WriteLine("process " + count + " files");
			Console.ReadLine();
		}

		static int WalkDirectoryTree(DirectoryInfo root, string original, string resized, string destination)
		{
			var count = 0;
			var files = root.GetFiles("*.jpg");
			foreach (var file in files)
			{
				var modifiedFile = new FileInfo(resized + file.Name);
				if (!file.Name.ToLower().EndsWith("_thumb.jpg"))
				{
					if (file.Exists)
					{
						count++;
						string newFolder = destination + file.DirectoryName.Substring(original.Length + 1);
						DirectoryInfo newFolderInfo = new DirectoryInfo(newFolder);
						if (!newFolderInfo.Exists) newFolderInfo.Create();
						File.Copy(file.FullName, newFolder + "\\" + file.Name, true);
						File.SetLastAccessTimeUtc(newFolder + "\\" + file.Name, DateTime.UtcNow);
						Console.WriteLine("Found: " + newFolder);
					}
					else
					{
						Console.WriteLine("Could not find modified: " + resized + file.Name);
					}
				}
			}

			// Now find all the subdirectories under this directory.
			var subDirs = root.GetDirectories();

			foreach (DirectoryInfo dirInfo in subDirs)
			{
				// Resursive call for each subdirectory.
				count += WalkDirectoryTree(dirInfo, original, resized, destination);
			}
			return count;
		}
	}
}
