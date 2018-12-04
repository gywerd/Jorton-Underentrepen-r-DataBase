using JudDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace JudRepository
{
    public class IttLetter
    {
        #region Fields
        private int id;
        private bool sent;
        private DateTime sentDate;

        #endregion

        #region Constructors
        /// <summary>
        /// Empty constructor
        /// </summary>
        public IttLetter()
        {
            this.id = 0;
            this.sent = false;
            this.sentDate = Convert.ToDateTime("1932-03-17");
        }

        /// <summary>
        /// Constructor for adding new ITT Letter
        /// </summary>
        /// <param name="sent">bool</param>
        /// <param name="sentDate">DateTime</param>
        public IttLetter(bool sent, DateTime sentDate)
        {
            this.id = 0;
            this.sent = sent;
            this.sentDate = DateTime.Now;
        }

        /// <summary>
        /// Constructor for adding ITT Letter from Db
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sent"></param>
        /// <param name="sentDate"></param>
        public IttLetter(int id, bool sent, DateTime sentDate)
        {
            this.id = id;
            this.sent = sent;
            this.sentDate = sentDate;
        }

        /// <summary>
        /// Constructor for accepts an existing Itt Letter
        /// </summary>
        /// <param name="ittLetter">IttLetter</param>
        public IttLetter(IttLetter ittLetter)
        {
            this.id = ittLetter.Id;
            this.sent = ittLetter.Sent;
            this.sentDate = ittLetter.SentDate;
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
        /// Methods that toggles value of Sent field
        /// </summary>
        public void ToggleSent()
        {
            if (sent)
            {
                sent = false;
            }
            else
            {
                sent = true;
            }
        }

        /// <summary>
        /// Returns main content as a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (sent)
            {
                string result = "Tilbudsbrev sendt: " + sentDate.ToShortDateString();
                return result;
            }
            else
            {
                string result = "Tilbudsbrev ikke sendt";
                return result;
            }
        }

        #endregion

        #region Properties
        public int Id { get => id; }

        public bool Sent { get => sent; }

        public DateTime SentDate
        {
            get => sentDate;
            set
            {
                DateTime tempValue = Convert.ToDateTime("1932-03-17");

                try
                {
                    if (value != tempValue)
                    {
                        sentDate = value;
                    }
                    else
                    {
                        sentDate = tempValue;
                    }
                }
                catch (Exception)
                {
                    sentDate = tempValue;
                }
            }
        }

        #endregion
    }
}
