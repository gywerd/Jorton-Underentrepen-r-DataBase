using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudRepository
{
    public class IndexedZipTown : ZipTown
    {
        #region Fields
        int index;

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public IndexedZipTown() : base(new ZipTown())
        {
            this.index = 0;
        }

        /// <summary>
        /// Constructor to add a new Indexed Job Description
        /// </summary>
        /// <param name="index">int</param>
        /// <param name="zipTown">JobDescription</param>
        public IndexedZipTown(int index, ZipTown zipTown) : base(zipTown)
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
