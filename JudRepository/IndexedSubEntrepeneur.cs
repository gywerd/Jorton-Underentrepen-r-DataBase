using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudRepository
{
    public class IndexedSubEntrepeneur : SubEntrepeneur
    {
        #region Fields
        int index;

        #endregion

        #region Constructors
        /// Constructor, that adds an index to an existing SubEntrepeneur
        /// </summary>
        /// <param name="index">int</param>
        /// <param name="subEntrepeneur">SubEntrepeneur</param>
        public IndexedSubEntrepeneur(int index, SubEntrepeneur subEntrepeneur) : base(subEntrepeneur)
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
