using JudDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JudRepository
{
    public class LetterData
    {
        #region Fields
        private int id;
        private string builder;
        private string projectName;
        private string answerDate = "";
        private string questionDate = "";
        private string correctionDate = "";
        private string conditionDate = "";
        private string materialUrl = "";
        private string conditionUrl = "";
        private int timeSpan = 0;
        private string passWord = "";

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public LetterData()
        {
            this.id = 0;
            this.projectName = "";
            this.builder = "";
            this.answerDate = "";
            this.questionDate = "";
            this.correctionDate = "";
            this.timeSpan = 0;
            this.materialUrl = "";
            this.conditionUrl = "";
            this.passWord = "";
        }

        /// <summary>
        /// Constructor to adding anew Description
        /// </summary>
        /// <param name="projectName">string</param>
        /// <param name="builder">string</param>
        /// <param name="answerDate">string</param>
        /// <param name="questionDate">string</param>
        /// <param name="correctionSheetDate">string</param>
        /// <param name="timeSpan">int</param>
        /// <param name="materialUrl">string</param>
        /// <param name="conditionUrl">string</param>
        /// <param name="passWord">string</param>
        public LetterData(string projectName, string builder, string answerDate, string questionDate, string correctionSheetDate, int timeSpan, string materialUrl, string conditionUrl, string passWord)
        {
            this.id = 0;
            this.builder = builder;
            this.answerDate = answerDate;
            this.questionDate = questionDate;
            this.correctionDate = correctionSheetDate;
            this.timeSpan = timeSpan;
            this.materialUrl = materialUrl;
            this.conditionUrl = conditionUrl;
            this.passWord = passWord;
        }

        /// <summary>
        /// Constructor to adding a Description from Db
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="projectName">string</param>
        /// <param name="builder">string</param>
        /// <param name="answerDate">string</param>
        /// <param name="questionDate">string</param>
        /// <param name="correctionSheetDate">string</param>
        /// <param name="timeSpan">string</param>
        /// <param name="materialUrl">string</param>
        /// <param name="conditionUrl">string</param>
        /// <param name="passWord">string</param>
        public LetterData(int id, string projectName, string builder, string answerDate, string questionDate, string correctionSheetDate, int timeSpan, string materialUrl, string conditionUrl, string passWord)
        {
            this.id = 0;
            this.builder = builder;
            this.answerDate = answerDate;
            this.questionDate = questionDate;
            this.correctionDate = correctionSheetDate;
            this.timeSpan = timeSpan;
            this.materialUrl = materialUrl;
            this.conditionUrl = conditionUrl;
            this.passWord = passWord;
        }

        /// <summary>
        /// Constructor, that accepts data from an existing Description
        /// </summary>
        /// <param name="letterData">PdfData</param>
        public LetterData(LetterData letterData)
        {
            this.id = letterData.Id;
            this.builder = letterData.Builder;
            this.answerDate = letterData.AnswerDate;
            this.questionDate = letterData.QuestionDate;
            this.correctionDate = letterData.CorrectionDate;
            this.timeSpan = letterData.TimeSpan;
            this.materialUrl = letterData.MaterialUrl;
            this.conditionUrl = letterData.ConditionUrl;
            this.passWord = letterData.PassWord;
        }

        #endregion

        #region Properties
        public int Id { get => id; }

        public string ProjectName
        {
            get => projectName;
            set
            {
                try
                {
                    projectName = value;
                }
                catch (Exception)
                {

                    projectName = "";
                }
            }
        }

        public string Builder
        {
            get => builder;
            set
            {
                try
                {
                    builder = value;
                }
                catch (Exception)
                {

                    builder = "";
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

        public string CorrectionDate
        {
            get => correctionDate;
            set
            {
                try
                {
                    correctionDate = value;
                }
                catch (Exception)
                {
                    correctionDate = "";
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
            string result = "Brevdata til projekt: " + projectName;
            return result;
        }

        #endregion

    }
}
