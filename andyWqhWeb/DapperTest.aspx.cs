using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using andyWqhBLL;
using andyWqhModel;

namespace andyWqhWeb
{
    public partial class DapperTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //DapperBLL.Instabce.InsertRecord(new CICUser { UserName ="andWqh", Password="andyWqh123", Email="andyWqh@163.com", PhoneNumber="18818572279" });
            CICUser user = DapperBLL.Instabce.GetUserInfo(new CICUser { UserName = "andWqh" });
            this.userName.Value = user.UserName;
            this.password.Value = user.Password;
            this.email.Value = user.Email;
            this.phoneNumber.Value = user.PhoneNumber;
        }
    }
}