using JudDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudRepository
{
    public class Region
    {
        #region Fields
        private int id;
        private string text;
        private string zips;

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public Region()
        {
            this.id = 0;
            this.text = "";
            this.zips = "";
        }

        /// <summary>
        /// Constructor to add a new Enterprise Form
        /// </summary>
        /// <param name="text">string</param>
        /// <param name="zips">string</param>
        public Region(string text, string zips)
        {
            this.id = 0;
            this.text = text;
            this.zips = zips;
        }

        /// <summary>
        /// Constructor to add an Enterprise Form from Db
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="text">string</param>
        /// <param name="zips">string</param>
        public Region(int id, string text, string zips)
        {
            this.id = id;
            this.text = text;
            this.zips = zips;
        }

        /// <summary>
        /// Constructor, that accepts data from an existing Enterprise Form
        /// </summary>
        /// <param name="region">Region</param>
        public Region(Region region)
        {
            if (region != null)
            {
                this.id = region.Id;
                this.text = region.Text;
                this.zips = region.Zips;
            }
            else
            {
                this.id = 0;
                this.text = "";
                this.zips = "";
            }
        }

        #endregion

        #region Properties
        public int Id { get => id; }

        public string Text
        {
            get => text;
            set
            {
                try
                {
                    text = value;
                }
                catch (Exception)
                {
                    text = "";
                }
            }
        }

        public string Zips
        {
            get => zips;
            set
            {
                try
                {
                    zips = value;
                }
                catch (Exception)
                {
                    zips = "";
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
            return text;
        }

        #endregion

    }
}
