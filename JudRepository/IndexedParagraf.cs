using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudRepository
{
    public class IndexedParagraf : Paragraf
    {
        #region Fields
        int index;

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public IndexedParagraf() : base(new Paragraf())
        {
            this.index = 0;
        }

        /// <summary>
        /// Constructor to add a new Indexed Paragraf
        /// </summary>
        /// <param name="index">int</param>
        /// <param name="paragraph">Paragraf</param>
        public IndexedParagraf(int index, Paragraf paragraph) : base(paragraph)
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
