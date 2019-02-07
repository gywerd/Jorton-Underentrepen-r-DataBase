using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class Contact
    {
        #region Fields
        private int id;
        private int person;
        protected int entity;
        protected string area;

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public Contact()
        {
            this.id = 0;
            person = 0;
            entity = 0;
            area = "";

        }

        /// <summary>
        /// Costructor to add a a new ContactPerson
        /// </summary>
        /// <param name="person">int</param>
        /// <param name="entity">int</param>
        /// <param name="area">string</param>
        public Contact(int person, int entity, string area)
        {
            this.id = 0;
            this.person = person;
            this.entity = entity;
            this.area = area;
        }

        /// <summary>
        /// Costructor to add a a ContactPerson from Db
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="person">int</param>
        /// <param name="entity">int</param>
        /// <param name="area">string</param>
        public Contact(int id, int person, int entity, string area)
        {
            this.id = id;
            this.person = person;
            this.entity = entity;
            this.area = area;
        }

        /// <summary>
        /// Constructor, that accepts data from an existing Contact
        /// </summary>
        /// <param name="contact">Contact</param>
        public Contact(Contact contact)
        {
            this.id = contact.Id;
            this.person = contact.Person;
            this.entity = contact.Entity;
            this.area = contact.Area;
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

        #endregion

        #region Properties
        public int Id { get => id; }

        public int Person { get => person; }

        public int Entity { get => entity; set => entity = value; }

        public string Area
        {
            get => area;
            set
            {
                try
                {
                    area = value;
                }
                catch (Exception)
                {
                    area = "";
                }
            }
        }

        #endregion

    }
}
