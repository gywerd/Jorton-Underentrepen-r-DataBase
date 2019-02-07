using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudRepository
{
    public class Person
    {
        #region Fields
        private int id;
        private string name;
        private ContactInfo contactInfo;

        #endregion

        #region Constructors
        /// <summary>
        /// Empty constructor
        /// </summary>
        public Person()
        {
            this.id = 0;
            this.name = "";
            this.contactInfo = new ContactInfo();
        }

        /// <summary>
        /// Constructor to create new Person
        /// </summary>
        /// <param name="name"></param>
        /// <param name="contactInfo"></param>
        public Person(string name, ContactInfo contactInfo)
        {
            this.id = 0;
            this.name = name;
            this.contactInfo = contactInfo;

        }

        /// <summary>
        /// Constructor to add Person from database
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="name">string</param>
        /// <param name="contactInfo">ContactInfo</param>
        public Person(int id, string name, ContactInfo contactInfo)
        {
            this.id = id;
            this.name = name;
            this.contactInfo = contactInfo;

        }

        /// <summary>
        /// Constructor that accepts data from existing Person
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="name">string</param>
        /// <param name="contactInfo">ContactInfo</param>
        public Person(Person person)
        {
            this.id = person.Id;
            this.name = person.Name;
            this.contactInfo = person.ContactInfo;

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

        public ContactInfo ContactInfo { get => contactInfo; set => contactInfo = value; }

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
            return "Navn: " + name + " / " + contactInfo.ToString();
        }

        /// <summary>
        /// Method, that returns main info as string with multiple lines
        /// </summary>
        /// <returns>string</returns>
        public string ToLongString()
        {
            return "Navn: " + name + "\n" + contactInfo.ToLongString();
        }

        #endregion

    }
}
