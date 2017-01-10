using System;
using asi.asicentral.interfaces;
using System.Configuration;
using System.Net;
using System.IO;

namespace asi.asicentral.services
{
    public class ImageConvertService : IImageConvertService
    {
        private ILogService _log { get; set; }

        public ImageConvertService()
        {
            _log = LogService.GetLog(this.GetType());
        }

        public bool ConvertImage(string inputFilePath, string targetFilePath, bool isBackgroundTransparent = true)
        {
            var success = false;
            var imageConvertSrvPath = ConfigurationManager.AppSettings["ImageConversionSrvPath"];
            if (string.IsNullOrEmpty(imageConvertSrvPath) || string.IsNullOrEmpty(inputFilePath) || string.IsNullOrEmpty(targetFilePath))
            {
                _log.Debug(string.Format("No image conversion service, or invalid input/target file name. Input: {0}; target: {1}", inputFilePath, targetFilePath));
                return success;
            }

            var inputExt = Path.GetExtension(inputFilePath);
            var targetExt = Path.GetExtension(targetFilePath);

            if (string.IsNullOrEmpty(inputExt) || string.IsNullOrEmpty(targetExt) || string.Compare(inputExt, targetExt, StringComparison.CurrentCultureIgnoreCase) == 0)
            {
                _log.Debug(string.Format("Invalid or same input/target file extension. Input: {0}; target: {1}", inputFilePath, targetFilePath));
                return success;
            }

            try
            {
                using (var webClient = new WebClient())
                {
                    var transparent = isBackgroundTransparent ? "true" : "false";
                    var url = string.Format("{0}/ConvertEPS.ashx?filePath={1}&targetFileName={2}&setBackgroundTransparent={3}",
                                             imageConvertSrvPath, inputFilePath, targetFilePath, transparent);

                    _log.Debug("Image convert web service request: " + url);
                    var wsResp = webClient.DownloadString(url);
                    if (string.IsNullOrEmpty(wsResp) || !wsResp.Equals("true"))
                    {
                        _log.Error("ImageConvertService ConvertImage failed, url: " + url);
                    }
                    else
                    {
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error("ImageConvertService ConvertImage Exception: " + ex.Message);
            }

            return success;
        }
    }
}
