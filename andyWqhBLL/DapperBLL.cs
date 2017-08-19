using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using andyWqhDAL;
using andyWqhModel;
using HelpClassLib;

namespace andyWqhBLL
{
    public class DapperBLL
    {
        DapperDAL dal = new DapperDAL();
        private static DapperBLL _instance;

        public static DapperBLL Instabce
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DapperBLL();
                }
                return _instance;
            }
        }

        public CICUser GetUserInfo(CICUser userInfo)
        {
            return dal.GetUserInfo(userInfo);
        }

        public List<Customer> OneReferences()
        {
            List<Customer> userList = dal.OneReferences();
            return userList;
        }

        public List<CICUser> OneToManyReferenc()
        {
            return dal.OneToManyReferenc();
        }

        public bool InsertRecord(CICUser userInfo)
        {
            return dal.InsertRecord(userInfo);
        }
    }
}
