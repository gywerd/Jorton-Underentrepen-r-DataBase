using JudDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudRepository
{
    public class EnterpriseForm
    {
        #region Fields
        private int id;
        private string text;

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public EnterpriseForm()
        {
            this.id = 0;
            this.text = "";
        }

        /// <summary>
        /// Constructor to add a new EnterpriseForm
        /// </summary>
        /// <param name="text">string</param>
        public EnterpriseForm(string text)
        {
            this.id = 0;
            this.text = text;
        }

        /// <summary>
        /// Constructor to add an EnterpriseForm from Db
        /// </summary>
        /// <param name="id">string</param>
        /// <param name="text">string</param>
        public EnterpriseForm(int id, string text)
        {
            this.id = id;
            this.text = text;
        }

        /// <summary>
        /// Constructor, that accepts data from an existing Enterprise Form
        /// </summary>
        /// <param name="Indexed">IndexedEnterpriseForm</param>
        public EnterpriseForm(EnterpriseForm enterpriseForm)
        {
            this.id = enterpriseForm.Id;
            this.text = enterpriseForm.Text;
        }

        /// <summary>
        /// Constructor, that accepts data from an existing Indexed Enterprise Form
        /// </summary>
        /// <param name="Indexed">IndexedEnterpriseForm</param>
        public EnterpriseForm(IndexedEnterpriseForm enterpriseForm)
        {
            this.id = enterpriseForm.Id;
            this.text = enterpriseForm.Text;
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
                    text = value;
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
