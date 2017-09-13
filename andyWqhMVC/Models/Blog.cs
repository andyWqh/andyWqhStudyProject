using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace andyWqhMVC.Models
{
    /// <summary>
    /// 博客实体
    /// </summary>
    public class Blog
    {
        public int BlogId { get; set; }

        public string Title { get; set; }

        public DateTime CreateTime { get; set; }
    }
}