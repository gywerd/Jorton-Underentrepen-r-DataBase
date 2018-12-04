using JudDataAccess;
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
        private string id;
        private string name;
        private Address address;
        private ContactInfo contactInfo;
        private string url;
        private CraftGroup craftGroup1;
        private CraftGroup craftGroup2;
        private CraftGroup craftGroup3;
        private CraftGroup craftGroup4;
        private Region region;
        private bool countryWide;
        private bool cooperative;
        private bool active;

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public LegalEntity()
        {
            this.id = "";
            name = "";
            address = new Address();
            contactInfo = new ContactInfo();
            url = "";
            craftGroup1 = new CraftGroup();
            craftGroup2 = new CraftGroup();
            craftGroup3 = new CraftGroup();
            craftGroup4 = new CraftGroup();
            region = new Region();
            countryWide = false;
            cooperative = false;
            active = false;
        }

        /// <summary>
        /// Constructor for adding new legal entity
        /// </summary>
        /// <param name="name">string</param>
        /// <param name="address">int</param>
        /// <param name="contactInfo">int</param>
        /// <param name="url">string</param>
        /// <param name="craftGroup1">int</param>
        /// <param name="craftGroup2">int</param>
        /// <param name="craftGroup3">int</param>
        /// <param name="craftGroup4">int</param>
        /// <param name="region">int</param>
        /// <param name="countryWide">bool</param>
        /// <param name="cooperative">bool</param>
        /// <param name="active"></param>
        public LegalEntity(string name, Address address, ContactInfo contactInfo, string url, CraftGroup craftGroup1, CraftGroup craftGroup2, CraftGroup craftGroup3, CraftGroup craftGroup4, Region region, bool countryWide = false, bool cooperative = false, bool active=false)
        {
            this.id = "";
            this.name = name;
            this.address = address;
            this.contactInfo = contactInfo;
            this.url = url;
            this.craftGroup1 = craftGroup1;
            this.craftGroup2 = craftGroup2;
            this.craftGroup3 = craftGroup3;
            this.craftGroup4 = craftGroup4;
            this.region = region;
            this.countryWide = countryWide;
            this.cooperative = cooperative;
            this.active = active;
        }

        /// <summary>
        /// Constructor for adding a legal entity from Db to list
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="name">string</param>
        /// <param name="address">Address</param>
        /// <param name="contactInfo">ContactInfo</param>
        /// <param name="url">string</param>
        /// <param name="craftGroup1">CraftGroup</param>
        /// <param name="craftGroup2">CraftGroup</param>
        /// <param name="craftGroup3">CraftGroup</param>
        /// <param name="craftGroup4">CraftGroup</param>
        /// <param name="region">Region</param>
        /// <param name="countryWide">bool</param>
        /// <param name="cooperative">bool</param>
        /// <param name="active"></param>
        public LegalEntity(string id, string name, Address address, ContactInfo contactInfo, string url, CraftGroup craftGroup1, CraftGroup craftGroup2, CraftGroup craftGroup3, CraftGroup craftGroup4, Region region, bool countryWide, bool cooperative, bool active)
        {
            this.id = id;
            this.name = name;
            this.address = address;
            this.contactInfo = contactInfo;
            this.url = url;
            this.craftGroup1 = craftGroup1;
            this.craftGroup2 = craftGroup2;
            this.craftGroup3 = craftGroup3;
            this.craftGroup4 = craftGroup4;
            this.region = region;
            this.countryWide = countryWide;
            this.cooperative = cooperative;
            this.active = active;
        }

        /// <summary>
        /// Constructor that accepts data from existing Indexed Legal Entity
        /// </summary>
        /// <param name="entity">IndexedLegalEntity</param>
        public LegalEntity(IndexedLegalEntity entity)
        {
            this.id = entity.Id;
            this.name = entity.Name;
            this.address = entity.Address;
            this.contactInfo = entity.ContactInfo;
            this.url = entity.Url;
            this.craftGroup1 = entity.CraftGroup1;
            this.craftGroup2 = entity.CraftGroup2;
            this.craftGroup3 = entity.CraftGroup3;
            this.craftGroup4 = entity.CraftGroup4;
            this.region = entity.Region;
            this.countryWide = entity.CountryWide;
            this.cooperative = entity.Cooperative;
            this.active = entity.Active;
        }

        /// <summary>
        /// Constructor that accepts data from existing Legal Entity
        /// </summary>
        /// <param name="legalEntity">LegalEntity</param>
        public LegalEntity(LegalEntity legalEntity)
        {
            this.id = legalEntity.Id;
            this.name = legalEntity.Name;
            this.address = legalEntity.Address;
            this.contactInfo = legalEntity.ContactInfo;
            this.url = legalEntity.Url;
            this.craftGroup1 = legalEntity.CraftGroup1;
            this.craftGroup2 = legalEntity.CraftGroup2;
            this.craftGroup3 = legalEntity.CraftGroup3;
            this.craftGroup4 = legalEntity.CraftGroup4;
            this.region = legalEntity.Region;
            this.countryWide = legalEntity.CountryWide;
            this.cooperative = legalEntity.Cooperative;
            this.active = legalEntity.Active;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Returns main content as a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string result = name + ", (" + id + ")";
            return result;
        }

        #endregion

        #region Properties
        public string Id { get => id; }

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

        public CraftGroup CraftGroup1 { get; set; }

        public CraftGroup CraftGroup2 { get; set; }

        public CraftGroup CraftGroup3 { get; set; }

        public CraftGroup CraftGroup4 { get; set; }

        public Region Region { get; set; }
        public bool CountryWide
        {
            get => countryWide;
            set
            {
                try
                {
                    countryWide = value;
                }
                catch (Exception)
                {
                    countryWide = false;
                }
            }
        }

        public bool Cooperative
        {
            get => cooperative;
            set
            {
                try
                {
                    cooperative = value;
                }
                catch (Exception)
                {
                    cooperative = false;
                }
            }
        }

        public bool Active
        {
            get => active;
            set
            {
                try
                {
                    active = value;
                }
                catch (Exception)
                {
                    active = false;
                }
            }
        }

        #endregion

    }
}
