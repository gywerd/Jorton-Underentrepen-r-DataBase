using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudRepository
{
    public class IndexedUserLevel : UserLevel
    {
        #region Fields
        int index;

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public IndexedUserLevel() : base(new UserLevel())
        {
            this.index = 0;
        }

        /// <summary>
        /// Constructor to add a new Indexed User Level
        /// </summary>
        /// <param name="index">int</param>
        /// <param name="level">UserLevel</param>
        public IndexedUserLevel(int index, UserLevel level) : base(level)
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
