using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudRepository
{
    public class IndexedProjectStatus : ProjectStatus
    {
        int index;

        /// <summary>
        /// Empty Constructor
        /// </summary>
        public IndexedProjectStatus() : base(new ProjectStatus())
        {
            this.index = 0;
        }

        /// <summary>
        /// Constructor, to create an Indexable Project Status
        /// </summary>
        /// <param name="index">int</param>
        /// <param name="status">ProjectStatus</param>
        public IndexedProjectStatus(int index, ProjectStatus status) : base(status)
        {
            this.index = index;
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public int Index
        {
            get => index;
        }
    }
}
