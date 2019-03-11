using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudRepository
{
    public class IndexedCategory : Category
    {
        #region Fields
        int index;

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public IndexedCategory() : base(new Category())
        {
            this.index = 0;
        }

        /// <summary>
        /// Constructor to add a new Indexed Category
        /// </summary>
        /// <param name="index">int</param>
        /// <param name="category">Category</param>
        public IndexedCategory(int index, Category category) : base(category)
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
