using JudDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudRepository
{
    public class Builder
    {
        #region Fields
        private int id;
        private string cvr;
        private string name;
        private Address address;
        private ContactInfo contactInfo;
        private string url;

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public Builder()
        {
            id = 0;
            cvr = "0";
            name = "";
            address = new Address();
            contactInfo = new ContactInfo();
            url = "";
        }

        /// <summary>
        /// Constructor for adding new builder
        /// </summary>
        /// <param name="cvr">string</param>
        /// <param name="name">string</param>
        /// <param name="address">Address</param>
        /// <param name="contactInfo">ContactInfo</param>
        /// <param name="url">string</param>
        public Builder(string cvr, string name, Address address, ContactInfo contactInfo, string url = "")
        {
            this.id = 0;
            this.cvr = cvr;
            this.name = name;
            this.address = address;
            this.contactInfo = contactInfo;
            this.url = url;
        }

        /// <summary>
        /// Constructor for adding a builder from Db to list
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="cvr">string</param>
        /// <param name="name">string</param>
        /// <param name="address">Address</param>
        /// <param name="contactInfo">ContactInfo</param>
        /// <param name="url">string</param>
        public Builder(int id, string cvr, string name, Address address, ContactInfo contactInfo, string url)
        {
            this.id = id;
            this.name = name;
            this.address = address;
            this.contactInfo = contactInfo;
            this.url = url;
        }

        /// <summary>
        /// Constructor, that accepts an existing builder
        /// </summary>
        /// <param name="builder">Builder</param>
        public Builder(Builder builder)
        {
            this.id = builder.Id;
            this.name = builder.Name;
            this.address = builder.Address;
            this.contactInfo = builder.ContactInfo;
            this.url = builder.Url;
        }


        #endregion

        #region Methods
        /// <summary>
        /// Returns main content as a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string result = name;
            return result;
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

        public Address Address { get; set; }

        public ContactInfo ContactInfo { get; set; }

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

    }
}

