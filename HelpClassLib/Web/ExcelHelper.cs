using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace HelpClassLib.Web
{

    /*
       1、整个Excel表格叫做工作表：WorkBook（工作薄），包含的叫页（工作表）：Sheet；行：Row；单元格Cell。
        2、NPOI是POI的C#版本，NPOI的行和列的index都是从0开始
        3、POI读取Excel有两种格式一个是HSSF，另一个是XSSF。 HSSF和XSSF的区别如下：
                HSSF is the POI Project's pure Java implementation of the Excel '97(-2007) file format.
                XSSF is the POI Project's pure Java implementation of the Excel 2007 OOXML (.xlsx) file format.
        即：HSSF适用2007以前的版本,XSSF适用2007版本及其以上的。
     */
    public class ExcelHelper:IDisposable
    {
        private string fileName = null; //文件名
        private IWorkbook workbook = null;
        private FileStream fs = null;
        private bool disposed;

        public ExcelHelper(string fileName)
        {
            this.fileName = fileName;
            disposed = false;
        }

        /// <summary>
        /// 将DataTable数据导入到excel中
        /// </summary>
        /// <param name="data">要导入的数据</param>
        /// <param name="isColumnWritten">DataTable的列名是否要导入</param>
        /// <param name="sheetName">要导入的excel的sheet的名称</param>
        /// <returns>导入数据行数(包含列名那一行)</returns>
        public int DataTableToExcel(DataTable data, string sheetName, bool isColumnWritten)
        {
            int i = 0;
            int j = 0;
            int count = 0;
            ISheet sheet = null;

            fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            if (fileName.IndexOf(".xlsx") > 0) // 2007版本
                workbook = new XSSFWorkbook();
            else if (fileName.IndexOf(".xls") > 0) // 2003版本
                workbook = new HSSFWorkbook();

            try
            {
                if (workbook != null)
                {
                    sheet = workbook.CreateSheet(sheetName);
                }
                else
                {
                    return -1;
                }

                if (isColumnWritten == true) //写入DataTable的列名
                {
                    IRow row = sheet.CreateRow(0);
                    for (j = 0; j < data.Columns.Count; ++j)
                    {
                        row.CreateCell(j).SetCellValue(data.Columns[j].ColumnName);
                    }
                    count = 1;
                }
                else
                {
                    count = 0;
                }

                for (i = 0; i < data.Rows.Count; ++i)
                {
                    IRow row = sheet.CreateRow(count);
                    for (j = 0; j < data.Columns.Count; ++j)
                    {
                        row.CreateCell(j).SetCellValue(data.Rows[i][j].ToString());
                    }
                    ++count;
                }
                workbook.Write(fs); //写入到excel
                return count;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 将excel中的数据导入到DataTable中
        /// </summary>
        /// <param name="sheetName">excel工作薄sheet的名称</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名</param>
        /// <returns>返回的DataTable</returns>
        public DataTable ExcelToDataTable(string sheetName, bool isFirstRowColumn)
        {
            ISheet sheet = null;
            DataTable data = new DataTable();
            int startRow = 0;
            try
            {
                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                if (fileName.IndexOf(".xlsx") > 0) // 2007版本
                    workbook = new XSSFWorkbook(fs);
                else if (fileName.IndexOf(".xls") > 0) // 2003版本
                    workbook = new HSSFWorkbook(fs);

                if (sheetName != null)
                {
                    sheet = workbook.GetSheet(sheetName);
                    if (sheet == null) //如果没有找到指定的sheetName对应的sheet，则尝试获取第一个sheet
                    {
                        sheet = workbook.GetSheetAt(0);
                    }
                }
                else
                {
                    sheet = workbook.GetSheetAt(0);
                }
                if (sheet != null)
                {
                    IRow firstRow = sheet.GetRow(0);
                    int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数

                    if (isFirstRowColumn)
                    {
                        for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                        {
                            ICell cell = firstRow.GetCell(i);
                            if (cell != null)
                            {
                                string cellValue = cell.StringCellValue;
                                if (cellValue != null)
                                {
                                    DataColumn column = new DataColumn(cellValue);
                                    data.Columns.Add(column);
                                }
                            }
                        }
                        startRow = sheet.FirstRowNum + 1;
                    }
                    else
                    {
                        startRow = sheet.FirstRowNum;
                    }

                    //最后一列的标号
                    int rowCount = sheet.LastRowNum;
                    for (int i = startRow; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue; //没有数据的行默认是null　　　　　　　

                        DataRow dataRow = data.NewRow();
                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                        {
                            if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                                dataRow[j] = row.GetCell(j).ToString();
                        }
                        data.Rows.Add(dataRow);
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (fs != null)
                        fs.Close();
                }

                fs = null;
                disposed = true;
            }
        }

        protected static void GridViewExportExcel(string fileName,GridView gv)
        {
           HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader(
               "context-disposition",string.Format("attachment;filename={0}",HttpUtility.UrlEncode(fileName,Encoding.UTF8)));
            HttpContext.Current.Response.ContentType = "application/ms-excel";
            using(StringWriter sw = new StringWriter())
            {
                using(HtmlTextWriter htw = new HtmlTextWriter(sw))
                {
                    Table table = new Table();
                    table.GridLines = GridLines.Both;
                    if(gv.HeaderRow != null)
                    {
                        PrepareControlForExport(gv.HeaderRow);
                        table.Rows.Add(gv.HeaderRow);
                    }

                    foreach(GridViewRow row in gv.Rows)
                    {
                        PrepareControlForExport(row);
                    }
                    if(gv.FooterRow != null)
                    {
                        PrepareControlForExport(gv.FooterRow);
                        table.Rows.Add(gv.FooterRow);
                    }

                    table.RenderControl(htw);
                    HttpContext.Current.Response.Write(sw.ToString());
                    HttpContext.Current.Response.End();
                }
            }
        }

        private static void PrepareControlForExport(Control ctr)
        {
            for (int i = 0; i < ctr.Controls.Count ; i++)
            {
                Control current = ctr.Controls[i];
                if(current is LinkButton)
                {
                    ctr.Controls.Remove(current);
                    ctr.Controls.AddAt(i,new LiteralControl((current as LinkButton).Text));
                }
                else if(current is ImageButton)
                {
                    ctr.Controls.Remove(current);
                    ctr.Controls.AddAt(i,new LiteralControl((current as ImageButton).AlternateText));
                }
                else if(current is HyperLink)
                {
                    ctr.Controls.Remove(current);
                    ctr.Controls.AddAt(i,new LiteralControl((current as HyperLink).Text));
                }
                else if(current is CheckBox)
                {
                    ctr.Controls.Remove(current);
                    ctr.Controls.AddAt(i,new LiteralControl((current as CheckBox).Checked?"True":"Flase"));
                }
                else if(current is DropDownList)
                {
                    ctr.Controls.Remove(current);
                    ctr.Controls.AddAt(i,new LiteralControl((current as DropDownList).SelectedItem.Text));
                }

                if(current.HasControls())
                {
                    PrepareControlForExport(current);
                }
            }
        }
    }
}
