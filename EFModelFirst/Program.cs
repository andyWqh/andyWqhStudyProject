using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFModelFirst
{
    class Program
    {
        static void Main(string[] args)
        {
            #region 添加数据
            //1.声明上下文
            ModelFirstContainer dbContext = new ModelFirstContainer();
            //2.对数据库的操作，添加数据
            //2.1 实例化实体，对实体赋值
            User user = new User 
            {
                userName = "Ares",
                realName = "慕容柳生",
                age  =  28,
                telPhone = "18818572279",
                createDate = DateTime.Now
            };
            //2.2 增
            //实体附加到上下文
            dbContext.UserSet.Attach(user);
            //添加到数据库
            dbContext.Entry(user).State = EntityState.Added;
            //3. 保存
            dbContext.SaveChanges();
            #endregion

            #region 查看数据库数据

            //1.Linq 语句
            var userList = from s in dbContext.UserSet
                       select s;

            foreach (var userItem in userList)
            {
                Console.WriteLine("Linq查询Id结果是：" + userItem.userId);
            }

            //方法二、使用lambda查询
            var itemlambda = dbContext.UserSet.Where<User>(m => m.userId == 2).FirstOrDefault();
            //var user = dbContext.UserSet.SingleOrDefault(m => m.userId ==2);
            Console.WriteLine("lambda查询Id结果是：" + itemlambda.userId);
            #endregion
        }
    }
}
