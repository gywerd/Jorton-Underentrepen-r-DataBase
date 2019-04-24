using JudDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudRepository
{
    public class Project
    {
        #region Fields
        private int id = 0;
        private int _case = 0;
        private Builder builder = new Builder();
        private ProjectStatus status = new ProjectStatus();
        private TenderForm tenderForm = new TenderForm();
        private EnterpriseForm enterpriseForm = new EnterpriseForm();
        private User executive = new User();
        private ProjectDetail details = new ProjectDetail();
        private bool enterpriseList = false;
        private bool copy = false;

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public Project() { }

        /// <summary>
        /// Constructor to add a new project
        /// </summary>
        /// <param name="Case">int</param>
        /// <param name="builder">Builder</param>
        /// <param name="status">ProjectStatus</param>
        /// <param name="tenderForm">TenderForm</param>
        /// <param name="enterpriseForm">EnterpriseForm</param>
        /// <param name="executive">User</param>
        /// <param name="details">ProjectDetail</param>
        /// <param name="enterPriseList">bool</param>
        /// <param name="copy">bool</param>
        public Project(int Case, Builder builder, ProjectStatus status, TenderForm tenderForm, EnterpriseForm enterpriseForm, User executive, ProjectDetail details, bool enterpriseList = false,  bool copy = false)
        {
            this.id = 0;
            this._case = Case;
            this.builder = builder;
            this.status = status;
            this.enterpriseList = enterpriseList;
            this.tenderForm = tenderForm;
            this.enterpriseForm = enterpriseForm;
            this.executive = executive;
            this.details = details;
            this.enterpriseList = enterpriseList;
            this.copy = copy;
        }

        /// <summary>
        /// Constructor to add a project from Db to List
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="Case">int</param>
        /// <param name="builder">string</param>
        /// <param name="status">int</param>
        /// <param name="tenderForm">int</param>
        /// <param name="enterpriseForm">int</param>
        /// <param name="executive">int</param>
        /// <param name="details">ProjectDetail</param>
        /// <param name="enterPriseList">bool</param>
        /// <param name="copy">bool</param>
        public Project(int id, int Case, Builder builder, ProjectStatus status, TenderForm tenderForm, EnterpriseForm enterpriseForm, User executive, ProjectDetail details, bool enterpriseList, bool copy = false)
        {
            this.id = id;
            this._case = Case;
            this.builder = builder;
            this.status = status;
            this.tenderForm = tenderForm;
            this.enterpriseForm = enterpriseForm;
            this.executive = executive;
            this.details = details;
            this.enterpriseList = enterpriseList;
            this.copy = copy;
        }

        /// <summary>
        /// Constructor, that accepts data from an existing Project
        /// </summary>
        /// <param name="project"></param>
        public Project(Project project)
        {
            this.id = project.Id;
            this._case = project.Case;
            this.builder = project.Builder;
            this.status = project.Status;
            this.tenderForm = project.TenderForm;
            this.enterpriseForm = project.EnterpriseForm;
            this.executive = project.Executive;
            this.details = project.Details;
            this.enterpriseList = project.EnterpriseList;
            this.copy = project.Copy;
        }

        /// <summary>
        /// Constructor, that accepts data from an existing Indexed Project
        /// </summary>
        /// <param name="project">IndexedProject</param>
        public Project(IndexedProject project)
        {
            this.id = project.Id;
            this._case = project.Case;
            this.builder = project.Builder;
            this.status = project.Status;
            this.tenderForm = project.TenderForm;
            this.enterpriseForm = project.EnterpriseForm;
            this.executive = project.Executive;
            this.details = project.Details;
            this.enterpriseList = project.EnterpriseList;
            this.copy = project.Copy;
        }

        #endregion

        #region Properties
        public int Id { get => id; }

        public int Case
        {
            get => _case;
            set
            {
                try
                {
                    _case = value;
                }
                catch (Exception)
                {
                    _case = 0;
                }
            }
        }

        public Builder Builder { get => builder; set => builder = value; }

        public ProjectStatus Status { get => status; set => status = value; }

        public TenderForm TenderForm { get => tenderForm; set => tenderForm = value; }

        public EnterpriseForm EnterpriseForm { get => enterpriseForm; set => enterpriseForm = value; }

        public User Executive { get => executive; set => executive = value; }

        public ProjectDetail Details { get => details; set => details = value; }

        public bool EnterpriseList { get => enterpriseList; }

        public bool Copy { get => copy; }

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
        /// Methods that toggles value of Copy field
        /// </summary>
        public void ToggleCopy()
        {
            if (copy)
            {
                copy = false;
            }
            else
            {
                copy = true;
            }
        }

        /// <summary>
        /// Methods that toggles value of Enterprises field
        /// </summary>
        public void ToggleEnterpriseList()
        {
            enterpriseList = true;
        }

        /// <summary>
        /// Returns main content as a string
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            string result = _case + " " + details.Name;
            return result;
        }

        #endregion

    }
}
