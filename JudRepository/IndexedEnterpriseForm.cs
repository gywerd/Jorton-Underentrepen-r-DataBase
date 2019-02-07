using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudRepository
{
    public class IndexedEnterpriseForm : EnterpriseForm
    {
        #region Fields
        int index;

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public IndexedEnterpriseForm() : base(new EnterpriseForm())
        {
            this.index = 0;
        }

        /// <summary>
        /// Constructor to add a new Indexed EnterpriseForm
        /// </summary>
        /// <param name="index">int</param>
        /// <param name="form">EnterpriseForm</param>
        public IndexedEnterpriseForm(int index, EnterpriseForm form) : base(form)
        {
            this.index = index;
        }

        #endregion

        #region Methods
        public override string ToString()
        {
            return base.ToString();
        }

        #endregion

        #region Properties
        public int Index
        {
            get => index;
        }

        #endregion
    }
}
