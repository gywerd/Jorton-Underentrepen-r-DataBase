using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudRepository
{
    public class ProjectDetail
    {
        #region Fields
        private int id = 0;
        private string name;
        private string description = "";
        private string period = "";
        private string answerDate = "";

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public ProjectDetail() { }

        /// <summary>
        /// Constructor to adding a new Project Detail
        /// </summary>
        /// <param name="description">string</param>
        /// <param name="period">string</param>
        /// <param name="answerDate">string</param>
        public ProjectDetail(string name, string description, string period, string answerDate)
        {
            this.name = name;
            this.description = description;
            this.period = period;
            this.answerDate = answerDate;
        }

        /// <summary>
        /// Constructor to adding a Description from Db
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="description">string</param>
        /// <param name="period">string</param>
        /// <param name="answerDate">string</param>
        public ProjectDetail(int id, string name, string description, string period, string answerDate)
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.period = period;
            this.answerDate = answerDate;
        }

        /// <summary>
        /// Constructor, that accepts data from an existing Description
        /// </summary>
        /// <param name="projectDetail">PdfData</param>
        public ProjectDetail(ProjectDetail projectDetail)
        {
            this.id = projectDetail.Id;
            this.name = projectDetail.Name;
            this.description = projectDetail.Description;
            this.period = projectDetail.Period;
            this.answerDate = projectDetail.AnswerDate;
        }

        #endregion

        #region Properties
        public int Id { get => id; }

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

        public string Description
        {
            get => description;
            set
            {
                try
                {
                    description = value;
                }
                catch (Exception)
                {

                    description = "";
                }
            }
        }

        public string Period
        {
            get => period;
            set
            {
                try
                {
                    period = value;
                }
                catch (Exception)
                {

                    period = "";
                }
            }
        }

        public string AnswerDate
        {
            get => answerDate;
            set
            {
                try
                {
                    answerDate = value;
                }
                catch (Exception)
                {

                    answerDate = "";
                }
            }
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
            string result = "Projektdetaljer for: " + name;
            return result;
        }

        #endregion

    }
}
