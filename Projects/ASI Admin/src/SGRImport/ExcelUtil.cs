using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGRImport
{
    public class ExcelUtil
    {
        Workbook _workbook = null;
        Worksheet _worksheet = null;

        public ExcelUtil( string fileName)
        {
            Application excel = new Application();
            _workbook = excel.Workbooks.Open(fileName);

            //Identify the worksheet
            if (_workbook.Worksheets.Count == 1)
            {
                _worksheet = _workbook.Worksheets.Item[0] as Worksheet;
            }
            else if (_workbook.Worksheets.Count > 1)
            {
                foreach (Worksheet worksheet in _workbook.Worksheets)
                {
                    if (worksheet.Name.Contains("Product Details"))
                    {
                        _worksheet = worksheet;
                        break;
                    }
                }
            }
            else {
                throw new Exception("Could not find a worksheet");
            }

        }
    }
}
