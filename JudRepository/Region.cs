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
        private string regionName;
        private string zips;

        //Region CRG = new Region(strConnection);
        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public Region()
        {
            this.id = 0;
            this.regionName = "";
            this.zips = "";
        }

        /// <summary>
        /// Constructor to add new EnterpriseForm
        /// </summary>
        /// <param name="regionName">string</param>
        /// <param name="zips">string</param>
        public Region(string regionName, string zips)
        {
            this.id = 0;
            this.regionName = regionName;
            this.zips = zips;
        }

        /// <summary>
        /// Constructor to add Enterprise Form from Db
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="regionName">string</param>
        /// <param name="zips">string</param>
        public Region(int id, string regionName, string zips)
        {
            this.id = id;
            this.regionName = regionName;
            this.zips = zips;
        }

        /// <summary>
        /// Constructor to add Enterprise Form from Db
        /// </summary>
        /// <param name="region">Region</param>
        public Region(Region region)
        {
            if (region != null)
            {
                this.id = region.Id;
                this.regionName = region.RegionName;
                this.zips = region.Zips;
            }
            else
            {
                this.id = 0;
                this.regionName = "";
                this.zips = "";
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Returns main content as a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return regionName;
        }

        #endregion

        #region Properties
        public int Id { get => id; }

        public string RegionName {
            get => regionName;
            set
            {
                try
                {
                    regionName = value;
                }
                catch (Exception)
                {
                    regionName = "";
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

    }
}
