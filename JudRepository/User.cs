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
        private UserLevel userLevel;

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
            this.userLevel = new UserLevel();
        }

        /// <summary>
        /// Constructor to add a new User
        /// </summary>
        /// <param name="person">Person</param>
        /// <param name="initials">string</param>
        /// <param name="jobDescription">JobDescription</param>
        /// <param name="userLevel">UserLevel</param>
        public User(Person person, string initials, JobDescription jobDescription, UserLevel userLevel)
        {
            this.id = 0;
            this.person = person;
            this.initials = initials;
            this.jobDescription = jobDescription;
            this.userLevel = userLevel;
        }

        /// <summary>
        /// Constructor to add a User from Db
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="person">Person</param>
        /// <param name="initials">string</param>
        /// <param name="jobDescription">JobDescription</param>
        /// <param name="userLevel">UserLevel</param>
        public User(int id, Person person, string initials, JobDescription jobDescription, UserLevel userLevel)
        {
            this.id = id;
            this.person = person;
            this.initials = initials;
            this.jobDescription = jobDescription;
            this.userLevel = userLevel;
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
            this.userLevel = user.UserLevel;
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
            this.userLevel = user.UserLevel;
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

        public UserLevel UserLevel { get => userLevel; set => userLevel = value; }

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
