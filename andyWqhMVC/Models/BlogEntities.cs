using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace andyWqhMVC.Models
{
    public class BlogEntities:DbContext
    {
        public DbSet<Blog> BlogList { get; set; }
    }
}