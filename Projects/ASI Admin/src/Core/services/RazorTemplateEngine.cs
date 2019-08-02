using asi.asicentral.interfaces;
using RazorEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.services
{
    public class RazorTemplateEngine : ITemplateService
    {
        private static object _syncLock = new object(); //created to enforce single thread for Razor engine
        IFileSystemService _fileService;

        public RazorTemplateEngine(IFileSystemService fileService)
        {
            _fileService = fileService;
        }

        public virtual string Render<T>(string templateName, T model)
        {
            lock (_syncLock)
            {
                return Razor.Parse<T>(GetTemplateContent(templateName), model);
            }
        }

        public virtual string Render(string templateName, dynamic model)
        {
            lock (_syncLock)
            {
                return Razor.Parse(GetTemplateContent(templateName), model);
            }
        }

        private string GetTemplateContent(string templateName)
        {
            if (_fileService.Exists(templateName))
            {
                return _fileService.ReadContent(templateName);
            }
            else throw new Exception("Could not find the template '" + templateName + "'");
        }
    }
}
