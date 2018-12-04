using JudDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudRepository
{
    public class JobDescription
    {
        #region Fields
        private int id;
        private string occupation;
        private string area;
        private bool procuration;

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public JobDescription()
        {
            this.id = 0;
            this.occupation = "";
            this.area = "";
            this.procuration = false;
        }

        /// <summary>
        /// Constructor for adding a new job descripton
        /// </summary>
        /// <param name="occupation">string</param>
        /// <param name="area">string (departement, jobtitle etc.)</param>
        /// <param name="procuration">bool</param>
        public JobDescription(string occupation, string area, bool procuration = false)
        {
            this.id = 0;
            this.occupation = occupation;
            this.area = area;
            this.procuration = procuration;
        }

        /// <summary>
        /// Constructor for adding a job descripton from Db to List
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="occupation">string</param>
        /// <param name="area">string</param>
        /// <param name="procuration">bool</param>
        public JobDescription(int id, string occupation, string area, bool procuration)
        {
            this.id = id;
            this.occupation = occupation;
            this.area = area;
            this.procuration = procuration;
        }

        /// <summary>
        /// Constructor for adding a job descripton from Db to List
        /// </summary>
        /// <param name="JobDescription">description</param>
        public JobDescription(JobDescription description)
        {
                this.id = description.Id;
                this.occupation = description.Occupation;
                this.area = description.Area;
                this.procuration = description.Procuration;
        }
        
        #endregion

        #region Methods
        public void ToggleProcuration()
        {
            if (procuration)
            {
                procuration = false;
            }
            else
            {
                procuration = true;
            }
        }

        /// <summary>
        /// Returns main content as a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return occupation;
        }

        #endregion

        #region Properties
        public int Id { get => id; }
        public string Occupation { get => occupation; set => occupation = value; }
        public string Area { get => area; set => area = value; }
        public bool Procuration { get => procuration; }
        #endregion

    }
}
