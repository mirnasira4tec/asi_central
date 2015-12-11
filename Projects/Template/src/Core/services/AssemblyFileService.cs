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
        private Assembly _commonAssembly;
        private bool _useCommon;

        public AssemblyFileService(Assembly assembly, Assembly commonAssembly)
        {
            _baseAssembly = assembly;
            _commonAssembly = commonAssembly;
        }

        public AssemblyFileService(Assembly assembly)
        {
            _baseAssembly = assembly;
        }

        public virtual string ReadContent(string fileName)
        {
            string content = string.Empty;

            var assembly = _useCommon ? _commonAssembly : _baseAssembly;

            using (Stream stream = assembly.GetManifestResourceStream(fileName))
            using (TextReader reader = new StreamReader(stream))
            {
                content = reader.ReadToEnd();
            }
            return content;
        }

        public virtual bool Exists(string fileName)
        {
            var exist = _baseAssembly.GetManifestResourceNames().Contains(fileName);
            _useCommon = false;  // need to reset everytime for singleton

            if (!exist && _commonAssembly != null)
            {
                _useCommon = _commonAssembly.GetManifestResourceNames().Contains(fileName);
                exist = _useCommon;
            }

            return exist;
        }
    }
}
