using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using andyWqhModel;
using HelpClassLib;
using Oracle.DataAccess.Client;
using System.Data;
using Dapper;

namespace andyWqhDAL
{
    public class DapperDAL
    {

        public CICUser GetUserInfo(CICUser userInfo)
        {
            CICUser user = new CICUser();
            using (IDbConnection conn = HelpClassLib.DataBase.OraHelper.GetSqlConnection())
            {
                string strSql = @" SELECT c.UserId,c.Username, c.PasswordHash AS Password,c.Email,
                                          c.PhoneNumber,c.IsFirstTimeLogin,c.AccessFailedCount,
                                          c.CreationDate,c.IsActive 
                                    FROM CICUser c where c.userName = :userName ";
                user = conn.Query<CICUser>(strSql, userInfo, null, true, null, CommandType.Text).FirstOrDefault();

            }
            return user;
        }
        /// <summary>
        /// 一对一映射关系
        /// </summary>
        /// <returns></returns>
        public  List<Customer> OneReferences()
        {
            List<Customer> userList = new List<Customer>();
            using (IDbConnection conn = HelpClassLib.DataBase.OraHelper.GetSqlConnection())
            {
                string strSql = @" SELECT c.UserId,c.Username, c.PasswordHash AS Password,c.Email,
                                          c.PhoneNumber,c.IsFirstTimeLogin,c.AccessFailedCount,
                                          c.CreationDate,c.IsActive,r.RoleId,r.RoleName 
                                    FROM CICUser c 
                                    INNER JOIN CICUserRole cr ON cr.UserId = c.UserId 
                                    INNER JOIN CICRole r ON r.RoleId = cr.RoleId ";
                userList = conn.Query<Customer,CICRole,Customer>(strSql,
                    (user, role) => { user.CICRole = role; return user; }, 
                    null, null, true, "RoleId", 60, CommandType.Text).ToList();

            }
            if (userList.Count > 0)
            { 
                userList.ForEach((user)=> Console.WriteLine("UserName:" + user.UserName +" Role:"+ user.CICRole.RoleName+" PhoneNumber:"+ user.PhoneNumber+"\n"));
                Console.ReadLine();
            }

            return userList;
        }

        /// <summary>
        /// 一对多关系映射
        /// </summary>
        /// <returns></returns>
        public List<CICUser> OneToManyReferenc()
        {
            List<CICUser> userList = new List<CICUser>();
            using (IDbConnection conn = HelpClassLib.DataBase.OraHelper.GetSqlConnection())
            {
                string strSql = @" SELECT c.UserId,
                                           c.Username,
                                           c.PasswordHash Password],
                                           c.Email,
                                           c.PhoneNumber,
                                           c.IsFirstTimeLogin,
                                           c.AccessFailedCount,
                                           c.CreationDate,
                                           c.IsActive,
                                           r.RoleId,
                                           r.RoleName
                                    FROM   CICUser c 
                                           LEFT JOIN CICUserRole cr
                                                ON  cr.UserId = c.UserId
                                           LEFT JOIN CICRole r
                                                ON  r.RoleId = cr.RoleId";
                var lookup = new Dictionary<int, CICUser>();
                userList = conn.Query<CICUser, CICRole, CICUser>(strSql, (user, role) =>
                {
                    CICUser UserInfo;
                    if (!lookup.TryGetValue(user.UserId, out UserInfo))
                    {
                        lookup.Add(user.UserId, UserInfo = user);
                    }
                    UserInfo.CICRole.Add(role);
                    return user;
                }, null, null, true, "RoleId", null, null).ToList();
                var result = lookup.Values;
            }
            if (userList.Count > 0)
            {
                userList.ForEach((item) => Console.WriteLine("UserName:" + item.UserName +
                                             "----Password:" + item.Password +
                                             "-----Role:" + item.CICRole.First().RoleName +
                                             "\n"));

                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("No Data In UserList!");
            }
            return userList;
        }

        /// <summary>
        /// 插入一条记录
        /// </summary>
        /// <param name="recordModel"></param>
        /// <returns></returns>
        public bool InsertRecord<T>(T recordModel)
        {
            string strSql = @" INSERT INTO CICUser(Username,PasswordHash,Email,PhoneNumber)
                               VALUES(:UserName, :Password,:Email, :PhoneNumber )";
            bool isTrue = false;
            using (IDbConnection conn = HelpClassLib.DataBase.OraHelper.GetSqlConnection())
            {
                CICUser user = new CICUser();
                user.UserName = "Dapper";
                user.Password = "654321";
                user.Email = "Dapper@infosys.com";
                user.PhoneNumber = "13795666243";
                isTrue = conn.Execute(strSql, recordModel) > 0;
            }
            return isTrue;
        }
    }
}
