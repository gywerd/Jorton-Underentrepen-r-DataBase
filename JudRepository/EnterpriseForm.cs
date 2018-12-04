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
        private string abbreviation;
        private string name;

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public EnterpriseForm()
        {
            this.abbreviation = "";
            this.name = "";
        }

        /// <summary>
        /// Constructor to add EnterpriseForm
        /// </summary>
        /// <param name="abbreviation">string</param>
        /// <param name="name">string</param>
        public EnterpriseForm(string abbreviation, string name)
        {
            this.abbreviation = abbreviation;
            this.name = name;
        }

        /// <summary>
        /// Constructor, that accepts an existing Enterprise Form
        /// </summary>
        /// <param name="abbreviation">string</param>
        /// <param name="form">string</param>
        public EnterpriseForm(EnterpriseForm form)
        {
            if (form != null)
            {
                this.abbreviation = form.Abbreviation;
                this.name = form.Name;
            }
            else
            {
                this.abbreviation = "";
                this.name = "";
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Returns main content as a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return name;
        }

        #endregion

        #region Properties
        public string Abbreviation
        {
            get => abbreviation;
            set
            {
                try
                {
                    if (value != null)
                    {
                        abbreviation = value;
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }

        public string Name
        {
            get => name;
            set
            {
                try
                {
                    name = value;
                }
                catch (Exception)
                {
                    name = "";
                }
            }
        }

        #endregion

    }
}
