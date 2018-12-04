using JudDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JudRepository
{
    public class Description
    {
        #region Fields
        private int id;
        private Project project;
        private Enterprise enterprise;
        private string text;

        #endregion

        #region Constructors
        /// <summary>
        /// Empty constructor
        /// </summary>
        public Description()
        {
            this.id = 0;
            this.project = new Project();
            this.enterprise = new Enterprise();
            this.text = "";
        }

        public Description(Project project, Enterprise enterprise, string content)
        {
            this.id = 0;
            this.project = project;
            this.enterprise = enterprise;
            this.text = content;
        }

        public Description(int id, Project project, Enterprise enterprise, string content)
        {
            this.id = id;
            this.project = project;
            this.enterprise = enterprise;
            this.text = content;
        }

        public Description(Description description)
        {
            this.id = description.Id;
            this.project = description.Project;
            this.enterprise = description.Enterprise;
            this.text = description.Text;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Method, that sets id, if id == 0
        /// </summary>
        /// <param name="id">int</param>
        public void SetId(int id)
        {
            if (int.TryParse(id.ToString(), out int parsedId) && this.id == 0 && parsedId >= 1)
            {
                this.id = parsedId;
            }
            else
            {
                this.id = 0;
            }
        }

        /// <summary>
        /// Returns main content as a string
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            return text;
        }

        #endregion

        #region Properties
        public int Id { get; }

        public Project Project { get; set; }
        public Enterprise Enterprise { get; set; }

        public string Text
        {
            get { return text; }
            set
            {
                try
                {
                    text = value.ToString();
                }
                catch (Exception)
                {
                    text = "";
                }
            }
        }

        #endregion

    }
}
