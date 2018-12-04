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
        private int id;
        private string description;

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public RequestStatus()
        {
            this.id = 0;
            this.description = "";
        }

        /// <summary>
        /// Costructor used to add new RequestStatus
        /// </summary>
        /// <param name="description">string</param>
        public RequestStatus(string description)
        {
            this.id = 0;
            this.description = description;
        }

        /// <summary>
        /// Costructor used to add RequestStatus from Db
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        public RequestStatus(int id, string description)
        {
            this.id = id;
            this.description = description;
        }

        /// <summary>
        /// Costructor used to add RequestStatus from Db
        /// </summary>
        /// <param name="status">RequestStatus</param>
        public RequestStatus(RequestStatus status)
        {
            this.id = status.Id;
            this.description = status.Description;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Returns main content as a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return description;
        }

        #endregion

        #region Properties
        public int Id { get => id; }

        public string Description
        {
            get => description;
            set
            {
                try
                {
                    if (value != null)
                    {
                        description = value;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        #endregion
    }
}
