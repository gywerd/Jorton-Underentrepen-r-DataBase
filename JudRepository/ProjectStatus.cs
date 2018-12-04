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
        private string description;

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public ProjectStatus()
        {
            this.id = 0;
            this.description = "";
        }

        /// <summary>
        /// Constructor add a new ProjectStatus
        /// </summary>
        /// <param name="description">string</param>
        public ProjectStatus(string description)
        {
            this.id = 0;
            this.description = description;
        }

        /// <summary>
        /// Constructor add a ProjectStatus from Db
        /// </summary>
        /// <param name="description"></param>
        public ProjectStatus(int id, string description)
        {
            this.id = id;
            this.description = description;
        }

        /// <summary>
        /// Constructor add a ProjectStatus
        /// </summary>
        /// <param name="status">ProjectStatus</param>
        public ProjectStatus(ProjectStatus status)
        {
            this.id = status.Id;
            this.description = status.Description;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Returns main content as a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return description;
        }

        #endregion

        #region Properties
        public int Id { get => id; }

        public string Description
        {
            get => description;
            set
            {
                try
                {
                    if (value != null)
                    {
                        description = value;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        #endregion
    }
}
