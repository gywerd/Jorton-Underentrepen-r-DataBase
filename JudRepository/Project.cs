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
        private int id;
        private int caseId;
        private string name;
        private Builder builder;
        private ProjectStatus status;
        private TenderForm tenderForm;
        private EnterpriseForm enterpriseForm;
        private User executive;
        private bool enterpriseList;
        private bool copy;

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public Project()
        {
            this.id = 0;
            this.name = "";
            this.builder = new Builder();
            this.status = new ProjectStatus();
            this.tenderForm = new TenderForm();
            this.enterpriseForm = new EnterpriseForm();
            this.executive = new User();
            enterpriseList = false;
            copy = false;
        }

        /// <summary>
        /// Constructor to add new project
        /// </summary>
        /// <param name="caseId">int</param>
        /// <param name="name">string</param>
        /// <param name="builder">Builder</param>
        /// <param name="status">ProjectStatus</param>
        /// <param name="tenderForm">TenderForm</param>
        /// <param name="enterpriseForm">EnterpriseForm</param>
        /// <param name="executive">User</param>
        /// <param name="enterPriseList">bool</param>
        /// <param name="copy">bool</param>
        public Project(int caseId, string name, Builder builder, ProjectStatus status, TenderForm tenderForm, EnterpriseForm enterpriseForm, User executive, bool enterpriseList = false,  bool copy = false)
        {
            this.id = 0;
            this.caseId = caseId;
            this.name = name;
            this.builder = builder;
            this.status = status;
            this.enterpriseList = enterpriseList;
            this.tenderForm = tenderForm;
            this.enterpriseForm = enterpriseForm;
            this.executive = executive;
            this.copy = copy;
        }

        /// <summary>
        /// Constructor to add project from Db to List
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="caseId">int</param>
        /// <param name="name">string</param>
        /// <param name="builder">string</param>
        /// <param name="status">int</param>
        /// <param name="tenderForm">int</param>
        /// <param name="enterpriseForm">int</param>
        /// <param name="executive">int</param>
        /// <param name="enterPriseList">bool</param>
        /// <param name="copy">bool</param>
        public Project(int id, int caseId, string name, Builder builder, ProjectStatus status, TenderForm tenderForm, EnterpriseForm enterpriseForm, User executive, bool enterpriseList, bool copy = false)
        {
            this.id = id;
            this.caseId = caseId;
            this.name = name;
            this.builder = builder;
            this.status = status;
            this.tenderForm = tenderForm;
            this.enterpriseForm = enterpriseForm;
            this.executive = executive;
            this.enterpriseList = enterpriseList;
            this.copy = copy;
        }

        /// <summary>
        /// Constructor that receives data from an existing Project
        /// </summary>
        /// <param name="project"></param>
        public Project(Project project)
        {
            this.id = project.Id;
            this.caseId = project.CaseId;
            this.name = project.Name;
            this.builder = project.Builder;
            this.status = project.Status;
            this.tenderForm = project.TenderForm;
            this.enterpriseForm = project.EnterpriseForm;
            this.executive = project.Executive;
            this.enterpriseList = project.EnterprisesList;
            this.copy = project.Copy;
        }

        #endregion

        #region Methods
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
        public void ToggleEnterprises()
        {
            enterpriseList = true;
        }

        /// <summary>
        /// Returns main content as a string
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            string result = caseId + " " + name;
            return result;
        }

        #endregion

        #region Properties
        public int Id { get => id; }

        public int CaseId
        {
            get => caseId;
            set
            {
                try
                {
                    caseId = value;
                }
                catch (Exception)
                {
                    caseId = 0;
                }
            }
        }

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

        public Builder Builder { get; set; }

        public ProjectStatus Status { get; set; }

        public TenderForm TenderForm { get; set; }

        public EnterpriseForm EnterpriseForm { get; set; }

        public User Executive { get; set; }

        public bool EnterprisesList { get => enterpriseList; }

        public bool Copy { get => copy; }

        #endregion
    }
}
