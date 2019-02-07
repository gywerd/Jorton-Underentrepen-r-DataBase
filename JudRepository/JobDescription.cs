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
        /// Costructor to add a a new JobDescripton
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
        /// Costructor to add a a Jobdescripton from Db to List
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
        /// Constructor, that accepts data from an existing job descripton from Db to List
        /// </summary>
        /// <param name="JobDescription">description</param>
        public JobDescription(JobDescription jobDescription)
        {
                this.id = jobDescription.Id;
                this.occupation = jobDescription.Occupation;
                this.area = jobDescription.Area;
                this.procuration = jobDescription.Procuration;
        }

        #endregion

        #region Properties
        public int Id { get => id; }

        public string Occupation
        {
            get => occupation;
            set
            {
                try
                {
                    occupation = value;
                }
                catch (Exception)
                {
                    occupation = "";
                }
            }
        }


        public string Area
        {
            get => area;
            set
            {
                try
                {
                    area = value;
                }
                catch (Exception)
                {
                    area = "";
                }
            }
        }

        public bool Procuration { get => procuration; }
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
        /// Method, that switches Procuration between true and false
        /// </summary>
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

    }
}
