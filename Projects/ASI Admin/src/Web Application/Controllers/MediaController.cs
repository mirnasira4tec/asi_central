using asi.asicentral.model;
using asi.asicentral.services;
using asi.asicentral.util;
using asi.asicentral.web.model;
using Ionic.Zip;
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
            LogService log = LogService.GetLog(this.GetType());
            try
            {
                MediaFolderModel model = InitMediaFolder();
                model.Path = string.IsNullOrEmpty(path) ? string.Empty : path;
                model.URL = string.IsNullOrEmpty(path) ? model.BaseURL : model.BaseURL + path.Replace("\\", "/");
                log.Debug(string.Format("Accessing file system '{0}' using base folder '{1}'", model.BasePath + model.Path, model.Path));
                model.Children = FileSystemHelper.GetFiles(model.BasePath + model.Path, model.BasePath);
                return View("List", model);
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw e;
            }
        }

        [HttpPost]
        public virtual ActionResult Upload(string uploadPath)
        {
            string refreshPath = uploadPath;
            string msg = string.Empty;
            uploadPath = ConfigurationManager.AppSettings["MediaPath"] + uploadPath;

            IList<HttpPostedFileBase> files = Request.Files.GetMultiple("files");
            foreach (HttpPostedFileBase file in files)
            {
                 if (file.ContentLength > 0)
                 {
                    var fileName = Path.GetFileName(file.FileName);
                    if (!System.IO.Directory.Exists(uploadPath))
                        System.IO.Directory.CreateDirectory(uploadPath);

                     var path = Path.Combine(uploadPath, fileName);
                    file.SaveAs(path);
                 }
            }
            return new RedirectResult(string.Format("/Media/List?path={0}", refreshPath));
        }

        public void Download(string file)
        {
            string basePath = ConfigurationManager.AppSettings["MediaPath"];
            string DirString = basePath + file;
            string SaveFileName = string.Empty;
            if(file == string.Empty)
                SaveFileName = string.Format("{0}.zip", basePath);
            else
                SaveFileName = string.Format("{0}\\{1}.zip", basePath, file.Substring(1));
            string devDirString = DirString.Replace("/Store", string.Empty);
            if (System.IO.Directory.Exists(DirString))
            {
                using (ZipFile zip = new ZipFile())
                {
                    zip.AddDirectory(DirString);
                    zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
                    zip.Comment = "This zip was created at " + System.DateTime.Now.ToString("G");
                    zip.Save(SaveFileName);

                    FileOrDirectorySave(SaveFileName);
                    if (System.IO.File.Exists(SaveFileName))
                        System.IO.File.Delete(SaveFileName);
                }
            } else if (System.IO.File.Exists(DirString)) { FileOrDirectorySave(DirString); }
            else if (System.IO.File.Exists(devDirString)) { FileOrDirectorySave(devDirString); }
        }

        [HttpPost]
        public ActionResult Delete(string file)
        {
            string filePath = ConfigurationManager.AppSettings["MediaPath"] + file;

            if (System.IO.Directory.Exists(filePath))
                System.IO.Directory.Delete(filePath, true);
            else if (System.IO.File.Exists(filePath))
                 System.IO.File.Delete(filePath);
            
            return new RedirectResult(string.Format("/Media/List?path={0}", GetUrlToSendToList(file)));
        }

        [HttpPost]
        public virtual ActionResult CreateDirectory(string directoryPath)
        {
            string directoryName = String.Format("{0}", Request.Form["dirName"]);

            string refreshPath = directoryPath;
            directoryPath = ConfigurationManager.AppSettings["MediaPath"] + directoryPath + @"\" + directoryName;

            if (!System.IO.Directory.Exists(directoryPath))
                System.IO.Directory.CreateDirectory(directoryPath);

            return new RedirectResult(string.Format("/Media/List?path={0}", refreshPath));
        }

        private void FileOrDirectorySave(string SavedPath)
        {
            string SaveFileName = SavedPath.Replace("\\", "/");
            SaveFileName = SaveFileName.Substring(SaveFileName.LastIndexOf('/') + 1);
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment; filename=" + SaveFileName);
            Response.WriteFile(SavedPath);
            Response.ContentType = "";
            Response.End();
        }

        private MediaFolderModel InitMediaFolder()
        {
            MediaFolderModel model = new MediaFolderModel();
            model.BasePath = ConfigurationManager.AppSettings["MediaPath"];
            if (string.IsNullOrEmpty(model.BasePath)) throw new Exception("The media properties need to be setup");
            if (!Directory.Exists(model.BasePath)) throw new Exception("The media properties seem to be incorrect");
            return model;
        }

        private string GetUrlToSendToList(string path)
        {
            string url = string.Empty;
            if (path != null && path != string.Empty)
                url = path.Substring(0, path.LastIndexOf('/'));
            return url;
        }
    }
}
