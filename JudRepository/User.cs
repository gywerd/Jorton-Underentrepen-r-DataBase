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
        private Person person;
        private string initials;
        private JobDescription jobDescription;
        private Authentication authentication;

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public User()
        {
            this.id = 0;
            this.person = new Person();
            this.initials = "";
            this.jobDescription = new JobDescription();
            this.authentication = new Authentication();
        }

        /// <summary>
        /// Constructor to add a new User
        /// </summary>
        /// <param name="person">Person</param>
        /// <param name="initials">string</param>
        /// <param name="jobDescription">JobDescription</param>
        /// <param name="authentication">Authentication</param>
        public User(Person person, string initials, JobDescription jobDescription, Authentication authentication)
        {
            this.id = 0;
            this.person = person;
            this.initials = initials;
            this.jobDescription = jobDescription;
            this.authentication = authentication;
        }

        /// <summary>
        /// Constructor to add a User from Db
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="person">Person</param>
        /// <param name="initials">string</param>
        /// <param name="jobDescription">JobDescription</param>
        /// <param name="authentication">Authentication</param>
        public User(int id, Person person, string initials, JobDescription jobDescription, Authentication authentication)
        {
            this.id = id;
            this.person = person;
            this.initials = initials;
            this.jobDescription = jobDescription;
            this.authentication = authentication;
        }

        /// <summary>
        /// Constructor, that accepts data from an existing User
        /// </summary>
        /// <param name="user">User</param>
        public User(User user)
        {
            this.id = user.Id;
            this.person = user.Person;
            this.initials = user.Initials;
            this.jobDescription = user.JobDescription;
            this.authentication = user.Authentication;
        }

        /// <summary>
        /// Constructor, that accepts data from an existing Indexed User
        /// </summary>
        /// <param name="user">IndexedUser</param>
        public User(IndexedUser user)
        {
            this.id = user.Id;
            this.person = user.Person;
            this.initials = user.Initials;
            this.jobDescription = user.JobDescription;
            this.authentication = user.Authentication;
        }

        #endregion

        #region Properties
        public int Id { get => id; }

        public Person Person { get => person; set => person = value; }

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

        public JobDescription JobDescription { get => jobDescription; set => jobDescription = value; }

        public Authentication Authentication { get => authentication; set => authentication = value; }

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
            bool result = false;

            if (authentication.PassWord == oldPassWord)
            {
                authentication.PassWord = newPassWord;
                result = true;
            }

            return result;
        }

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
        /// Method returns user name as string
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            return person.Name + " (" + initials + ")";

        }

        #endregion

    }
}
