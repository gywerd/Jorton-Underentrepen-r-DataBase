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
        Bizz CBZ = new Bizz();
        string date = "";
        string fileName = string.Empty;
        string pdfPath = "";
        string strConnection = "";
        Shipping shipping;
        FileStream fileStreamCreate;
        List<Shipping> shippings;
        List<Enterprise> ProjectEnterprises = new List<Enterprise>();
        List<Receiver> ProjectReceivers = new List<Receiver>();

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
        /// <param name="project">Project</param>
        /// <param name="list">List<IndexedEnterprise></param>
        /// <param name="users">List<User></param>
        /// <returns>PdfPTable</returns>
        private PdfPTable AddContentToEnterprisesPDF(PdfPTable tableLayout, Project project, List<Enterprise> list, List<User> users)
        {
            float[] headers = { 30, 35, 35 };  //Header Widths
            tableLayout.SetWidths(headers);        //Set the pdf headers
            tableLayout.WidthPercentage = 100;       //Set the PDF File witdh percentage

            DateTime today = DateTime.Today;
            string date = @"Udskrift pr.: " + today.ToShortDateString();
            string executive = @"Tilbudsansvarlig: " + project.Executive.Person.Name;

            //Add Title to the PDF file at the top
            tableLayout.AddCell(new PdfPCell(new Phrase("Jorton A/S", new Font(Font.FontFamily.HELVETICA, 24, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("Entrepriseliste", new Font(Font.FontFamily.HELVETICA, 24, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(date, new Font(Font.FontFamily.HELVETICA, 12, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("Sagsnr: " + project.Case, new Font(Font.FontFamily.HELVETICA, 12, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 20, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
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
        /// <returns>PdfPTable</returns>
        private PdfPTable AddContentToIttLetterCommonPDF(PdfPTable tableLayout, Project project)
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
            Chunk c = new Chunk(shipping.LetterData.ConditionUrl, new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(0, 0, 0)));
            c.SetAnchor(shipping.LetterData.ConditionUrl);
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
        private PdfPTable AddContentToIttLetterCompanyPDF(PdfPTable tableLayout, Shipping shipping)
        {
            float[] headers = { 30, 35, 35 };  //Header Widths
            tableLayout.SetWidths(headers);        //Set the pdf headers
            tableLayout.WidthPercentage = 100;       //Set the PDF File witdh percentage

            DateTime today = DateTime.Today;
            string date = @"Aabenraa den " + today.ToString(@"dd-MM-yyyy");
            string executivePhone = @"Tlf.: ";
            if (executivePhone != "")
            {
                executivePhone += shipping.Project.Executive.Person.ContactInfo.Phone;
            }
            else
            {
                executivePhone += shipping.Project.Executive.Person.ContactInfo.Mobile;
            }
            string executiveMail = @"Mail: " + shipping.Project.Executive.Person.ContactInfo.Email;
            string receiverContactName = @"Att.: " + shipping.SubEntrepeneur.Contact.Person.Name;

            //Add address and colophon
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 48, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(shipping.SubEntrepeneur.Entrepeneur.Entity.Name, new Font(Font.FontFamily.HELVETICA, 12, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(shipping.Project.Executive.Initials, new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_BOTTOM });
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
            Chunk urlChunk = new Chunk(this.shipping.LetterData.MaterialUrl, new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(0, 0, 0)));
            urlChunk.SetAnchor(this.shipping.LetterData.MaterialUrl);

            tableLayout.AddCell(new PdfPCell(new Phrase("UDBUDSBREV VEDR. " + shipping.Project.Name + "\n", new Font(Font.FontFamily.HELVETICA, 12, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("I anmodes hermed om at give tilbud på følgende entreprise:\n", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(enterpriseLine + "\n", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("Tilbud bedes fremsendt til undertegnede senest den " + this.shipping.LetterData.AnswerDate + ", kl. 12.00\n", new Font(Font.FontFamily.HELVETICA, 12, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("Spørgsmål til udbuddet fremsendes senest den " + this.shipping.LetterData.QuestionDate + ", kl. 12.00", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("Sidste rettelsesblad udsendes " + this.shipping.LetterData.CorrectionDate + ", kl. 12.00 - fra rådgiveren\n", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("Bygherrens tilbudsliste ønskes modtaget udfyldt i Excel-format og gerne suppleret med eksemplar i PDF format.\n", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("JORTON forventer, at vores samarbejdspartnere overholder gældende overenskomster og regler for anvendelse af udenlandsk arbejdskraft.", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("JORTON afgiver hovedentreprisetilbud til " + this.shipping.LetterData.Builder + ". Udførelsesperioden er " + this.shipping.LetterData.TimeSpan + " jf. bygherrens udbudstidsplan.\n", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("Såfremt I efter gennemgang af udbudsmaterialet ikke er interesseret i at afgive tilbud, eller der er mangler ved eller opstår spørgsmål til udbudsmaterialet, er I naturligvis velkommen til at kontakte undertegnede.\n", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("Udbudsmaterialet kan ses og hentes på denne adresse:\n", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(urlChunk)) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("\n", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("Adgangskode: " + this.shipping.LetterData.PassWord + "\n", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("Vi ser frem til at modtage jeres tilbud.\n", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("Med venlig hilsen\n", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(shipping.Project.Executive.Person.Name + "\n", new Font(Font.FontFamily.HELVETICA, 12, 2, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(shipping.Project.Executive.JobDescription.Occupation + "\n", new Font(Font.FontFamily.HELVETICA, 12, 2, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("Bilag – oplistning af udbudsdokumenter", new Font(Font.FontFamily.HELVETICA, 12, 0, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_BOTTOM });

            return tableLayout;
        }

        /// <summary>
        /// Method, that adds content to the EnterprisList PDF
        /// </summary>
        /// <param name="tableLayout">PdfPTable</param>
        /// <param name="project">Project</param>
        /// <param name="enterprise">List<IndexedEnterprise></param>
        /// <param name="users">List<User></param>
        /// <returns>PdfPTable</returns>
        private PdfPTable AddContentToSubEntrepeneursPDF(PdfPTable tableLayout, Project project, List<Enterprise> enterprise, List<IndexedSubEntrepeneur> entrepeneurList, List<User> users)
        {
            float[] headers = { 20, 20, 10, 10, 13, 7, 6, 6, 8 };  //Header Widths
            tableLayout.SetWidths(headers);        //Set the pdf headers
            tableLayout.WidthPercentage = 100;       //Set the PDF File witdh percentage

            DateTime today = DateTime.Today;
            string date = @"Udskrift pr.: " + today.ToShortDateString();
            string executive = @"Tilbudsansvarlig: " + project.Executive.Person.Name;

            //Add Title to the PDF file at the top
            tableLayout.AddCell(new PdfPCell(new Phrase("Jorton A/S", new Font(Font.FontFamily.HELVETICA, 18, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 18, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("Fagentrepenører", new Font(Font.FontFamily.HELVETICA, 18, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 2, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 18, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 2, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(date, new Font(Font.FontFamily.HELVETICA, 12, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("Sagsnr: " + project.Case, new Font(Font.FontFamily.HELVETICA, 12, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 20, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM });
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
            foreach (Enterprise temp in enterprise)
            {
                if (temp.Project.Id == project.Id)
                {
                    tableLayout.AddCell(new PdfPCell(new Phrase(temp.Name, new Font(Font.FontFamily.HELVETICA, 10, 1, iTextSharp.text.BaseColor.BLACK))) { Colspan = 9, HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = iTextSharp.text.BaseColor.WHITE });
                    foreach (IndexedSubEntrepeneur tempSub in entrepeneurList)
                    {
                        if (tempSub.Enterprise.Id == temp.Id)
                        {
                            Contact contact = tempSub.Contact;
                            ContactInfo info = contact.Person.ContactInfo;
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
                            AddCellToBodyLeft(tableLayout, tempSub.Entrepeneur.Entity.Name);
                            AddCellToBodyLeft(tableLayout, contact.Person.Name);
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

        private PdfPTable AddContentToSubEntrepeneursPDFForAgreement(PdfPTable tableLayout, Project project, List<Enterprise> enterprise, List<IndexedSubEntrepeneur> entrepeneurList, List<User> users)
        {
            float[] headers = { 20, 20, 20, 20, 6, 6, 8 };  //Header Widths
            tableLayout.SetWidths(headers);        //Set the pdf headers
            tableLayout.WidthPercentage = 100;       //Set the PDF File witdh percentage

            DateTime today = DateTime.Today;
            string date = @"Udskrift pr.: " + today.ToShortDateString();
            string executive = @"Tilbudsansvarlig: " + project.Executive.Person.Name;

            //Add Title to the PDF file at the top
            tableLayout.AddCell(new PdfPCell(new Phrase("Jorton A/S", new Font(Font.FontFamily.HELVETICA, 18, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 18, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("Fagentrepenører", new Font(Font.FontFamily.HELVETICA, 18, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 18, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase(date, new Font(Font.FontFamily.HELVETICA, 12, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 3, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM });
            tableLayout.AddCell(new PdfPCell(new Phrase("Sagsnr: " + project.Case, new Font(Font.FontFamily.HELVETICA, 12, 1, new iTextSharp.text.BaseColor(24, 80, 116)))) { Colspan = 1, Border = 0, PaddingBottom = 20, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM });
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
            foreach (Enterprise temp in enterprise)
            {
                if (temp.Project.Id == project.Id)
                {
                    tableLayout.AddCell(new PdfPCell(new Phrase(temp.Name, new Font(Font.FontFamily.HELVETICA, 10, 1, iTextSharp.text.BaseColor.BLACK))) { Colspan = 9, HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = iTextSharp.text.BaseColor.WHITE });
                    foreach (IndexedSubEntrepeneur tempSub in entrepeneurList)
                    {
                        if (tempSub.Enterprise.Id == temp.Id)
                        {
                            Contact contact = tempSub.Contact;
                            ContactInfo info = contact.Person.ContactInfo;
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
                            AddCellToBodyLeft(tableLayout, tempSub.Entrepeneur.Entity.Name);
                            AddCellToBodyLeft(tableLayout, contact.Person.Name);
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

        public string GenerateEnterprisesPdf(Bizz cbz, List<Enterprise> list, List<User> users)
        {
            this.CBZ = cbz;
            date = DateTime.Today.ToString(@"yyyy-MM-dd");
            pdfPath = @"PDF_Documents\Projekt_" + CBZ.TempProject.Case.ToString() + "_Entrepriseliste_" + date + ".pdf";
            
            //Create document
            Document document = new Document(PageSize.A4.Rotate(), 48, 48, 48, 48);

            //Create PDF Table
            PdfPTable tableLayout = new PdfPTable(3);

            //Create a PDF file in specific path
            PdfWriter.GetInstance(document, new FileStream(pdfPath, FileMode.Create));

            //Open the PDF document
            document.Open();

            //Add Content to PDF
            document.Add(AddContentToEnterprisesPDF(tableLayout, CBZ.TempProject, list, users));

            // Closing the document
            document.Close();

            return pdfPath;

        }

        public string GenerateIttLetterCommonPdf(Bizz cbz, Project project, List<Shipping> shippings)
        {
            this.CBZ = cbz;
            this.shippings = shippings;
            date = DateTime.Today.ToString(@"yyyy-MM-dd");
            pdfPath = @"PDF_Documents\Projekt_" + project.Name.Replace(" ", "_") + "_Foelgebrev_faelles_" + date + ".pdf";
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
                document.Add(AddContentToIttLetterCommonPDF(tableLayout, project));

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

        public string GenerateIttLetterCompanyPdf(Bizz cbz, Shipping shipping)
        {
            this.CBZ = cbz;
            this.shipping = shipping;
            date = DateTime.Today.ToString(@"yyyy-MM-dd");
            pdfPath = @"PDF_Documents\Projekt_" + shipping.Project.Name.Replace(" ", "_") + "_Foelgebrev_til_" + shipping.SubEntrepeneur.Entrepeneur.Entity.Name.Replace(" ", "_") + "_" + date + ".pdf";
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
                document.Add(AddContentToIttLetterCompanyPDF(tableLayout, shipping));

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

        public string GenerateSubEntrepeneursPdf(Bizz cbz, List<Enterprise> enterprise, List<IndexedSubEntrepeneur> entrepeneurList, List<User> users)
        {
            this.CBZ = bizz;
            date = DateTime.Today.ToString(@"yyyy-MM-dd");
            pdfPath = @"PDF_Documents\Projekt" + CBZ.TempProject.Case.ToString() + "_Underentrenoerer_" + date + ".pdf";

            //Create document
            Document document = new Document(PageSize.A4.Rotate(), 48, 48, 48, 48);

            //Create PDF Table
            PdfPTable tableLayout = new PdfPTable(9);

            //Create a PDF file in specific path
            PdfWriter.GetInstance(document, new FileStream(pdfPath, FileMode.Create));

            //Open the PDF document
            document.Open();

            //Add Content to PDF
            document.Add(AddContentToSubEntrepeneursPDF(tableLayout, CBZ.TempProject, enterprise, entrepeneurList, users));

            // Closing the document
            document.Close();

            return pdfPath;

        }

        public string GenerateSubEntrepeneursPdfForAgreement(Bizz cbz, List<Enterprise> enterprise, List<IndexedSubEntrepeneur> entrepeneurList, List<User> users)
        {
            this.CBZ = bizz;
            date = DateTime.Today.ToString(@"yyyy-MM-dd");
            pdfPath = @"PDF_Documents\Projekt_" + CBZ.TempProject.Case.ToString() + "_Underentrenoerer_" + date + ".pdf";

            //Create document
            Document document = new Document(PageSize.A4.Rotate(), 48, 48, 48, 48);

            //Create PDF Table
            PdfPTable tableLayout = new PdfPTable(7);

            //Create a PDF file in specific path
            PdfWriter.GetInstance(document, new FileStream(pdfPath, FileMode.Create));

            //Open the PDF document
            document.Open();

            //Add Content to PDF
            document.Add(AddContentToSubEntrepeneursPDFForAgreement(tableLayout, CBZ.TempProject, enterprise, entrepeneurList, users));

            // Closing the document
            document.Close();

            return pdfPath;

        }

        private string GetBluePrintsLine()
        {
            throw new NotImplementedException();
        }

        private Contact GetContact(int id)
        {
            Contact result = new Contact();
            foreach (Contact contact in CBZ.Contacts)
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
            foreach (ContactInfo info in CBZ.ContactInfoList)
            {
                if (info.Id == id)
                {
                    result = info;
                    break;
                }
            }
            return result;
        }

        private string GetCompleteSetDescriptionLine()
        {
            throw new NotImplementedException();
        }

        private string GetEnterpriseLine()
        {
            RefreshProjectEnterprises(shipping.Project);

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

        private User GetExecutive(int id)
        {
            User result = new User();
            foreach (User user in CBZ.Users)
            {
                if (user.Id == id)
                {
                    result = user;
                    break;
                }
            }
            return result;
        }

        private string GetIttLetterSentDate(int id)
        {
            string result = "";
            foreach (IttLetter letter in CBZ.IttLetters)
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
            throw new NotImplementedException();
        }

        private string GetOfferStatus(SubEntrepeneur sub)
        {
            string result = "Ukendt";
            foreach (Offer offer in CBZ.Offers)
            {
                if (sub.Offer.Id == offer.Id && !offer.Received && !offer.Chosen)
                {
                    foreach (IttLetter letter in CBZ.IttLetters)
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
            RefreshProjectReceivers(shippings);

            string result = "• " + ProjectReceivers[0] + "\n";
            if (ProjectReceivers.Count > 1)
            {
                int iMax = ProjectReceivers.Count - 1;
                for (int i = 1; i < iMax; i++)
                {
                    result += "• " + ProjectReceivers[i] + "\n";
                }
                result += "• " + ProjectReceivers[iMax] + "\n";
            }
            return result;
        }

        private string GetTimeSchedulesLine()
        {
            throw new NotImplementedException();
        }

        private void RefreshProjectEnterprises(Project project)
        {
            ProjectEnterprises.Clear();

            foreach (Enterprise enterprise in CBZ.Enterprises)
            {
                if (enterprise.Project.Id == project.Id)
                {
                    ProjectEnterprises.Add(enterprise);
                }
            }
        }

        /// <summary>
        /// Method, that refreshes Project Receivers
        /// </summary>
        /// <param name="shipping">Project</param>
        private void RefreshProjectReceivers(List<Shipping> shippings)
        {
            ProjectReceivers.Clear();

            foreach (Shipping shipping in shippings)
            {
                ProjectReceivers.Add(shipping.Receiver);
            }
        }

        #endregion
    }

}
