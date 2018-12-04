using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudRepository
{
    public class IndexedLegalEntity : LegalEntity
    {
        int index;

        /// <summary>
        /// Empty Constructor
        /// </summary>
        public IndexedLegalEntity() : base(new LegalEntity())
        {
            this.index = 0;
        }

        /// <summary>
        /// Constructor, to create an Indexable Legal Entity
        /// </summary>
        /// <param name="index">int</param>
        /// <param name="legalEntity">LegalEntity</param>
        public IndexedLegalEntity(int index, LegalEntity legalEntity) : base(legalEntity)
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
