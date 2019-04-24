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
    public class IttLetterShipping
    {
        #region Fields
        int id = 0;
        private SubEntrepeneur subEntrepeneur = new SubEntrepeneur();
        private Receiver receiver = new Receiver();
        private LetterData letterData = new LetterData();
        private string commonPdfPath = @"PDF_Documents\";
        private string personalPdfPath = @"PDF_Documents\";


        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public IttLetterShipping() { }

        /// <summary>
        /// Costructor to add a new ITT Letter Shipping
        /// </summary>
        /// <param name="subEntrepeneur">SubEntrepeneur</param>
        /// <param name="receiver">Receiver</param>
        /// <param name="letterData">PdfData</param>
        /// <param name="commmonPdfPath">string</param>
        /// <param name="pdfPath">string</param>
        public IttLetterShipping(SubEntrepeneur subEntrepeneur, Receiver receiver, LetterData letterData, string commmonPdfPath, string pdfPath)
        {
            this.id = 0;
            this.receiver = receiver;
            this.subEntrepeneur = subEntrepeneur;
            this.letterData = letterData;
            this.commonPdfPath = commmonPdfPath;
            this.personalPdfPath = pdfPath;
        }

        /// <summary>
        /// Costructor to add an ITT Letter Shipping from Db
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="subEntrepeneur">SubEntrepeneur</param>
        /// <param name="receiver">Receiver</param>
        /// <param name="letterData">PdfData</param>
        /// <param name="commmonPdfPath">string</param>
        /// <param name="pdfPath">string</param>
        public IttLetterShipping(int id, SubEntrepeneur subEntrepeneur, Receiver receiver, LetterData letterData, string commmonPdfPath, string pdfPath)
        {
            this.id = id;
            this.subEntrepeneur = subEntrepeneur;
            this.receiver = receiver;
            this.letterData = letterData;
            this.commonPdfPath = commmonPdfPath;
            this.personalPdfPath = pdfPath;
        }

        /// <summary>
        /// Constructor, that accepts data from an existing Shipping
        /// </summary>
        /// <param name="shipping">Shipping</param>
        public IttLetterShipping(IttLetterShipping shipping)
        {
            this.id = shipping.Id;
            this.subEntrepeneur = shipping.SubEntrepeneur;
            this.receiver = shipping.Receiver;
            this.letterData = shipping.LetterData;
            this.commonPdfPath = shipping.CommonPdfPath;
            this.personalPdfPath = shipping.PersonalPdfPath;
        }

        #endregion

        #region Properties
        public int Id { get => id; }

        public SubEntrepeneur SubEntrepeneur { get => subEntrepeneur; set => subEntrepeneur = value; }

        public Receiver Receiver { get => receiver; set => receiver = value; }

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

        public string PersonalPdfPath
        {
            get => personalPdfPath;
            set
            {
                try
                {
                    if (value != null)
                    {
                        personalPdfPath = value;
                    }
                    else
                    {
                        personalPdfPath = "";
                    }
                }
                catch (Exception)
                {
                    personalPdfPath = "";
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
            return commonPdfPath + "\n" + personalPdfPath;
        }

        /// <summary>
        /// Returns main content as a string
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            return commonPdfPath + ", " + personalPdfPath;
        }

        #endregion

    }
}
