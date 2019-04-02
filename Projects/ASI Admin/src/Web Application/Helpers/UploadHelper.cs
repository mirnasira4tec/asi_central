using asi.asicentral.model.asicentral;
using asi.asicentral.model.store;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace asi.asicentral.web.Helpers
{
    public class UploadHelper
    {
        public static IXLWorksheets GetExcelSheets(HttpPostedFileBase file)
        {
            IXLWorksheets sheets = null;
            if (file != null)
            {
                var fileName = Path.GetFileName(file.FileName);
                
                string fileExtension = Path.GetExtension(fileName);
                
                if (fileExtension == ".xls" || fileExtension == ".xlsx")
                {
                    using (XLWorkbook workBook = new XLWorkbook(file.InputStream))
                    {
                        sheets = workBook.Worksheets;
                    }
                }
            }
             return sheets;
        }
        
    }
}