using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace EFDBFirst
{
    class Program
    {

        static AspNetEntities dbContext = new AspNetEntities();
        /// <summary>
        /// DBFirst优先
        /// Deleted 标记实体已删除
        /// Detached 实体未附加，也未处于被跟踪状态
        /// Modified 实体已修改更新状态
        /// New 以新增状态附加实体
        /// Unmodified 实体处于原始状态，即未修改状态
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            string msg = string.Empty;
            //新增用户信息
            UserInfo userInfo = new UserInfo()
            {
                UserName = "慕容柳生",
                RealName = "andyWqh",
                Age = 28,
                Email = "andyWqh@163.com",
                Sex = true,
                Phone = "23450343",
                Mobile = "18818572279"
            };
            // msg =  AddEntity(userInfo);

            //根据userId编辑用户信息
            //msg = UpdateEntityById(3);
            msg = DeleteEntityByUserId(2);
            //根据userId删除指定用户
            Console.WriteLine(msg);
            Console.ReadKey();
        }

        /// <summary>
        /// 新增用户信息
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        private static string AddEntity(UserInfo userInfo)
        {
            string msg = string.Empty;
            if (userInfo == null)
            {
                msg = "新增的用户实体不能为空!";
            }
            else
            {
                try
                {
                    dbContext.UserInfo.Add(userInfo);
                    dbContext.Entry(userInfo).State = EntityState.Added;
                    dbContext.SaveChanges();
                    msg = "新增成功!";
                }
                catch (Exception)
                {
                    msg = "数据异常，请联系管理员!";
                    throw;
                }
            }
            return msg;
        }

        /// <summary>
        /// 根据userId删除用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private static string DeleteEntityByUserId(int userId)
        {
            string msg = string.Empty;
            if (userId <= 0)
            {
                msg = "用户ID不能小于0";
            }
            else
            {
                try
                {
                    UserInfo userInfo = dbContext.UserInfo.FirstOrDefault(m => m.UserID == userId);
                    if (userInfo != null)
                    {
                        dbContext.UserInfo.Attach(userInfo);
                        dbContext.Entry(userInfo).State = EntityState.Deleted;
                        dbContext.SaveChanges();
                        msg = "删除成功!";
                    }
                    else
                    {
                        msg = "系统不存在该userId对应的用户";
                    }
                }
                catch (Exception)
                {
                    msg = "数据异常，请联系管理员!";
                    throw;
                }
            }

            return msg;
        }

        /// <summary>
        /// 根据userId修改信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private static string UpdateEntityById(int userId)
        {
            string msg = string.Empty;
            if (userId <= 0)
            {
                msg = "用户ID不能小于0";
            }
            else
            {
                try
                {
                    //根据userId查询用户信息
                    UserInfo userInfo = dbContext.UserInfo.FirstOrDefault(m => m.UserID == userId);
                    if (userInfo != null)
                    {
                        userInfo.Mobile = "15270020554";
                        userInfo.Phone = "3838438";
                        userInfo.RealName = "andyWqh";
                        userInfo.Email = "andyWqh@163.com";
                        //实体附加到上下文并进行操作
                        dbContext.UserInfo.Attach(userInfo);
                        dbContext.Entry(userInfo).State = EntityState.Modified;//表示实体被修改更新
                        //2.3调用SaveChange保存
                        dbContext.SaveChanges();//最后调用SaveChanges（）将所做的操作保存在基础数据库中
                        msg = "修改成功!";
                    }
                    else
                    {
                        msg = "系统不存在该userId对应的用户";
                    }
                }
                catch (Exception)
                {
                    msg = "数据异常，请联系管理员!";
                    throw;
                }
            }
            return msg;
        }
    }
}
