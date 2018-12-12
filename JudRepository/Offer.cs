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
    public class Offer
    {
        #region Fields
        private int id;
        private bool received;
        private DateTime receivedDate;
        private double price;
        private bool chosen;

        #endregion

        #region Constructors
        /// <summary>
        /// Empty constructor
        /// </summary>
        public Offer()
        {
            this.id = 0;
            this.received = false;
            this.receivedDate = Convert.ToDateTime("1932-03-17");
            this.price = 0;
            this.chosen = false;
        }

        /// <summary>
        /// Constructor used to add new offer
        /// </summary>
        /// <param name="received">bool</param>
        /// <param name="receivedDate">DateTime?</param>
        /// <param name="price">double</param>
        /// <param name="chosen">bool</param>
        public Offer(bool received, double price, bool chosen, DateTime receivedDate)
        {
            this.id = 0;
            this.received = received;
            this.receivedDate = receivedDate;
            this.price = price;
            this.chosen = chosen;
        }

        /// <summary>
        /// Constructor used to add offer from Db to list
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="received">bool</param>
        /// <param name="receivedDate">DateTime?</param>
        /// <param name="price">double</param>
        /// <param name="chosen">bool</param>
        public Offer(int id, bool received, DateTime receivedDate, double price, bool chosen)
        {
            this.id = id;
            this.received = received;
            this.receivedDate = receivedDate;
            this.price = price;
            this.chosen = chosen;
        }

        /// <summary>
        /// Constructor, thats accepts an existing Offer
        /// </summary>
        /// <param name="offer">Offer</param>
        public Offer(Offer offer)
        {
            this.id = offer.Id;
            this.received = offer.Received;
            this.receivedDate = offer.ReceivedDate;
            this.price = offer.Price;
            this.chosen = offer.Chosen;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Method to add received and received date to 
        /// </summary>
        /// <param name="receivedDate">DateTime?</param>
        public void AddReceived(DateTime? receivedDate = null)
        {
            if (receivedDate == null)
            {
                receivedDate = DateTime.Now;
            }
            if (receivedDate.Value.ToShortDateString().Substring(0, 10) != "17-03-1932")
            {
                this.received = true;
            }
            else
            {
                this.received = false;
            }
            if (this.receivedDate.ToShortDateString().Substring(0, 10) != receivedDate.Value.ToShortDateString().Substring(0, 10))
            {
                this.receivedDate = Convert.ToDateTime(receivedDate);
            }
        }

        /// <summary>
        /// Method to add received and received date to 
        /// </summary>
        /// <param name="receivedDate">DateTime?</param>
        public void ResetReceived()
        {
            this.received = false;
            this.receivedDate = Convert.ToDateTime("1932-03-17");
        }

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
        /// Method to add received and received date to 
        /// </summary>
        /// <param name="receivedDate">DateTime?</param>
        public void SetReceivedDate(DateTime date)
        {
            this.receivedDate = date;
        }

        /// <summary>
        /// Method that toggles chosen state
        /// </summary>
        public void ToggleChosen()
        {
            if (chosen)
            {
                chosen = false;
            }
            else
            {
                chosen = true;
            }
        }

        /// <summary>
        /// Methods that toggles value of Received field
        /// </summary>
        public void ToggleReceived()
        {
            if (received)
            {
                received = false;
            }
            else
            {
                received = true;
            }
        }

        /// <summary>
        /// Returns main content as a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (received)
            {
                string result = "Offer received: " + receivedDate.ToShortDateString();
                return result;
            }
            else
            {
                string result = "Offer not received";
                return result;
            }
        }

        #endregion

        #region Properties
        public int Id { get => id; }
        public bool Received { get => received; }
        public DateTime ReceivedDate { get => receivedDate; }
        public double Price
        {
            get => price;
            set
            {
                try
                {
                    if (value >= 0)
                    {
                        price = value;
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }
        public bool Chosen { get => chosen; }
        #endregion
    }
}