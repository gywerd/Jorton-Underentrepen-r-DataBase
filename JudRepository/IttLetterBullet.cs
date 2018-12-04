using JudDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudRepository
{
    public class IttLetterBullet
    {
        #region Fields
        protected int id;
        protected IttLetterParagraph paragraph;
        protected string text;

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public IttLetterBullet()
        {
            this.id = 0;
            paragraph = new IttLetterParagraph();
            text = "";
        }

        /// <summary>
        /// Constructor for adding new IttLetterBullet
        /// </summary>
        /// <param name="paragraph">int</param>
        /// <param name="name">string</param>
        public IttLetterBullet(IttLetterParagraph paragraph, string name)
        {
            this.id = 0;
            this.paragraph = paragraph;
            this.text = name;
        }

        /// <summary>
        /// Constructor for adding IttLetterBullet from Db
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="paragraph">int</param>
        /// <param name="name">string</param>
        public IttLetterBullet(int id, IttLetterParagraph paragraph, string name)
        {
            this.id = id;
            this.paragraph = paragraph;
            this.text = name;
        }

        /// <summary>
        /// Constructor for that accepts data from existing IttLetterBullet
        /// </summary>
        /// <param name="bullet">IttLetterBullet</param>
        public IttLetterBullet(IttLetterBullet bullet)
        {
            this.id = bullet.id;
            this.paragraph = bullet.Paragraph;
            this.text = bullet.Text;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Method, that returns main info as a string
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            return text;
        }

        #endregion

        #region Properties
        public int Id { get => id; }

        public IttLetterParagraph Paragraph { get; set; }

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
    }
}
