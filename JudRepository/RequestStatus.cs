using JudDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudRepository
{
    public class RequestStatus
    {
        #region Fields
        private int id = 0;
        private string text = "";

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public RequestStatus()
        {
        }

        /// <summary>
        /// Costructor to add a new RequestStatus
        /// </summary>
        /// <param name="text">string</param>
        public RequestStatus(string text)
        {
            this.text = text;
        }

        /// <summary>
        /// Costructor to add a RequestStatus from Db
        /// </summary>
        /// <param name="id"></param>
        /// <param name="text"></param>
        public RequestStatus(int id, string text)
        {
            this.id = id;
            this.text = text;
        }

        /// <summary>
        /// Costructor, that accepts an existing Request Status
        /// </summary>
        /// <param name="requestStatus">RequestStatus</param>
        public RequestStatus(RequestStatus requestStatus)
        {
            this.id = requestStatus.Id;
            this.text = requestStatus.Text;
        }

        /// <summary>
        /// Costructor, that accepts an existing Indexed Request Status
        /// </summary>
        /// <param name="requestStatus">IndexedRequestStatus</param>
        public RequestStatus(IndexedRequestStatus requestStatus)
        {
            this.id = requestStatus.Id;
            this.text = requestStatus.Text;
        }

        #endregion

        #region Properties
        public int Id { get => id; }

        public string Text
        {
            get => text;
            set
            {
                try
                {
                    if (value != null)
                    {
                        text = value.ToString();
                    }
                }
                catch (Exception)
                {
                    text = "";
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
            return text;
        }

        #endregion

    }
}
