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
        private string cc = "";
		private string sender = "";
		private string body = "";
        private Bizz CBZ = new Bizz();
			
		List<string> filePaths = new List<string>();
		#endregion

		#region Constructors
		/// <summary>
		/// Empty constructor
		/// </summary>
		public Email() { }

        /// <summary>
        /// Construct a mail attachment form a byte array
        /// </summary>
        /// <param name="subject">string</param>
        /// <param name="to">string</param>
        /// <param name="body">string</param>
        /// <param name="attachments">Attachment[]</param>
        public Email(Bizz cbz, string subject, string to, string cc, string body, string[] filePaths)
		{
            this.CBZ = cbz;
            this.subject = subject;
			this.to = to;
            this.cc = cc;
			this.body = body; 
				
			if(filePaths.Count() > 0)
			{
                this.filePaths.Clear();
				foreach(string filePath in filePaths)
				{
					this.filePaths.Add(filePath);
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
            Outlook.Recipient recipient = app.Session.CreateRecipient(sender);
            mailItem.Subject = this.subject;
			mailItem.To = this.to;
            mailItem.CC = this.cc;
			mailItem.Body = this.body;

            if (this.filePaths.Count > 0)
			{
				foreach(string fileName in this.filePaths)
				{
                    string absolute_path = Path.Combine(CBZ.AppPath + fileName);
                    mailItem.Attachments.Add(Path.GetFullPath((new Uri(absolute_path)).LocalPath), Outlook.OlAttachmentType.olByValue, 1, fileName.Remove(0,14));

                }
			}

            mailItem.Importance = Outlook.OlImportance.olImportanceHigh;
			mailItem.Display(false);
			mailItem.Send();
		}

        public byte[] FileToByteArray(string fileName)
        {
            byte[] buff = null;
            FileStream fs = new FileStream(fileName,
                                           FileMode.Open,
                                           FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            long numBytes = new FileInfo(fileName).Length;
            buff = br.ReadBytes((int)numBytes);
            return buff;
        }
        #endregion
    }
}