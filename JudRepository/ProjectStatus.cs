using JudDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudRepository
{
    public class ProjectStatus
    {
        #region Fields
        private int id;
        private string text;

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public ProjectStatus()
        {
            this.id = 0;
            this.text = "";
        }

        /// <summary>
        /// Constructor to add a new Project Status
        /// </summary>
        /// <param name="text">string</param>
        public ProjectStatus(string text)
        {
            this.id = 0;
            this.text = text;
        }

        /// <summary>
        /// Constructor to add a Project Status from Db
        /// </summary>
        /// <param name="text"></param>
        public ProjectStatus(int id, string text)
        {
            this.id = id;
            this.text = text;
        }

        /// <summary>
        /// Constructor, that accepts data from an existing Project Status
        /// </summary>
        /// <param name="status">ProjectStatus</param>
        public ProjectStatus(ProjectStatus status)
        {
            this.id = status.Id;
            this.text = status.Text;
        }

        /// <summary>
        /// Constructor, that accepts data from an existing Indexed Project Status
        /// </summary>
        /// <param name="status">IndexedProjectStatus</param>
        public ProjectStatus(IndexedProjectStatus status)
        {
            this.id = status.Id;
            this.text = status.Text;
        }

        #endregion

        #region Properties
        public int Id { get => id; }

        public string Text
        {
            get => text;
            set
            {
                try
                {
                    if (value != null)
                    {
                        text = value;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Method, that sets id, if id == 0
        /// </summary>
        public void SetId(int id)
        {
            try
            {
                if (this.id == 0 && id >= 1)
                {
                    this.id = id;
                }
            }
            catch (Exception)
            {
                this.id = 0;
            }
        }

        /// <summary>
        /// Returns main content as a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return text;
        }

        #endregion

    }
}
