using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudRepository
{
    public class IndexedIttLetterBullet : IttLetterBullet
    {
        int index;

        /// <summary>
        /// Empty Constructor
        /// </summary>
        public IndexedIttLetterBullet() : base(new IttLetterBullet())
        {
            this.index = 0;
        }

        /// <summary>
        /// Constructor, to create an Indexable Request
        /// </summary>
        /// <param name="index">int</param>
        /// <param name="bullet">IttLetterBullet</param>
        public IndexedIttLetterBullet(int index, IttLetterBullet bullet) : base(bullet)
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
