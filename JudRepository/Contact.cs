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
        protected LegalEntity legalEntity;
        protected string name;
        protected string area;
        protected ContactInfo contactInfo;

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public Contact()
        {
            this.id = 0;
            legalEntity = new LegalEntity();
            name = "";
            area = "";
            contactInfo = new ContactInfo();

        }

        /// <summary>
        /// Constructor for adding ContactPerson
        /// </summary>
        /// <param name="legalEntity">int</param>
        /// <param name="name">int</param>
        /// <param name="description">string</param>
        /// <param name="email">string</param>
        /// <param name="mobile">string</param>
        public Contact(LegalEntity legalEntity, string name, string description, ContactInfo contactInfo)
        {
            this.id = 0;
            this.legalEntity = legalEntity;
            this.name = name;
            this.area = description;
            this.contactInfo = contactInfo;
        }

        /// <summary>
        /// Constructor for adding ContactPerson from Db
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="legalEntityId">int</param>
        /// <param name="name">int</param>
        /// <param name="description">string</param>
        /// <param name="email">string</param>
        /// <param name="mobile">string</param>
        public Contact(int id, LegalEntity legalEntity, string name, string description, ContactInfo contactInfo)
        {
            this.id = id;
            this.legalEntity = legalEntity;
            this.name = name;
            this.area = description;
            this.contactInfo = contactInfo;
        }

        /// <summary>
        /// Constructor that accepts data from existing Contact
        /// </summary>
        /// <param name="contact">Contact</param>
        public Contact(Contact contact)
        {
            if (contact != null)
            {
                this.id = contact.Id;
                this.legalEntity = contact.LegalEntity;
                this.name = contact.Name;
                this.area = contact.Area;
                this.contactInfo = contact.ContactInfo;
            }
            else
            {
                id = 0;
                legalEntity = new LegalEntity();
                name = "";
                area = "";
                contactInfo = new ContactInfo();
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
            string tempName = name;
            if (area != "")
            {
                tempName += " (" + area + ")";
            }
            return tempName;
        }

        #endregion

        #region Properties
        public int Id { get => id; }

        public LegalEntity LegalEntity { get; set; }
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

        public ContactInfo ContactInfo { get; set; }

        #endregion

    }
}
