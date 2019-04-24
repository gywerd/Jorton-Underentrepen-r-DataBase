using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudRepository
{
    public class RequestShipping
    {
        #region Fields
        private int id = 0;
        private SubEntrepeneur subEntrepeneur = new SubEntrepeneur();
        private Receiver receiver = new Receiver();
        private string acceptUrl = "";
        private string declineUrl = "";
        private string requestPdfPath = "";

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public RequestShipping() { }

        /// <summary>
        /// Constructor, that creates Request Data with a SubEntrepeneur and empty fields
        /// </summary>
        /// <param name="subEntrepeneur">SubEntrepeneur</param>
        public RequestShipping(SubEntrepeneur subEntrepeneur)
        {
            this.subEntrepeneur = subEntrepeneur;
            SetAcceptDeclineUrls();

        }

        /// <summary>
        /// Constructor, that creates Request Data with SubEntrepeneur and fields
        /// </summary>
        /// <param name="subEntrepeneur">SubEntrepeneur</param>
        /// <param name="receiver">Receiver</param>
        /// <param name="period">string</param>
        /// <param name="answerDate">string</param>
        /// <param name="requestPdfPath">string</param>
        public RequestShipping(SubEntrepeneur subEntrepeneur, Receiver receiver, string requestPdfPath, string acceptUrl = "", string declineUrl = "")
        {
            this.subEntrepeneur = subEntrepeneur;
            this.receiver = receiver;
            this.requestPdfPath = requestPdfPath;
            if (acceptUrl == "" & declineUrl == "")
            {
                SetAcceptDeclineUrls();
            }
        }

        /// <summary>
        /// Constructor, used for reading Request Data from Db
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="subEntrepeneur">SubEntrepeneur</param>
        /// <param name="receiver">Receiver</param>
        /// <param name="acceptUrl">string</param>
        /// <param name="declineUrl">string</param>
        /// <param name="requestPdfPath">string</param>
        public RequestShipping(int id, SubEntrepeneur subEntrepeneur, Receiver receiver, string acceptUrl, string declineUrl, string requestPdfPath)
        {
            this.id = id;
            this.subEntrepeneur = subEntrepeneur;
            this.receiver = receiver;
            this.acceptUrl = acceptUrl;
            this.declineUrl = declineUrl;
            this.requestPdfPath = requestPdfPath;
        }

        /// <summary>
        /// Constructor, that accepts data from an existing Request Data
        /// </summary>
        /// <param name="requestShipping">RequestData</param>
        public RequestShipping(RequestShipping requestShipping)
        {
            this.id = requestShipping.Id;
            this.subEntrepeneur = requestShipping.SubEntrepeneur;
            this.receiver = requestShipping.Receiver;
            this.acceptUrl = requestShipping.AcceptUrl;
            this.declineUrl = requestShipping.DeclineUrl;
            this.requestPdfPath = requestShipping.RequestPdfPath;
        }
        #endregion

        #region Properties
        public int Id { get => id; }

        public SubEntrepeneur SubEntrepeneur { get => subEntrepeneur; set => subEntrepeneur = value; }

        public Receiver Receiver { get => receiver; set => receiver = value; }

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
        private void SetAcceptDeclineUrls()
        {
            acceptUrl = @"mailto:" + subEntrepeneur.Enterprise.Project.Executive.Person.ContactInfo.Email + @"?subject=" + subEntrepeneur.Entrepeneur.Entity.Name + ".%20Vi%20ønsker%20at%20afgive%20tilbud";
            declineUrl = @"mailto:" + subEntrepeneur.Enterprise.Project.Executive.Person.ContactInfo.Email + @"?subject=" + subEntrepeneur.Entrepeneur.Entity.Name + ".%20Vi%20ønsker%20ikke%20at%20afgive%20tilbud";
        }

        public override string ToString()
        {
            return subEntrepeneur.Entrepeneur.Entity.Name + @" (" + subEntrepeneur.Entrepeneur.Entity.Cvr + @")";
        }

        #endregion

    }
}
