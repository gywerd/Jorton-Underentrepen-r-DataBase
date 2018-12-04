using JudDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JudRepository
{
    public class User
    {
        #region Fields
        private int id;
        private string initials;
        private string name;
        private string passWord;
        private ContactInfo contactInfo;
        private JobDescription jobDescription;
        private bool administrator;

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constryctor
        /// </summary>
        public User()
        {
            this.id = 0;
            this.name = "";
            this.passWord = "1234";
            this.contactInfo = new ContactInfo();
            this.jobDescription = new JobDescription();
            this.administrator = false;
        }

        /// <summary>
        /// Constructor to add user
        /// </summary>
        /// <param name="initials">string</param>
        /// <param name="name">string</param>
        /// <param name="contactInfo">int</param>
        /// <param name="jobDescription">int</param>
        /// <param name="passWord">string</param>
        /// <param name="admin">bool</param>
        public User(string initials, string name, ContactInfo contactInfo, JobDescription jobDescription, string password = "1234", bool admin = false)
        {
            this.id = 0;
            this.initials = initials;
            this.name = name;
            this.passWord = password;
            this.contactInfo = contactInfo;
            this.jobDescription = jobDescription;
            this.administrator = admin;
        }

        /// <summary>
        /// Constructor to add user from Db to list
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="initials">string</param>
        /// <param name="name">string</param>
        /// <param name="contactInfo">int</param>
        /// <param name="jobDescription">int</param>
        /// <param name="passWord">string</param>
        /// <param name="admin">bool</param>
        public User(int id, string initials, string name, string passWord, ContactInfo contactInfo, JobDescription jobDescription, bool admin)
        {
            this.id = id;
            this.initials = initials;
            this.name = name;
            this.passWord = passWord;
            this.contactInfo = contactInfo;
            this.jobDescription = jobDescription;
            this.administrator = admin;
        }

        /// <summary>
        /// Constructor to add user from Db to list
        /// </summary>
        /// <param name="user">User</param>
        public User(User user)
        {
            this.id = user.Id;
            this.initials = user.Initials;
            this.name = user.Name;
            this.passWord = user.PassWord;
            this.contactInfo = user.ContactInfo;
            this.jobDescription = user.JobDescription;
            this.administrator = user.Administrator;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Method, that changes password, if eligible
        /// </summary>
        /// <param name="oldPassWord"></param>
        /// <param name="newPassWord"></param>
        /// <returns></returns>
        public bool ChangePassword(string oldPassWord, string newPassWord)
        {
            if (passWord == oldPassWord)
            {
                passWord = newPassWord;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Method returns user name as string
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            string result = name + " (" + initials + ")";
            return result;
        }

        #endregion

        #region Properties
        public int Id { get => id; }

        public string Initials
        {
            get => initials;
            set
            {
                try
                {
                    initials = value;
                }
                catch (Exception)
                {
                    initials = "";
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

        public ContactInfo ContactInfo { get; set; }

        public JobDescription JobDescription { get; set; }

        public string PassWord { get => passWord; }

        public bool Administrator { get => administrator; }
        #endregion
    }
}
