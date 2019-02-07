using JudDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace JudRepository
{
    public class Contact
    {
        #region Fields
        private int id;
        private Person person;
        protected Entrepeneur entrepeneur;
        protected string area;

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public Contact()
        {
            this.id = 0;
            person = new Person();
            entrepeneur = new Entrepeneur();
            area = "";

        }

        /// <summary>
        /// Costructor to add a a new ContactPerson
        /// </summary>
        /// <param name="person">Person</param>
        /// <param name="entrepeneur">Entrepeneur</param>
        /// <param name="area">string</param>
        public Contact(Person person, Entrepeneur entrepeneur, string area)
        {
            this.id = 0;
            this.person = person;
            this.entrepeneur = entrepeneur;
            this.area = area;
        }

        /// <summary>
        /// Costructor to add a a ContactPerson from Db
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="person">Person</param>
        /// <param name="entrepeneur">Entrepeneur</param>
        /// <param name="area">string</param>
        public Contact(int id, Person person, Entrepeneur entrepeneur, string area)
        {
            this.id = id;
            this.person = person;
            this.entrepeneur = entrepeneur;
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
                this.entrepeneur = contact.Entity;
                this.area = contact.Area;
        }

        /// <summary>
        /// Constructor, that accepts data from an existing Indexed Contact
        /// </summary>
        /// <param name="contact">IndexedContact</param>
        public Contact(IndexedContact contact)
        {
            this.id = contact.Id;
            this.person = contact.Person;
            this.entrepeneur = contact.Entity;
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

        /// <summary>
        /// Method, that returns main info as string
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            return person.Name + " (" + area + ")";
        }

        #endregion

        #region Properties
        public int Id { get => id; }

        public Person Person { get => person; set => person = value; }

        public Entrepeneur Entity { get => entrepeneur; set => entrepeneur = value; }

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
