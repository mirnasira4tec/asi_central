using asi.asicentral.interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.services
{
    /// <summary>
    /// Reads file contained within the assembly
    /// </summary>
    public class AssemblyFileService : IFileSystemService
    {
        private Assembly _baseAssembly;

        public AssemblyFileService(Assembly assembly)
        {
            _baseAssembly = assembly;
        }

        public string ReadContent(string fileName)
        {
            string content = string.Empty;

            using (Stream stream = _baseAssembly.GetManifestResourceStream(fileName))
            using (TextReader reader = new StreamReader(stream))
            {
                content = reader.ReadToEnd();
            }
            return content;
        }

        public bool Exists(string fileName)
        {
            return _baseAssembly.GetManifestResourceNames().Contains(fileName);
        }
    }
}
