using JudDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudRepository
{
    public class TenderForm
    {
        #region Fields
        private int id;
        private string description;

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public TenderForm()
        {
            this.id = 0;
            this.description = "";
        }

        /// <summary>
        /// Method to add a new Tender Form
        /// </summary>
        /// <param name="description">string</param>
        public TenderForm(string description)
        {
            this.id = 0;
            this.description = description;
        }

        /// <summary>
        /// Method to add a Tender Form from Db
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        public TenderForm(int id, string description)
        {
            this.id = id;
            this.description = description;
        }

        /// <summary>
        /// Method to add a Tender Form
        /// </summary>
        /// <param name="form">TenderForm</param>
        public TenderForm(TenderForm form)
        {
                this.id = form.Id;
                this.description = form.Description;
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
    }
    #endregion
}