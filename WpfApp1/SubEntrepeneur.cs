using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class SubEntrepeneur
    {
        #region Fields
        private int id;
        private int enterprise;
        private int entrepeneur;
        private int contact;
        private int request;
        private int ittLetter;
        private int offer;
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
            this.enterprise = 0;
            this.entrepeneur = 0;
            this.contact = 0;
            this.request = 0;
            this.ittLetter = 0;
            this.offer = 0;
            this.reservations = false;
            this.uphold = false;
            this.agreementConcluded = false;
            this.active = false;
        }

        /// <summary>
        /// Constructor to add a new Sbentrepeneur
        /// </summary>
        /// <param name="enterprise">int</param>
        /// <param name="entrepeneur">int</param>
        /// <param name="contact">int</param>
        /// <param name="request">int</param>
        /// <param name="ittLetter">int</param>
        /// <param name="offer">int</param>
        /// <param name="reservations">bool</param>
        /// <param name="uphold">bool</param>
        /// <param name="agreementConcluded">bool</param>
        /// <param name="active">bool</param>
        public SubEntrepeneur(int enterprise, int entrepeneur, int contact, int request, int ittLetter, int offer, bool reservations, bool uphold, bool agreementConcluded, bool active)
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
        /// <param name="enterprise">int</param>
        /// <param name="entrepeneur">int</param>
        /// <param name="contact">int</param>
        /// <param name="request">int</param>
        /// <param name="ittLetter">int</param>
        /// <param name="offer">int</param>
        /// <param name="reservations">bool</param>
        /// <param name="uphold">bool</param>
        /// <param name="agreementConcluded">bool</param>
        /// <param name="active">bool</param>
        public SubEntrepeneur(int id, int enterprise, int entrepeneur, int contact, int request, int ittLetter, int offer, bool reservations, bool uphold, bool agreementConcluded, bool active)
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

        #endregion

        #region Properties
        public int Id { get => id; }

        public int Enterprise { get => enterprise; }

        public int Entrepeneur { get => entrepeneur; set => entrepeneur = value; }

        public int Contact { get => contact; }

        public int Request { get => request; }

        public int IttLetter { get => ittLetter; }

        public int Offer { get => offer; }

        public bool Reservations { get => reservations; }

        public bool Uphold { get => uphold; }

        public bool AgreementConcluded { get => agreementConcluded; }

        public bool Active { get => active; }

        #endregion
    }
}
