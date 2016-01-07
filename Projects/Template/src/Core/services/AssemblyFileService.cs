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
        private Assembly[] _baseAssemblies;
        private int _activeAssembly;

        public AssemblyFileService(Assembly[] assemblies)
        {
            _baseAssemblies = assemblies;
        }

        public AssemblyFileService(Assembly assembly)
        {
            _baseAssemblies = new Assembly[] { assembly };
        }

        public virtual string ReadContent(string fileName)
        {
            string content = string.Empty;

            if (_activeAssembly < _baseAssemblies.Length)
            {
                using (Stream stream = _baseAssemblies[_activeAssembly].GetManifestResourceStream(fileName))
                using (TextReader reader = new StreamReader(stream))
                {
                    content = reader.ReadToEnd();
                }
            }
            return content;
        }

        public virtual bool Exists(string fileName)
        {
            _activeAssembly = 0;
            var exist = false;
            for (int i = 0; i < _baseAssemblies.Length; i++)
            {
                exist = _baseAssemblies[i].GetManifestResourceNames().Contains(fileName);
                if( exist )
                {
                    _activeAssembly = i;
                    break;
                }
            }

            return exist;
        }
    }
}
