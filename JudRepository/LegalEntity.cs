using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudRepository
{
    public class LegalEntity
    {
        #region Fields
        private int id;
        private string cvr;
        private string name;
        private string coName;
        private Address address;
        private ContactInfo contactInfo;
        private string url;

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public LegalEntity()
        {
            this.id = 0;
            this.cvr = "";
            this.name = "";
            this.coName = "";
            this.address = new Address();
            this.contactInfo = new ContactInfo();
            this.url = "";
        }

        /// <summary>
        /// Costructor to add a a new legal entity
        /// </summary>
        /// <param name="cvr">string</param>
        /// <param name="name">string</param>
        /// <param name="coName">string</param>
        /// <param name="address">string</param>
        /// <param name="contactInfo">int</param>
        /// <param name="url">string</param>
        public LegalEntity(string cvr, string name, string coName, Address address, ContactInfo contactInfo, string url)
        {
            this.id = 0;
            this.cvr = cvr;
            this.name = name;
            this.coName = coName;
            this.address = address;
            this.contactInfo = contactInfo;
            this.url = url;
        }

        /// <summary>
        /// Costructor to add a a legal entity from Db to list
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="name">string</param>
        /// <param name="coName">string</param>
        /// <param name="address">Address</param>
        /// <param name="contactInfo">ContactInfo</param>
        /// <param name="url">string</param>
        public LegalEntity(int id, string cvr, string name, string coName, Address address, ContactInfo contactInfo, string url)
        {
            this.id = id;
            this.cvr = cvr;
            this.name = name;
            this.coName = coName;
            this.address = address;
            this.contactInfo = contactInfo;
            this.url = url;
        }

        /// <summary>
        /// Constructor, that accepts data from an existing Legal Entity
        /// </summary>
        /// <param name="entity">LegalEntity</param>
        public LegalEntity(LegalEntity entity)
        {
            this.id = entity.Id;
            this.cvr = entity.Cvr;
            this.name = entity.Name;
            this.coName = entity.CoName;
            this.address = entity.Address;
            this.contactInfo = entity.ContactInfo;
            this.url = entity.Url;
        }

        #endregion

        #region Properties
        public int Id { get => id; }

        public string Cvr
        {
            get => cvr;
            set
            {
                try
                {
                    cvr = value;
                }
                catch (Exception)
                {
                    cvr = "";
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

        public string CoName
        {
            get => coName;
            set
            {
                try
                {
                    coName = value;
                }
                catch (Exception)
                {
                    coName = "";
                }
            }
        }

        public Address Address { get => address; set => address = value; }

        public ContactInfo ContactInfo { get => contactInfo; set => contactInfo = value; }

        public string Url
        {
            get => url;
            set
            {
                try
                {
                    url = value;
                }
                catch (Exception)
                {
                    url = "";
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
        /// Returns main content as a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string result = name + ", (" + Cvr + ")";
            return result;
        }

        #endregion

    }
}
