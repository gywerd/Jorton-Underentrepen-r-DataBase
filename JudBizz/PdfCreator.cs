using iTextSharp.text;
using iTextSharp.text.pdf;
using itextsharp.pdfa;
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
        Bizz Bizz = new Bizz();
        string date = "";
        string fileName = string.Empty;
        string pdfPath = "";
        string strConnection = "";
        FileStream fileStreamCreate;
        IttLetterPdfData letterData;

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
        // Method to add single cell to the header
        private static void AddCellToHeader(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 10, 1, iTextSharp.text.BaseColor.WHITE))) { HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = new iTextSharp.text.BaseColor(0, 104, 64) });
        }

        // Method to add single cell to the body - aligned left
        private static void AddCellToBodyLeft(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 10, 0, iTextSharp.text.BaseColor.BLACK))) { HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = iTextSharp.text.BaseColor.WHITE });
        }

        // Method to add single cell to the body - aligned right
        private static void AddCellToBodyRight(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 10, 0, iTextSharp.text.BaseColor.BLACK))) { HorizontalAlignment = Element.ALIGN_RIGHT, Padding = 5, BackgroundColor = iTextSharp.text.BaseColor.WHITE });
        }

        /// <summary>
        /// Method, that adds content to the EnterprisList PDF
        /// </summary>
        /// <param name="tableLayout">PdfPTable</param>
        /// <param name="project">Project</param>
        /// <param name="list">List<IndexableEnterprise></param>
        /// <param name="users">List<User></param>
        /// <returns>PdfPTable</returns>
        private PdfPTable AddContentToEnterpriseListPDF(PdfPTable tableLayout, Project project, List<Enterprise> list, List<User> users)
        {
            float[] headers = { 30, 35, 35 };  //Header Widths
            tableLayout.SetWidths(headers);        //Set the pdf headers
            tableLayout.WidthPercentage = 100;       //Set the PDF File witdh percentage

            DateTime today = DateTime.Today;
            string date = @"Udskrift pr.: " + today.ToShortDateString();
            string executive = @"Tilbudsansvarlig: " + project.Executive.Name;

            //Add Title to the PDF file at the top
            tableLayout.AddCell(new PdfPCell(new Phrase("Jorton A/S", new Font(Font.FontFamily.HELVETICA, 24, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("Entrepriseliste", new Font(Font.FontFamily.HELVETICA, 24, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(date, new Font(Font.FontFamily.HELVETICA, 12, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("Sagsnr: " + project.CaseId, new Font(Font.FontFamily.HELVETICA, 12, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 20, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(project.Name, new Font(Font.FontFamily.HELVETICA, 12, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 20, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(executive, new Font(Font.FontFamily.HELVETICA, 12, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 20, HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_BOTTOM });

            //Add header
            AddCellToHeader(tableLayout, "Entreprise");
            AddCellToHeader(tableLayout, "Beskrivelse");
            AddCellToHeader(tableLayout, "Tilbudsliste");

            //Add body
            foreach (Enterprise temp in list)
            {
                if (temp.Project.Id == project.Id)
                {
                    AddCellToBodyLeft(tableLayout, temp.Name);
                    AddCellToBodyLeft(tableLayout, temp.Elaboration);
                    AddCellToBodyLeft(tableLayout, temp.OfferList);
                }
            }

            return tableLayout;

        }

        /// <summary>
        /// Method, that adds content to the IttLetter PDF
        /// </summary>
        /// <param name="tableLayout">PdfPTable</param>
        /// <param name="project">Project</param>
        /// <param name="entity">LegalEntity</param>
        /// <param name="entrepeneur">SubEntrepeneur</param>
        /// <returns>PdfPTable</returns>
        private PdfPTable AddContentToIttLetterCommonPDF(PdfPTable tableLayout, Project project, SubEntrepeneur entrepeneur)
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
            string set = GetCompleteSetDescriptionLine();
            string docs = GetProjectDocumentsLine();
            string bluePrints = GetBluePrintsLine();
            string schedules = GetTimeSchedulesLine();
            string miscellaneus = GetMiscellaneusLine();
            Chunk c = new Chunk(letterData.ConditionUrl, new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(0, 0, 0)));
            c.SetAnchor(letterData.ConditionUrl);
            Chunk urlChunk = new Chunk("Klik for at se", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(0, 0, 0)));
            urlChunk.Chunks.Add(c);

            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("• Komplet sæt beskrivelse i henhold til vedlagte dokumenter:", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 2, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 2, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(set, new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("• Projektdokumenter:", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 2, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 2, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(docs, new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("• Tegninger i henhold til Tegningsliste:", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 2, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 2, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(bluePrints, new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("• Tidsplaner:", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 2, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 2, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(schedules, new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(miscellaneus, new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 2, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(urlChunk)) { Colspan = 2, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });

            return tableLayout;
        }

        /// <summary>
        /// Method, that adds content to the company IttLetter PDF
        /// </summary>
        /// <param name="tableLayout">PdfPTable</param>
        /// <param name="project">Project</param>
        /// <param name="entity">LegalEntity</param>
        /// <param name="entrepeneur">SubEntrepeneur</param>
        /// <returns>PdfPTable</returns>
        private PdfPTable AddContentToIttLetterCompanyPDF(PdfPTable tableLayout, Project project, LegalEntity entity, SubEntrepeneur entrepeneur)
        {
            float[] headers = { 30, 35, 35 };  //Header Widths
            tableLayout.SetWidths(headers);        //Set the pdf headers
            tableLayout.WidthPercentage = 100;       //Set the PDF File witdh percentage

            DateTime today = DateTime.Today;
            string date = @"Aabenraa den " + today.ToString(@"dd-MM-yyyy");
            User executive = project.Executive;
            ContactInfo executiveInfo = executive.ContactInfo;
            string executivePhone = @"Tlf.: ";
            if (executivePhone != "")
            {
                executivePhone += executiveInfo.Phone;
            }
            else
            {
                executivePhone += executiveInfo.Mobile;
            }
            string executiveMail = @"Mail: " + executiveInfo.Email;
            Contact receiver = entrepeneur.Contact;
            ContactInfo receiverInfo = receiver.ContactInfo;
            string receiverContactName = @"Att.: " + receiver.Name;

            //Add address and colophon
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 48, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(entity.Name, new Font(Font.FontFamily.HELVETICA, 12, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(executive.Initials, new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_BOTTOM });
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
            string executiveTitle = GetExecutiveTitle(executive);
            Chunk urlChunk = new Chunk(letterData.MaterialUrl, new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(0, 0, 0)));
            urlChunk.SetAnchor(letterData.MaterialUrl);

            tableLayout.AddCell(new PdfPCell(new Phrase("UDBUDSBREV VEDR. " + project.Name + "\n", new Font(Font.FontFamily.HELVETICA, 12, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("I anmodes hermed om at give tilbud på følgende entreprise:\n", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(enterpriseLine + "\n", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("Tilbud bedes fremsendt til undertegnede senest den " + letterData.AnswerDate + ", kl. 12.00\n", new Font(Font.FontFamily.HELVETICA, 12, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("Spørgsmål til udbuddet fremsendes senest den " + letterData.QuestionDate + ", kl. 12.00", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("Sidste rettelsesblad udsendes " + letterData.CorrectionSheetDate + ", kl. 12.00 - fra rådgiveren\n", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("Bygherrens tilbudsliste ønskes modtaget udfyldt i Excel-format og gerne suppleret med eksemplar i PDF format.\n", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("JORTON forventer, at vores samarbejdspartnere overholder gældende overenskomster og regler for anvendelse af udenlandsk arbejdskraft.", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("JORTON afgiver hovedentreprisetilbud til " + letterData.Builder + ". Udførelsesperioden er " + letterData.TimeSpan + " jf. bygherrens udbudstidsplan.\n", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("Såfremt I efter gennemgang af udbudsmaterialet ikke er interesseret i at afgive tilbud, eller der er mangler ved eller opstår spørgsmål til udbudsmaterialet, er I naturligvis velkommen til at kontakte undertegnede.\n", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("Udbudsmaterialet kan ses og hentes på denne adresse:\n", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(urlChunk)) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("\n", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("Adgangskode: " + letterData.PassWord + "\n", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("Vi ser frem til at modtage jeres tilbud.\n", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("Med venlig hilsen\n", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(executive.Name + "\n", new Font(Font.FontFamily.HELVETICA, 12, 2, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(executiveTitle + "\n", new Font(Font.FontFamily.HELVETICA, 12, 2, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("Bilag – oplistning af udbudsdokumenter", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });

            return tableLayout;
        }

        /// <summary>
        /// Method, that adds content to the EnterprisList PDF
        /// </summary>
        /// <param name="tableLayout">PdfPTable</param>
        /// <param name="project">Project</param>
        /// <param name="enterpriseList">List<IndexableEnterprise></param>
        /// <param name="users">List<User></param>
        /// <returns>PdfPTable</returns>
        private PdfPTable AddContentToSubEntrepeneursPDF(PdfPTable tableLayout, Project project, List<Enterprise> enterpriseList, List<IndexedSubEntrepeneur> entrepeneurList, List<User> users)
        {
            float[] headers = { 20, 20, 10, 10, 13, 7, 6, 6, 8 };  //Header Widths
            tableLayout.SetWidths(headers);        //Set the pdf headers
            tableLayout.WidthPercentage = 100;       //Set the PDF File witdh percentage

            DateTime today = DateTime.Today;
            string date = @"Udskrift pr.: " + today.ToShortDateString();
            string executive = @"Tilbudsansvarlig: " + project.Executive.Name;

            //Add Title to the PDF file at the top
            tableLayout.AddCell(new PdfPCell(new Phrase("Jorton A/S", new Font(Font.FontFamily.HELVETICA, 18, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 18, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("Fagentrepenører", new Font(Font.FontFamily.HELVETICA, 18, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 2, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 18, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 2, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(date, new Font(Font.FontFamily.HELVETICA, 12, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("Sagsnr: " + project.CaseId, new Font(Font.FontFamily.HELVETICA, 12, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 20, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 18, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(project.Name, new Font(Font.FontFamily.HELVETICA, 12, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 2, Border = 0, PaddingBottom = 20, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM });
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
            foreach (Enterprise temp in enterpriseList)
            {
                if (temp.Project.Id == project.Id)
                {
                    tableLayout.AddCell(new PdfPCell(new Phrase(temp.Name, new Font(Font.FontFamily.HELVETICA, 10, 1, iTextSharp.text.BaseColor.BLACK))) { Colspan = 9, HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = iTextSharp.text.BaseColor.WHITE });
                    foreach (IndexedSubEntrepeneur tempSub in entrepeneurList)
                    {
                        if (tempSub.EnterpriseList.Id == temp.Id)
                        {
                            Contact contact = tempSub.Contact;
                            ContactInfo info = contact.ContactInfo;
                            string uphold = "Nej";
                            if (tempSub.Uphold)
                            {
                                uphold = "Ja";
                            }
                            string reservations = "Nej";
                            if (tempSub.Uphold)
                            {
                                reservations = "Ja";
                            }
                            AddCellToBodyLeft(tableLayout, tempSub.Entrepeneur.Name);
                            AddCellToBodyLeft(tableLayout, contact.Name);
                            AddCellToBodyRight(tableLayout, info.Phone);
                            AddCellToBodyRight(tableLayout, info.Mobile);
                            AddCellToBodyLeft(tableLayout, info.Email);
                            AddCellToBodyLeft(tableLayout, GetOfferStatus(tempSub));
                            AddCellToBodyLeft(tableLayout, uphold);
                            AddCellToBodyLeft(tableLayout, reservations);
                            AddCellToBodyRight(tableLayout, tempSub.Offer.Price.ToString());
                        }
                    }
                }
            }

            return tableLayout;
        }

        private PdfPTable AddContentToSubEntrepeneursPDFForAgreement(PdfPTable tableLayout, Project project, List<Enterprise> enterpriseList, List<IndexedSubEntrepeneur> entrepeneurList, List<User> users)
        {
            float[] headers = { 20, 20, 20, 20, 6, 6, 8 };  //Header Widths
            tableLayout.SetWidths(headers);        //Set the pdf headers
            tableLayout.WidthPercentage = 100;       //Set the PDF File witdh percentage

            DateTime today = DateTime.Today;
            string date = @"Udskrift pr.: " + today.ToShortDateString();
            string executive = @"Tilbudsansvarlig: " + project.Executive.Name;

            //Add Title to the PDF file at the top
            tableLayout.AddCell(new PdfPCell(new Phrase("Jorton A/S", new Font(Font.FontFamily.HELVETICA, 18, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 18, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("Fagentrepenører", new Font(Font.FontFamily.HELVETICA, 18, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 18, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(date, new Font(Font.FontFamily.HELVETICA, 12, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("Sagsnr: " + project.CaseId, new Font(Font.FontFamily.HELVETICA, 12, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 20, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 18, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(project.Name, new Font(Font.FontFamily.HELVETICA, 12, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 20, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM });
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

            //Add body
            foreach (Enterprise temp in enterpriseList)
            {
                if (temp.Project.Id == project.Id)
                {
                    tableLayout.AddCell(new PdfPCell(new Phrase(temp.Name, new Font(Font.FontFamily.HELVETICA, 10, 1, iTextSharp.text.BaseColor.BLACK))) { Colspan = 9, HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = iTextSharp.text.BaseColor.WHITE });
                    foreach (IndexedSubEntrepeneur tempSub in entrepeneurList)
                    {
                        if (tempSub.EnterpriseList.Id == temp.Id)
                        {
                            Contact contact = tempSub.Contact;
                            ContactInfo info = contact.ContactInfo;
                            String ittletterSentDate = "Sendt: " + tempSub.IttLetter.SentDate.ToShortDateString();
                            string uphold = "Nej";
                            if (tempSub.Uphold)
                            {
                                uphold = "Ja";
                            }
                            string reservations = "Nej";
                            if (tempSub.Uphold)
                            {
                                reservations = "Ja";
                            }
                            AddCellToBodyLeft(tableLayout, tempSub.Entrepeneur.Name);
                            AddCellToBodyLeft(tableLayout, contact.Name);
                            AddCellToBodyRight(tableLayout, info.ToLongString());
                            AddCellToBodyLeft(tableLayout, ittletterSentDate);
                            AddCellToBodyLeft(tableLayout, uphold);
                            AddCellToBodyLeft(tableLayout, reservations);
                            AddCellToBodyRight(tableLayout, tempSub.Offer.Price.ToString());
                        }
                    }
                }
            }

            return tableLayout;
        }

        public string GenerateEnterpriseListPdf(Bizz bizz, List<Enterprise> list, List<User> users)
        {
            date = DateTime.Today.ToString(@"yyyy-MM-dd");
            pdfPath = @"PDF_Documents\Projekt_" + Bizz.TempProject.CaseId.ToString() + "_Entrepriseliste_" + date + ".pdf";
            
            //Create document
            Document document = new Document(PageSize.A4.Rotate(), 48, 48, 48, 48);

            //Create PDF Table
            PdfPTable tableLayout = new PdfPTable(3);

            //Create a PDF file in specific path
            PdfWriter.GetInstance(document, new FileStream(pdfPath, FileMode.Create));

            //Open the PDF document
            document.Open();

            //Add Content to PDF
            document.Add(AddContentToEnterpriseListPDF(tableLayout, Bizz.TempProject, list, users));

            // Closing the document
            document.Close();

            return pdfPath;

        }

        public string GenerateIttLetterCommonPdf(Bizz bizz, Project project, SubEntrepeneur entrepeneur, IttLetterPdfData letterData)
        {
            this.Bizz = bizz;
            this.letterData = letterData;
            date = DateTime.Today.ToString(@"yyyy-MM-dd");
            pdfPath = @"PDF_Documents\Projekt_" + project.Name + "_Foelgebrev_faelles_" + date + ".pdf";
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
                document.Open();

                //Add Content to PDF
                document.Add(AddContentToIttLetterCommonPDF(tableLayout, project, entrepeneur));

            }
            catch (Exception ex)
            {
                MessageBox.Show(@"Der opstod en fejl ved indsætning af indhold dokument\n\n" + ex, "Indsæt indhold i dokument", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                // Closing the document
                document.Close();
            }

            return pdfPath;

        }

        public string GenerateIttLetterCompanyPdf(Bizz bizz, Project project, LegalEntity entity, SubEntrepeneur entrepeneur, IttLetterPdfData letterData)
        {
            this.Bizz = bizz;
            this.letterData = letterData;
            date = DateTime.Today.ToString(@"yyyy-MM-dd");
            pdfPath = @"PDF_Documents\Projekt_" + project.Name + "_Foelgebrev_til_" + entity.Name + "_" + date + ".pdf";
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
                document.Open();

                //Add Content to PDF
                document.Add(AddContentToIttLetterCompanyPDF(tableLayout, project, entity, entrepeneur));

            }
            catch (Exception ex)
            {
                MessageBox.Show(@"Der opstod en fejl ved indsætning af indhold dokument\n\n" + ex, "Indsæt indhold i dokument", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                // Closing the document
                document.Close();
            }

            return pdfPath;

        }

        public string GenerateSubEntrepeneursPdf(Bizz bizz, List<Enterprise> enterpriseList, List<IndexedSubEntrepeneur> entrepeneurList, List<User> users)
        {
            this.Bizz = bizz;
            date = DateTime.Today.ToString(@"yyyy-MM-dd");
            pdfPath = @"PDF_Documents\Projekt" + Bizz.TempProject.CaseId.ToString() + "_Underentrenoerer_" + date + ".pdf";

            //Create document
            Document document = new Document(PageSize.A4.Rotate(), 48, 48, 48, 48);

            //Create PDF Table
            PdfPTable tableLayout = new PdfPTable(9);

            //Create a PDF file in specific path
            PdfWriter.GetInstance(document, new FileStream(pdfPath, FileMode.Create));

            //Open the PDF document
            document.Open();

            //Add Content to PDF
            document.Add(AddContentToSubEntrepeneursPDF(tableLayout, Bizz.TempProject, enterpriseList, entrepeneurList, users));

            // Closing the document
            document.Close();

            return pdfPath;

        }

        public string GenerateSubEntrepeneursPdfForAgreement(Bizz bizz, List<Enterprise> enterpriseList, List<IndexedSubEntrepeneur> entrepeneurList, List<User> users)
        {
            this.Bizz = bizz;
            date = DateTime.Today.ToString(@"yyyy-MM-dd");
            pdfPath = @"PDF_Documents\Projekt_" + Bizz.TempProject.CaseId.ToString() + "_Underentrenoerer_" + date + ".pdf";

            //Create document
            Document document = new Document(PageSize.A4.Rotate(), 48, 48, 48, 48);

            //Create PDF Table
            PdfPTable tableLayout = new PdfPTable(7);

            //Create a PDF file in specific path
            PdfWriter.GetInstance(document, new FileStream(pdfPath, FileMode.Create));

            //Open the PDF document
            document.Open();

            //Add Content to PDF
            document.Add(AddContentToSubEntrepeneursPDFForAgreement(tableLayout, Bizz.TempProject, enterpriseList, entrepeneurList, users));

            // Closing the document
            document.Close();

            return pdfPath;

        }

        private string GetBluePrintsLine()
        {
            string result = "• " + letterData.BluePrints[0] + "\n";
            if (letterData.BluePrints.Count > 1)
            {
                int iMax = letterData.BluePrints.Count - 1;
                for (int i = 1; i < iMax; i++)
                {
                    result += "• " + letterData.BluePrints[i] + "\n";
                }
                result += "• " + letterData.BluePrints[iMax] + "\n";
            }
            return result;
        }

        private string GetCompleteSetDescriptionLine()
        {
            string result = "• " + letterData.DescriptionList[0] + "\n";
            if (letterData.DescriptionList.Count > 1)
            {
                int iMax = letterData.DescriptionList.Count - 1;
                for (int i = 1; i < iMax; i++)
                {
                    result += "• " + letterData.DescriptionList[i] + "\n";
                }
                result += "• " + letterData.DescriptionList[iMax] + "\n";
            }
            return result;
        }

        private Contact GetContact(int id)
        {
            Contact result = new Contact();
            foreach (Contact contact in Bizz.Contacts)
            {
                if (contact.Id == id)
                {
                    result = contact;
                    break;
                }
            }
            return result;
        }

        private ContactInfo GetContactInfo(int id)
        {
            ContactInfo result = new ContactInfo();
            foreach (ContactInfo info in Bizz.ContactInfoList)
            {
                if (info.Id == id)
                {
                    result = info;
                    break;
                }
            }
            return result;
        }

        private string GetEnterpriseLine()
        {
            string result = letterData.Enterprises[0].Name;
            if (letterData.Enterprises.Count > 1)
            {
                int iMAx = letterData.Enterprises.Count - 1;
                for (int i = 1; i < iMAx; i++)
                {
                    result += @", " + letterData.Enterprises[i].Name;
                }

                result += @" & " + letterData.Enterprises[iMAx].Name;
            }
            return result;
        }

        private User GetExecutive(int id)
        {
            User result = new User();
            foreach (User user in Bizz.Users)
            {
                if (user.Id == id)
                {
                    result = user;
                    break;
                }
            }
            return result;
        }

        private string GetExecutiveTitle(User executive)
        {
            string result = "";
            foreach (JobDescription description in Bizz.JobDescriptions)
            {
                if (description.Id == executive.JobDescription.Id)
                {
                    result = description.Occupation;
                    break;
                }
            }
            return result;
        }

        private string GetIttLetterSentDate(int id)
        {
            string result = "";
            foreach (IttLetter letter in Bizz.IttLetters)
            {
                if (letter.Id == id)
                {
                    result = letter.SentDate.ToLongDateString();
                    break;
                }
            }
            if (result == "31. december 1899")
            {
                result = "ukendt";
            }
            return result;
        }

        private string GetMiscellaneusLine()
        {
            string result = "• " + letterData.Miscellaneous[0] + "\n";
            if (letterData.Miscellaneous.Count > 1)
            {
                int iMax = letterData.Miscellaneous.Count - 1;
                for (int i = 1; i < iMax; i++)
                {
                    result += "• " + letterData.Miscellaneous[i] + "\n";
                }
                result += "• " + letterData.Miscellaneous[iMax] + "\n";
            }
            return result;
        }

        private string GetOfferPrice(int id)
        {
            string result = "";
            foreach (Offer offer in Bizz.Offers)
            {

                if (id == offer.Id)
                {
                    result = Convert.ToString(offer.Price);
                    break;
                }
            }
            return result;
        }

        private string GetOfferStatus(SubEntrepeneur sub)
        {
            string result = "Ukendt";
            foreach (Offer offer in Bizz.Offers)
            {
                if (sub.Offer.Id == offer.Id && !offer.Received && !offer.Chosen)
                {
                    foreach (IttLetter letter in Bizz.IttLetters)
                    {
                        result = "Nej";
                        if (letter.Id == sub.IttLetter.Id && letter.Sent)
                        {
                            result = "Sendt";
                            break;
                        }
                    }
                }
                if (sub.Offer.Id == offer.Id && offer.Received && !offer.Chosen)
                {
                    result = "Modtaget";
                }
                if (sub.Offer.Id == offer.Id && offer.Chosen)
                {
                    result = "Valgt";
                }
            }
            return result;
        }

        private string GetProjectDocumentsLine()
        {
            string result = "• " + letterData.IttLetterReceivers[0] + "\n";
            if (letterData.IttLetterReceivers.Count > 1)
            {
                int iMax = letterData.IttLetterReceivers.Count - 1;
                for (int i = 1; i < iMax; i++)
                {
                    result += "• " + letterData.IttLetterReceivers[i] + "\n";
                }
                result += "• " + letterData.IttLetterReceivers[iMax] + "\n";
            }
            return result;
        }

        private string GetTimeSchedulesLine()
        {
            string result = "• " + letterData.TimeSchedules[0] + "\n";
            if (letterData.TimeSchedules.Count > 1)
            {
                int iMax = letterData.TimeSchedules.Count - 1;
                for (int i = 1; i < iMax; i++)
                {
                    result += "• " + letterData.TimeSchedules[i] + "\n";
                }
                result += "• " + letterData.TimeSchedules[iMax] + "\n";
            }
            return result;
        }

        #endregion
    }
}
