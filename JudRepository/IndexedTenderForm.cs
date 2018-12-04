using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudRepository
{
    public class IndexedTenderForm : TenderForm
    {
        int index;

        /// <summary>
        /// Empty Constructor
        /// </summary>
        public IndexedTenderForm() : base(new TenderForm())
        {
            this.index = 0;
        }

        /// <summary>
        /// Constructor, to create an Indexable TenderForm
        /// </summary>
        /// <param name="index">int</param>
        /// <param name="form">TenderForm</param>
        public IndexedTenderForm(int index, TenderForm form) : base(form)
        {
            this.index = index;
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public int Index { get => index; }
    }
}
