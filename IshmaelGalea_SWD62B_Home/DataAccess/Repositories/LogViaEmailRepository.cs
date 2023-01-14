using DataAccess.context;
using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace DataAccess.Repositories
{
    public class LogViaEmailRepository : ILogRepository
    {
        public void Log(string msg, string ipaddress, string user)
        {
            string email = "joe@gmail.com";
            string password = "Joe123#";
            string smtpClient = "smtp.office365.com";
            int port = 587;

            SmtpClient smtpclient = new SmtpClient(smtpClient, port);
            smtpclient.EnableSsl = true;
            smtpclient.Credentials = new NetworkCredential(email, password);

            MailMessage msgmail = new MailMessage();
            msgmail.From = new MailAddress(email);
            msgmail.To.Add("joe@gmail.com");
            msgmail.Subject = "Log Message";
            msgmail.Body = $"Message: {msg} \n IPAddress: {ipaddress} \n User: {user}";
            smtpclient.Send(msgmail);
        }
        public void Log(Exception exception, string ipaddress, string user)
        {
            string email = "joe@gmail.com";
            string password = "Joe123#";
            string smtpClient = "smtp.office365.com";
            int port = 587;

            SmtpClient smtpclient = new SmtpClient(smtpClient, port);
            smtpclient.EnableSsl = true; 
            smtpclient.Credentials = new NetworkCredential(email, password);

            MailMessage msgmail = new MailMessage();
            msgmail.From = new MailAddress(email);
            msgmail.To.Add("joe@gmail.com");
            msgmail.Subject = "Log Message";
            msgmail.Body = $"There has been an error: {exception.Message} \n IPAddress: {ipaddress} \n User: {user}";
            smtpclient.Send(msgmail);

        }
    }
}
