using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudRepository
{
    public class IndexedEnterpriseForm : EnterpriseForm
    {
        int index;

        /// <summary>
        /// Empty Constructor
        /// </summary>
        public IndexedEnterpriseForm() : base(new EnterpriseForm())
        {
            this.index = 0;
        }

        /// <summary>
        /// Constructor, to create an Indexable EnterpriseForm
        /// </summary>
        /// <param name="index">int</param>
        /// <param name="form">EnterpriseForm</param>
        public IndexedEnterpriseForm(int index, EnterpriseForm form) : base(form)
        {
            this.index = index;
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public int Index
        {
            get => index;
        }
    }
}
