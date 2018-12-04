using JudDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudRepository
{
    public class SubEntrepeneur
    {
        #region Fields
        private int id;
        private Enterprise enterpriseList;
        private LegalEntity entrepeneur;
        private Contact contact;
        private Request request;
        private IttLetter ittLetter;
        private Offer offer;
        private bool reservations;
        private bool uphold;
        private bool agreementConcluded;
        private bool active;

        #endregion

        #region Constructors
        /// <summary>
        /// Empty constructor
        /// </summary>
        /// <param name="entrepeneurs">List<LegalEntity></param>
        public SubEntrepeneur()
        {
            this.id = 0;
            this.enterpriseList = new Enterprise();
            this.entrepeneur = new LegalEntity();
            this.contact = new Contact();
            this.request = new Request();
            this.ittLetter = new IttLetter();
            this.offer = new Offer();
            this.reservations = false;
            this.uphold = false;
            this.agreementConcluded = false;
            this.active = false;
        }

        /// <summary>
        /// Constructor to add new subentrepeneur
        /// </summary>
        /// <param name="entrepeneurs">List<LegalEntity></param>
        /// <param name="enterpriseList">int?</param>
        /// <param name="entrepeneur">string</param>
        /// <param name="contact">int?</param>
        /// <param name="request">int?</param>
        /// <param name="ittLetter">int?</param>
        /// <param name="offer">int?</param>
        /// <param name="reservations">bool</param>
        /// <param name="uphold">bool</param>
        /// <param name="agreementConcluded">bool</param>
        /// <param name="active">bool</param>
        public SubEntrepeneur(Enterprise enterpriseList, LegalEntity entrepeneur, Contact contact, Request request, IttLetter ittLetter, Offer offer, bool reservations, bool uphold, bool agreementConcluded, bool active)
        {
            this.id = 0;
            this.enterpriseList = enterpriseList;
            this.entrepeneur = entrepeneur;
            this.contact = contact;
            this.request = request;
            this.ittLetter = ittLetter;
            this.offer = offer;
            this.reservations = reservations;
            this.uphold = uphold;
            this.agreementConcluded = agreementConcluded;
            this.active = active;
        }

        /// <summary>
        /// Constructor to add subentrepeneur from Db to List
        /// </summary>
        /// <param name="entrepeneurs">List<LegalEntity></param>
        /// <param name="id">int</param>
        /// <param name="enterpriseList">int?</param>
        /// <param name="entrepeneur">string</param>
        /// <param name="contact">int?</param>
        /// <param name="request">int?</param>
        /// <param name="ittLetter">int?</param>
        /// <param name="offer">int?</param>
        /// <param name="reservations">bool</param>
        /// <param name="uphold">bool</param>
        /// <param name="agreementConcluded">bool</param>
        /// <param name="active">bool</param>
        public SubEntrepeneur(int id, Enterprise enterpriseList, LegalEntity entrepeneur, Contact contact, Request request, IttLetter ittLetter, Offer offer, bool reservations, bool uphold, bool agreementConcluded, bool active)
        {
            this.id = id;
            this.enterpriseList = enterpriseList;
            this.entrepeneur = entrepeneur;
            this.contact = contact;
            this.request = request;
            this.ittLetter = ittLetter;
            this.offer = offer;
            this.reservations = reservations;
            this.uphold = uphold;
            this.agreementConcluded = agreementConcluded;
            this.active = active;
        }

        /// <summary>
        /// Constructor for that accepts data from existing SubEntrepeneur
        /// </summary>
        /// <param name="entrepeneurs">List<LegalEntity></param>
        /// <param name="subEntrepeneur">SubEntrepeneur</param>
        public SubEntrepeneur(SubEntrepeneur subEntrepeneur)
        {
            this.id = subEntrepeneur.Id;
            this.contact = subEntrepeneur.Contact;
            this.enterpriseList = subEntrepeneur.EnterpriseList;
            this.entrepeneur = subEntrepeneur.Entrepeneur;
            this.request = subEntrepeneur.Request;
            this.ittLetter = subEntrepeneur.IttLetter;
            this.offer = subEntrepeneur.Offer;
            this.reservations = subEntrepeneur.Reservations;
            this.uphold = subEntrepeneur.Uphold;
            this.agreementConcluded = subEntrepeneur.AgreementConcluded;
            this.active = subEntrepeneur.Active;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Method, that toggles value of active
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
        /// Method, that toggles value of agreementConcluded
        /// </summary>
        public void ToggleAgreementConcluded()
        {
            if (agreementConcluded)
            {
                agreementConcluded = false;
            }
            else
            {
                agreementConcluded = true;
            }
        }

        /// <summary>
        /// <summary>
        /// Method that togle value of reservations
        /// </summary>
        public void ToggleReservations()
        {
            if (reservations)
            {
                reservations = false;
            }
            else
            {
                reservations = true;
            }
        }

        /// <summary>
        /// Method, that toggles value of uphold
        /// </summary>
        public void ToggleUphold()
        {
            if (uphold)
            {
                uphold = false;
            }
            else
            {
                uphold = true;
            }
        }

        /// <summary>
        /// Returns main content as a string
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            string result = Entrepeneur.Name;
            return result;
        }

        #endregion

        #region Properties
        public int Id { get => id; }

        public Enterprise EnterpriseList { get; set; }

        public LegalEntity Entrepeneur { get; set; }

        public Contact Contact { get; set; }

        public Request Request { get; set; }

        public IttLetter IttLetter { get; set; }

        public Offer Offer { get; set; }

        public bool Reservations { get => reservations; }

        public bool Uphold { get => uphold; }

        public bool AgreementConcluded { get => agreementConcluded; }

        public bool Active { get => active; }

        #endregion
    }
}
