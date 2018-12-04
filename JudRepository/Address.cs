using JudDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudRepository
{
    public class Address : ZipTown
    {
        #region Fields
        private int id;
        private string street;
        private string place;

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public Address() : base("","")
        {
            this.id = 0;
            this.street = "";
            this.place = "";
        }

        /// <summary>
        /// Constructor to add a new address
        /// </summary>
        /// <param name="street">string</param>
        /// <param name="zipTown">int</param>
        /// <param name="place">string</param>
        public Address(string street, ZipTown zip, string place = "") : base(zip)
        {
            this.id = 0;
            this.street = street;
            this.place = place;
        }

        /// <summary>
        /// Constructor to add a new address
        /// </summary>
        /// <param name="street">string</param>
        /// <param name="place">string</param>
        /// <param name="zip">string</param>
        /// <param name="town">string</param>
        public Address(string street, string zip, string town, string place = "") : base(zip, town)
        {
            this.id = 0;
            this.street = street;
            this.place = place;
        }

        /// <summary>
        /// Constructor to add address from Db to list
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="street">string</param>
        /// <param name="place">string</param>
        /// <param name="zip">ZipTown</param>
        public Address(int id, string street, string place, ZipTown zip) : base(zip)
        {
            this.id = id;
            this.street = street;
            this.place = place;
        }

        /// <summary>
        /// Constructor that accepts an existing Address
        /// </summary>
        /// <param name="address">Address</param>
        public Address(Address address) : base(address.Zip, address.Town)
        {
            this.id = address.Id;
            this.street = address.Street;
            this.place = address.Place;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Returns main content as a string with one row
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            return street + ", " + place + ", " + base.ToString();
        }

        /// <summary>
        /// Returns main content as a string with multiple rows
        /// </summary>
        /// <returns>string</returns>
        public string ToLongString()
        {
            return street + "\n" + place + "\n" + base.ToString();
        }

        #endregion

        #region Properties
        public int Id { get => id; }

        public string Street
        {
            get => street;
            set
            {
                try
                {
                    street = value;
                }
                catch (Exception)
                {
                    street = "";
                }
            }
        }

        public string Place
        {
            get => place;
            set
            {
                try
                {
                    place = value;
                }
                catch (Exception)
                {
                    place = "";
                }
            }
        }

        public ZipTown ZipTown { get; set; }

        #endregion
    }
}
