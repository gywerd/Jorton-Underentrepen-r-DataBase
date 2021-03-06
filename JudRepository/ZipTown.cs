﻿using JudDataAccess;
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
        private int id;
        private string zip;
        private string town;

        #endregion

        #region Constructors
        /// <summary>
        /// Empty constructor
        /// </summary>
        public ZipTown()
        {
            this.id = 1000;
            this.zip = "";
            this.town = "";
        }

        /// <summary>
        /// Costructor to add a new ZipTown
        /// </summary>
        /// <param name="zip">string</param>
        /// <param name="town">string</param>
        public ZipTown(string zip, string town)
        {
            this.id = 1000;
            this.zip = zip;
            this.town = town;
        }

        /// <summary>
        /// Costructor to add a ZipTown from Db
        /// </summary>
        /// <param name="zip">string</param>
        /// <param name="town">string</param>
        public ZipTown(int id, string zip, string town)
        {
            this.id = id;
            this.zip = zip;
            this.town = town;
        }

        /// <summary>
        /// Constructor, that accepts data from an existing ZipTown
        /// </summary>
        /// <param name="zipTown">ZipTown</param>
        public ZipTown(ZipTown zipTown)
        {
            this.id = zipTown.Id;
            this.zip = zipTown.zip;
            this.town = zipTown.town;
        }

        #endregion

        #region Properties
        public int Id { get => id; }
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
        /// Method, that converts main info to string
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            return this.zip + " " + this.town;
        }

        #endregion

    }
}