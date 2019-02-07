using JudDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudRepository
{
    public class Address
    {
        #region Fields
        private int id;
        private string street;
        private string place;
        private ZipTown zipTown;

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public Address()
        {
            this.id = 0;
            this.street = "";
            this.place = "";
            this.zipTown = new ZipTown();
        }

        /// <summary>
        /// Constructor to add a new address
        /// </summary>
        /// <param name="street">string</param>
        /// <param name="zipTown">ZipTown</param>
        /// <param name="place">string</param>
        public Address(string street, string place, ZipTown zipTown)
        {
            this.id = 0;
            this.street = street;
            this.place = place;
            this.zipTown = zipTown;
        }

        /// <summary>
        /// Constructor to add address from Db to list
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="street">string</param>
        /// <param name="place">string</param>
        /// <param name="zipTown">ZipTown</param>
        public Address(int id, string street, string place, ZipTown zipTown)
        {
            this.id = id;
            this.street = street;
            this.place = place;
            this.zipTown = zipTown;
        }

        /// <summary>
        /// Constructor, that accepts data from an existing Address
        /// </summary>
        /// <param name="address">Address</param>
        public Address(Address address)
        {
            this.id = address.Id;
            this.street = address.Street;
            this.place = address.Place;
            this.zipTown = address.ZipTown;
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

        public ZipTown ZipTown { get => zipTown; set => zipTown = value; }

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

    }
}
