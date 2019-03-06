using JudBizz;
using JudRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JudGui
{
    /// <summary>
    /// Interaction logic for UcIttLettersPrepareCommonLetter.xaml
    /// </summary>
    public partial class UcIttLettersPrepareCommonLetter : UserControl
    {
        #region Fields
        public Bizz CBZ;
        public UserControl UcMain;
        public PdfCreator PdfCreator;
        public List<Shipping> Shippings = new List<Shipping>();
        public List<Contact> ProjectContacts = new List<Contact>();
        public List<Enterprise> ProjectEnterprises = new List<Enterprise>();
        public List<IndexedEnterprise> IndexedEnterprises = new List<IndexedEnterprise>();
        public List<IndexedBullet> IndexedBullets = new List<IndexedBullet>();
        public List<IndexedParagraph> IndexedParagraphs = new List<IndexedParagraph>();
        public List<IndexedEntrepeneur> IndexedLegalEntities = new List<IndexedEntrepeneur>();
        public List<JudRepository.Paragraph> paragraphs = new List<JudRepository.Paragraph>();
        public List<SubEntrepeneur> ProjectSubEntrepeneurs = new List<SubEntrepeneur>();
        public List<Receiver> Receivers = new List<Receiver>();

        #endregion

        #region Constructors
        public UcIttLettersPrepareCommonLetter(Bizz cbz, UserControl ucRight)
        {
            InitializeComponent();
            this.CBZ = cbz;
            this.UcMain = ucRight;
            GenerateComboBoxCaseIdItems();
            PdfCreator = new PdfCreator(CBZ.StrConnection);
        }

        #endregion

        #region Buttons
        private void ButtonAddBullet_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxNewBullet.Text != "")
            {
                int dbAnswer = 0;
                Exception exception = new Exception();
                CBZ.TempBullet = new Bullet(CBZ.TempParagraph, TextBoxNewBullet.Text);
                try
                {
                    dbAnswer = CBZ.CreateInDb(CBZ.TempBullet);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
                if (dbAnswer < 1)
                {
                    Exception tempEx = new Exception();
                    if (exception != tempEx)
                    {
                        MessageBox.Show("Databasen meldte en fejl. Linjen blev ikke tilføjet til afsnittet\n" + exception, "", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        MessageBox.Show("Databasen meldte en fejl. Linjen blev ikke tilføjet til afsnittet", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    RefreshList("Bullets");
                    GetIndexedBullets();
                    ListBoxBullets.ItemsSource = IndexedBullets;
                    ListBoxBullets.SelectedIndex = 0;
                    TextBoxNewBullet.Text = "";
                }
            }
        }

        private void ButtonAddParagraph_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxNewBullet.Text != "")
            {
                int dbAnswer = 0;
                Exception exception = new Exception();
                this.CBZ.TempParagraph = new JudRepository.Paragraph(this.CBZ.TempProject, this.TextBoxNewParagraph.Text);
                try
                {
                    dbAnswer = CBZ.CreateInDb(CBZ.TempParagraph);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
                if (dbAnswer < 1)
                {
                    Exception tempEx = new Exception();
                    if (exception != tempEx)
                    {
                        MessageBox.Show("Databasen meldte en fejl. Linjen blev ikke tilføjet til afsnittet\n" + exception, "", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        MessageBox.Show("Databasen meldte en fejl. Linjen blev ikke tilføjet til afsnittet", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    RefreshList("Paragraphs");
                    GetIndexedParagraphs();
                    ComboBoxParagraphs.ItemsSource = "";
                    ComboBoxParagraphs.ItemsSource = IndexedBullets;
                    ComboBoxParagraphs.SelectedIndex = 0;
                    TextBoxNewParagraph.Text = "";
                }
            }
        }

        private void ButtonPrepare_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckReceiversListExist();
                CheckParagraphsExist();
                CheckParagraphBulletsExist();

                string commonPdfPath = GetCommonPdfPath();

                UpdateCommonPdfPathInShippingList(commonPdfPath);

                CBZ.RefreshList("ShippingList");
                RefreshShippingList();
                ComboBoxCaseId.SelectedIndex = 0;
                ComboBoxParagraphs.ItemsSource = "";
                ComboBoxParagraphs.SelectedIndex = -1;
                ListBoxBullets.ItemsSource = "";
                ListBoxBullets.SelectedIndex = -1;

            }
            catch (ArgumentNullException ane)
            {
                switch (ane.ParamName)
                {
                    case "missingBullet":
                        MessageBox.Show("Mindst et afsnit mangler underpunkter. Fælles del af Udbudsbrev kan ikke genereres.", "Forbered Udbudsbrev", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                    case "pdf":
                        MessageBox.Show("Pdf'en med Fælles del af Udbudsbrev kunne ikke genereres.\n" + ane.ToString(), "Forbered Udbudsbrev", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                    case "shipping":
                        MessageBox.Show("Forsendelseslisten kunne ikke opdateres.\n" + ane.ToString(), "Forbered Udbudsbrev", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                    case "noParagraphs":
                        MessageBox.Show("Der er ingen afsnit. Fælles del af Udbudsbrev kan ikke genereres.", "Forbered Udbudsbrev", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                    case "noReceivers":
                        MessageBox.Show("Der er ingen modtagerliste. Fælles del af Udbudsbrev kan ikke genereres.", "Forbered Udbudsbrev", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                }
            }
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            //Warning about lost changes before closing
            if (MessageBox.Show("Vil du lukke klargøring af Udbudsbrev?", "Luk Klargør Udbudsbrev", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                //Close right UserControl
                CBZ.UcMainActive = false;
                UcMain.Content = new UserControl();
            }
        }

        #endregion

        #region Events
        private void ComboBoxCaseId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = ComboBoxCaseId.SelectedIndex;
            foreach (IndexedProject temp in CBZ.IndexedActiveProjects)
            {
                if (temp.Index == selectedIndex)
                {
                    CBZ.TempProject = new Project(temp.Id, temp.Case, temp.Name, temp.Builder, temp.Status, temp.TenderForm, temp.EnterpriseForm, temp.Executive, temp.EnterprisesList, temp.Copy);
                }
            }
            TextBoxName.Text = CBZ.TempProject.Name;
            GetProjectDetails();
            SetCheckBoxReceiversListExist();
            ComboBoxParagraphs.ItemsSource = IndexedParagraphs;
            ComboBoxParagraphs.SelectedIndex = 0;
        }

        private void ComboBoxParagraphs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = ComboBoxParagraphs.SelectedIndex;
            if (IndexedParagraphs.Count == 0)
            {
                GetIndexedParagraphs();
            }
            foreach (IndexedParagraph temp in IndexedParagraphs)
            {
                if (temp.Index == selectedIndex)
                {
                    this.CBZ.TempParagraph = new JudRepository.Paragraph(temp.Id, temp.Project, temp.Text);
                    break;
                }
            }
            CBZ.RefreshList("LegalEntities");
            IndexedLegalEntities.Clear();
            GetIndexedBullets();
            ListBoxBullets.ItemsSource = IndexedBullets;
            ListBoxBullets.SelectedIndex = 0;
        }

        private void ListBoxBullets_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        #endregion

        #region Methods
        /// <summary>
        /// Method that adds Paragraphs to Db
        /// </summary>
        //private void AddParagraphs()
        //{
        //    string[] names = { "Komplet sæt beskrivelse i henhold til vedlagte dokumenter", "Projektdokumenter", "Tegninger i henhold til Tegningsliste", "Tidsplaner", "Øvrigt udbudsmateriale" };
        //    int i = 0;
        //    foreach (string name in names)
        //    {
        //        Paragraph temp = new Paragraph(Bizz.TempProject, names[i]);
        //        int dbAnswer = Bizz.CreateInDbReturnInt(temp);
        //        if (dbAnswer < 1)
        //        {
        //            MessageBox.Show("Databasen meldte fejl. Afsnittet blev ikke tilføjet.", "Tilføj Afsnit", MessageBoxButton.OK, MessageBoxImage.Error);
        //        }
        //        i++;
        //    }
        //    RefreshParagraphs();
        //}

        /// <summary>
        /// Method that adds a Shipping to Shipping List
        /// </summary>
        /// <param name="id">int</param>
        private void AddShipping(int id)
        {
            Shipping shipping = new Shipping();
            foreach (Shipping temp in CBZ.Shippings)
            {
                if (temp.Id == id)
                {
                    shipping = temp;
                    break;
                }
            }
            Shippings.Add(shipping);
        }

        /// <summary>
        /// Method, that checks, whether Bullets for an Paragraph exist
        /// </summary>
        /// <param name="paragraphId"></param>
        /// <returns></returns>
        private bool CheckBulletExist(int paragraphId)
        {
            bool result = false;

            foreach (Bullet bullet in CBZ.Bullets)
            {
                if (bullet.Paragraph.Id == paragraphId)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that checks, whether a LegalEntity exists in in a list
        /// </summary>
        /// <param name="entrepeneur">LegalEntity</param>
        /// <returns>bool</returns>
        private bool CheckEntrepeneur(Entrepeneur entrepeneur, List<Entrepeneur> list)
        {
            bool result = false;
            foreach (Entrepeneur entr in list)
            {
                if (entr.Id == entrepeneur.Id)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// Method that checks wether paragraphs have already been added
        /// </summary>
        private void CheckParagraphsExist()
        {
            bool paragraphsExist = false;

            foreach (JudRepository.Paragraph temp in this.CBZ.Paragraphs)
            {
                if (temp.Project.Id == this.CBZ.TempProject.Id)
                {
                    paragraphsExist = true;
                    break;
                }

            }

            if (!paragraphsExist)
            {
                throw new ArgumentNullException("noParagraphs");
            }
        }

        /// <summary>
        /// Method, that checks, whether all Paragraphs have a Bullet
        /// </summary>
        private void CheckParagraphBulletsExist()
        {

            foreach (IndexedParagraph paragraph in IndexedParagraphs)
            {
                bool bulletExist = CheckBulletExist(paragraph.Id);
                if (!bulletExist)
                {
                    throw new ArgumentNullException("missingBullet");
                }
            }
        }

        /// <summary>
        /// Method, that checks, whether CheckBoxReceiversListExist is checked
        /// </summary>
        private void CheckReceiversListExist()
        {
            if (CheckBoxReceiversListExist.IsChecked == false)
            {
                throw new ArgumentNullException("noReceivers");
            }
        }

        /// <summary>
        /// Method, that generates Items for ComboBoxCaseId
        /// </summary>
        private void GenerateComboBoxCaseIdItems()
        {
            ComboBoxCaseId.Items.Clear();
            //foreach (IndexedProject temp in Bizz.IndexedActiveProjects)
            //{
            //    ComboBoxCaseId.Items.Add(temp);
            //}
            ComboBoxCaseId.ItemsSource = "";
            ComboBoxCaseId.ItemsSource = CBZ.IndexedActiveProjects;
        }

        /// <summary>
        /// Method, that creates a PDF and returns the path
        /// </summary>
        /// <returns>string</returns>
        private string GetCommonPdfPath()
        {
            string result = "";

            try
            {
                result = PdfCreator.GenerateIttLetterCommonPdf(CBZ, CBZ.TempProject, Shippings);
            }
            catch (Exception)
            {
                throw new ArgumentNullException("pdf");
            }

            return result;

        }

        /// <summary>
        /// Method that creates an Indexed Bullet List
        /// </summary>
        private void GetIndexedBullets()
        {
            IndexedBullets.Clear();
            IndexedBullets.Add(new IndexedBullet(0, CBZ.Bullets[0]));
            int i = 1;
            foreach (Bullet temp in CBZ.Bullets)
            {
                if (temp.Paragraph.Id == CBZ.TempParagraph.Id)
                {
                    IndexedBullet other = new IndexedBullet(i, temp);
                    IndexedBullets.Add(other);
                    i++;
                }
            }
        }

        /// <summary>
        /// Method that creates an Indexed Enterprise List
        /// </summary>
        private void GetIndexedParagraphs()
        {
            IndexedParagraphs.Clear();
            IndexedParagraphs.Add(new IndexedParagraph(0, CBZ.Paragraphs[0]));
            //if (!ParagraphsExist())
            //{
            //    AddParagraphs();
            //}
            int i = 1;
            foreach (JudRepository.Paragraph temp in this.CBZ.Paragraphs)
            {
                if (temp.Project.Id == this.CBZ.TempProject.Id)
                {
                    IndexedParagraph other = new IndexedParagraph(i, temp);
                    this.IndexedParagraphs.Add(other);
                    i++;
                }
            }
        }

        /// <summary>
        /// Method that creates a list of indexed Entrepeneurs
        /// </summary>
        private void GetIndexedEntrepeneurs()
        {
            List<Entrepeneur> tempResult = new List<Entrepeneur>();
            IndexedEntrepeneur temp = new IndexedEntrepeneur(0, CBZ.Entrepeneurs[0]);
            IndexedLegalEntities.Clear();
            IndexedLegalEntities.Add(temp);
            foreach (Enterprise enterprise in CBZ.Enterprises)
            {
                foreach (Entrepeneur entrepeneur in CBZ.Entrepeneurs)
                {
                    if (!CheckEntrepeneur(entrepeneur, tempResult))
                    {
                        tempResult.Add(entrepeneur);
                    }
                }
            }
            int i = 1;
            foreach (Entrepeneur sub in tempResult)
            {
                temp = new IndexedEntrepeneur(i, sub);
                IndexedLegalEntities.Add(temp);
                i++;
            }
        }

        /// <summary>
        /// Method, that generates List of ProjectSubEntrepeneurs
        /// </summary>
        /// <param name="id">int</param>
        /// <returns></returns>
        private void GetProjectDetails()
        {
            ProjectSubEntrepeneurs.Clear();
            ProjectEnterprises.Clear();
            IndexedEnterprises.Clear();
            Receivers.Clear();
            Shippings.Clear();
            foreach (Enterprise enterprise in CBZ.Enterprises)
            {
                if (enterprise.Project.Id == CBZ.TempProject.Id)
                {
                    foreach (SubEntrepeneur sub in CBZ.SubEntrepeneurs)
                    {
                        if (sub.Enterprise.Id == enterprise.Id)
                        {
                            ProjectSubEntrepeneurs.Add(sub);
                        }
                        foreach (Shipping shipping in CBZ.Shippings)
                        {
                            if (shipping.SubEntrepeneur.Id == sub.Id)
                            {
                                Receivers.Add(shipping.Receiver);
                                Shippings.Add(shipping);
                            }
                        }
                    }
                    ProjectEnterprises.Add(enterprise);
                }
            }
            GetIndexedParagraphs();
            GetIndexedEntrepeneurs();
        }

        /// <summary>
        /// Method, that refreshes content of a list
        /// </summary>
        /// <param name="list">string</param>
        private void RefreshList(string list)
        {
            CBZ.RefreshList(list);
        }

        /// <summary>
        /// Method, that refreshes content of the Shipping List
        /// </summary>
        private void RefreshShippingList()
        {
            Shippings.Clear();
            foreach (Receiver receiver in Receivers)
            {
                AddShipping(receiver.Id);
            }
        }

        /// <summary>
        /// Method, that determines, whether CheckBoxReceiversList should be checked
        /// </summary>
        private void SetCheckBoxReceiversListExist()
        {
            if (Receivers.Count <= 1)
            {
                CheckBoxReceiversListExist.IsChecked = true;
            }
            else
            {
                CheckBoxReceiversListExist.IsChecked = false;
            }
        }

        /// <summary>
        /// Method, that adds the common PDF path to Shippings in ShippingList
        /// </summary>
        /// <param name="commonPdfPath">string</param>
        private void UpdateCommonPdfPathInShippingList(string commonPdfPath)
        {
            try
            {
                foreach (Shipping shipping in Shippings)
                {
                    CBZ.TempShipping = shipping;
                    CBZ.TempShipping.CommonPdfPath = commonPdfPath;
                    CBZ.UpdateInDb(CBZ.TempShipping);
                }

            }
            catch (Exception)
            {
                throw new ArgumentNullException("shipping");
            }

        }

        #endregion

    }
}
