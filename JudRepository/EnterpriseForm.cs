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
        private string name;

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public EnterpriseForm()
        {
            this.id = 0;
            this.name = "";
        }

        /// <summary>
        /// Constructor to add EnterpriseForm
        /// </summary>
        /// <param name="id">string</param>
        /// <param name="name">string</param>
        public EnterpriseForm(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        /// <summary>
        /// Constructor, that accepts an existing Enterprise Form
        /// </summary>
        /// <param name="abbreviation">string</param>
        /// <param name="form">string</param>
        public EnterpriseForm(EnterpriseForm form)
        {
            this.id = form.Id;
            this.name = form.Name;
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
        public int Id { get => id; }

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
