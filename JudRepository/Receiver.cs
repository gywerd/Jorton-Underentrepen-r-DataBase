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
    public class Receiver
    {
        #region Fields
        private int id = 0;
        private string cvr = "";
        private string name = "";
        private string attention = "";
        private string street = "";
        private string place = "";
        private string zipTown = "";
        private string email = "";

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public Receiver()
        {
            this.id = 0;
            this.cvr = "";
            this.name = "";
            this.attention = "";
            this.street = "";
            this.place = "";
            this.zipTown = "";
        }

        /// <summary>
        /// Costructor to add a a new ITT Letter Reciver
        /// </summary>
        /// <param name="cvr">string</param>
        /// <param name="name">string</param>
        /// <param name="attention">string</param>
        /// <param name="street">string</param>
        /// <param name="zip">string</param>
        /// <param name="email">string</param>
        /// <param name="place">string</param>
        public Receiver(string cvr, string name, string attention, string street, string zipTown, string email, string place = "")
        {
            this.id = 0;
            this.cvr = cvr;
            this.name = name;
            this.attention = attention;
            this.street = street;
            this.place = place;
            this.zipTown = zipTown;
            this.email = email;
        }

        /// <summary>
        /// Costructor to add a an ITT Letter Receiver from Db
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="project">Project</param>
        /// <param name="cvr">string</param>
        /// <param name="name">string</param>
        /// <param name="attention">string</param>
        /// <param name="street">string</param>
        /// <param name="place">string</param>
        /// <param name="zipTown">string</param>
        /// <param name="email">string</param>
        public Receiver(int id, string cvr, string name, string attention, string street, string place, string zipTown, string email)
        {
            this.id = id;
            this.cvr = cvr;
            this.name = name;
            this.attention = attention;
            this.street = street;
            this.place = place;
            this.zipTown = zipTown;
            this.email = email;
        }

        /// <summary>
        /// Constructor, that accepts data from existing Itt Letter Receiver
        /// </summary>
        /// <param name="receiver">Receiver</param>
        public Receiver(Receiver receiver)
        {
                this.id = receiver.Id;
                this.cvr = receiver.Cvr;
                this.name = receiver.Name;
                this.attention = receiver.Attention;
                this.street = receiver.Street;
                this.place = receiver.place;
                this.zipTown = receiver.ZipTown;
                this.email = receiver.Email;
        }

        #endregion

        #region Properties
        public int Id { get => id; }

        public string Cvr
        {
            get => cvr;
            set
            {
                try
                {
                    cvr = value;
                }
                catch (Exception)
                {
                    cvr = "";
                }
            }
        }

        public string Name
        {
            get => name;
            set
            {
                try
                {
                    name = value;
                }
                catch (Exception)
                {
                    name = "";
                }
            }
        }

        public string Attention
        {
            get => attention;
            set
            {
                try
                {
                    attention = value;
                }
                catch (Exception)
                {
                    attention = "";
                }
            }
        }

        public string Street
        {
            get => street;
            set
            {
                try
                {
                    street = value;
                }
                catch (Exception)
                {
                    street = "";
                }
            }
        }

        public string Place
        {
            get => place;
            set
            {
                try
                {
                    place = value;
                }
                catch (Exception)
                {
                    place = "";
                }
            }
        }

        public string ZipTown
        {
            get => zipTown;
            set
            {
                try
                {
                    zipTown = value;
                }
                catch (Exception)
                {
                    zipTown = "";
                }
            }
        }

        public string Email
        {
            get => email;
            set
            {
                try
                {
                    email = value;
                }
                catch (Exception)
                {
                    email = "";
                }
            }
        }

        #endregion

        #region Methods
        public void SetId(int id)
        {
            if (int.TryParse(id.ToString(), out int parsedId) && this.id == 0 && parsedId >= 1)
            {
                this.id = parsedId;
            }
            else
            {
                this.id = 0;
            }
        }

        /// <summary>
        /// Returns main content as string with multiple rows
        /// </summary>
        /// <returns>string</returns>
        public string ToLongString()
        {
            return name + "\n" + attention + "\n" + street + "\n" + place + "\n" + zipTown + "\n" + email;
        }

        /// <summary>
        /// Returns main content as a string
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            return name + ". " + attention + ". " + street + ". " + place + ". " + zipTown + ". " + email;
        }

        #endregion

    }
}
