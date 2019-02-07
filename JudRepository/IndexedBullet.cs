using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudRepository
{
    public class IndexedBullet : Bullet
    {
        #region Fields
        int index;

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public IndexedBullet() : base(new Bullet())
        {
            this.index = 0;
        }

        /// <summary>
        /// Constructor to add a new Indexed Request
        /// </summary>
        /// <param name="index">int</param>
        /// <param name="bullet">Bullet</param>
        public IndexedBullet(int index, Bullet bullet) : base(bullet)
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
