using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudRepository
{
    public class Entrepeneur
    {
        #region Fields
        private int id;
        private LegalEntity entity;
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
        public Entrepeneur()
        {
            this.id = 0;
            entity = new LegalEntity();
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
        /// Costructor to add a a new Entrepeneur
        /// </summary>
        /// <param name="entity">LegalEntity</param>
        /// <param name="craftGroup1">CraftGroup</param>
        /// <param name="craftGroup2">CraftGroup</param>
        /// <param name="craftGroup3">CraftGroup</param>
        /// <param name="craftGroup4">CraftGroup</param>
        /// <param name="region">Region</param>
        /// <param name="countryWide">bool</param>
        /// <param name="cooperative">bool</param>
        /// <param name="active"></param>
        public Entrepeneur(LegalEntity entity, CraftGroup craftGroup1, CraftGroup craftGroup2, CraftGroup craftGroup3, CraftGroup craftGroup4, Region region, bool countryWide = false, bool cooperative = false, bool active = false)
        {
            this.id = 0;
            this.entity = entity;
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
        /// Costructor to add a a legal entity from Db to list
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="entity">LegalEntity</param>
        /// <param name="craftGroup1">CraftGroup</param>
        /// <param name="craftGroup2">CraftGroup</param>
        /// <param name="craftGroup3">CraftGroup</param>
        /// <param name="craftGroup4">CraftGroup</param>
        /// <param name="region">Region</param>
        /// <param name="countryWide">bool</param>
        /// <param name="cooperative">bool</param>
        /// <param name="active"></param>
        public Entrepeneur(int id, LegalEntity entity, CraftGroup craftGroup1, CraftGroup craftGroup2, CraftGroup craftGroup3, CraftGroup craftGroup4, Region region, bool countryWide, bool cooperative, bool active)
        {
            this.id = id;
            this.entity = entity;
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
        /// Constructor, that accepts data from an existing Entrepeneur
        /// </summary>
        /// <param name="entrepeneur">Entrepeneur</param>
        public Entrepeneur(Entrepeneur entrepeneur)
        {
            this.id = entrepeneur.Id;
            this.entity = entrepeneur.Entity;
            this.craftGroup1 = entrepeneur.CraftGroup1;
            this.craftGroup2 = entrepeneur.CraftGroup2;
            this.craftGroup3 = entrepeneur.CraftGroup3;
            this.craftGroup4 = entrepeneur.CraftGroup4;
            this.region = entrepeneur.Region;
            this.countryWide = entrepeneur.CountryWide;
            this.cooperative = entrepeneur.Cooperative;
            this.active = entrepeneur.Active;
        }

        /// <summary>
        /// Constructor, that accepts data from an existing Indexed Entrepeneur
        /// </summary>
        /// <param name="entrepeneur">IndexedEntrepeneur</param>
        public Entrepeneur(IndexedEntrepeneur entrepeneur)
        {
            this.entity = entrepeneur.Entity;
            this.craftGroup1 = entrepeneur.CraftGroup1;
            this.craftGroup2 = entrepeneur.CraftGroup2;
            this.craftGroup3 = entrepeneur.CraftGroup3;
            this.craftGroup4 = entrepeneur.CraftGroup4;
            this.region = entrepeneur.Region;
            this.countryWide = entrepeneur.CountryWide;
            this.cooperative = entrepeneur.Cooperative;
            this.active = entrepeneur.Active;
        }

        #endregion

        #region Properties
        public int Id { get => id; }

        public LegalEntity Entity { get => entity; set => entity = value; }

        public CraftGroup CraftGroup1 { get => craftGroup1; set => craftGroup1 = value; }

        public CraftGroup CraftGroup2 { get => craftGroup2; set => craftGroup2 = value; }

        public CraftGroup CraftGroup3 { get => craftGroup3; set => craftGroup3 = value; }

        public CraftGroup CraftGroup4 { get => craftGroup4; set => craftGroup4 = value; }

        public Region Region { get => region; set => region = value; }

        public bool CountryWide { get => countryWide; }

        public bool Cooperative { get => cooperative; }

        public bool Active { get => active; }

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
        /// Methods that toggles value of Copy field
        /// </summary>
        public void ToggleActive()
        {
            if (active)
            {
                active = false;
            }
            else
            {
                active = true;
            }
        }

        /// <summary>
        /// Methods that toggles value of Copy field
        /// </summary>
        public void ToggleCountryWide()
        {
            if (countryWide)
            {
                countryWide = false;
            }
            else
            {
                countryWide = true;
            }
        }

        /// <summary>
        /// Methods that toggles value of Copy field
        /// </summary>
        public void ToggleCooperative()
        {
            if (cooperative)
            {
                cooperative = false;
            }
            else
            {
                cooperative = true;
            }
        }

        /// <summary>
        /// Returns main content as a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string result = entity.ToString();
            return result;
        }

        #endregion

    }
}
