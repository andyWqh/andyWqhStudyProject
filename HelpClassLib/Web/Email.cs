using System;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace HelpClassLib.Web
{
    /// <summary>
    /// Class Email.
    /// </summary>
    public class Email
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="mailFrom">发送人</param>
        /// <param name="mailTo">接收人</param>
        /// <param name="subject">主题</param>
        /// <param name="body">正文</param>
        /// <param name="mailServer">邮件服务器</param>
        /// <param name="mailUserName">用户名</param>
        /// <param name="mailUserPassword">密码</param>
        /// <param name="displayName">是否显示名称</param>
        /// <returns></returns>
        public static int SendEmail(string mailFrom, string mailTo, string subject, string body, string mailServer, string mailUserName, string mailUserPassword, string displayName = "")
        {
            try
            {
                var message = new MailMessage(mailFrom, mailTo)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    BodyEncoding = Encoding.UTF8,
                    Priority = MailPriority.High,
                    From = new MailAddress(mailFrom, displayName)
                };
                var client = new SmtpClient(mailServer)
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(mailUserName, mailUserPassword),
                    DeliveryMethod = SmtpDeliveryMethod.Network
                };
                client.Send(message);
                return 1;
            }
            catch (Exception e)
            {
                //new Log().Error(e.Message + e.StackTrace);
                return -1;
            }
        }
    }
}