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
    /// Interaction logic for UcIttLetters.xaml
    /// </summary>
    public partial class UcIttLetters : UserControl
    {
        #region Fields
        public Bizz CBZ = new Bizz();
        public PdfCreator PdfCreator;
        public UserControl UcMain;

        public bool result;
        public static string macAddress;

        public List<IndexedSubEntrepeneur> IndexedSubEntrepeneurs;
        public Shipping Shipping = new Shipping();
        public List<Contact> ProjectContacts = new List<Contact>();
        public List<Enterprise> ProjectEnterprises = new List<Enterprise>();
        public List<Shipping> ProjectShippings = new List<Shipping>();
        public List<SubEntrepeneur> ProjectSubEntrepeneurs = new List<SubEntrepeneur>();
        public List<Receiver> ProjectReceivers = new List<Receiver>();

        #endregion

        #region Constructors
        public UcIttLetters(Bizz cbz, UserControl ucMain)
        {
            InitializeComponent();
            this.CBZ = cbz;
            macAddress = CBZ.MacAddress;
            this.UcMain = ucMain;

            CBZ.TempShipping = new Shipping();
            CBZ.TempLetterData = new LetterData();
            CBZ.TempProject = new Project();
            CBZ.TempReceiver = new Receiver();
            CBZ.TempSubEntrepeneur = new SubEntrepeneur();

            GenerateComboBoxCaseIdItems();

        }

        #endregion

        #region Buttons
        private void ButtonSend_Click(object sender, RoutedEventArgs e)
        {
            if (CheckBoxDetails.IsChecked == true)
            {
                result = false;

                CreateReceiverList();

                if (result)
                {
                    //Show Confirmation
                    MessageBox.Show("Modtager(e)n(ne) blev føjet til modtagerlisten.", "Udbudsbreve", MessageBoxButton.OK, MessageBoxImage.Information);

                    PrepareCommonIttLetter();
                    PreparePersonalIttLetters();
                    SendIttLetters();

                    //Reset Boxes
                    ComboBoxCaseId.SelectedIndex = -1;
                    CheckBoxDetails.IsChecked = false;
                    CheckBoxShowSent.IsChecked = false;

                    //Update lists and fields
                    CBZ.RefreshProjectList("All", CBZ.TempProject.Id);
                }
                else
                {
                    //Show error
                    MessageBox.Show("Databasen returnerede en fejl. Modtager(e)n(ne) blev ikke føjet til modtagerlisten. Prøv igen.", "Udbudsbreve", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                //Show error
                MessageBox.Show("Projektet mangler uddybende oplysninger. Ret dette under 'Projekter' => 'Uddybning' og prøv igen.", "Udbudsbreve", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonClearAll_Click(object sender, RoutedEventArgs e)
        {
            ListBoxEntrepeneurs.UnselectAll();
        }

        private void ButtonChoseAll_Click(object sender, RoutedEventArgs e)
        {
            ListBoxEntrepeneurs.SelectAll();
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                //Warning about lost changes before closing
                if (MessageBox.Show("Vil du lukke Vælg Modtagere?", "Udbudsbreve", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    CBZ.CloseUcMain(UcMain);
                }
            }
            else
            {
                CBZ.CloseUcMain(UcMain);
            }
        }

        #endregion

        #region Events
        private void CheckBoxShowSent_ToggleChecked(object sender, RoutedEventArgs e)
        {
            CBZ.TempShipping = new Shipping();
            GetIndexedEntrepeneurs();
            ListBoxEntrepeneurs.SelectedIndex = -1;
            SetCheckboxes();

        }

        private void ComboBoxCaseId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxCaseId.SelectedIndex >= 0)
            {
                CBZ.TempProject = new Project((IndexedProject)ComboBoxCaseId.SelectedItem);
                CBZ.TempLetterData = new LetterData();
                CBZ.TempLetterData.ProjectName = CBZ.TempProject.Details.Name;
                CBZ.TempLetterData.Builder = CBZ.TempProject.Builder.Entity.Name;

                TextBoxName.Text = CBZ.TempProject.Details.Name;
                RefreshProjectSubEntrepeneurs();
                ListBoxEntrepeneurs.ItemsSource = "";
                ListBoxEntrepeneurs.ItemsSource = IndexedSubEntrepeneurs;

                //Set CBZ.UcMainEdited
                if (!CBZ.UcMainEdited)
                {
                    CBZ.UcMainEdited = true;
                }

            }
            else
            {
                TextBoxName.Text = "";
                ListBoxEntrepeneurs.SelectedIndex = -1;
                ListBoxEntrepeneurs.ItemsSource = "";
                CBZ.TempShipping = new Shipping();
                CBZ.TempLetterData = new LetterData();
                CBZ.TempProject = new Project();
                CBZ.TempSubEntrepeneur = new SubEntrepeneur();
                TextBoxAnswerDate.Text = "";
                TextBoxConditionDate.Text = "";
                TextBoxConditionUrl.Text = "";
                TextBoxCorrectionSheetDate.Text = "";
                TextBoxMaterialUrl.Text = "";
                TextBoxPassword.Text = "";
                TextBoxQuestionDate.Text = "";
                TextBoxTimeSpan.Text = "";

                //Reset CBZ.UcMainEdited
                if (CBZ.UcMainEdited)
                {
                    CBZ.UcMainEdited = false;
                }

            }

        }

        private void ListBoxEntrepeneurs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedItemsCount = ListBoxEntrepeneurs.SelectedItems.Count;

            switch (selectedItemsCount)
            {
                case 1:
                    CBZ.TempSubEntrepeneur = new SubEntrepeneur((IndexedSubEntrepeneur)ListBoxEntrepeneurs.SelectedItem);
                    break;
                default:
                    CBZ.TempSubEntrepeneur = null;
                    break;
            }

        }

        private void TextBoxAnswerDate_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ComboBoxCaseId.SelectedIndex >= 0)
            {
                if (CBZ.TempLetterData.AnswerDate != TextBoxAnswerDate.Text)
                {
                    CBZ.TempLetterData.AnswerDate = TextBoxAnswerDate.Text;
                }

                SetCheckboxes();

            }
            else
            {
                TextBoxAnswerDate.Text = "";
            }

        }

        private void TextBoxConditionDate_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ComboBoxCaseId.SelectedIndex >= 0)
            {
                if (CBZ.TempLetterData.ConditionDate != TextBoxConditionDate.Text)
                {
                    CBZ.TempLetterData.ConditionDate = TextBoxConditionDate.Text;
                }

                SetCheckboxes();

            }
            else
            {
                TextBoxConditionDate.Text = "";
            }

        }

        private void TextBoxConditionUrl_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ComboBoxCaseId.SelectedIndex >= 0)
            {
                if (CBZ.TempLetterData.ConditionUrl != TextBoxConditionUrl.Text)
                {
                    CBZ.TempLetterData.ConditionUrl = TextBoxConditionUrl.Text;
                }

                SetCheckboxes();

            }
            else
            {
                TextBoxConditionUrl.Text = "";
            }

        }

        private void TextBoxCorrectionSheetDate_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ComboBoxCaseId.SelectedIndex >= 0)
            {
                if (CBZ.TempLetterData.CorrectionDate != TextBoxCorrectionSheetDate.Text)
                {
                    CBZ.TempLetterData.CorrectionDate = TextBoxCorrectionSheetDate.Text;
                }

                SetCheckboxes();

            }
            else
            {
                TextBoxCorrectionSheetDate.Text = "";
            }

        }

        private void TextBoxMaterialUrl_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ComboBoxCaseId.SelectedIndex >= 0)
            {
                if (CBZ.TempLetterData.MaterialUrl != TextBoxMaterialUrl.Text)
                {
                    CBZ.TempLetterData.MaterialUrl = TextBoxMaterialUrl.Text;
                }

                SetCheckboxes();

            }
            else
            {
                TextBoxMaterialUrl.Text = "";
            }

        }

        private void TextBoxPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ComboBoxCaseId.SelectedIndex >= 0)
            {
                if (CBZ.TempLetterData.PassWord != TextBoxPassword.Text)
                {
                    CBZ.TempLetterData.PassWord = TextBoxPassword.Text;
                }

                SetCheckboxes();

            }
            else
            {
                TextBoxPassword.Text = "";
            }

        }

        private void TextBoxQuestionDate_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ComboBoxCaseId.SelectedIndex >= 0)
            {
                if (CBZ.TempLetterData.QuestionDate != TextBoxQuestionDate.Text)
                {
                    CBZ.TempLetterData.QuestionDate = TextBoxQuestionDate.Text;
                }

                SetCheckboxes();

            }
            else
            {
                TextBoxQuestionDate.Text = "";
            }

        }

        private void TextBoxTimeSpan_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ComboBoxCaseId.SelectedIndex >= 0)
            {
                if (CBZ.TempLetterData.TimeSpan != Convert.ToInt32(TextBoxTimeSpan.Text))
                {
                    CBZ.TempLetterData.TimeSpan = Convert.ToInt32(TextBoxTimeSpan.Text);
                }

                SetCheckboxes();

            }
            else
            {
                TextBoxTimeSpan.Text = "";
            }

        }

        #endregion

        #region Methods
        /// <summary>
        /// Method, that adds multiple Receivers to Db
        /// </summary>
        /// <returns></returns>
        private void AddReceivers()
        {
            result = false;
            ProjectReceivers.Clear();
            ProjectShippings.Clear();

            foreach (var item in ListBoxEntrepeneurs.SelectedItems)
            {
                IndexedSubEntrepeneur subEntrepeneur = new IndexedSubEntrepeneur((IndexedSubEntrepeneur)item);
                FillReceiver(subEntrepeneur);

                //Code that ads a enterprise to Enterprise List
                int tempResult = CBZ.CreateInDb(CBZ.TempReceiver);

                //Code, that checks result
                if (!result)
                {
                    if (tempResult < 1)
                    {
                        ProjectReceivers.Add(CBZ.TempReceiver);

                        CBZ.TempShipping = new Shipping(subEntrepeneur, CBZ.TempReceiver, CBZ.TempLetterData, @"PDF_Documents\", @"PDF_Documents\", @"PDF_Documents\", macAddress);
                        CBZ.TempShipping.SetId(CBZ.CreateInDb(CBZ.TempShipping));
                        ProjectShippings.Add(CBZ.TempShipping);

                        if (!result)
                        {
                            result = true;
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Method, that checks if Receivers have been selected in ListBoxEntrepeneurs
        /// </summary>
        /// <returns></returns>
        private bool CheckReceiversExist()
        {
            bool result = false;

            if (ListBoxEntrepeneurs.SelectedItems.Count >= 1)
            {
                result = true;
            }

            return result;
        }

        private void CreateReceiverList()
        {
            bool receivers = CheckReceiversExist();

            switch (receivers)
            {
                case false:
                    //Show Confirmation
                    MessageBox.Show("Du har ikke valgt nogen modtagere. Der blev ikke føjet modtagere til modtagerlisten.", "Udbudsbreve", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                case true:
                    AddReceivers();
                    break;
            }

        }

        /// <summary>
        /// Method, that creates a Shipping
        /// </summary>
        /// <param name="project">Project</param>
        /// <param name="receiver">Receiver</param>
        /// <param name="subEntrepeneur">SubEntrepeneur</param>
        /// <param name="letterData">PdfData</param>
        private void CreateShipping(Project project, Receiver receiver, IndexedSubEntrepeneur subEntrepeneur)
        {
            CBZ.TempShipping = new Shipping(subEntrepeneur, receiver, new LetterData(), @"PDF_Documents\", @"PDF_Documents\", @"PDF_Documents\", macAddress);

            try
            {
                int id = CBZ.CreateInDb(CBZ.TempShipping);
                CBZ.TempShipping.SetId(id);
                CBZ.TempShipping.PersonalIttLetterPdfPath = @"PDF_Documents\";
                CBZ.UpdateInDb(CBZ.TempShipping);
                CBZ.ProjectLists.Shippings.Add(CBZ.TempShipping);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Databasen returnerede en fejl. Forsendelsen blev ikke opdateret.\n" + ex, "Udbudsbreve", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Method, that adds data to an ITT-letter receiver
        /// </summary>
        /// <param name="entrepeneur">IndexedSubEntrepeneur</param>
        private void FillReceiver(IndexedSubEntrepeneur entrepeneur)
        {
            string cvr = entrepeneur.Entrepeneur.Entity.Cvr;
            string companyName = entrepeneur.Entrepeneur.Entity.Name;
            string attention = @"Att. " + entrepeneur.Contact.Person.Name;
            string street = entrepeneur.Entrepeneur.Entity.Address.Street;
            string place = entrepeneur.Entrepeneur.Entity.Address.Place;
            string zipTown = entrepeneur.Entrepeneur.Entity.Address.ZipTown.ToString();
            string email = entrepeneur.Contact.Person.ContactInfo.Email;

            CBZ.TempReceiver = new Receiver(cvr, companyName, attention, street, zipTown, email, place);

        }

 
        /// <summary>
        /// Method, that generates Items for ComboBoxCaseId
        /// </summary>
        private void GenerateComboBoxCaseIdItems()
        {
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
                result = PdfCreator.GenerateIttLetterCommonPdf(CBZ);
            }
            catch (Exception)
            {
                throw new ArgumentNullException("pdf");
            }

            return result;

        }

        /// <summary>
        /// Method, that creates an indexable Enterprises List
        /// </summary>
        /// <returns>List<IndexedEnterprise></returns>
        private List<IndexedEnterprise> GetIndexedEnterprises()
        {
            List<IndexedEnterprise> result = new List<IndexedEnterprise>();
            int i = 0;
            foreach (Enterprise enterprise in ProjectEnterprises)
            {
                IndexedEnterprise temp = new IndexedEnterprise(i, enterprise);
                result.Add(temp);
                i++;
            }
            return result;
        }

        /// <summary>
        /// Method that creates a list of indexable Legal Entities
        /// </summary>
        /// <returns>List<IndexedLegalEntity></returns>
        private void GetIndexedEntrepeneurs()
        {
            CBZ.RefreshIndexedList("SubEntrepeneursFromProjectSubEntrepeneurs");

            IndexedSubEntrepeneurs.Clear();

            foreach (IndexedSubEntrepeneur subEntrepeneur in CBZ.IndexedSubEntrepeneurs)
            {
                if (CheckBoxShowSent.IsChecked == false)
                {
                    if (!subEntrepeneur.IttLetter.Sent)
                    {
                        IndexedSubEntrepeneurs.Add(subEntrepeneur);
                    }
                }
                else
                {
                    IndexedSubEntrepeneurs.Add(subEntrepeneur);
                }
            }

            ListBoxEntrepeneurs.ItemsSource = "";
            ListBoxEntrepeneurs.ItemsSource = IndexedSubEntrepeneurs;
        }

        /// <summary>
        /// Method, that prepares the Common IttLetter 
        /// </summary>
        private void PrepareCommonIttLetter()
        {
            try
            {
                string commonPdfPath = GetCommonPdfPath();

                UpdateCommonPdfPathInShippingList(commonPdfPath);

            }
            catch (ArgumentNullException argNex)
            {
                switch (argNex.ParamName)
                {
                    case "pdf":
                        MessageBox.Show("Pdf'en med Fælles del af Udbudsbrev kunne ikke genereres.\n" + argNex.ToString(), "Udbudsbreve", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                    case "shipping":
                        MessageBox.Show("Forsendelseslisten kunne ikke opdateres.\n" + argNex.ToString(), "Udbudsbreve", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                    default:
                        MessageBox.Show("Der opstod en fejl under forberedelse af den fælles del af Udbudsbrev. Fælles del af Udbudsbrev kan ikke genereres.\n" + argNex.ToString(), "Udbudsbreve", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                }

                throw argNex;
            }
        }

        /// <summary>
        /// Method, that prepares the personal IttLetters
        /// </summary>
        private void PreparePersonalIttLetters()
        {
            try
            {
                // Code that save changes to the PdfData
                int result = CBZ.CreateInDb(CBZ.TempLetterData);

                if (result > 0)
                {

                    foreach (Shipping shipping in ProjectShippings)
                    {
                        CBZ.TempShipping = shipping;
                        CBZ.TempShipping.PersonalIttLetterPdfPath = PdfCreator.GenerateIttLetterCompanyPdf(CBZ, shipping);
                        CBZ.UpdateInDb(CBZ.TempShipping);

                    }

                }
                else
                {
                    MessageBox.Show("Personlig del af Udbudsbrevet blev ikke oprettet. Prøv igen", "Udbudsbreve", MessageBoxButton.OK, MessageBoxImage.Information);
                }

            }
            catch (Exception ex)
            {
                //Show error
                MessageBox.Show("Der opstod en fejl.\n" + ex, "Udbudsbreve", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        /// <summary>
        /// Method, that generates List of ProjectSubEntrepeneurs
        /// </summary>
        /// <returns></returns>
        private void RefreshProjectSubEntrepeneurs()
        {
            CBZ.RefreshProjectList("All", CBZ.TempProject.Id);

            IndexedSubEntrepeneurs = new List<IndexedSubEntrepeneur>();

            int i = 0;

            foreach (SubEntrepeneur subEntrepeneur in CBZ.ProjectLists.SubEntrepeneurs)
            {
                IndexedSubEntrepeneurs.Add(new IndexedSubEntrepeneur(i, subEntrepeneur));
                i++;
            }

        }

        /// <summary>
        /// Method, that sends IttLetters
        /// </summary>
        private void SendIttLetters()
        {
            try
            {
                //Make som code, that sends emails
                foreach (Shipping shipping in ProjectShippings)
                {
                    CBZ.TempShipping = shipping;
                    string[] fileNames = new string[] { CBZ.TempShipping.PersonalIttLetterPdfPath, CBZ.TempShipping.CommonIttLetterPdfPath };
                    Email email = new Email(CBZ, "Udbudsbrev om underentreprise på " + CBZ.TempShipping.Receiver.Name, CBZ.TempShipping.Receiver.Email, CBZ.TempShipping.SubEntrepeneur.Enterprise.Project.Executive.Person.ContactInfo.Email, "Dette er en prøve", fileNames);
                    if (!CBZ.TempShipping.SubEntrepeneur.IttLetter.Sent)
                    {
                        CBZ.TempShipping.SubEntrepeneur.IttLetter.ToggleSent();
                        CBZ.TempShipping.SubEntrepeneur.Request.SentDate = DateTime.Now;
                    }
                    CBZ.UpdateInDb(CBZ.TempShipping.SubEntrepeneur.IttLetter);
                }

                MessageBox.Show("Udbudsbrevet blev sendt.", "Forespørgsler", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Udbudsbrevet blev ikke sendt.\n" + ex.ToString(), "Forespørgsler", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        /// <summary>
        /// Method, that sets IsChecked for CheckBoxes
        /// </summary>
        private void SetCheckboxes()
        {
            if (CBZ.ProjectLists.Paragrafs.Count >= 1 && TextBoxAnswerDate.Text.Length >= 1 && TextBoxConditionDate.Text.Length >= 1 && TextBoxConditionUrl.Text.Length >= 1 && TextBoxCorrectionSheetDate.Text.Length >= 1 && TextBoxMaterialUrl.Text.Length >= 1 && TextBoxPassword.Text.Length >= 1 && TextBoxQuestionDate.Text.Length >= 1 && TextBoxTimeSpan.Text.Length >= 1)
            {
                CheckBoxDetails.IsChecked = true;
            }
            else
            {
                CheckBoxDetails.IsChecked = false;
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
                foreach (Shipping shipping in ProjectShippings)
                {
                    CBZ.TempShipping = shipping;
                    CBZ.TempShipping.CommonIttLetterPdfPath = commonPdfPath;
                    CBZ.UpdateInDb(CBZ.TempShipping);
                }

                CBZ.RefreshProjectList("Shippings", CBZ.TempProject.Id);

            }
            catch (Exception)
            {
                throw new ArgumentNullException("shipping");
            }

        }

        #endregion

    }
}
