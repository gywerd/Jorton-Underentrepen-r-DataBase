using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudRepository
{
    public class IndexedParagraph : Paragraph
    {
        #region Fields
        int index;

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public IndexedParagraph() : base(new Paragraph())
        {
            this.index = 0;
        }

        /// <summary>
        /// Constructor to add a new Indexed Paragraph
        /// </summary>
        /// <param name="index">int</param>
        /// <param name="paragraph">Paragraph</param>
        public IndexedParagraph(int index, Paragraph paragraph) : base(paragraph)
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
