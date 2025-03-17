using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Application = System.Windows.Forms.Application;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Reflection;
using Microsoft.Office.Interop.Excel;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using DataTable = System.Data.DataTable;

namespace TcPCM_Connect_Global
{
    public class ExcelSAP
    {
        public string Export(List<string> calcList, string fileLocation)
        {
            Microsoft.Office.Interop.Excel.Application application = null;
            Excel.Workbook workBook = null;

            try
            {


            }
            catch(Exception e)
            {
                return e.Message;
            }
            return null;
        }
    }
}
