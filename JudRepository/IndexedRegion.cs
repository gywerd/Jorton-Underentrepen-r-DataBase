using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudRepository
{
    public class IndexedRegion : Region
    {
        #region Fields
        int index;

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public IndexedRegion() : base(new Region())
        {
            this.index = 0;
        }

        /// <summary>
        /// Constructor to add a new Indexed Request
        /// </summary>
        /// <param name="index">int</param>
        /// <param name="region">Region</param>
        public IndexedRegion(int index, Region region) : base(region)
        {
            this.index = index;
        }

        #endregion

        #region Method
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
