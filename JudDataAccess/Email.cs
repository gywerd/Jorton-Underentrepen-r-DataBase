using System;
using System.Configuration;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using Outlook = Microsoft.Office.Interop.Outlook;
using Office = Microsoft.Office.Core;

namespace JudDataAccess
{
    public class Email
    {
			private string subject = "";
			private string to = "";
			private string body = "";
			private string logPath = @"\log";
			
			List<MailAttachment> attachments = new List<MailAttachment>();
			
			
			public Email(){ }
			
      /// <summary>
      /// Construct a mail attachment form a byte array
      /// </summary>
      /// <param name="subject">string</param>
      /// <param name="to">string</param>
      /// <param name="body">string</param>
      /// <param name="attachments">string</param>
			public Email(string subject, string to, string body, MailAttachment[] attachments)
			{
				this.subject = subject;
				this.to = to;
                this.body = body; 
				
				if(attachments.Count() > 0)
				{
					foreach(MailAttachment attachment in attachments)
					{
						this.attachments.Add(attachment);
					}
				}
				
				CreateMailItem();
			}
			
			private void CreateMailItem()
			{
					//Outlook.MailItem mailItem = (Outlook.MailItem)
					// this.Application.CreateItem(Outlook.OlItemType.olMailItem);
					Outlook.Application app = new Outlook.Application();
					Outlook.MailItem mailItem = (Outlook.MailItem)app.CreateItem(Outlook.OlItemType.olMailItem);
					mailItem.Subject = "This is the subject";
					mailItem.To = "someone@example.com";
					mailItem.Body = "This is the message.";
					mailItem.Attachments.Add(logPath);//logPath is a string holding path to the log.txt file
					if(this.attachments.Count > 0)
					{
						foreach(MailAttachment attachment in this.attachments)
						{
							mailItem.Attachments.Add(attachment);
						}
					}
					mailItem.Importance = Outlook.OlImportance.olImportanceHigh;
					mailItem.Display(false);
			}
    }
}