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
        /// Constructor to add a new Request
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
        /// Constructor to add a Request from Db
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
        /// Constructor, that accepts data from an existing Request
        /// </summary>
        /// <param name="request">Request</param>
        public Request(Request request)
        {
                this.id = request.Id;
                this.status = request.Status;
                this.sentDate = request.SentDate;
                this.receivedDate = request.ReceivedDate;
        }

        /// <summary>
        /// Constructor, that accepts data from an existing Indexed Request
        /// </summary>
        /// <param name="request">IndexedRequest</param>
        public Request(IndexedRequest request)
        {
            this.id = request.Id;
            this.status = request.Status;
            this.sentDate = request.SentDate;
            this.receivedDate = request.ReceivedDate;
        }

        #endregion

        #region Properties
        public int Id { get => id; }

        public RequestStatus Status { get => status; set => status = value; }

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

        #region Methods

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

    }
}
