using JudDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JudRepository
{
    public class IttLetterPdfData
    {
        #region Fields
        private int id;
        private Project project;
        private Builder builder;
        private string answerDate = "";
        private string questionDate = "";
        private string correctionSheetDate = "";
        private string conditionDate = "";
        private int timeSpan = 0;
        private string materialUrl = "";
        private string conditionUrl = "";
        private string passWord = "";

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public IttLetterPdfData()
        {
            this.id = 0;
            this.project = new Project();
            this.builder = new Builder();

            this.answerDate = "";
            this.questionDate = "";
            this.correctionSheetDate = "";
            this.timeSpan = 0;
            this.materialUrl = "";
            this.conditionUrl = "";
            this.passWord = "";
        }

        /// <summary>
        /// Constructor to adding new Description
        /// </summary>
        /// <param name="project">int</param>
        /// <param name="builder">int</param>
        /// <param name="answerDate">string</param>
        /// <param name="questionDate">string</param>
        /// <param name="correctionSheetDate">string</param>
        /// <param name="timeSpan">int</param>
        /// <param name="materialUrl">string</param>
        /// <param name="conditionUrl">string</param>
        /// <param name="passWord">string</param>
        /// <param name="enterprises">List<Enterprise></param>
        /// <param name="descriptionList">List<Description></param>
        /// <param name="receivers">List<IttLetterReceiver></param>
        /// <param name="shippingList">List<IttLetterShipping></param>
        /// <param name="bluePrints">List<string> bluePrints</param>
        /// <param name="schedules">List<string> schedules</param>
        /// <param name="miscellaneusList">List<Miscellaneus></param>
        public IttLetterPdfData(Project project, Builder builder, string answerDate, string questionDate, string correctionSheetDate, int timeSpan, string materialUrl, string conditionUrl, string passWord)
        {
            this.id = 0;
            this.project = project;
            this.builder = builder;
            this.answerDate = answerDate;
            this.questionDate = questionDate;
            this.correctionSheetDate = correctionSheetDate;
            this.timeSpan = timeSpan;
            this.materialUrl = materialUrl;
            this.conditionUrl = conditionUrl;
            this.passWord = passWord;
        }

        /// <summary>
        /// Constructor to adding Description from Db
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="project">int</param>
        /// <param name="builder">int</param>
        /// <param name="answerDate">string</param>
        /// <param name="questionDate">string</param>
        /// <param name="correctionSheetDate">string</param>
        /// <param name="timeSpan">string</param>
        /// <param name="materialUrl">string</param>
        /// <param name="conditionUrl">string</param>
        /// <param name="passWord">string</param>
        /// <param name="enterprises">List<Enterprise></param>
        /// <param name="descriptionList">List<Description></param>
        /// <param name="receivers">List<IttLetterReceiver></param>
        /// <param name="shippingList">List<IttLetterShipping></param>
        /// <param name="bluePrints">List<string> bluePrints</param>
        /// <param name="schedules">List<string> schedules</param>
        /// <param name="miscellaneus">List<string></param>
        public IttLetterPdfData(int id, Project project, Builder builder, string answerDate, string questionDate, string correctionSheetDate, int timeSpan, string materialUrl, string conditionUrl, string passWord)
        {
            this.id = 0;
            this.project = project;
            this.builder = builder;
            this.answerDate = answerDate;
            this.questionDate = questionDate;
            this.correctionSheetDate = correctionSheetDate;
            this.timeSpan = timeSpan;
            this.materialUrl = materialUrl;
            this.conditionUrl = conditionUrl;
            this.passWord = passWord;
        }

        /// <summary>
        /// Constructor to adding Description from Db
        /// </summary>
        /// <param name="pdfData">IttLetterPdfData</param>
        public IttLetterPdfData(IttLetterPdfData pdfData)
        {
            this.id = pdfData.Id;
            this.project = pdfData.Project;
            this.builder = pdfData.Builder;
            this.answerDate = pdfData.AnswerDate;
            this.questionDate = pdfData.QuestionDate;
            this.correctionSheetDate = pdfData.CorrectionSheetDate;
            this.timeSpan = pdfData.TimeSpan;
            this.materialUrl = pdfData.MaterialUrl;
            this.conditionUrl = pdfData.ConditionUrl;
            this.passWord = pdfData.PassWord;
        }

        #endregion

        #region Properties
        public int Id { get => id; }

        public Project Project { get; set; }

        public Builder Builder { get; set; }

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

        public string QuestionDate
        {
            get => questionDate;
            set
            {
                try
                {
                    questionDate = value;
                }
                catch (Exception)
                {
                    questionDate = "";
                }
            }
        }

        public string CorrectionSheetDate
        {
            get => correctionSheetDate;
            set
            {
                try
                {
                    correctionSheetDate = value;
                }
                catch (Exception)
                {
                    correctionSheetDate = "";
                }
            }
        }

        public int TimeSpan
        {
            get => timeSpan;
            set
            {
                try
                {
                    timeSpan = value;
                }
                catch (Exception)
                {
                    timeSpan = 0;
                }
            }
        }

        public string MaterialUrl
        {
            get => materialUrl;
            set
            {
                try
                {
                    materialUrl = value;
                }
                catch (Exception)
                {
                    materialUrl = "";
                }
            }
        }

        public string ConditionUrl
        {
            get => conditionUrl;
            set
            {
                try
                {
                    conditionUrl = value;
                }
                catch (Exception)
                {
                    conditionUrl = "";
                }
            }
        }

        public string PassWord
        {
            get => passWord;
            set
            {
                try
                {
                    passWord = value;
                }
                catch (Exception)
                {
                    passWord = "";
                }
            }
        }

        public string ConditionDate
        {
            get => conditionDate;
            set
            {
                try
                {
                    conditionDate = value;
                }
                catch (Exception)
                {
                    conditionDate = "";
                }
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Method, that sets id, if id == 0
        /// </summary>
        /// <param name="id">int</param>
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
        /// Returns main content as a string
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            string result = "PDF-data til projekt: " + project.Name;
            return result;
        }

        #endregion

    }
}
