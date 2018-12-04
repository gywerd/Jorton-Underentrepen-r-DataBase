using JudDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudRepository
{
    public class ZipTown
    {
        #region Fields
        private string zip;
        private string town;

        #endregion

        #region Constructors
        /// <summary>
        /// Empty constructor
        /// </summary>
        public ZipTown()
        {
            this.zip = "";
            this.town = "";
        }

        /// <summary>
        /// Constructor for adding ZipTown
        /// </summary>
        /// <param name="zip">string</param>
        /// <param name="town">string</param>
        public ZipTown(string zip, string town)
        {
            this.zip = zip;
            this.town = town;
        }

        /// <summary>
        /// Constructor, that accepts an existing ZipTown
        /// </summary>
        /// <param name="zipTown">ZipTown</param>
        public ZipTown(ZipTown zipTown)
        {
            if (zipTown != null)
            {
                this.zip = zipTown.zip;
                this.town = zipTown.town;
            }
            else
            {
                this.zip = "";
                this.town = "";
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Method, that converts main info to string
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            return this.zip + " " + this.town;
        }

        #endregion

        #region Properties
        public string Zip
        {
            get => zip;
            set
            {
                try
                {
                    zip = value;
                }
                catch (Exception)
                {
                    zip = "";
                }
            }
        }

        public string Town
        {
            get => town;
            set
            {
                try
                {
                    town = value;
                }
                catch (Exception)
                {
                    town = "";
                }
            }
        }

        #endregion
    }
}