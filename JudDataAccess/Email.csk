using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Web;
using System.IO;
using System.Net.Mime;
using System.Net.Mail;

namespace JudDataAccess
{
    public class Email
    {
        #region Fields
        public static MailAttachment[] mailAttachments;
        private static string host = ConfigurationManager.AppSettings["SMTPHost"];
        #endregion

        #region Constructors
        public Email()
        {
        }

        /// <summary>
        /// Send an email from [DELETED]
        /// </summary>
        /// <param name="to">Message to address</param>
        /// <param name="body">Text of message to send</param>
        /// <param name="subject">Subject line of message</param>
        /// <param name="fromAddress">Message from address</param>
        /// <param name="fromDisplay">Display name for "message from address"</param>
        /// <param name="credentialUser">User whose credentials are used for message send</param>
        /// <param name="credentialPassword">User password used for message send</param>
        /// <param name="attachments">Optional attachments for message</param>
        public Email(string to,
                                 string body,
                                 string subject,
                                 string fromAddress,
                                 string fromDisplay,
                                 string credentialUser,
                                 string credentialPassword,
                                 params MailAttachment[] attachments)
        {
            //body = UpgradeEmailFormat(body);
            try
            {
                MailMessage mail = new MailMessage();
                mail.Body = body;
                mail.IsBodyHtml = true;
                mail.To.Add(new MailAddress(to));
                mail.From = new MailAddress(fromAddress, fromDisplay, Encoding.UTF8);
                mail.Subject = subject;
                mail.SubjectEncoding = Encoding.UTF8;
                mail.Priority = MailPriority.Normal;
                foreach (MailAttachment ma in attachments)
                {
                    mail.Attachments.Add(ma.File);
                }
                SmtpClient smtp = new SmtpClient();
                smtp.Credentials = new System.Net.NetworkCredential(credentialUser, credentialPassword);
                smtp.Host = host;
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder(1024);
                sb.Append("\nTo:" + to);
                sb.Append("\nbody:" + body);
                sb.Append("\nsubject:" + subject);
                sb.Append("\nfromAddress:" + fromAddress);
                sb.Append("\nfromDisplay:" + fromDisplay);
                sb.Append("\ncredentialUser:" + credentialUser);
                sb.Append("\ncredentialPasswordto:" + credentialPassword);
                sb.Append("\nHosting:" + host);
                Trace.TraceInformation(sb.ToString());
                Trace.TraceError(ex.ToString());
                Trace.TraceWarning("Mail blev ikke sendt");
                Trace.Listeners.Add(new TextWriterTraceListener("MyTextFile.log"));
                throw ex;
            }
        }

        #endregion

    }

}
