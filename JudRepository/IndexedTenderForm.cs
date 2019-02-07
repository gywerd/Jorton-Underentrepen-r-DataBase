using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudRepository
{
    public class IndexedTenderForm : TenderForm
    {
        #region Fields
        int index;

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public IndexedTenderForm() : base(new TenderForm())
        {
            this.index = 0;
        }

        /// <summary>
        /// Constructor to add a new Indexed Tender Form
        /// </summary>
        /// <param name="index">int</param>
        /// <param name="form">TenderForm</param>
        public IndexedTenderForm(int index, TenderForm form) : base(form)
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
        public int Index { get => index; }

        #endregion
    }
}
