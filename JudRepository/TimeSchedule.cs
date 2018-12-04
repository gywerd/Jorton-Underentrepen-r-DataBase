using JudDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JudRepository
{
    public class TimeSchedule
    {
        #region Fields
        private int id;
        private Project project;
        private string text;

        #endregion

        #region Constructors
        /// <summary>
        /// Empty constructor
        /// </summary>
        public TimeSchedule()
        {
            this.id = 0;
            this.project = new Project();
            this.text = "";
        }

        public TimeSchedule(Project project, string text)
        {
            this.id = 0;
            this.project = project;
            this.text = text;
        }

        public TimeSchedule(int id, Project project, string text)
        {
            this.id = id;
            this.project = project;
            this.text = text;
        }

        public TimeSchedule(TimeSchedule schedule)
        {
            this.id = schedule.Id;
            this.project = schedule.Project;
            this.text = schedule.Text;
        }

        #endregion

        #region Methods
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