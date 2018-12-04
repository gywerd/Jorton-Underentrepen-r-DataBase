using JudDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudRepository
{
    public class IttLetterParagraph
    {
        #region Fields
        protected int id;
        protected Project project;
        protected string name;

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public IttLetterParagraph()
        {
            this.id = 0;
            project = new Project();
            name = "";
        }

        /// <summary>
        /// Constructor for adding new IttLetterParagraph
        /// </summary>
        /// <param name="project">int</param>
        /// <param name="name">string</param>
        public IttLetterParagraph(Project project, string name)
        {
            this.id = 0;
            this.project = project;
            this.name = name;
        }

        /// <summary>
        /// Constructor for adding IttLetterParagraph from Db
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="project">int</param>
        /// <param name="name">string</param>
        public IttLetterParagraph(int id, Project project, string name)
        {
            this.id = id;
            this.project = project;
            this.name = name;
        }

        /// <summary>
        /// Constructor for that accepts data from existing IttLetterParagraph
        /// </summary>
        /// <param name="paragraph">IttLetterParagraph</param>
        public IttLetterParagraph(IttLetterParagraph paragraph)
        {
            this.id = paragraph.id;
            this.project = paragraph.Project;
            this.name = paragraph.Name;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Method, that returns main info as a string
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            return name;
        }

        #endregion

        #region Properties
        public int Id { get => id; }

        public Project Project { get; set; }

        public string Name
        {
            get => name;
            set
            {
                try
                {
                    name = value;
                }
                catch (Exception)
                {
                    name = "";
                }
            }
        }

        #endregion
    }
}
