using JudDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudRepository
{
    public class Paragraf
    {
        #region Fields
        protected int id = 0;
        protected Project project = new Project();
        protected string text = "";

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public Paragraf() { }

        /// <summary>
        /// Costructor to add a a new Paragraf
        /// </summary>
        /// <param name="project">int</param>
        /// <param name="text">string</param>
        public Paragraf(Project project, string text)
        {
            this.project = project;
            this.text = text;
        }

        /// <summary>
        /// Costructor to add a a Paragraf from Db
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="project">int</param>
        /// <param name="text">string</param>
        public Paragraf(int id, Project project, string text)
        {
            this.id = id;
            this.project = project;
            this.text = text;
        }

        /// <summary>
        /// Constructor, that accepts data from an existing Paragraf
        /// </summary>
        /// <param name="paragraf">Paragraf</param>
        public Paragraf(Paragraf paragraf)
        {
            if (paragraf != null)
            {
                this.id = paragraf.id;
                this.project = paragraf.Project;
                this.text = paragraf.Text;
            }

            else
            {
                this.id = 0;
                this.project = new Project();
                this.text = "";
            }
        }

        /// <summary>
        /// Constructor, that accepts data from an existing Indexed Paragraf
        /// </summary>
        /// <param name="paragraf">IndexedParagraf</param>
        public Paragraf(IndexedParagraf paragraf)
        {
            this.id = paragraf.id;
            this.project = paragraf.Project;
            this.text = paragraf.Text;
        }

        #endregion

        #region Properties
        public int Id { get => id; }

        public Project Project { get => project; set => project = value; }

        public string Text
        {
            get => text;
            set
            {
                try
                {
                    text = value;
                }
                catch (Exception)
                {
                    text = "";
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
        /// Method, that returns main info as a string
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            return text;
        }

        #endregion

    }
}
