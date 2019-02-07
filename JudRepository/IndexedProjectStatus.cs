using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudRepository
{
    public class IndexedProjectStatus : ProjectStatus
    {
        #region Fields
        int index;

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public IndexedProjectStatus() : base(new ProjectStatus())
        {
            this.index = 0;
        }

        /// <summary>
        /// Constructor to add a new Indexed Project Status
        /// </summary>
        /// <param name="index">int</param>
        /// <param name="status">ProjectStatus</param>
        public IndexedProjectStatus(int index, ProjectStatus status) : base(status)
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
