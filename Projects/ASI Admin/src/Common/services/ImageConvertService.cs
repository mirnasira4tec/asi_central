using System;
using asi.asicentral.interfaces;
using System.Configuration;
using System.Net;
using System.IO;
using System.Threading.Tasks;

namespace asi.asicentral.services
{
    public class ImageConvertService : IImageConvertService
    {
        private ILogService _log { get; set; }

        public ImageConvertService()
        {
            _log = LogService.GetLog(this.GetType());
        }

        public virtual async Task ConvertImageAsync(string inputFilePath, string targetFilePath, bool isBackgroundTransparent = true)
        {
            var imageConvertSrvPath = ConfigurationManager.AppSettings["ImageConversionSrvPath"];
            if (string.IsNullOrEmpty(imageConvertSrvPath) || string.IsNullOrEmpty(inputFilePath) || string.IsNullOrEmpty(targetFilePath))
            {
                var errMsg = string.Format("ImageConvertService.ConvertImage: No image conversion service, or invalid input/target file name. Input: {0}; target: {1}", inputFilePath, targetFilePath);
                throw new InvalidOperationException(errMsg);
            }

            var inputExt = Path.GetExtension(inputFilePath);
            var targetExt = Path.GetExtension(targetFilePath);

            if (string.IsNullOrEmpty(inputExt) || string.IsNullOrEmpty(targetExt) || string.Compare(inputExt, targetExt, StringComparison.CurrentCultureIgnoreCase) == 0)
            {
                throw new InvalidOperationException(string.Format("Invalid or same input/target file extension. Input: {0}; target: {1}", inputFilePath, targetFilePath));
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
                        throw new InvalidOperationException("ImageConvertService ConvertImage failed, url: " + url);
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error("ImageConvertService ConvertImage Exception: " + ex.Message);
                throw ex;
            }
        }
    }
}
