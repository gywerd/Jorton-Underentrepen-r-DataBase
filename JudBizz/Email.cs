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

namespace JudBizz
{
	public class Email
	{
		#region Fields
		private string subject = "";
		private string to = "";
		private string sender = "";
		private string body = "";
		private string logPath = @"\log";
			
		List<Attachment> attachments = new List<Attachment>();
		#endregion

		#region Constructors
		/// <summary>
		/// Empty constructor
		/// </summary>
		public Email(){ }

		/// <summary>
		/// Construct a mail attachment form a byte array
		/// </summary>
		/// <param name="subject">string</param>
		/// <param name="to">string</param>
		/// <param name="body">string</param>
		/// <param name="attachments">Attachment[]</param>
		public Email(string subject, string to, string sender, string body, Attachment[] attachments = null)
		{
			if (attachments == null)
			{
				attachments = new Attachment[1];
			}

			this.subject = subject;
			this.to = to;
			this.sender = sender;
			this.body = body; 
				
			if(attachments.Count() > 0)
			{
				foreach(Attachment attachment in attachments)
				{
					this.attachments.Add(attachment);
				}
			}
				
			CreateMailItem();
		}

		#endregion

		#region Methods
		/// <summary>
		/// Method, that creates a Mail Item
		/// </summary>
		private void CreateMailItem()
		{
			//Outlook.MailItem mailItem = (Outlook.MailItem)
			// this.Application.CreateItem(Outlook.OlItemType.olMailItem);
			Outlook.Application app = new Outlook.Application();
			Outlook.MailItem mailItem = (Outlook.MailItem)app.CreateItem(Outlook.OlItemType.olMailItem);
			mailItem.Subject = this.subject;
			mailItem.To = this.to;
			mailItem.Sender.Address = this.sender;
			mailItem.Body = this.body;
			mailItem.Attachments.Add(logPath);//logPath is a string holding path to the log.txt file
			if(this.attachments.Count > 0)
			{
				foreach(Attachment attachment in this.attachments)
				{
					mailItem.Attachments.Add(attachment);
				}
			}
			mailItem.Importance = Outlook.OlImportance.olImportanceHigh;
			mailItem.Display(false);
		}

		#endregion
	}
}