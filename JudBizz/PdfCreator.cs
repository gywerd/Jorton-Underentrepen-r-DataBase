using itextsharp.pdfa;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using iTextSharp.tool.xml;
using JudRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JudBizz
{
    public class PdfCreator
    {
        #region Fields
        Bizz CBZ = new Bizz();
        string date = "";
        string fileName = "";
        string pdfPath = "";
        string strConnection = "";
        FileStream fileStreamCreate;
        List<Bullet> ProjectBullets = new List<Bullet>();
        List<Enterprise> ProjectEnterprises = new List<Enterprise>();
        List<Paragraf> ProjectParagrafs = new List<Paragraf>();
        List<Receiver> ProjectReceivers = new List<Receiver>();
        List<Shipping> ProjectShippings = new List<Shipping>();

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public PdfCreator(string strCon)
        {
            strConnection = strCon;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Method to add single cell to the header 
        /// </summary>
        /// <param name="tableLayout">PdfPTable</param>
        /// <param name="cellText">string</param>
        private static void AddCellToHeader(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 10, 1, iTextSharp.text.BaseColor.WHITE))) { HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = new iTextSharp.text.BaseColor(0, 104, 64) });
        }

        /// <summary>
        /// Method to add single cell to the body - aligned left
        /// </summary>
        /// <param name="tableLayout">PdfPTable</param>
        /// <param name="cellText">string</param>
        private static void AddCellToBodyLeft(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 10, 0, iTextSharp.text.BaseColor.BLACK))) { HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = iTextSharp.text.BaseColor.WHITE });
        }

        /// <summary>
        /// Method to add single cell to the body - aligned right
        /// </summary>
        /// <param name="tableLayout">PdfPTable</param>
        /// <param name="cellText">string</param>
        private static void AddCellToBodyRight(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 10, 0, iTextSharp.text.BaseColor.BLACK))) { HorizontalAlignment = Element.ALIGN_RIGHT, Padding = 5, BackgroundColor = iTextSharp.text.BaseColor.WHITE });
        }

        /// <summary>
        /// Method, that adds content to the EnterprisList PDF
        /// </summary>
        /// <param name="tableLayout">PdfPTable</param>
        /// <returns>PdfPTable</returns>
        private PdfPTable AddContentToEnterprisesPdfTable(PdfPTable tableLayout)
        {
            float[] headers = { 50, 50 };  //Header Widths
            tableLayout.SetWidths(headers);        //Set the pdf headers
            tableLayout.WidthPercentage = 100;       //Set the PDF File witdh percentage

            DateTime today = DateTime.Today;
            string date = @"Udskrift pr.: " + today.ToShortDateString();
            string executive = @"Tilbudsansvarlig: " + CBZ.TempProject.Executive.Person.Name;

            //Add Title to the PDF file at the top
            tableLayout.AddCell(new PdfPCell(new Phrase("Jorton A/S", new Font(Font.FontFamily.HELVETICA, 24, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("Entrepriseliste", new Font(Font.FontFamily.HELVETICA, 24, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(date, new Font(Font.FontFamily.HELVETICA, 12, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("Sagsnr: " + CBZ.TempProject.Case, new Font(Font.FontFamily.HELVETICA, 12, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 20, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(CBZ.TempProject.Details.Name, new Font(Font.FontFamily.HELVETICA, 12, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 20, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(executive, new Font(Font.FontFamily.HELVETICA, 12, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 20, HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_BOTTOM });

            //Add header
            AddCellToHeader(tableLayout, "Entreprise");
            AddCellToHeader(tableLayout, "Beskrivelse");

            //Add body
            RefreshProjectEnterprises();

            foreach (Enterprise enterprise in ProjectEnterprises)
            {
                AddCellToBodyLeft(tableLayout, enterprise.Name);
                AddCellToBodyLeft(tableLayout, enterprise.Elaboration);
            }

            return tableLayout;

        }

        /// <summary>
        /// Method, that adds content to the IttLetter PDF
        /// </summary>
        /// <param name="tableLayout">PdfPTable</param>
        /// <param name="project">Project</param>
        /// <returns>PdfPTable</returns>
        private PdfPTable AddContentToIttLetterCommonPdfTable(PdfPTable tableLayout, Project project)
        {
            float[] headers = { 5, 5, 90 };  //Header Widths
            tableLayout.SetWidths(headers);        //Set the pdf headers
            tableLayout.WidthPercentage = 100;       //Set the PDF File witdh percentage

            DateTime today = DateTime.Today;
            string date = @"Aabenraa den " + today.ToString(@"dd-MM-yyyy");

            //Add address and colophon
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 48, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("Gældende for afgivelse af tilbuddet er følgende udbudsmateriale:\n", new Font(Font.FontFamily.HELVETICA, 12, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });

            //Add body
            RefreshProjectParagrafs();
            Chunk c = new Chunk(CBZ.TempShipping.LetterData.ConditionUrl, new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(0, 0, 0)));
            c.SetAnchor(CBZ.TempShipping.LetterData.ConditionUrl);
            Chunk urlChunk = new Chunk("Klik for at se", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(0, 0, 0)));
            urlChunk.Chunks.Add(c);

            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });

            foreach (Paragraf paragraf in ProjectParagrafs)
            {
                tableLayout.AddCell(new PdfPCell(new Phrase("• " + paragraf.Text + ":", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 2, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
                tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 2, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });

                string lines = "";

                foreach (Bullet bullet in ProjectBullets)
                {
                    if (bullet.Paragraf.Id == paragraf.Id)
                    {
                        lines += "• " + bullet.Text + "\n";
                    }
                }

                lines.Remove(lines.Length - 2, 2);

                tableLayout.AddCell(new PdfPCell(new Phrase(lines, new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });

                tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
                tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            }
            tableLayout.AddCell(new PdfPCell(new Phrase(urlChunk)) { Colspan = 2, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });

            return tableLayout;
        }

        /// <summary>
        /// Method, that adds content to the company IttLetter PDF
        /// </summary>
        /// <param name="tableLayout">PdfPTable</param>
        /// <param name="shipping">Shipping</param>
        /// <returns>PdfPTable</returns>
        private PdfPTable AddContentToIttLetterCompanyPdfTable(PdfPTable tableLayout, Shipping shipping)
        {
            float[] headers = { 30, 35, 35 };  //Header Widths
            tableLayout.SetWidths(headers);        //Set the pdf headers
            tableLayout.WidthPercentage = 100;       //Set the PDF File witdh percentage

            DateTime today = DateTime.Today;
            string date = @"Aabenraa den " + today.ToString(@"dd-MM-yyyy");
            string executivePhone = @"Tlf.: ";
            if (executivePhone != "")
            {
                executivePhone += shipping.SubEntrepeneur.Enterprise.Project.Executive.Person.ContactInfo.Phone;
            }
            else
            {
                executivePhone += shipping.SubEntrepeneur.Enterprise.Project.Executive.Person.ContactInfo.Mobile;
            }
            string executiveMail = @"Mail: " + shipping.SubEntrepeneur.Enterprise.Project.Executive.Person.ContactInfo.Email;
            string receiverContactName = @"Att.: " + shipping.SubEntrepeneur.Contact.Person.Name;

            //Add address and colophon
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 48, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(shipping.SubEntrepeneur.Entrepeneur.Entity.Name, new Font(Font.FontFamily.HELVETICA, 12, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(shipping.SubEntrepeneur.Enterprise.Project.Executive.Initials, new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(receiverContactName, new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(executivePhone, new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 2, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(executiveMail, new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 2, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(date, new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });

            //Add body
            string enterpriseLine = GetEnterpriseLine();
            Chunk urlChunk = new Chunk(CBZ.TempShipping.LetterData.MaterialUrl, new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(0, 0, 0)));
            urlChunk.SetAnchor(CBZ.TempShipping.LetterData.MaterialUrl);

            tableLayout.AddCell(new PdfPCell(new Phrase("UDBUDSBREV VEDR. " + shipping.SubEntrepeneur.Enterprise.Project.Details.Name + "\n", new Font(Font.FontFamily.HELVETICA, 12, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("I anmodes hermed om at give tilbud på følgende entreprise:\n", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(enterpriseLine + "\n", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("Tilbud bedes fremsendt til undertegnede senest den " + CBZ.TempShipping.LetterData.AnswerDate + ", kl. 12.00\n", new Font(Font.FontFamily.HELVETICA, 12, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("Spørgsmål til udbuddet fremsendes senest den " + CBZ.TempShipping.LetterData.QuestionDate + ", kl. 12.00", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("Sidste rettelsesblad udsendes " + CBZ.TempShipping.LetterData.CorrectionDate + ", kl. 12.00 - fra rådgiveren\n", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("Bygherrens tilbudsliste ønskes modtaget udfyldt i Excel-format og gerne suppleret med eksemplar i PDF format.\n", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("JORTON forventer, at vores samarbejdspartnere overholder gældende overenskomster og regler for anvendelse af udenlandsk arbejdskraft.", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("JORTON afgiver hovedentreprisetilbud til " + CBZ.TempShipping.LetterData.Builder + ". Udførelsesperioden er " + this.CBZ.TempShipping.LetterData.TimeSpan + " jf. bygherrens udbudstidsplan.\n", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("Såfremt I efter gennemgang af udbudsmaterialet ikke er interesseret i at afgive tilbud, eller der er mangler ved eller opstår spørgsmål til udbudsmaterialet, er I naturligvis velkommen til at kontakte undertegnede.\n", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("Udbudsmaterialet kan ses og hentes på denne adresse:\n", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(urlChunk)) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("\n", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("Adgangskode: " + this.CBZ.TempShipping.LetterData.PassWord + "\n", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("Vi ser frem til at modtage jeres tilbud.\n", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("Med venlig hilsen\n", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(shipping.SubEntrepeneur.Enterprise.Project.Executive.Person.Name + "\n", new Font(Font.FontFamily.HELVETICA, 12, 2, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(shipping.SubEntrepeneur.Enterprise.Project.Executive.JobDescription.Occupation + "\n", new Font(Font.FontFamily.HELVETICA, 12, 2, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("Bilag – oplistning af udbudsdokumenter", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });

            return tableLayout;
        }

        /// <summary>
        /// Method, that adds content to the company IttLetter PDF
        /// </summary>
        /// <param name="tableLayout">PdfPTable</param>
        /// <param name="project">Project</param>
        /// <param name="receiver">Receiver</param>
        /// <returns>PdfPTable</returns>
        private PdfPTable AddContentToRequestPdfTable(PdfPTable tableLayout)
        {
            float[] headers = { 30, 35, 35 };  //Header Widths
            tableLayout.SetWidths(headers);        //Set the pdf headers
            tableLayout.WidthPercentage = 100;       //Set the PDF File witdh percentage

            DateTime today = DateTime.Today;
            string date = CBZ.TempShipping.SubEntrepeneur.Enterprise.Project.Executive.Department.Address.ZipTown.Town +@" den " + today.ToLongDateString();
            string jortonAddress = CBZ.GetAddress(1450).ToLongString();
            string jortonId = CBZ.TempShipping.SubEntrepeneur.Enterprise.Project.Executive.Department.Url + "\nCvr.nr: " + CBZ.TempShipping.SubEntrepeneur.Enterprise.Project.Executive.Department.Cvr + "\n";
            string executiveName = @"Att.: " + CBZ.TempShipping.SubEntrepeneur.Enterprise.Project.Executive.Person.Name;
            string executiveInitials = @"lpj/" + CBZ.TempShipping.SubEntrepeneur.Enterprise.Project.Executive.Initials;
            string executivePhone = @"Tlf.: " + CBZ.TempShipping.SubEntrepeneur.Enterprise.Project.Executive.Person.ContactInfo.Phone;
            string executiveMail = @"E-Mail: " + CBZ.TempShipping.SubEntrepeneur.Enterprise.Project.Executive.Person.ContactInfo.Email;
            string receiverContactName = @"Att.: " + CBZ.TempShipping.SubEntrepeneur.Contact.Person.Name;

            //Add address and colophon
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 36, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(CBZ.TempShipping.SubEntrepeneur.Entrepeneur.Entity.Name, new Font(Font.FontFamily.HELVETICA, 12, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(@"Jorton A/S", new Font(Font.FontFamily.HELVETICA, 12, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(receiverContactName, new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 2, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(executiveName, new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(CBZ.TempShipping.SubEntrepeneur.Entrepeneur.Entity.Address.ToLongString(), new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(jortonAddress, new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_TOP });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 2, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(jortonId, new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_TOP });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 2, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(executiveInitials, new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 2, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(executivePhone, new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 2, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(executiveMail, new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(" ", new Font(Font.FontFamily.HELVETICA, 8, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 2, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(date, new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(" ", new Font(Font.FontFamily.HELVETICA, 8, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });

            //Add body
            if (CBZ.TempShipping.AcceptUrl == "" || CBZ.TempShipping.DeclineUrl == "")
            {
                CBZ.TempShipping.SetAcceptDeclineUrls();
            }
            Chunk acceptUrlChunk = new Chunk("Vi ønsker at afgive tilbud", new Font(Font.FontFamily.HELVETICA, 12, 2, new iTextSharp.text.BaseColor(0, 0, 255)));
            acceptUrlChunk.SetAnchor(CBZ.TempShipping.AcceptUrl);
            Chunk declineUrlChunk = new Chunk("Vi ønsker ikke at afgive tilbud", new Font(Font.FontFamily.HELVETICA, 12, 2, new iTextSharp.text.BaseColor(0, 0, 255)));
            declineUrlChunk.SetAnchor(CBZ.TempShipping.DeclineUrl);

            tableLayout.AddCell(new PdfPCell(new Phrase("FORESPØRGSEL VEDR. " + CBZ.TempShipping.SubEntrepeneur.Enterprise.Project.Details.Name + "\n", new Font(Font.FontFamily.HELVETICA, 12, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("Jorton AS er prækvalificeret til at afgive tilbud i hovedentreprise på ovennævnte projekt.\n", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(" ", new Font(Font.FontFamily.HELVETICA, 8, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("I den forbindelse indbydes " +  CBZ.TempShipping.SubEntrepeneur.Entrepeneur.Entity.Name + " til at afgive tilbud på:\n", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(CBZ.TempShipping.SubEntrepeneur.Enterprise.Name, new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(" ", new Font(Font.FontFamily.HELVETICA, 8, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("Du kan tilkendegive din interesse ved at aktivere et af de to link:\n", new Font(Font.FontFamily.HELVETICA, 12, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(acceptUrlChunk)) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(declineUrlChunk)) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(" ", new Font(Font.FontFamily.HELVETICA, 8, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("Bygherren beskriver projektet således:\n", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(CBZ.TempShipping.SubEntrepeneur.Enterprise.Project.Details + "\n", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(" ", new Font(Font.FontFamily.HELVETICA, 8, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("Udførelsesperioden er " + CBZ.TempShipping.SubEntrepeneur.Enterprise.Project.Details.Period + "\n", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(" ", new Font(Font.FontFamily.HELVETICA, 8, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("Tilbudsfristen er " + CBZ.TempShipping.SubEntrepeneur.Enterprise.Project.Details.AnswerDate + "\n", new Font(Font.FontFamily.HELVETICA, 12, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(" ", new Font(Font.FontFamily.HELVETICA, 8, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("Hvis I vælger at afgive tilbud, så anfør gerne kontaktperson. Den valgte kontaktperson vil snarest modtage udbudsbrev med adgang til udbudsmaterialet.\n", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(" ", new Font(Font.FontFamily.HELVETICA, 8, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });

            //Add Regards
            string contactInfo = GetContactInfo();
            tableLayout.AddCell(new PdfPCell(new Phrase("Med venlig hilsen", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(" ", new Font(Font.FontFamily.HELVETICA, 8, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("Jorton A/S\n", new Font(Font.FontFamily.HELVETICA, 12, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(CBZ.TempShipping.SubEntrepeneur.Enterprise.Project.Executive.Person.Name, new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("Tilbudsleder", new Font(Font.FontFamily.HELVETICA, 12, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(contactInfo + "\n", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });

            return tableLayout;
        }

        /// <summary>
        /// Method, that adds content to the Pdf PTable
        /// </summary>
        /// <param name="tableLayout">PdfPTable</param>
        /// <returns>PdfPTable</returns>
        private PdfPTable AddContentToSubEntrepeneursPdfTable(PdfPTable tableLayout)
        {
            float[] headers = { 20, 20, 10, 10, 13, 7, 6, 6, 8 };  //Header Widths
            tableLayout.SetWidths(headers);        //Set the pdf headers
            tableLayout.WidthPercentage = 100;       //Set the PDF File witdh percentage

            DateTime today = DateTime.Today;
            string date = @"Udskrift pr.: " + today.ToShortDateString();
            string executive = @"Tilbudsansvarlig: " + CBZ.TempProject.Executive.Person.Name;

            //Add Title to the PDF file at the top
            tableLayout.AddCell(new PdfPCell(new Phrase("Jorton A/S", new Font(Font.FontFamily.HELVETICA, 18, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 18, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("Fagentrepenører", new Font(Font.FontFamily.HELVETICA, 18, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 2, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 18, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 2, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(date, new Font(Font.FontFamily.HELVETICA, 12, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("Sagsnr: " + CBZ.TempProject.Case, new Font(Font.FontFamily.HELVETICA, 12, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 20, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 18, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(CBZ.TempProject.Details.Name, new Font(Font.FontFamily.HELVETICA, 12, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 2, Border = 0, PaddingBottom = 20, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 18, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 2, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(executive, new Font(Font.FontFamily.HELVETICA, 12, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 3, Border = 0, PaddingBottom = 20, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM });

            //Add header
            AddCellToHeader(tableLayout, "");
            AddCellToHeader(tableLayout, "Kontaktperson");
            AddCellToHeader(tableLayout, "Tlf.");
            AddCellToHeader(tableLayout, "Mobil");
            AddCellToHeader(tableLayout, "Email");
            AddCellToHeader(tableLayout, "Afgiver tilbud");
            AddCellToHeader(tableLayout, "Vedstå");
            AddCellToHeader(tableLayout, "Forbe.");
            AddCellToHeader(tableLayout, "Pris");

            //Add body
            RefreshProjectEnterprises();

            foreach (Enterprise tempEnterprise in ProjectEnterprises)
            {
                if (tempEnterprise.Project.Id == CBZ.TempProject.Id)
                {
                    tableLayout.AddCell(new PdfPCell(new Phrase(tempEnterprise.Name, new Font(Font.FontFamily.HELVETICA, 10, 1, iTextSharp.text.BaseColor.BLACK))) { Colspan = 9, HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = iTextSharp.text.BaseColor.WHITE });
                    foreach (IndexedSubEntrepeneur subEntrepeneur in CBZ.IndexedSubEntrepeneurs)
                    {
                        if (subEntrepeneur.Enterprise.Id == tempEnterprise.Id)
                        {
                            Contact contact = subEntrepeneur.Contact;
                            ContactInfo info = contact.Person.ContactInfo;
                            string uphold = "Nej";
                            if (subEntrepeneur.Uphold)
                            {
                                uphold = "Ja";
                            }
                            string reservations = "Nej";
                            if (subEntrepeneur.Uphold)
                            {
                                reservations = "Ja";
                            }
                            AddCellToBodyLeft(tableLayout, subEntrepeneur.Entrepeneur.Entity.Name);
                            AddCellToBodyLeft(tableLayout, contact.Person.Name);
                            AddCellToBodyRight(tableLayout, info.Phone);
                            AddCellToBodyRight(tableLayout, info.Mobile);
                            AddCellToBodyLeft(tableLayout, info.Email);
                            AddCellToBodyLeft(tableLayout, GetOfferStatus(subEntrepeneur));
                            AddCellToBodyLeft(tableLayout, uphold);
                            AddCellToBodyLeft(tableLayout, reservations);
                            AddCellToBodyRight(tableLayout, subEntrepeneur.Offer.Price.ToString());
                        }
                    }
                }
            }

            return tableLayout;
        }

        /// <summary>
        /// Method, that adds content to the Pdf PTable to be used when making agreements
        /// </summary>
        /// <param name="tableLayout">PdfPTable</param>
        /// <returns>PdfPTable</returns>
        private PdfPTable AddContentToSubEntrepeneursPdfTableForAgreement(PdfPTable tableLayout, List<IndexedSubEntrepeneur> subEntrepeneurs)
        {
            float[] headers = { 20, 20, 20, 20, 6, 6, 8 };  //Header Widths
            tableLayout.SetWidths(headers);        //Set the pdf headers
            tableLayout.WidthPercentage = 100;       //Set the PDF File witdh percentage

            DateTime today = DateTime.Today;
            string date = @"Udskrift pr.: " + today.ToShortDateString();
            string executive = @"Tilbudsansvarlig: " + CBZ.TempProject.Executive.Person.Name;

            //Add Title to the PDF file at the top
            tableLayout.AddCell(new PdfPCell(new Phrase("Jorton A/S", new Font(Font.FontFamily.HELVETICA, 18, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 18, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("Fagentrepenører", new Font(Font.FontFamily.HELVETICA, 18, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 18, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(date, new Font(Font.FontFamily.HELVETICA, 12, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("Sagsnr: " + CBZ.TempProject.Case, new Font(Font.FontFamily.HELVETICA, 12, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 20, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 18, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(CBZ.TempProject.Details.Name, new Font(Font.FontFamily.HELVETICA, 12, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 20, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 18, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(executive, new Font(Font.FontFamily.HELVETICA, 12, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 3, Border = 0, PaddingBottom = 20, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM });

            //Add header
            AddCellToHeader(tableLayout, "");
            AddCellToHeader(tableLayout, "Kontaktperson");
            AddCellToHeader(tableLayout, "Kontaktoplysninger");
            AddCellToHeader(tableLayout, "Tilbudbrev");
            AddCellToHeader(tableLayout, "Vedstå");
            AddCellToHeader(tableLayout, "Forbe.");
            AddCellToHeader(tableLayout, "Pris");

            RefreshProjectEnterprises();

            //Add body
            foreach (Enterprise temp in ProjectEnterprises)
            {
                if (temp.Project.Id == CBZ.TempProject.Id)
                {
                    tableLayout.AddCell(new PdfPCell(new Phrase(temp.Name, new Font(Font.FontFamily.HELVETICA, 10, 1, iTextSharp.text.BaseColor.BLACK))) { Colspan = 9, HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = iTextSharp.text.BaseColor.WHITE });
                    foreach (IndexedSubEntrepeneur subEntrepeneur in subEntrepeneurs)
                    {
                        if (subEntrepeneur.Enterprise.Id == temp.Id)
                        {
                            Contact contact = subEntrepeneur.Contact;
                            ContactInfo info = contact.Person.ContactInfo;
                            String ittletterSentDate = "Sendt: " + subEntrepeneur.IttLetter.SentDate.ToShortDateString();
                            string uphold = "Nej";
                            if (subEntrepeneur.Uphold)
                            {
                                uphold = "Ja";
                            }
                            string reservations = "Nej";
                            if (subEntrepeneur.Uphold)
                            {
                                reservations = "Ja";
                            }
                            AddCellToBodyLeft(tableLayout, subEntrepeneur.Entrepeneur.Entity.Name);
                            AddCellToBodyLeft(tableLayout, contact.Person.Name);
                            AddCellToBodyRight(tableLayout, info.ToLongString());
                            AddCellToBodyLeft(tableLayout, ittletterSentDate);
                            AddCellToBodyLeft(tableLayout, uphold);
                            AddCellToBodyLeft(tableLayout, reservations);
                            AddCellToBodyRight(tableLayout, subEntrepeneur.Offer.Price.ToString());
                        }
                    }
                }
            }

            return tableLayout;
        }

        /// <summary>
        /// Method, that generates a pdf document with of EnterprisesList
        /// </summary>
        /// <param name="cbz">Bizz</param>
        /// <returns>string</returns>
        public string GenerateEnterprisesPdf(Bizz cbz)
        {
            this.CBZ = cbz;
            date = DateTime.Today.ToString(@"yyyy-MM-dd");
            fileName = "Projekt_" + CBZ.TempProject.Case.ToString() + "_Entrepriseliste_" + date + ".pdf";
            pdfPath = @"PDF_Documents\" + fileName;
            
            //Create document
            Document document = new Document(PageSize.A4.Rotate(), 48, 48, 48, 48);

            //Create PDF Table
            PdfPTable tableLayout = new PdfPTable(3);

            //Create a PDF file in specific path
            PdfWriter.GetInstance(document, new FileStream(pdfPath, FileMode.Create));

            //Open the PDF document
            document.Open();

            //Add Content to PDF
            document.Add(AddContentToEnterprisesPdfTable(tableLayout));

            // Closing the document
            document.Close();

            return pdfPath;

        }

        /// <summary>
        /// Method, that generates common part of pdf letter
        /// </summary>
        /// <param name="cbz">Bizz</param>
        /// <returns>string</returns>
        public string GenerateIttLetterCommonPdf(Bizz cbz)
        {
            this.CBZ = cbz;
            RefreshProjectShippings();
            date = DateTime.Today.ToString(@"yyyy-MM-dd");
            fileName = "Projekt_" + CBZ.TempProject.Details.Name.Replace(" ", "_") + "_Foelgebrev_faelles_" + date + ".pdf";
            pdfPath = @"PDF_Documents\" + fileName;
            fileStreamCreate = new FileStream(pdfPath, FileMode.Create);

            //step 1
            //Create document
            Document document = new Document(PageSize.A4.Rotate(), 48, 48, 48, 48);

            //Create PDF Table
            PdfPTable tableLayout = new PdfPTable(3);

            // step 2
            //Create a PDF file in specific path
            PdfWriter.GetInstance(document, fileStreamCreate);

            //Open the PDF document
            document.Open();

            try
            {
                // step 2
                PdfWriter docWriter = PdfWriter.GetInstance(document, fileStreamCreate);
                PdfWriterEvents writerEvent = new PdfWriterEvents("images/jorton-logo");
                docWriter.PageEvent = writerEvent;
                //docWriter.PageEvent = new ITextEvents();

                //Open the PDF document
                //document.Open();

                //Add Content to PDF
                document.Add(AddContentToIttLetterCommonPdfTable(tableLayout, CBZ.TempProject));

            }
            catch (Exception ex)
            {
                MessageBox.Show(@"Der opstod en fejl ved indsætning af indhold dokument\n\n" + ex, "PdfCreator", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                // Closing the document
                document.Close();
            }

            return pdfPath;

        }

        /// <summary>
        /// Method, that generates private part of pdf letter
        /// </summary>
        /// <param name="cbz">Bizz</param>
        /// <param name="shipping">Shipping</param>
        /// <returns>string</returns>
        public string GenerateIttLetterCompanyPdf(Bizz cbz, Shipping shipping)
        {
            this.CBZ = cbz;
            CBZ.TempShipping = shipping;
            date = DateTime.Today.ToString(@"yyyy-MM-dd");
            fileName = "Projekt_" + shipping.SubEntrepeneur.Enterprise.Project.Details.Name.Replace(" ", "_") + "_Foelgebrev_firma_til_" + shipping.SubEntrepeneur.Entrepeneur.Entity.Name.Replace(" ", "_") + "_" + date + ".pdf";
            pdfPath = @"PDF_Documents\" + fileName;
            fileStreamCreate = new FileStream(pdfPath, FileMode.Create);

            //step 1
            //Create document
            Document document = new Document(PageSize.A4.Rotate(), 48, 48, 48, 48);

            //Create PDF Table
            PdfPTable tableLayout = new PdfPTable(3);

            // step 2
            //Create a PDF file in specific path
            PdfWriter.GetInstance(document, fileStreamCreate);

            //Open the PDF document
            document.Open();

            try
            {
                // step 2
                PdfWriter docWriter = PdfWriter.GetInstance(document, fileStreamCreate);
                PdfWriterEvents writerEvent = new PdfWriterEvents("images/jorton-logo");
                docWriter.PageEvent = writerEvent;
                //docWriter.PageEvent = new ITextEvents();

                //Open the PDF document
                //document.Open();

                //Add Content to PDF
                document.Add(AddContentToIttLetterCompanyPdfTable(tableLayout, CBZ.TempShipping));

            }
            catch (Exception ex)
            {
                MessageBox.Show(@"Der opstod en fejl ved indsætning af indhold dokument\n\n" + ex, "PdfCreator", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                // Closing the document
                document.Close();
            }

            return pdfPath;

        }

        /// <summary>
        /// Method, that generates Request pdf letter
        /// </summary>
        /// <param name="cbz">Bizz</param>
        /// <param name="requestData">RequestData</param>
        /// <returns></returns>
        public string GenerateRequestPdf(Bizz cbz, Shipping requestData)
        {
            //this.CBZ = cbz;
            //CBZ.TempRequestData = requestData;
            //date = DateTime.Today.ToString(@"yyyy-MM-dd");
            //fileName = "Projekt_" + cbz.TempRequestData.Project.Name.Replace(" ", "_") + "_Forespørgsel_til_" + cbz.TempRequestData.Receiver.Name.Replace(" ", "_") + "_" + date + ".pdf";
            //pdfPath = @"PDF_Documents\" + fileName;
            //fileStreamCreate = new FileStream(pdfPath, FileMode.Create);

            ////step 1
            ////Create document
            //Document document = new Document(PageSize.A4.Rotate(), 48, 48, 48, 48);

            ////Create PDF Table
            //PdfPTable tableLayout = new PdfPTable(3);

            //// step 2
            ////Create a PDF file in specific path
            //PdfWriter.GetInstance(document, fileStreamCreate);

            //////Open the PDF document
            ////document.Open();

            //try
            //{
            //    // step 2
            //    PdfWriter docWriter = PdfWriter.GetInstance(document, fileStreamCreate);
            //    PdfWriterEvents writerEvent = new PdfWriterEvents(@"images\jorton-logo.png");
            //    docWriter.PageEvent = writerEvent;
            //    docWriter.PageEvent = new ITextEvents();

            //    //Open the PDF document
            //    document.Open();

            //    //Add Content to PDF
            //    document.Add(AddContentToRequestPdfTable(tableLayout));
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(@"Der opstod en fejl ved indsætning af indhold dokument\n\n" + ex, "Indsæt indhold i dokument", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
            //finally
            //{
            //    // Closing the document
            //    document.Close();
            //}

            //return pdfPath;
            this.CBZ = cbz;
            date = DateTime.Today.ToString(@"yyyy-MM-dd");
            string receiver = cbz.TempShipping.SubEntrepeneur.Entrepeneur.Entity.Name.Replace(" ", "_");
            receiver = receiver.Replace(@"/", "");
            fileName = @"Projekt_" + CBZ.TempShipping.SubEntrepeneur.Enterprise.Project.Details.Name.Replace(" ", "_") + @"_Forespørgsel_til_" + receiver + @"_" + date + @".pdf";
            pdfPath = @"PDF_Documents\" + fileName;

            //Create document
            Document document = new Document(PageSize.A4, 48, 48, 48, 48);

            //Create PDF Table
            PdfPTable tableLayout = new PdfPTable(3);

            //Create a PDF file in specific path
            string absolute_path = System.IO.Path.Combine(CBZ.AppPath + pdfPath);
            PdfWriter.GetInstance(document, new FileStream(absolute_path, FileMode.Create));

            //Open the PDF document
            document.Open();

            //Add Content to PDF
            document.Add(AddContentToRequestPdfTable(tableLayout));

            // Closing the document
            document.Close();

            return pdfPath;

        }

        /// <summary>
        /// Method, that generates a pdf document with a SubEntrepeneurs list
        /// </summary>
        /// <param name="cbz">Bizz</param>
        /// <param name="subEntrepeneurs">List<IndexedSubEntrepeneur></param>
        /// <returns>string</returns>
        public string GenerateSubEntrepeneursPdf(Bizz cbz, List<IndexedSubEntrepeneur> subEntrepeneurs)
        {
            this.CBZ = cbz;
            date = DateTime.Today.ToString(@"yyyy-MM-dd");
            CBZ.IndexedSubEntrepeneurs = subEntrepeneurs;
            fileName = "Projekt" + CBZ.TempProject.Case.ToString() + "_Underentrenoerer_" + date + ".pdf";
            pdfPath = @"PDF_Documents\" + fileName;

            //Create document
            Document document = new Document(PageSize.A4.Rotate(), 48, 48, 48, 48);

            //Create PDF Table
            PdfPTable tableLayout = new PdfPTable(9);

            //Create a PDF file in specific path
            PdfWriter.GetInstance(document, new FileStream(pdfPath, FileMode.Create));

            //Open the PDF document
            document.Open();

            //Add Content to PDF
            document.Add(AddContentToSubEntrepeneursPdfTable(tableLayout));

            // Closing the document
            document.Close();

            return pdfPath;

        }

        /// <summary>
        /// Method, that generates a pdf document with a SubEntrepeneurs list, to be used when making agreements
        /// </summary>
        /// <param name="cbz">Bizz</param>
        /// <param name="entrepeneurs">List<IndexedSubEntrepeneur></param>
        /// <returns>string</returns>
        public string GenerateSubEntrepeneursPdfForAgreement(Bizz cbz, List<IndexedSubEntrepeneur> entrepeneurs)
        {
            this.CBZ = cbz;
            date = DateTime.Today.ToString(@"yyyy-MM-dd");
            fileName = "Projekt_" + CBZ.TempProject.Case.ToString() + "_Underentrenoerer_" + date + ".pdf";
            pdfPath = @"PDF_Documents\" + fileName;

            //Create document
            Document document = new Document(PageSize.A4.Rotate(), 48, 48, 48, 48);

            //Create PDF Table
            PdfPTable tableLayout = new PdfPTable(7);

            //Create a PDF file in specific path
            PdfWriter.GetInstance(document, new FileStream(pdfPath, FileMode.Create));

            //Open the PDF document
            document.Open();

            //Add Content to PDF
            document.Add(AddContentToSubEntrepeneursPdfTableForAgreement(tableLayout, entrepeneurs));

            // Closing the document
            document.Close();

            return pdfPath;

        }

        /// <summary>
        /// Method, that retrieves Contact Info for Request Pdf
        /// </summary>
        /// <returns>string</returns>
        private string GetContactInfo()
        {
            return CBZ.TempShipping.SubEntrepeneur.Enterprise.Project.Executive.Person.ContactInfo.Phone + @"/" + CBZ.TempShipping.SubEntrepeneur.Enterprise.Project.Executive.Person.ContactInfo.Email;
        }

        /// <summary>
        /// Method, that retrieves an enterprise line
        /// </summary>
        /// <returns>string</returns>
        private string GetEnterpriseLine()
        {
            RefreshProjectEnterprises();

            string result = ProjectEnterprises[0].Name;

            if (ProjectEnterprises.Count > 1)
            {
                int iMAx = ProjectEnterprises.Count - 1;

                for (int i = 1; i < iMAx; i++)
                {
                    result += @", " + ProjectEnterprises[i].Name;
                }

                result += @" & " + ProjectEnterprises[iMAx].Name;
            }
            return result;
        }

        /// <summary>
        /// Method, that returns an Offer Status from a SubEntrepeneur
        /// </summary>
        /// <param name="subEntrepeneur">string</param>
        /// <returns></returns>
        private string GetOfferStatus(SubEntrepeneur subEntrepeneur)
        {
            string result = "Ukendt";
            foreach (Offer offer in CBZ.Offers)
            {
                if (subEntrepeneur.Offer.Id == offer.Id && !offer.Received && !offer.Chosen)
                {
                    foreach (IttLetter letter in CBZ.IttLetters)
                    {
                        result = "Nej";
                        if (letter.Id == subEntrepeneur.IttLetter.Id && letter.Sent)
                        {
                            result = "Sendt";
                            break;
                        }
                    }
                }
                if (subEntrepeneur.Offer.Id == offer.Id && offer.Received && !offer.Chosen)
                {
                    result = "Modtaget";
                }
                if (subEntrepeneur.Offer.Id == offer.Id && offer.Chosen)
                {
                    result = "Valgt";
                }
            }
            return result;
        }

        /// <summary>
        /// Method, that refreshes Indexed SubEntrepeneurs list
        /// </summary>
        private void RefreshIndexedSubEntrepeneurs()
        {
            CBZ.IndexedSubEntrepeneurs.Clear();

            CBZ.IndexedSubEntrepeneurs.Add(new IndexedSubEntrepeneur(0, CBZ.SubEntrepeneurs[0]));

            int i = 1;

            foreach (Enterprise enterprise in ProjectEnterprises)
            {
                foreach (SubEntrepeneur subEntrepeneur in CBZ.SubEntrepeneurs)
                {
                    if (subEntrepeneur.Enterprise.Id == enterprise.Id)
                    {
                        CBZ.IndexedSubEntrepeneurs.Add(new IndexedSubEntrepeneur(i, subEntrepeneur));
                    }
                }
            }
        }

        /// <summary>
        /// Method, that refreshes Project Paragrafs list
        /// </summary>
        private void RefreshProjectEnterprises()
        {
            ProjectEnterprises.Clear();

            foreach (Enterprise enterprise in CBZ.Enterprises)
            {
                if (enterprise.Project.Id == CBZ.TempProject.Id)
                {
                    ProjectEnterprises.Add(enterprise);
                }
            }
        }

        /// <summary>
        /// Method, that refreshes Project Paragrafs list
        /// </summary>
        private void RefreshProjectParagrafs()
        {
            ProjectParagrafs.Clear();
            ProjectBullets.Clear();

            foreach (Paragraf paragraf in CBZ.Paragrafs)
            {
                if (paragraf.Project.Id == CBZ.TempProject.Id)
                {
                    foreach (Bullet bullet in CBZ.Bullets)
                    {
                        if (bullet.Paragraf.Id == paragraf.Id)
                        {
                            ProjectBullets.Add(bullet);
                        }
                    }
                    ProjectParagrafs.Add(paragraf);
                }
            }
        }

        /// <summary>
        /// Method, that refreshes Project Receivers list
        /// </summary>
        private void RefreshProjectReceivers()
        {
            ProjectReceivers.Clear();

            foreach (Shipping shipping in ProjectShippings)
            {
                ProjectReceivers.Add(shipping.Receiver);
            }
        }

        /// <summary>
        /// Method, that refreshes Project Shippings list
        /// </summary>
        private void RefreshProjectShippings()
        {
            CBZ.RefreshList("Shippings");
            ProjectShippings.Clear();

            foreach (Shipping shipping in CBZ.Shippings)
            {
                if (shipping.SubEntrepeneur.Enterprise.Project.Id == CBZ.TempProject.Id)
                {
                    ProjectShippings.Add(shipping);
                }
            }
        }

        #endregion
    }

}
