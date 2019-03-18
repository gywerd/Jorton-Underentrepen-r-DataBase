using JudDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudRepository
{
    public class Bullet
    {
        #region Fields
        protected int id;
        protected Paragraf paragraph;
        protected string text;

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public Bullet()
        {
            this.id = 0;
            paragraph = new Paragraf();
            text = "";
        }

        /// <summary>
        /// Costructor to add a a new Bullet
        /// </summary>
        /// <param name="paragraph">int</param>
        /// <param name="text">string</param>
        public Bullet(Paragraf paragraph, string text)
        {
            this.id = 0;
            this.paragraph = paragraph;
            this.text = text;
        }

        /// <summary>
        /// Costructor to add a a Bullet from Db
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="paragraph">int</param>
        /// <param name="text">string</param>
        public Bullet(int id, Paragraf paragraph, string text)
        {
            this.id = id;
            this.paragraph = paragraph;
            this.text = text;
        }

        /// <summary>
        /// Constructor, that accepts data from an existing Bullet
        /// </summary>
        /// <param name="bullet">Bullet</param>
        public Bullet(Bullet bullet)
        {
            this.id = bullet.id;
            this.paragraph = bullet.Paragraf;
            this.text = bullet.Text;
        }

        /// <summary>
        /// Constructor, that accepts data from an existing Indexed Bullet
        /// </summary>
        /// <param name="bullet">IndexedBullet</param>
        public Bullet(IndexedBullet bullet)
        {
            this.id = bullet.id;
            this.paragraph = bullet.Paragraf;
            this.text = bullet.Text;
        }

        #endregion

        #region Properties
        public int Id { get => id; }

        public Paragraf Paragraf { get => paragraph; set => paragraph = value; }

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
        /// Method, that returns main info as a string
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            return text;
        }

        #endregion

    }
}
