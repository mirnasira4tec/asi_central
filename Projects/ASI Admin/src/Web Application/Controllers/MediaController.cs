using asi.asicentral.util;
using asi.asicentral.web.model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace asi.asicentral.web.Controllers
{
    public class MediaController : Controller
    {
        public ActionResult Index()
        {
            return List();
        }

        public ActionResult List(string path = null)
        {
            MediaFolderModel model = InitMediaFolder();
            model.Path = string.IsNullOrEmpty(path) ? string.Empty : path;
            model.URL = string.IsNullOrEmpty(path) ? model.BaseURL : model.BaseURL + path.Replace("\\", "/");;
            model.Children = FileSystemHelper.GetFiles(model.BasePath + model.Path, model.BasePath);
            return View("List", model);
        }

        private MediaFolderModel InitMediaFolder()
        {
            MediaFolderModel model = new MediaFolderModel();
            model.BasePath = ConfigurationManager.AppSettings["MediaPath"];
            model.BaseURL = ConfigurationManager.AppSettings["MediaURL"];
            if (string.IsNullOrEmpty(model.BasePath) || string.IsNullOrEmpty(model.BaseURL)) throw new Exception("The media properties need to be setup");
            if (!Directory.Exists(model.BasePath)) throw new Exception("The media properties seem to be incorrect");
            return model;
        }
    }
}
