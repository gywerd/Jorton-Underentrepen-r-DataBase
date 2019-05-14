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
    public class Shipping
    {
        #region Fields
        int id = 0;
        private SubEntrepeneur subEntrepeneur = new SubEntrepeneur();
        private Receiver receiver = new Receiver();
        private LetterData letterData = new LetterData();
        private string acceptUrl = "";
        private string declineUrl = "";

        private string requestPdfPath = @"PDF_Documents\";
        private string commonIttLetterPdfPath = @"PDF_Documents\";
        private string personalIttLetterPdfPath = @"PDF_Documents\";


        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public Shipping() { }

        /// <summary>
        /// Constructor, that creates a new Shipping with a SubEntrepeneur and empty fields
        /// </summary>
        /// <param name="subEntrepeneur">SubEntrepeneur</param>
        public Shipping(SubEntrepeneur subEntrepeneur)
        {
            this.subEntrepeneur = subEntrepeneur;

        }

        /// <summary>
        /// Costructor to add a new Shipping
        /// </summary>
        /// <param name="subEntrepeneur">SubEntrepeneur</param>
        /// <param name="receiver">Receiver</param>
        /// <param name="letterData">PdfData</param>
        /// <param name="requestPdfPath">string</param>
        /// <param name="commmonIttLetterPdfPath">string</param>
        /// <param name="personalIttLetterPdfPath">string</param>
        /// <param name="acceptUrl">string</param>
        /// <param name="declineUrl">string</param>
        public Shipping(SubEntrepeneur subEntrepeneur, Receiver receiver, LetterData letterData, string requestPdfPath, string commmonIttLetterPdfPath, string personalIttLetterPdfPath, string acceptUrl = "", string declineUrl = "")
        {
            this.subEntrepeneur = subEntrepeneur;
            this.receiver = receiver;
            this.letterData = letterData;
            if (acceptUrl == "" && declineUrl == "" && receiver.Email.Length >= 1)
            {
                SetAcceptDeclineUrls();
            }
            else if (acceptUrl != "" && declineUrl != "")
            {
                this.acceptUrl = acceptUrl;
                this.declineUrl = declineUrl;
            }
            this.requestPdfPath = requestPdfPath;
            this.commonIttLetterPdfPath = commmonIttLetterPdfPath;
            this.personalIttLetterPdfPath = personalIttLetterPdfPath;
        }

        /// <summary>
        /// Costructor to add a Shipping from Db
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="subEntrepeneur">SubEntrepeneur</param>
        /// <param name="receiver">Receiver</param>
        /// <param name="letterData">PdfData</param>
        /// <param name="commmonIttLetterPdfPath">string</param>
        /// <param name="personalIttLetterPdfPath">string</param>
        public Shipping(int id, SubEntrepeneur subEntrepeneur, Receiver receiver, LetterData letterData, string acceptUrl, string declineUrl, string requestPdfPath, string commmonIttLetterPdfPath, string personalIttLetterPdfPath)
        {
            this.id = id;
            this.subEntrepeneur = subEntrepeneur;
            this.receiver = receiver;
            this.letterData = letterData;
            this.acceptUrl = acceptUrl;
            this.declineUrl = declineUrl;
            this.requestPdfPath = requestPdfPath;
            this.commonIttLetterPdfPath = commmonIttLetterPdfPath;
            this.personalIttLetterPdfPath = personalIttLetterPdfPath;
        }

        /// <summary>
        /// Constructor, that accepts data from an existing Shipping
        /// </summary>
        /// <param name="shipping">Shipping</param>
        public Shipping(Shipping shipping)
        {
            this.id = shipping.Id;
            this.subEntrepeneur = shipping.SubEntrepeneur;
            this.receiver = shipping.Receiver;
            this.letterData = shipping.LetterData;
            this.acceptUrl = shipping.AcceptUrl;
            this.declineUrl = shipping.DeclineUrl;
            this.requestPdfPath = shipping.RequestPdfPath;
            this.commonIttLetterPdfPath = shipping.CommonIttLetterPdfPath;
            this.personalIttLetterPdfPath = shipping.PersonalIttLetterPdfPath;
        }

        #endregion

        #region Properties
        public int Id { get => id; }

        public SubEntrepeneur SubEntrepeneur { get => subEntrepeneur; set => subEntrepeneur = value; }

        public Receiver Receiver
        {
            get => receiver;
            set
            {
                receiver = value;
                SetAcceptDeclineUrls();
            }
        }

        public LetterData LetterData { get => letterData; set => letterData = value; }

        public string CommonIttLetterPdfPath
        {
            get => commonIttLetterPdfPath;
            set
            {
                try
                {
                    if (value != null)
                    {
                        commonIttLetterPdfPath = value;
                    }
                    else
                    {
                        commonIttLetterPdfPath = "";
                    }
                }
                catch (Exception)
                {
                    commonIttLetterPdfPath = "";
                }
            }
        }

        public string PersonalIttLetterPdfPath
        {
            get => personalIttLetterPdfPath;
            set
            {
                try
                {
                    if (value != null)
                    {
                        personalIttLetterPdfPath = value;
                    }
                    else
                    {
                        personalIttLetterPdfPath = "";
                    }
                }
                catch (Exception)
                {
                    personalIttLetterPdfPath = "";
                }
            }
        }

        public string AcceptUrl { get => acceptUrl; }

        public string DeclineUrl { get => declineUrl; }

        public string RequestPdfPath
        {
            get => requestPdfPath;
            set
            {
                try
                {
                    requestPdfPath = value;
                }
                catch (Exception)
                {
                    requestPdfPath = "";
                }
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Method, that generates AcceptUrl and DeclineUrl from Entrepeneur and Executive Email
        /// </summary>
        public void SetAcceptDeclineUrls()
        {
            acceptUrl = @"mailto:" + Receiver.Email + @"?subject=" + Receiver.Name + ".%20Vi%20ønsker%20at%20afgive%20tilbud";
            declineUrl = @"mailto:" + Receiver.Email + @"?subject=" + Receiver.Name + ".%20Vi%20ønsker%20ikke%20at%20afgive%20tilbud";
        }

        /// <summary>
        /// Method, that sets id, if id == 0
        /// </summary>
        /// <param name="id">int</param>
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
        /// Returns main content as a string
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            return "Forsendelsesoplysninger til " + subEntrepeneur.Entrepeneur.Entity.Name + " for projektet " + subEntrepeneur.Enterprise.Project.Details.Name;
        }

        #endregion

    }
}
