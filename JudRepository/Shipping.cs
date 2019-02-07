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
        private Project project;
        private Receiver receiver;
        private SubEntrepeneur subEntrepeneur;
        private LetterData letterData;
        private string commonPdfPath = "";
        private string pdfPath = "";


        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public Shipping()
        {
            this.id = 0;
            this.project = new Project();
            this.receiver = new Receiver();
            this.subEntrepeneur = new SubEntrepeneur();
            this.letterData = new LetterData();
            this.commonPdfPath = @"PDF_Documents\";
            this.pdfPath = @"PDF_Documents\";
        }

        /// <summary>
        /// Costructor to add a new ITT Letter Shipping
        /// </summary>
        /// <param name="project">Project</param>
        /// <param name="receiver">Receiver</param>
        /// <param name="subEntrepeneur">SubEntrepeneur</param>
        /// <param name="letterData">PdfData</param>
        /// <param name="commmonPdfPath">string</param>
        /// <param name="pdfPath">string</param>
        public Shipping(Project project, Receiver receiver, SubEntrepeneur subEntrepeneur, LetterData letterData, string commmonPdfPath, string pdfPath)
        {
            this.id = 0;
            this.project = project;
            this.receiver = receiver;
            this.subEntrepeneur = subEntrepeneur;
            this.letterData = letterData;
            this.commonPdfPath = commmonPdfPath;
            this.pdfPath = pdfPath;
        }

        /// <summary>
        /// Costructor to add an ITT Letter Shipping from Db
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="project">Project</param>
        /// <param name="receiver">Receiver</param>
        /// <param name="subEntrepeneur">SubEntrepeneur</param>
        /// <param name="letterData">PdfData</param>
        /// <param name="commmonPdfPath">string</param>
        /// <param name="pdfPath">string</param>
        public Shipping(int id, Project project, Receiver receiver, SubEntrepeneur subEntrepeneur, LetterData letterData, string commmonPdfPath, string pdfPath)
        {
            this.id = id;
            this.project = project;
            this.receiver = receiver;
            this.subEntrepeneur = subEntrepeneur;
            this.letterData = letterData;
            this.commonPdfPath = commmonPdfPath;
            this.pdfPath = pdfPath;
        }

        /// <summary>
        /// Constructor, that accepts data from an existing Shipping
        /// </summary>
        /// <param name="shipping">Shipping</param>
        public Shipping(Shipping shipping)
        {
            this.id = shipping.Id;
            this.project = shipping.Project;
            this.receiver = shipping.Receiver;
            this.subEntrepeneur = shipping.SubEntrepeneur;
            this.letterData = shipping.LetterData;
            this.commonPdfPath = shipping.CommonPdfPath;
            this.pdfPath = shipping.PdfPath;
        }

        #endregion

        #region Properties
        public int Id { get => id; }

        public Project Project { get => project; set => project = value; }

        public Receiver Receiver { get => receiver; set => receiver = value; }

        public SubEntrepeneur SubEntrepeneur { get => subEntrepeneur; set => subEntrepeneur = value; }

        public LetterData LetterData { get => letterData; set => letterData = value; }

        public string CommonPdfPath
        {
            get => commonPdfPath;
            set
            {
                try
                {
                    if (value != null)
                    {
                        commonPdfPath = value;
                    }
                    else
                    {
                        commonPdfPath = "";
                    }
                }
                catch (Exception)
                {
                    commonPdfPath = "";
                }
            }
        }

        public string PdfPath
        {
            get => pdfPath;
            set
            {
                try
                {
                    if (value != null)
                    {
                        pdfPath = value;
                    }
                    else
                    {
                        pdfPath = "";
                    }
                }
                catch (Exception)
                {
                    pdfPath = "";
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
        /// Returns main content as string with multiple rows
        /// </summary>
        /// <returns>string</returns>
        public string ToLongString()
        {
            return commonPdfPath + "\n" + pdfPath;
        }

        /// <summary>
        /// Returns main content as a string
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            return commonPdfPath + ", " + pdfPath;
        }

        #endregion

    }
}
