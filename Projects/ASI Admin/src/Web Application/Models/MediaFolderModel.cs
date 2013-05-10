using asi.asicentral.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace asi.asicentral.web.model
{
    public class MediaFolderModel
    {
        public string BasePath { get; set; }
        public string BaseURL { get; set; }
        public string Path { get; set; }
        public string URL { get; set; }
        public IList<FileModel> Children { get; set; }
    }
}