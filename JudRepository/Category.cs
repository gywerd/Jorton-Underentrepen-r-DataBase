using JudDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudRepository
{
    public class Category
    {
        #region Fields
        private int id;
        private string name;


        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public Category()
        {
            this.id = 0;
            this.name = "";
        }

        /// <summary>
        /// Constructor to add new Category 
        /// </summary>
        /// <param name="name">string</param>
        public Category(string name)
        {
            this.id = 0;
            this.name = name;
        }

        /// <summary>
        /// Constructor to add craft group from Db to list
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="name">string</param>
        public Category(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        /// <summary>
        /// Constructor, that accepts an existing Category
        /// </summary>
        /// <param name="category">Category</param>
        public Category(Category category)
        {
            if (category != null)
            {
                this.id = category.Id;
                this.name = category.Name;
            }
            else
            {
                this.id = 0;
                this.name = "";
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Method, that returns main info as string
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            return name;
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

        #endregion

    }
}
