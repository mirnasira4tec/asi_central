using asi.asicentral.model;
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
            MediaFolderModel model = InitMediaFolder();
            model.Path = string.IsNullOrEmpty(path) ? string.Empty : path;
            model.URL = string.IsNullOrEmpty(path) ? model.BaseURL : model.BaseURL + path.Replace("\\", "/");;
            model.Children = FileSystemHelper.GetFiles(model.BasePath + model.Path, model.BasePath);
            return View("List", model);
        }

        //[HttpGet]
        //public virtual ActionResult UploadFiles()
        //{
        //    return View("UploadFiles");
        //}

        [HttpPost]
        public virtual ActionResult Upload(string uploadPath)
        {
            //string uploadPath = ConfigurationManager.AppSettings["UploadPath"];
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
            return RedirectToAction("Index");
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
            }
            else if (System.IO.File.Exists(DirString))
            {
                FileOrDirectorySave(DirString);
            }
        }

        public ActionResult Delete(string file)
        {
            string filePath = ConfigurationManager.AppSettings["MediaPath"] + file;

            if (System.IO.Directory.Exists(filePath))
                System.IO.Directory.Delete(filePath, true);
            else if (System.IO.File.Exists(filePath))
                 System.IO.File.Delete(filePath);
            
            return RedirectToAction("Index");
        }

        private void FileOrDirectorySave(string SaveFileName)
        {
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment; filename=" + SaveFileName);
            Response.WriteFile(SaveFileName);
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
    }
}
