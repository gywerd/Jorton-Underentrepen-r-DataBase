using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudRepository
{
    public class IndexedBuilder : Builder
    {
        #region Fields
        int index;

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public IndexedBuilder() : base(new Builder())
        {
            this.index = 0;
        }

        /// <summary>
        /// Constructor to add a new Indexed Legal Entity
        /// </summary>
        /// <param name="index">int</param>
        /// <param name="builder">Builder</param>
        public IndexedBuilder(int index, Builder builder) : base(builder)
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
