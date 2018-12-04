using JudDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JudRepository
{
    public class Enterprise
    {
        #region Fields
        protected int id;
        protected Project project;
        protected string name;
        protected string elaboration;
        protected string offerList;
        protected CraftGroup craftGroup1;
        protected CraftGroup craftGroup2;
        protected CraftGroup craftGroup3;
        protected CraftGroup craftGroup4;

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public Enterprise()
        {
            this.id = 0;
            project = new Project();
            name = "";
            elaboration = "";
            offerList = "";
            craftGroup1 = new CraftGroup();
            craftGroup2 = new CraftGroup();
            craftGroup3 = new CraftGroup();
            craftGroup4 = new CraftGroup();
        }

        /// <summary>
        /// Constructor for adding new Enterprise
        /// </summary>
        /// <param name="project">int</param>
        /// <param name="name">string</param>
        /// <param name="craftGroup1">int</param>
        /// <param name="elaboration">string</param>
        /// <param name="offerList">string</param>
        /// <param name="craftGroup2">int</param>
        /// <param name="craftGroup3">int</param>
        /// <param name="craftGroup4">int</param>
        public Enterprise(Project project, string name, CraftGroup craftGroup1, CraftGroup craftGroup2, CraftGroup craftGroup3, CraftGroup craftGroup4, string elaboration = "", string offerList = "")
        {
            this.id = 0;
            this.project = project;
            this.name = name;
            this.elaboration = elaboration;
            this.offerList = offerList;
            this.craftGroup1 = craftGroup1;
            this.craftGroup2 = craftGroup2;
            this.craftGroup3 = craftGroup3;
            this.craftGroup4 = craftGroup4;
        }

        /// <summary>
        /// Constructor for adding Enterprise from Db
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="project">Project</param>
        /// <param name="name">string</param>
        /// <param name="elaboration">string</param>
        /// <param name="offerList">string</param>
        /// <param name="craftGroup1">CraftGroup</param>
        /// <param name="craftGroup2">CraftGroup</param>
        /// <param name="craftGroup3">CraftGroup</param>
        /// <param name="craftGroup4">CraftGroup</param>
        public Enterprise(int id, Project project, string name, string elaboration, string offerList, CraftGroup craftGroup1, CraftGroup craftGroup2, CraftGroup craftGroup3, CraftGroup craftGroup4)
        {
            this.id = id;
            this.project = project;
            this.name = name;
            this.elaboration = elaboration;
            this.offerList = offerList;
            this.craftGroup1 = craftGroup1;
            this.craftGroup2 = craftGroup2;
            this.craftGroup3 = craftGroup3;
            this.craftGroup4 = craftGroup4;
        }

        /// <summary>
        /// Constructor for that accepts data from existing Indexed Enterprise
        /// </summary>
        /// <param name="indexEnterprise">IndexedEnterprise</param>
        public Enterprise(IndexedEnterprise indexEnterprise)
        {
            this.id = indexEnterprise.id;
            this.project = indexEnterprise.Project;
            this.name = indexEnterprise.Name;
            this.elaboration = indexEnterprise.Elaboration;
            this.offerList = indexEnterprise.OfferList;
            this.craftGroup1 = indexEnterprise.CraftGroup1;
            this.craftGroup2 = indexEnterprise.CraftGroup2;
            this.craftGroup3 = indexEnterprise.CraftGroup3;
            this.craftGroup4 = indexEnterprise.CraftGroup4;
        }

        /// <summary>
        /// Constructor for that accepts data from existing Enterprise
        /// </summary>
        /// <param name="enterprise">Enterprise</param>
        public Enterprise(Enterprise enterprise)
        {
            this.id = enterprise.id;
            this.project = enterprise.Project;
            this.name = enterprise.Name;
            this.elaboration = enterprise.Elaboration;
            this.offerList = enterprise.OfferList;
            this.craftGroup1 = enterprise.CraftGroup1;
            this.craftGroup2 = enterprise.CraftGroup2;
            this.craftGroup3 = enterprise.CraftGroup3;
            this.craftGroup4 = enterprise.CraftGroup4;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Method, that returns main info as a string
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

        public string Elaboration
        {
            get => elaboration;
            set
            {
                try
                {
                    elaboration = value;
                }
                catch (Exception)
                {
                    elaboration = "";
                }
            }
        }

        public string OfferList
        {
            get => offerList;
            set
            {
                try
                {
                    offerList = value;
                }
                catch (Exception)
                {
                    offerList = "";
                }
            }
        }

        public CraftGroup CraftGroup1 { get; set; }

        public CraftGroup CraftGroup2 { get; set; }

        public CraftGroup CraftGroup3 { get; set; }

        public CraftGroup CraftGroup4 { get; set; }

        #endregion
    }
}
