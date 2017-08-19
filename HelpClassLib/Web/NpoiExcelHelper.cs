using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;

namespace HelpClassLib.Web
{
    /// <summary>
    /// npoi导入excel工具类
    /// </summary>
    public static class NpoiExcelHelper
    {
        public static IFont GetFontStyle(HSSFWorkbook workBook)
        {
            return GetFontStyle(workBook, false);
        }

        public static IFont GetFontStyle(HSSFWorkbook workBook, bool bold)
        {
            return GetFontStyle(workBook, bold, string.Empty);
        }

        public static IFont GetFontStyle(HSSFWorkbook workBook, string fontFamily)
        {
            return GetFontStyle(workBook, fontFamily, false);
        }

        public static IFont GetFontStyle(HSSFWorkbook workBook, bool bold, string fontColor)
        {
            return GetFontStyle(workBook, string.Empty, bold, fontColor);
        }

        public static IFont GetFontStyle(HSSFWorkbook workBook, string fontFamily, bool bold)
        {
            return GetFontStyle(workBook, fontFamily, string.Empty, bold, string.Empty);
        }

        public static IFont GetFontStyle(HSSFWorkbook workBook, string fontFamily, bool bold, string fontColor)
        {
            return GetFontStyle(workBook, fontFamily, string.Empty, bold, fontColor);
        }

        public static IFont GetFontStyle(HSSFWorkbook workBook, string fontFamily, string fontSize, bool bold, string fontColor)
        {
            IFont font = workBook.CreateFont();

            if (!string.IsNullOrEmpty(fontFamily))
            {
                font.FontName = fontFamily;
            }

            if (!string.IsNullOrEmpty(fontSize))
            {
                font.FontHeightInPoints = short.Parse(fontSize);
            }

            if (bold)
            {
                font.Boldweight = (short)FontBoldWeight.Bold;
            }

            if (!string.IsNullOrEmpty(fontColor))
            {
                short result;
                if (short.TryParse(fontColor, out result))
                {
                    font.Color = result;
                }
            }

            return font;
        }


        public static ICellStyle GetCellStyle(HSSFWorkbook workbook)
        {
            return GetCellStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center);
        }

        public static ICellStyle GetCellStyle(HSSFWorkbook workbook, bool border = false, bool warpText = false)
        {
            return GetCellStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, border, warpText);
        }

        public static ICellStyle GetCellStyle(HSSFWorkbook workbook, HorizontalAlignment ha, VerticalAlignment va)
        {
            return GetCellStyle(workbook, ha, va, border: false, warpText: false);
        }

        public static ICellStyle GetCellStyle(HSSFWorkbook workbook, HorizontalAlignment ha, VerticalAlignment va, bool border, bool warpText)
        {
            ICellStyle style = workbook.CreateCellStyle();
            style.Alignment = ha;
            style.VerticalAlignment = va;

            if (border)
            {
                style.BorderTop = BorderStyle.Thin;
                style.BorderRight = BorderStyle.Thin;
                style.BorderBottom = BorderStyle.Thin;
                style.BorderLeft = BorderStyle.Thin;
            }

            style.WrapText = warpText;

            return style;
        }


        public static void GetMergedRegion(ISheet sheet, int rowStart, int rowEnd, int colStart, int colEnd)
        {
            CellRangeAddress region = new CellRangeAddress(rowStart, rowEnd, colStart, colEnd);

            sheet.AddMergedRegion(region);
        }


        public static string ToStr(ICell cell)
        {
            if (cell == null)
            {
                return string.Empty;
            }

            return CellTypeToValue(cell);
        }

        public static int ToInt(ICell cell)
        {
            if (cell == null)
            {
                return default(int);
            }

            string result = CellTypeToValue(cell);

            if (string.IsNullOrWhiteSpace(result))
            {
                return default(int);
            }

            return int.Parse(result);
        }

        public static DateTime? ToDateTime(ICell cell)
        {
            if (cell == null)
            {
                return null;
            }

            string result = CellTypeToValue(cell);

            if (string.IsNullOrWhiteSpace(result))
            {
                return null;
            }

            return DateTime.Parse(result);
        }

        private static string CellTypeToValue(ICell cell)
        {
            if (cell == null)
            {
                return string.Empty;
            }

            switch (cell.CellType)
            {
                case CellType.Numeric:
                    {
                        return cell.NumericCellValue.ToString();
                    }
                case CellType.Formula:
                    {
                        return cell.StringCellValue.ToString();
                    }
                case CellType.String:
                    {
                        return cell.StringCellValue.ToString();
                    }
                case CellType.Blank:
                    {
                        return string.Empty;
                    }
                default:
                    {
                        return string.Empty;
                    }
            }
        }


        /// <summary>
        /// 导出到 Excel
        /// 默认水平和垂直都是居中
        /// </summary>
        /// <param name="sheetName">sheetName</param>
        /// <param name="header">表头</param>
        /// <param name="content">内容</param>
        /// <param name="ha">水平</param>
        /// <param name="va">垂直</param>
        /// <param name="border">是否加边框</param>
        /// <param name="warpText">是否启用换行</param>
        /// <returns></returns>
        public static HSSFWorkbook ImportExcel(string sheetName, List<string> header, List<List<string>> content, HorizontalAlignment ha = HorizontalAlignment.Center, VerticalAlignment va = VerticalAlignment.Center, bool border = false, bool warpText = false)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet(sheetName);

            ICellStyle style = GetCellStyle(workbook, ha: ha, va: va, border: border, warpText: warpText);

            int rowNumber = 0;
            IRow rowHead = sheet.CreateRow(rowNumber);
            for (int i = 0; i < header.Count(); i++)
            {
                rowHead.CreateCell(i, CellType.String).SetCellValue(header[i]);
                rowHead.GetCell(i).CellStyle = style;
            }

            foreach (List<string> item in content)
            {
                ++rowNumber;
                IRow row = sheet.CreateRow(rowNumber);
                for (int i = 0; i < item.Count(); i++)
                {
                    row.CreateCell(i, CellType.String).SetCellValue(item[i]);
                    row.GetCell(i).CellStyle = style;
                }
            }

            return workbook;
        }
    }
}
