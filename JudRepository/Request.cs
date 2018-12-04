using JudDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace JudRepository
{
    public class Request
    {
        #region Fields
        private int id;
        private RequestStatus status;
        private DateTime sentDate;
        private DateTime receivedDate;

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public Request()
        {
            this.id = 0;
            DateTime date = Convert.ToDateTime("1932-03-17");
            this.status = new RequestStatus();
            this.sentDate = date;
            this.receivedDate = date;
        }

        /// <summary>
        /// Constructor to add new Request
        /// </summary>
        /// <param name="status">bool</param>
        /// <param name="sentDate">DateTime?</param>
        /// <param name="receivedDate">DateTime?</param>
        public Request(RequestStatus status, DateTime sentDate, DateTime receivedDate)
        {
            this.id = 0;
            this.status = status;
            this.sentDate = sentDate;
            this.receivedDate = receivedDate;
        }

        /// <summary>
        /// Constructor to add Request from Db
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="status">bool</param>
        /// <param name="sentDate">DateTime?</param>
        /// <param name="receivedDate">DateTime?</param>
        public Request(int id, RequestStatus status, DateTime sentDate, DateTime receivedDate)
        {
            this.id = id;
            this.status = status;
            this.sentDate = sentDate;
            this.receivedDate = receivedDate;
        }

        /// <summary>
        /// Constructor, that accepts data from existing Request
        /// </summary>
        /// <param name="request">Request</param>
        public Request(Request request)
        {
                this.id = request.Id;
                this.status = request.Status;
                this.sentDate = request.SentDate;
                this.receivedDate = request.ReceivedDate;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Method that checks a date
        /// If date is receivedDate: status = {1, 2, 3}
        /// If date is sentDate: status = {2, 3}
        /// </summary>
        /// <param name="date">DateTime</param>
        public void CheckDate(DateTime date)
        {
            if (date.ToShortDateString().Substring(0, 10) != "17-03-1932")
            {
                this.receivedDate = date;
            }
            else
            {
                this.receivedDate = DateTime.Now;
            }
        }

        /// <summary>
        /// Method, that create Delete SQL-query
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>string</returns>
        private string CreateDeleteFromSqlQuery(int id)
        {
            //DELETE FROM table_name WHERE condition;
            string result = @"DELETE FROM dbo.Requests WHERE Id = " + id + ";";
            return result;
        }

        /// <summary>
        /// Method, that creates sqlQuery to update status of a Request entry in Db
        /// </summary>
        /// <param name="status">int</param>
        /// <param name="id">int</param>
        /// <returns>string</returns>
        private string CreateUpdateStatusSqlQuery(int status, int id, string date)
        {
            //UPDATE [dbo].[Requests] SET [Status] = <Status, int,>,[SentDate] = <SentDate, date,>,[ReceivedDate] = <ReceivedDate, date,> WHERE [Id] = <Id, int>
            string query = "";

            switch (status)
            {
                case 0:
                    query = @"UPDATE [dbo].[Requests] SET [Status] = 0, [SentDate] = '1932-03-17', [ReceivedDate] = '1932-03-17' WHERE [Id] = " + id.ToString();
                    break;
                case 1:
                    query = @"UPDATE [dbo].[Requests] SET [Status] = 1, [SentDate] = '" + date + @"', [ReceivedDate] = '1932-03-17' WHERE [Id] = " + id.ToString();
                    break;
                case 2:
                    query = @"UPDATE [dbo].[Requests] SET [Status] = 2, [ReceivedDate] = '" + date + @"' WHERE [Id] = " + id.ToString();
                    break;
                case 3:
                    query = @"UPDATE [dbo].[Requests] SET [Status] = 3, [ReceivedDate] = '" + date + @"' WHERE [Id] = " + id.ToString();
                    break;
                case 4:
                    query = @"UPDATE [dbo].[Requests] SET [Status] = 4, [ReceivedDate] = '" + date + @"' WHERE [Id] = " + id.ToString();
                    break;
                case 5:
                    query = @"UPDATE [dbo].[Requests] SET [Status] = 5, [ReceivedDate] = '" + date + @"' WHERE [Id] = " + id.ToString();
                    break;
                case 6:
                    query = @"UPDATE [dbo].[Requests] SET [Status] = 6, [ReceivedDate] = '" + date + @"' WHERE [Id] = " + id.ToString();
                    break;
                case 7:
                    query = @"UPDATE [dbo].[Requests] SET [Status] = 7, [ReceivedDate] = '" + date + @"' WHERE [Id] = " + id.ToString();
                    break;
                case 8:
                    query = @"UPDATE [dbo].[Requests] SET [Status] = 8, [ReceivedDate] = '" + date + @"' WHERE [Id] = " + id.ToString();
                    break;
                case 9:
                    query = @"UPDATE [dbo].[Requests] SET [Status] = 9, [ReceivedDate] = '" + date + @"' WHERE [Id] = " + id.ToString();
                    break;
                case 10:
                    query = @"UPDATE [dbo].[Requests] SET [Status] = 10, [ReceivedDate] = '" + date + @"' WHERE [Id] = " + id.ToString();
                    break;
            }
            return query;
        }

        /// <summary>
        /// Method, that sets id, if id == 0
        /// </summary>
        public void SetId(int id)
        {
            try
            {
                if (this.id == 0 && id >= 1)
                {
                    this.id = id;
                }
            }
            catch (Exception)
            {
                this.id = 0;
            }
        }

        /// <summary>
        /// Returns main content as a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            switch (status.Id)
            {
                case 0:
                    return "Forespørgsel ikke sendt.";
                case 1:
                    return "Forespørgsel sendt: " + sentDate.ToShortDateString();
                case 2:
                    return "Forespørgsel bekræftet: " + receivedDate.ToShortDateString();
                case 3:
                    return "Forespørgsel annulleret: " + receivedDate.ToShortDateString();
            }
            return "";
        }

        #endregion

        #region Properties
        public int Id { get => id; }

        public RequestStatus Status { get; set; }

        public DateTime SentDate
        {
            get => sentDate;
            set
            {
                try
                {
                    sentDate = value;
                }
                catch (Exception)
                {
                    sentDate = Convert.ToDateTime("1932-03-17");
                }
            }
        }

        public DateTime ReceivedDate
        {
            get => receivedDate;
            set
            {
                try
                {
                    receivedDate = value;
                }
                catch (Exception)
                {
                    receivedDate = Convert.ToDateTime("1932-03-17");
                }
            }
        }

        #endregion

    }
}
