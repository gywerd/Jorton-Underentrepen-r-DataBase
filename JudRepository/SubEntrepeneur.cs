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
        private Entrepeneur entrepeneur;
        private Enterprise enterprise;
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
        public SubEntrepeneur()
        {
            this.id = 0;
            this.enterprise = new Enterprise();
            this.entrepeneur = new Entrepeneur();
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
        /// Constructor to add a new Sbentrepeneur
        /// </summary>
        /// <param name="entrepeneur">LegalEntity</param>
        /// <param name="enterprise">Enterprise</param>
        /// <param name="contact">Contact</param>
        /// <param name="request">Request</param>
        /// <param name="ittLetter">IttLetter</param>
        /// <param name="offer">Offer</param>
        /// <param name="reservations">bool</param>
        /// <param name="uphold">bool</param>
        /// <param name="agreementConcluded">bool</param>
        /// <param name="active">bool</param>
        public SubEntrepeneur(Entrepeneur entrepeneur, Enterprise enterprise, Contact contact, Request request, IttLetter ittLetter, Offer offer, bool reservations, bool uphold, bool agreementConcluded, bool active)
        {
            this.id = 0;
            this.enterprise = enterprise;
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
        /// Constructor to add a Subentrepeneur from Db to List
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="entrepeneur">LegalEntity</param>
        /// <param name="enterprise">Enterprise</param>
        /// <param name="contact">Contact</param>
        /// <param name="request">Request</param>
        /// <param name="ittLetter">IttLetter</param>
        /// <param name="offer">Offer</param>
        /// <param name="reservations">bool</param>
        /// <param name="uphold">bool</param>
        /// <param name="agreementConcluded">bool</param>
        /// <param name="active">bool</param>
        public SubEntrepeneur(int id, Entrepeneur entrepeneur, Enterprise enterprise, Contact contact, Request request, IttLetter ittLetter, Offer offer, bool reservations, bool uphold, bool agreementConcluded, bool active)
        {
            this.id = id;
            this.entrepeneur = entrepeneur;
            this.enterprise = enterprise;
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
        /// Constructor, that accepts data from an existing SubEntrepeneur
        /// </summary>
        /// <param name="subEntrepeneur">SubEntrepeneur</param>
        public SubEntrepeneur(SubEntrepeneur subEntrepeneur)
        {
            this.id = subEntrepeneur.Id;
            this.entrepeneur = subEntrepeneur.Entrepeneur;
            this.enterprise = subEntrepeneur.Enterprise;
            this.contact = subEntrepeneur.Contact;
            this.request = subEntrepeneur.Request;
            this.ittLetter = subEntrepeneur.IttLetter;
            this.offer = subEntrepeneur.Offer;
            this.reservations = subEntrepeneur.Reservations;
            this.uphold = subEntrepeneur.Uphold;
            this.agreementConcluded = subEntrepeneur.AgreementConcluded;
            this.active = subEntrepeneur.Active;
        }

        /// <summary>
        /// Constructor, that accepts data from an existing Indexed SubEntrepeneur
        /// </summary>
        /// <param name="subEntrepeneur">IndexedSubEntrepeneur</param>
        public SubEntrepeneur(IndexedSubEntrepeneur subEntrepeneur)
        {
            this.id = subEntrepeneur.Id;
            this.entrepeneur = subEntrepeneur.Entrepeneur;
            this.enterprise = subEntrepeneur.Enterprise;
            this.contact = subEntrepeneur.Contact;
            this.request = subEntrepeneur.Request;
            this.ittLetter = subEntrepeneur.IttLetter;
            this.offer = subEntrepeneur.Offer;
            this.reservations = subEntrepeneur.Reservations;
            this.uphold = subEntrepeneur.Uphold;
            this.agreementConcluded = subEntrepeneur.AgreementConcluded;
            this.active = subEntrepeneur.Active;
        }

        #endregion

        #region Properties
        public int Id { get => id; }

        public Entrepeneur Entrepeneur { get => entrepeneur; set => entrepeneur = value; }

        public Enterprise Enterprise { get => enterprise; set => enterprise = value; }

        public Contact Contact { get => contact; set => contact = value; }

        public Request Request { get => request; set => request = value; }

        public IttLetter IttLetter { get => ittLetter; set => ittLetter = value; }

        public Offer Offer { get => offer; set => offer = value; }

        public bool Reservations { get => reservations; }

        public bool Uphold { get => uphold; }

        public bool AgreementConcluded { get => agreementConcluded; }

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
            string result = Entrepeneur.Entity.Name;
            return result;
        }

        #endregion

    }
}
