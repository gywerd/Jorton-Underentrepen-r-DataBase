using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudRepository
{
    public class IndexedEnterprise : Enterprise
    {
        #region Fields
        int index;

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public IndexedEnterprise() : base() { }

        /// <summary>
        /// Constructor, that add a new Indexed Enterprise
        /// </summary>
        /// <param name="index">int</param>
        /// <param name="enterprise">Enterprise</param>
        public IndexedEnterprise(int index, Enterprise enterprise) : base(enterprise)
        {
            this.index = index;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Method, that return main info as string
        /// </summary>
        /// <returns>string</returns>
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
