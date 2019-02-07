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
        private string text;


        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public Category()
        {
            this.id = 0;
            this.text = "";
        }

        /// <summary>
        /// Constructor to add a new Category 
        /// </summary>
        /// <param name="text">string</param>
        public Category(string text)
        {
            this.id = 0;
            this.text = text;
        }

        /// <summary>
        /// Constructor to add a Category from Db
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="text">string</param>
        public Category(int id, string text)
        {
            this.id = id;
            this.text = text;
        }

        /// <summary>
        /// Constructor, that accepts data from an existing Category
        /// </summary>
        /// <param name="category">Category</param>
        public Category(Category category)
        {
            if (category != null)
            {
                this.id = category.Id;
                this.text = category.Text;
            }
            else
            {
                this.id = 0;
                this.text = "";
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
        /// Method, that returns main info as string
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            return text;
        }

        #endregion

        #region Properties
        public int Id { get => id; }

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

    }
}
