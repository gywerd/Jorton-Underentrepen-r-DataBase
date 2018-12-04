using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudRepository
{
    public class IndexedIttLetterParagraph : IttLetterParagraph
    {
        int index;

        /// <summary>
        /// Empty Constructor
        /// </summary>
        public IndexedIttLetterParagraph() : base(new IttLetterParagraph())
        {
            this.index = 0;
        }

        /// <summary>
        /// Constructor, to create an Indexable Paragraph
        /// </summary>
        /// <param name="index">int</param>
        /// <param name="paragraph">IttLetterParagraph</param>
        public IndexedIttLetterParagraph(int index, IttLetterParagraph paragraph) : base(paragraph)
        {
            this.index = index;
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public int Index { get => index; }
    }
}
