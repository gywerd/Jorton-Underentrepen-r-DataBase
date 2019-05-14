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
        private string text;

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public TenderForm()
        {
            this.id = 0;
            this.text = "";
        }

        /// <summary>
        /// Constructor to add a new Tender Form
        /// </summary>
        /// <param name="text">string</param>
        public TenderForm(string text)
        {
            this.id = 0;
            this.text = text;
        }

        /// <summary>
        /// Constructor to add a Tender Form from Db
        /// </summary>
        /// <param name="id"></param>
        /// <param name="text"></param>
        public TenderForm(int id, string text)
        {
            this.id = id;
            this.text = text;
        }

        /// <summary>
        /// Constructor, that accepts data from an existing Tender Form
        /// </summary>
        /// <param name="form">TenderForm</param>
        public TenderForm(TenderForm form)
        {
            if (form != null)
            {
                this.id = form.Id;
                this.text = form.Text;
            }
            else
            {
                this.id = 0;
                this.text = "";
            }
        }

        /// <summary>
        /// Constructor, that accepts data from an existing Indexed Tender Form
        /// </summary>
        /// <param name="form">IndexedTenderForm</param>
        public TenderForm(IndexedTenderForm form)
        {
            this.id = form.Id;
            this.text = form.Text;
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
                        text = value;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
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