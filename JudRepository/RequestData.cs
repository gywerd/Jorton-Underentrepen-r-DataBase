using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudRepository
{
    public class RequestData
    {
        #region Fields
        private int id = 0;
        private Project project = new Project();
        private LegalEntity receiver = new LegalEntity();
        private string receiverAttention = "";
        private string enterpriseLine = "";
        private string acceptUrl = "";
        private string declineUrl = "";
        private string projectDescription = "";
        private string period = "";
        private string answerDate = "";
        private string requestUrl = "";

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public RequestData() { }

        /// <summary>
        /// Constructor, that creates Request Data with a Project and empty fields
        /// </summary>
        /// <param name="project">Project</param>
        public RequestData(Project project)
        {
            this.project = project;
        }

        /// <summary>
        /// Constructor, that creates Request Data with Project and fields
        /// </summary>
        /// <param name="project">Project</param>
        /// <param name="receiver">LegalEntity</param>
        /// <param name="receiverAttention">string</param>
        /// <param name="receiverEmail">string</param>
        /// <param name="enterpriseLine">string</param>
        /// <param name="projectDescription">string</param>
        /// <param name="period">string</param>
        /// <param name="answerDate">string</param>
        public RequestData(Project project, LegalEntity receiver, string receiverAttention, string enterpriseLine, string projectDescription, string period, string answerDate, string requestUrl)
        {
            this.project = project;
            this.receiver = receiver;
            this.receiverAttention = receiverAttention;
            this.enterpriseLine = enterpriseLine;
            this.projectDescription = projectDescription;
            this.period = period;
            this.answerDate = answerDate;
            this.requestUrl = requestUrl;
        }

        /// <summary>
        /// Constructor, used for reading Request Data from Db
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="project">Project</param>
        /// <param name="receiver">LegalEntity</param>
        /// <param name="attention">string</param>
        /// <param name="receiverEmail">string</param>
        /// <param name="enterpriseLine">string</param>
        /// <param name="acceptUrl">string</param>
        /// <param name="declineUrl">string</param>
        /// <param name="projectDescription">string</param>
        /// <param name="period">string</param>
        /// <param name="answerDate">string</param>
        public RequestData(int id, Project project, LegalEntity receiver, string attention, string enterpriseLine, string acceptUrl, string declineUrl, string projectDescription, string period, string answerDate, string requestUrl)
        {
            this.id = id;
            this.project = project;
            this.receiver = receiver;
            this.receiverAttention = attention;
            this.enterpriseLine = enterpriseLine;
            this.acceptUrl = acceptUrl;
            this.declineUrl = declineUrl;
            this.projectDescription = projectDescription;
            this.period = period;
            this.answerDate = answerDate;
            this.requestUrl = requestUrl;
        }

        /// <summary>
        /// Constructor, that accepts data from an existing Request Data
        /// </summary>
        /// <param name="requestData">RequestData</param>
        public RequestData(RequestData requestData)
        {
            this.id = requestData.Id;
            this.project = requestData.Project;
            this.receiver = requestData.Receiver;
            this.receiverAttention = requestData.ReceiverAttention;
            this.enterpriseLine = requestData.EnterpriseLine;
            this.acceptUrl = requestData.AcceptUrl;
            this.declineUrl = requestData.DeclineUrl;
            this.projectDescription = requestData.ProjectDescription;
            this.period = requestData.Period;
            this.answerDate = requestData.AnswerDate;
        }
        #endregion

        #region Properties
        public int Id { get => id; }

        public Project Project { get => project; set => project = value; }

        public LegalEntity Receiver
        {
            get => receiver;
            set
            {
                receiver = value;
                SetAcceptDeclineUrls();
            }
        }

        public string ReceiverAttention
        {
            get => receiverAttention;
            set
            {
                try
                {
                    receiverAttention = value;
                }
                catch (Exception)
                {
                    receiverAttention = "";
                }
            }
        }

        public string EnterpriseLine
        {
            get => enterpriseLine;
            set
            {
                try
                {
                    enterpriseLine = value;
                }
                catch (Exception)
                {
                    enterpriseLine = "";
                }
            }
        }

        public string AcceptUrl { get => acceptUrl; }

        public string DeclineUrl { get => declineUrl; }

        public string ProjectDescription
        {
            get => projectDescription;
            set
            {
                try
                {
                    projectDescription = value;
                }
                catch (Exception)
                {
                    projectDescription = "";
                }
            }
        }

        public string Period
        {
            get => period;
            set
            {
                try
                {
                    period = value;
                }
                catch (Exception)
                {
                    period = "";
                }
            }
        }

        public string AnswerDate
        {
            get => answerDate;
            set
            {
                try
                {
                    answerDate = value;
                }
                catch (Exception)
                {
                    answerDate = "";
                }
            }
        }

        public string RequestUrl
        {
            get => requestUrl;
            set
            {
                try
                {
                    requestUrl = value;
                }
                catch (Exception)
                {
                    requestUrl = "";
                }
            }
        }

        #endregion

        #region Methods
        private void SetAcceptDeclineUrls()
        {
            acceptUrl = @"mailto:" + project.Executive.Person.ContactInfo.Email + @"?subject=" + project.Name + ".%20Vi%20ønsker%20at%20afgive%20tilbud";
            declineUrl = @"mailto:" + project.Executive.Person.ContactInfo.Email + @"?subject=" + project.Name + ".%20Vi%20ønsker%20ikke%20at%20afgive%20tilbud";
        }

        #endregion

    }
}
