using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelpClassLib.Web
{
    /// <summary>
    /// EasyUI 行内搜索类 SearchOpt.
    /// </summary>
    public class SearchOpt
    {
        /// <summary>
        /// Gets or sets the field.
        /// </summary>
        /// <value>The field.</value>
        public string field { get; set; }
        /// <summary>
        /// Gets or sets the type.
        /// type : label,text,textarea,checkbox,numberbox,validatebox,datebox,combobox,combotree
        /// </summary>
        /// <value>The type.</value>
        public string type { get; set; }
        /// <summary>
        /// Gets or sets the options.
        /// options:{precision:1}
        /// </summary>
        /// <value>The options.</value>
        public string options { get; set; }
        /// <summary>
        /// Gets or sets the op.
        /// op  :  contains,equal,notequal,beginwith,endwith,less,lessorequal,greater,greaterorequal
        /// </summary>
        /// <value>The op.</value>
        public string op { get; set; }

        /// <summary>
        /// 查询参数值
        /// </summary>
        public string value { get; set; }
    }

    /// <summary>
    /// 业务类型 (easyUI 在列分类时用到)
    /// </summary>

    public class BusinessType
    {
        //业务类型对应的列数
        public int ColumnCount { get; set; }
        /// <summary>
        /// Gets or sets the b type identifier.
        /// 业务类型ID 0航班信息，1保障进度，2起飞信息,3到达信息
        /// </summary>
        /// <value>The b type identifier.</value>
        public int BTypeId { get; set; }
    }

    public enum FlightBusinessName
    {
        航班信息 = 0,
        起飞信息 = 1,
        保障信息 = 2,
        降落信息 = 3
    }

    /// <summary>
    /// Class DataGrid.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataGrid<T>
    {
        public int total { get; set; }

        public List<T> rows { get; set; }
    }

    /// <summary>
    ///  航班类目类型
    /// </summary>
    public class CategoryType
    {
        //业务类型对应的列数
        public int ColumnCount { get; set; }
        /// <summary>
        /// Gets or sets the b type identifier.
        /// 业务类型ID 0，飞机信息 1,进港航班信息 2，进港航班保障 3,出港航班保障 4，出港航班信息
        /// </summary>
        /// <value>The b type identifier.</value>
        public int BTypeId { get; set; }
    }

    public enum FlightCategoryType
    {
        飞机信息 = 0,
        进港航班信息 = 1,
        进港航班保障 = 2,
        出港航班保障 = 3,
        出港航班信息 = 4

    }
}
