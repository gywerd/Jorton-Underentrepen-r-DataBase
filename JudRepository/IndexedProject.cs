using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudRepository
{
    public class IndexedProject : Project
    {
        #region Fields
        int index;

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public IndexedProject() : base(new Project())
        {
            this.index = 0;
        }

        /// <summary>
        /// Constructor to add a new Indexed Project
        /// </summary>
        /// <param name="index">int</param>
        /// <param name="project">Project</param>
        public IndexedProject(int index, Project project) : base(project)
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
