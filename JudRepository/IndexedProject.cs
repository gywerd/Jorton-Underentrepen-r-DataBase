using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudRepository
{
    public class IndexedProject : Project
    {
        int index;

        /// <summary>
        /// Empty Constructor
        /// </summary>
        public IndexedProject() : base(new Project())
        {
            this.index = 0;
        }

        /// <summary>
        /// Constructor, to create an Indexable Project
        /// </summary>
        /// <param name="index">int</param>
        /// <param name="project">Project</param>
        public IndexedProject(int index, Project project) : base(project)
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
