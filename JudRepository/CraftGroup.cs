using JudDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JudRepository
{
    public class CraftGroup
    {
        #region Fields
        private int id;
        private Category category;
        private string designation;
        private string description;
        private bool active;

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public CraftGroup()
        {
            this.id = 0;
            this.category = new Category();
            this.designation = "";
            this.description = "";
            this.active = false;
        }

        /// <summary>
        /// Constructor to add a new Craft Group
        /// </summary>
        /// <param name="category">int</param>
        /// <param name="designation">string</param>
        /// <param name="description">string</param>
        /// <param name="active">bool</param>
        public CraftGroup(Category category, string designation, string description, bool active)
        {
            this.id = 0;
            this.category = category;
            this.designation = designation;
            this.description = description;
            this.active = active;
        }

        /// <summary>
        /// Constructor to add a Craft Group from Db
        /// </summary>
        /// <param name="id">string</param>
        /// <param name="active">bool</param>
        /// <param name="category">int</param>
        /// <param name="designation">string</param>
        /// <param name="description">string</param>
        public CraftGroup(int id, Category category, string designation, string description, bool active)
        {
            this.id = id;
            this.category = category;
            this.designation = designation;
            this.description = description;
            this.active = active;
        }

        /// <summary>
        /// Constructor, that accepts data from an existing Craft Group
        /// </summary>
        /// <param name="craftGroup">CraftGroup</param>
        public CraftGroup(CraftGroup craftGroup)
        {
            this.id = craftGroup.Id;
            this.category = craftGroup.Category;
            this.designation = craftGroup.Designation;
            this.description = craftGroup.Description;
            this.active = craftGroup.Active;
        }

        #endregion

        #region Properties
        public int Id { get => id; }

        public Category Category { get => category; set => category = value; }
        public string Designation
        {
            get => designation;
            set
            {
                try
                {
                    designation = value;
                }
                catch (Exception)
                {
                    designation = "";
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

        public bool Active { get => active; }

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
        public void ToggleActive()
        {
            if (active)
            {
                active = false;
            }
            else
            {
                active = true;
            }
        }

        /// <summary>
        /// Method, that returns main info as string
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            return designation;
        }

        #endregion

    }
}
