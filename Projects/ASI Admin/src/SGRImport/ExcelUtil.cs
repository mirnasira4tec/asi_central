﻿using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SGRImport
{
    public class ExcelUtil : IDisposable
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

        public void AddWorksheet(string name)
        {
            if (_workbook != null)
            {
                if (_worksheet != null)
                {
                    Marshal.FinalReleaseComObject(_worksheet);
                    _worksheet = null;
                }
                _worksheet = _workbook.Worksheets.Add(Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                _worksheet.Name = name;
            }
            else
            {
                throw new Exception("Need to have a workbook to do this");
            }

        }

        public string[] GetHeaders()
        {
            int colIndex = 1;
            List<string> headers = new List<string>();
            string temp = _worksheet.Cells[1, colIndex].Value;
            while (!string.IsNullOrWhiteSpace(temp))
            {
                headers.Add(temp.Trim());
                colIndex++;
                temp = _worksheet.Cells[1, colIndex].Value;
            }
            return headers.ToArray();
        }

        public string GetValue(int rowIndex, int colIndex)
        {
            string tempValue = null;
            if (_worksheet != null && rowIndex > 0 && colIndex > 0)
            {
                object cellValue = _worksheet.Cells[rowIndex, colIndex].Value;
                if (cellValue != null) tempValue = cellValue.ToString();
                if (string.IsNullOrEmpty(tempValue)) tempValue = null;
                else tempValue = tempValue.Trim();
            }
            return tempValue;
        }

        public void SetValue(int rowIndex, int colIndex, object value)
        {
            if (_worksheet != null && rowIndex > 0 && colIndex > 0)
            {
                _worksheet.Cells[rowIndex, colIndex].Value = value;
            }
            else
            {
                throw new Exception("Invalid call to this method");
            }
        }

        public void Dispose()
        {
            try
            {
                if (_worksheet != null) Marshal.FinalReleaseComObject(_worksheet);
                if (_workbook != null)
                {
                    Application excel = _workbook.Application;
                    _workbook.Close(true, Type.Missing, Type.Missing); //save the changes
                    Marshal.FinalReleaseComObject(_workbook);
                    _workbook = null;
                    excel.Quit();
                    Marshal.FinalReleaseComObject(excel);
                    excel = null;
                }
            }
            catch (Exception)
            {
                //nothing to do
            }
        }
    }
}
