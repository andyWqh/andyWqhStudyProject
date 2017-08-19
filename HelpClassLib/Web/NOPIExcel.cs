using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.HSSF.UserModel;

namespace HelpClassLib.Web
{
    public class NOPIExcel
    {
         static HSSFWorkbook hssworkbook;

        #region import DataTable  
            
        public static DataTable ImportExcelFile(string filePath)
        {
            try
            {
                using(FileStream file = new FileStream(filePath,FileMode.Open,FileAccess.Read))
                {
                    hssworkbook = new HSSFWorkbook(file);
                }
            }
            catch (Exception )
            {
               
            }
            NPOI.SS.UserModel.ISheet sheet = hssworkbook.GetSheetAt(0);
            IEnumerator rows = sheet.GetRowEnumerator();
            DataTable dt = new DataTable();
            for (int i = 0; i < sheet.GetRow(0).LastCellNum; i++)
            {
                dt.Columns.Add(Convert.ToChar((int)'A' + i) + "");
            }
            while (rows.MoveNext())
            {
                HSSFRow row = (HSSFRow)rows.Current;
                DataRow dr = dt.NewRow();
                for (int j = 0; j < row.LastCellNum; j++)
                {
                    NPOI.SS.UserModel.ICell cell = row.GetCell(j);
                    if (cell == null)
                    {
                        dr[j] = null;
                    }
                    else
                    {
                        dr[j] = cell + "";
                    }
                }
                dt.Rows.Add(dr);
            }

            return dt;
        }

        #endregion

        #region ExportExcel
        
        public static void ExportExcel(DataTable dt,string filePath)
        {
            if (!string.IsNullOrEmpty(filePath) && dt != null && dt.Rows.Count > 0)
            {
                NPOI.HSSF.UserModel.HSSFWorkbook book = new HSSFWorkbook();
                NPOI.SS.UserModel.ISheet sheet = book.CreateSheet(dt.TableName);
                NPOI.SS.UserModel.IRow row = sheet.CreateRow(0);
                //循环读取DataTable列名称
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    row.CreateCell(i).SetCellValue(dt.Columns[i].ColumnName);
                }
                for (int i= 0; i < dt.Rows.Count; i++)
                {
                    NPOI.SS.UserModel.IRow row2 = sheet.CreateRow(i + 1);
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        row2.CreateCell(j).SetCellValue(Convert.ToString(dt.Rows[i][j]));
                    }
                }
                //写到客户端
                using (MemoryStream ms = new MemoryStream())
                {
                    book.Write(ms);
                    using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        byte[] data = ms.ToArray();
                        fs.Write(data, 0, data.Length);
                        fs.Flush();
                    }
                    book = null;
                }
            }
        }
        #endregion
    }
}
