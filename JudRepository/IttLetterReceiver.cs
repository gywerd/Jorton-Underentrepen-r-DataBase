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
    public class IttLetterReceiver
    {
        #region Fields
        private int id = 0;
        private IttLetterShipping shipping;
        private Project project;
        private string companyId = "";
        private string companyName = "";
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
        public IttLetterReceiver()
        {
            this.id = 0;
            this.shipping = new IttLetterShipping();
            this.companyId = "";
            this.companyName = "";
            this.attention = "";
            this.street = "";
            this.place = "";
            this.zipTown = "";
        }

        /// <summary>
        /// Constructor for adding new ITT Letter Reciver
        /// </summary>
        /// <param name="shipping">IttLetterShipping</param>
        /// <param name="project">Project</param>
        /// <param name="companyId">string</param>
        /// <param name="companyName">string</param>
        /// <param name="attention">string</param>
        /// <param name="street">string</param>
        /// <param name="zip">string</param>
        /// <param name="email">string</param>
        /// <param name="place">string</param>
        public IttLetterReceiver(IttLetterShipping shipping, Project project, string companyId, string companyName, string attention, string street, string zipTown, string email, string place = "")
        {
            this.id = 0;
            this.shipping = shipping;
            this.project = project;
            this.companyId = companyId;
            this.companyName = companyName;
            this.attention = attention;
            this.street = street;
            this.place = place;
            this.zipTown = zipTown;
            this.email = email;
        }

        /// <summary>
        /// Constructor for adding ITT Letter Receiver from Db
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="shipping">IttLetterShipping</param>
        /// <param name="project">Project</param>
        /// <param name="companyId">string</param>
        /// <param name="companyName">string</param>
        /// <param name="attention">string</param>
        /// <param name="street">string</param>
        /// <param name="place">string</param>
        /// <param name="zipTown">string</param>
        /// <param name="email">string</param>
        public IttLetterReceiver(int id, IttLetterShipping shipping, Project project, string companyId, string companyName, string attention, string street, string place, string zipTown, string email)
        {
            this.id = id;
            this.shipping = shipping;
            this.project = project;
            this.companyId = companyId;
            this.companyName = companyName;
            this.attention = attention;
            this.street = street;
            this.place = place;
            this.zipTown = zipTown;
            this.email = email;
        }

        /// <summary>
        /// Constructor that accepts data from existing Itt Letter Receiver
        /// </summary>
        /// <param name="receiver">IttLetterReceiver</param>
        public IttLetterReceiver(IttLetterReceiver receiver)
        {
                this.id = receiver.Id;
                this.shipping = receiver.shipping;
                this.project = receiver.Project;
                this.companyId = receiver.companyId;
                this.companyName = receiver.companyName;
                this.attention = receiver.attention;
                this.street = receiver.Street;
                this.place = receiver.place;
                this.zipTown = receiver.zipTown;
                this.email = receiver.Email;
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
            string result = companyName + "\n" + attention + "\n" + street;
            if (place != "")
            {
                result += "\n" + place;
            }
            result += "\n" + zipTown;
            if (email != "")
            {
                result += "\n" + email;
            }
            return result;
        }

        /// <summary>
        /// Returns main content as a string
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            string result = companyName + ". " + attention + ". " + street;
            if (place != "")
            {
                result += ". " + place;
            }
            result += ". " + zipTown;
            if (email != "")
            {
                result += ". " + email;
            }
            result += ".";
            return result;
        }

        #endregion

        #region Properties
        public int Id { get => id; }

        public IttLetterShipping Shipping { get; set; }

        public Project Project { get; set; }
        public string CompanyId
        {
            get => companyId;
            set
            {
                try
                {
                    companyId = value;
                }
                catch (Exception)
                {
                    companyId = "";
                }
            }
        }

        public string CompanyName
        {
            get => companyName;
            set
            {
                try
                {
                    companyName = value;
                }
                catch (Exception)
                {
                    companyName = "";
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
    }
}
