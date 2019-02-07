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
        private LegalEntity entity;
        private bool active;

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public Builder()
        {
            this.id = 0;
            this.entity = new LegalEntity();
            this.active = false;
        }

        /// <summary>
        /// Costructor to add a a new builder
        /// </summary>
        /// <param name="entity">LegalEntity</param>
        /// <param name="active">bool</param>
        public Builder(LegalEntity entity, bool active)
        {
            this.id = 0;
            this.entity = entity;
            this.active = active;
        }

        /// <summary>
        /// Costructor to add a a builder from Db to list
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="entity">LegalEntity</param>
        /// <param name="active">bool</param>
        public Builder(int id, LegalEntity entity, bool active)
        {
            this.id = id;
            this.entity = entity;
            this.active = active;
        }

        /// <summary>
        /// Constructor, that accepts data from an existing builder
        /// </summary>
        /// <param name="builder">Builder</param>
        public Builder(Builder builder)
        {
            this.id = builder.Id;
            this.entity = builder.Entity;
            this.active = builder.Active;
        }


        #endregion

        #region Properties
        public int Id { get => id; }

        public LegalEntity Entity { get => entity; set => entity = value; }

        public bool Active { get => active; }


        #endregion

        #region Methods
        /// <summary>
        /// Returns main content as a string
        /// </summary>
        /// <returns></returns>
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
        /// Methods that toggles value of Active field
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
        /// Returns main content as a string
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            return entity.ToString();
        }

        #endregion

    }
}

