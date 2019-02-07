using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudRepository
{
    public class Authentication
    {
        #region Fields
        private int id;
        private UserLevel userLevel;
        private string passWord;

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public Authentication()
        {
            this.id = 0;
            this.userLevel = new UserLevel();
            this.passWord = "";
        }

        /// <summary>
        /// Constructor to add a new address
        /// </summary>
        /// <param name="street">string</param>
        /// <param name="zipTown">ZipTown</param>
        /// <param name="place">string</param>
        public Authentication(UserLevel userLevel, string text)
        {
            this.id = 0;
            this.userLevel = userLevel;
            this.passWord = text;
        }

        /// <summary>
        /// Constructor to add address from Db to list
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="street">string</param>
        /// <param name="place">string</param>
        /// <param name="zipTown">ZipTown</param>
        public Authentication(int id, UserLevel userLevel, string text)
        {
            this.id = id;
            this.userLevel = userLevel;
            this.passWord = text;
        }

        /// <summary>
        /// Constructor, that accepts data from an existing Address
        /// </summary>
        /// <param name="authentication">Address</param>
        public Authentication(Authentication authentication)
        {
            this.id = authentication.Id;
            this.userLevel = authentication.UserLevel;
            this.passWord = authentication.PassWord;
        }

        #endregion

        #region Properties
        public int Id { get => id; }

        public UserLevel UserLevel { get => userLevel; set => userLevel = value; }

        public string PassWord
        {
            get => passWord;
            set
            {
                try
                {
                    passWord = value;
                }
                catch (Exception)
                {
                    passWord = "";
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
            return userLevel.ToString();
        }

        #endregion

    }
}
