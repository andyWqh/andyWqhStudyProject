using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace andyWqhModel
{
    /// <summary>
    /// 生成PDF参数实体
    /// </summary>
   public class PDFReportPara
    {
        private string _route;
        private string _docNameZh = "";
        private string _docNameEn = "";
        public string Route
        {
            get { return _route; }
            set { _route = value; }
        }
        public string DocNameZh
        {
            get { return _docNameZh; }
            set { _docNameZh = value; }
        }
        public string DocNameEn
        {
            get { return _docNameEn; }
            set { _docNameEn = value; }
        }
    }
}
