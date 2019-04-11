using JudDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace JudRepository
{
    public class ContactInfo
    {
        #region Fields
        int id;
        string phone;
        string fax;
        string mobile;
        string email;

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public ContactInfo()
        {
            this.id = 0;
            phone = "";
            fax = "";
            mobile = "";
            email = "";
        }

        /// <summary>
        /// Costructor to add a a new ContactPerson
        /// </summary>
        /// <param name="phone">string</param>
        /// <param name="fax">string</param>
        /// <param name="mobile">string</param>
        /// <param name="email">string</param>
        public ContactInfo(string phone, string fax, string mobile, string email)
        {
            this.id = 0;
            this.phone = phone;
            this.fax = fax;
            this.mobile = mobile;
            this.email = email;
        }

        /// <summary>
        /// Costructor to add a a ContactPerson from Db
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="phone">string</param>
        /// <param name="fax">string</param>
        /// <param name="mobile">string</param>
        /// <param name="email">string</param>
        public ContactInfo(int id, string phone, string fax, string mobile, string email)
        {
            this.id = id;
            this.phone = phone;
            this.fax = fax;
            this.mobile = mobile;
            this.email = email;
        }

        /// <summary>
        /// Constructor, that accepts data from an existing Contact
        /// </summary>
        /// <param name="contactInfo">Contact</param>
        public ContactInfo(ContactInfo contactInfo)
        {
            if (contactInfo != null)
            {
                this.id = contactInfo.Id;
                this.phone = contactInfo.Phone;
                this.fax = contactInfo.Fax;
                this.mobile = contactInfo.Mobile;
                this.email = contactInfo.Email;
            }
            else
            {
                id = 0;
                phone = "";
                fax = "";
                mobile = "";
                email = "";
            }
        }

        #endregion

        #region Properties
        public int Id { get => id; }

        public string Phone
        {
            get => phone;
            set
            {
                try
                {
                    phone = value;
                }
                catch (Exception)
                {
                    phone = "";
                }
            }
        }

        public string Fax
        {
            get => fax;
            set
            {
                try
                {
                    fax = value;
                }
                catch (Exception)
                {
                    fax = "";
                }
            }
        }

        public string Mobile
        {
            get => mobile;
            set
            {
                try
                {
                    mobile = value;
                }
                catch (Exception)
                {
                    mobile = "";
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
        /// <summary>
        /// Method, that checks wether an Email address is invalid
        /// </summary>
        /// <param name="email">string</param>
        /// <returns>bool</returns>
        public bool CheckEmail(string email)
        {
            bool result;

            try
            {
                MailAddress m = new MailAddress(email);

                result = true;
            }
            catch (FormatException)
            {
                result = false;
            }

            return result;
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
        /// Method, that returns main info as string
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            string tempName = "Tlf: " + phone + " / Fax:" + fax + " / Mobil:" + mobile + " / Email:" + email;
            return tempName;
        }

        /// <summary>
        /// Method, that returns main info as string with multiple lines
        /// </summary>
        /// <returns>string</returns>
        public string ToLongString()
        {
            string tempName = "";
            if (phone != null && phone != "")
            {
                tempName += "Tlf: " + phone + "\n";
            }
            if (fax != null && fax != "")
            {
                tempName += "Fax:" + fax + "\n";
            }
            if (mobile != null && mobile != "")
            {
                tempName += "Mobil:" + mobile + "\n";
            }
            if (email != null && email != "")
            {
                tempName += "Email:" + email;
            }
            if (tempName == "")
            {
                tempName = "Ingen Kontaktinfo";
            }
            return tempName;
        }

        #endregion

    }
}
