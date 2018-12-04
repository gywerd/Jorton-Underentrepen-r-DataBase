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
        Project project;
        string commonPdfPath = "";
        string pdfPath = "";


        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public IttLetterShipping()
        {
            this.id = 0;
            this.project = new Project();
            this.commonPdfPath = @"PDF_Documents\";
            this.pdfPath = "";
        }

        /// <summary>
        /// Constructor for adding new ITT Letter Shipping
        /// </summary>
        /// <param name="commmonPdfPath">string</param>
        /// <param name="pdfPath">string</param>
        public IttLetterShipping(Project project, string commmonPdfPath, string pdfPath)
        {
            this.id = 0;
            this.project = project;
            this.commonPdfPath = commmonPdfPath;
            this.pdfPath = pdfPath;
        }

        /// <summary>
        /// Constructor for adding ITT Letter Shipping from Db
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="commmonPdfPath">string</param>
        /// <param name="pdfPath">string</param>
        public IttLetterShipping(int id, Project project, string commmonPdfPath, string pdfPath)
        {
            this.id = id;
            this.project = project;
            this.commonPdfPath = commmonPdfPath;
            this.pdfPath = pdfPath;
        }

        /// <summary>
        /// Constructor for accepts an existing Itt Letter Shipping
        /// </summary>
        /// <param name="shipping">IttLetterShipping</param>
        public IttLetterShipping(IttLetterShipping shipping)
        {
            this.id = shipping.Id;
            this.project = shipping.Project;
            this.commonPdfPath = shipping.CommonPdfPath;
            this.pdfPath = shipping.PdfPath;
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

        #region Properties
        public int Id { get => id; }

        public Project Project { get; set; }

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
    }
}
