using System;
using System.Net.Mail;

namespace SMSand_EMAILService.cs
{
    public static class EmailService
    {

        public static void EmailSend(string emailID, string docterName)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            mail.From = new MailAddress("mjbhasmichael0@gmail.com");
            mail.To.Add(emailID);
            mail.Subject = "KLARITI EYE HOSPITAL";
            mail.Body += "Welcome to Klariti Eye Hospital" + "<br/>" +
                "Hi" + "<br/>" + "Dr" + "." + docterName + "," + "<br/>" +
                "please Login with your Username and Password as Mentioned Below" + "<br/>" + "<br/>" + "<br/>" + "<br/>" +
                 "Username : " + emailID + "<br/>" + "Password :" + docterName + "<br/>" + "<br/>" +
                 "logon to your page by clicking on this Activation Link" + "--->" + "http://localhost:4200/#/login" + "<br/>" +
                 "Thanks & Regards" + "<br/>" +
                 "Klariti Eye Hospital";
            mail.IsBodyHtml = true;
            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("mjbhasmichael0@gmail.com", "9710050004");
            SmtpServer.EnableSsl = true;
            SmtpServer.Send(mail);
        }

    }
}
