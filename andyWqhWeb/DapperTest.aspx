<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DapperTest.aspx.cs" Inherits="andyWqhWeb.DapperTest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Dapper测试页面</title>
</head>
<body>
    <form id="form1" action="DapperTest.aspx" method="post">
        <div class ="userInfo">
            <label for ="userName">用户名:</label>
            <input type="text" id="userName" runat="server"/>
            <label for="password">密 码:</label>
            <input type="text" id="password" runat="server"/>
            <label for="email">邮 箱:</label>
            <input type="text" id="email" runat="server"/>
            <label for="phoneNumber">手 机:</label>
            <input type="text" id="phoneNumber" runat ="server"/>
        </div>
    </form>
</body>
</html>
