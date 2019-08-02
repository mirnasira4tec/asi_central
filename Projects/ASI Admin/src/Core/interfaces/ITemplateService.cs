using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.interfaces
{
    public interface ITemplateService
    {
        string Render<T>(string templateName, T model);
        string Render(string templateName, dynamic model);
    }
}
