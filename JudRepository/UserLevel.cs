using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudRepository
{
    public class UserLevel
    {
        #region Fields
        private int id;
        private string text;

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public UserLevel()
        {
            this.id = 0;
            this.text = "";
        }

        /// <summary>
        /// Constructor to add a new address
        /// </summary>
        /// <param name="street">string</param>
        /// <param name="zipTown">ZipTown</param>
        /// <param name="place">string</param>
        public UserLevel(string text)
        {
            this.id = 0;
            this.text = text;
        }

        /// <summary>
        /// Constructor to add address from Db to list
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="street">string</param>
        /// <param name="place">string</param>
        /// <param name="zipTown">ZipTown</param>
        public UserLevel(int id, string text)
        {
            this.id = id;
            this.text = text;
        }

        /// <summary>
        /// Constructor, that accepts data from an existing Address
        /// </summary>
        /// <param name="userLevel">Address</param>
        public UserLevel(UserLevel userLevel)
        {
            this.id = userLevel.Id;
            this.text = userLevel.Text;
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
        /// Returns main content as a string with one row
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            return text;
        }

        #endregion

    }
}
