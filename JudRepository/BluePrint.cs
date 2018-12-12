using JudDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JudRepository
{
    public class BluePrint
    {
        #region Fields
        int id;
        Project project;
        string name;
        Description description;
        string url;

        #endregion

        #region Constructors
        public BluePrint()
        {
            this.id = 0;
            this.project = new Project();
            this.name = "";
            this.description = new Description();
            this.url = "";
        }

        public BluePrint(Project project, string name, Description description, string url)
        {
            this.id = 0;
            this.project = project;
            this.name = name;
            this.description = description;
            this.url = url;
        }

        public BluePrint(int id, Project project, string name, Description description, string url)
        {
            this.id = id;
            this.project = project;
            this.name = name;
            this.description = description;
            this.url = url;
        }

        public BluePrint(BluePrint bluePrint)
        {
            this.id = bluePrint.Id;
            this.project = bluePrint.Project;
            this.name = bluePrint.Name;
            this.description = bluePrint.Description;
            this.url = bluePrint.Url;
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
            return name;
        }

        #endregion

        #region Properties
        public int Id { get => id; }

        public Project Project { get; set; }

        public string Name
        {
            get { return name; }
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

        public Description Description
        {
            get { return description; }
            set
            {
                try
                {
                    description = value;
                }
                catch (Exception)
                {
                    description = new Description();
                }
            }
        }

        public string Url
        {
            get { return url; }
            set
            {
                try
                {
                    url = value;
                }
                catch (Exception)
                {
                    url = "";
                }
            }
        }

        #endregion

    }
}