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
    /// Interaction logic for UcIttLettersPreparePersonalLetters.xaml
    /// </summary>
    public partial class UcIttLettersPreparePersonalLetters : UserControl
    {
        #region Fields
        public Bizz CBZ;
        public UserControl UcMain;
        public IttLetterShipping Shipping = new IttLetterShipping();
        public PdfCreator PdfCreator;
        public static string macAddress;

        public List<Contact> ProjectContacts = new List<Contact>();
        public List<Enterprise> ProjectEnterprises = new List<Enterprise>();
        public List<Receiver> ProjectReceivers = new List<Receiver>();
        public List<IttLetterShipping> ProjectShippings = new List<IttLetterShipping>();
        public List<SubEntrepeneur> ProjectSubEntrepeneurs = new List<SubEntrepeneur>();

        #endregion

        #region Constructors
        public UcIttLettersPreparePersonalLetters(Bizz cbz, UserControl ucMain)
        {
            InitializeComponent();
            this.CBZ = cbz;
            macAddress = CBZ.MacAddress;
            this.UcMain = ucMain;
            ComboBoxCaseId.ItemsSource = CBZ.IndexedActiveProjects;
            ComboBoxCaseId.SelectedIndex = 0;
        }

        #endregion

        #region Buttons
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                //Warning about lost changes before closing
                if (MessageBox.Show("Vil du lukke klargøring af Udbudsbrev ?", "Udbudsbreve", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    //Close right UserControl
                    UcMain.Content = new UserControl();
                    CBZ.UcMainEdited = false;
                }
            }
            else
            {
                //Close right UserControl
                UcMain.Content = new UserControl();
                CBZ.UcMainEdited = false;
            }
        }

        private void ButtonPrepare_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Code that save changes to the PdfData
                int result = CBZ.CreateInDb(CBZ.TempLetterData);

                if (result > 0)
                {

                    foreach (IttLetterShipping shipping in ProjectShippings)
                    {
                        CBZ.TempIttLetterShipping = shipping;
                        CBZ.TempIttLetterShipping.PersonalPdfPath = PdfCreator.GenerateIttLetterCompanyPdf(CBZ, shipping);
                        CBZ.UpdateInDb(CBZ.TempIttLetterShipping);

                    }

                    //Show Confirmation
                    MessageBox.Show("Personlig del af Udbudsbrevet blev oprettet", "Udbudsbreve", MessageBoxButton.OK, MessageBoxImage.Information);

                    //Update PdfData List
                    CBZ.RefreshList("PdfDataList");

                    //Reset UcIttLetterPreparePersonal
                    ComboBoxCaseId.SelectedIndex = 0;
                    CBZ.RefreshList("ShippingList");
                    RefreshProjectShippings();
                    CBZ.TempProject = new Project();
                    CBZ.TempBuilder = new Builder();
                    CBZ.TempLetterData = new LetterData();
                    TextBoxAnswerDate.Text = "";
                    TextBoxQuestionDate.Text = "";
                    TextBoxCorrectionSheetDate.Text = "";
                    TextBoxTimeSpan.Text = "";
                    TextBoxMaterialUrl.Text = "";
                    TextBoxConditionUrl.Text = "";
                    TextBoxPassword.Text = "";
                    CBZ.UcMainEdited = false;

                }
                else
                {
                    MessageBox.Show("Personlig del af Udbudsbrevet blev ikke oprettet. Prøv igen", "Udbudsbreve", MessageBoxButton.OK, MessageBoxImage.Information);
                }

            }
            catch (Exception ex)
            {
                //Show error
                MessageBox.Show("Der opstod en fejl.\n" + ex, "Forbered UdbudsBrev", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        #endregion

        #region Events
        private void ComboBoxCaseId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = ComboBoxCaseId.SelectedIndex;
            if (selectedIndex != 1)
            {
                CBZ.TempProject = new Project((IndexedProject)ComboBoxCaseId.SelectedItem);

                TextBoxName.Text = CBZ.TempProject.Details.Name;
                RefreshProjectSubEntrepeneurs();
                RefreshProjectReceivers();
            }
            else
            {
                TextBoxName.Text = "";
                ProjectSubEntrepeneurs.Clear();
                ProjectEnterprises.Clear();
            }

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }

        }

        private void TextBoxAnswerDate_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempLetterData.AnswerDate = TextBoxAnswerDate.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }

        }

        private void TextBoxConditionDate_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempLetterData.ConditionDate = TextBoxConditionDate.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }

        }

        private void TextBoxConditionUrl_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempLetterData.ConditionUrl = TextBoxConditionUrl.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }

        }

        private void TextBoxCorrectionSheetDate_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempLetterData.CorrectionDate = TextBoxCorrectionSheetDate.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }

        }

        private void TextBoxMaterialUrl_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempLetterData.MaterialUrl = TextBoxMaterialUrl.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }

        }

        private void TextBoxPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempLetterData.PassWord = TextBoxPassword.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }

        }
        private void TextBoxQuestionDate_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempLetterData.QuestionDate = TextBoxQuestionDate.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }

        }

        private void TextBoxTimeSpan_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempLetterData.TimeSpan = Convert.ToInt32(TextBoxTimeSpan.Text);

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }

        }

        #endregion

        #region Methods
        /// <summary>
        /// Method, that checks, whether a LegalEntity exists in Receivers list
        /// </summary>
        /// <param name="entrepeneur">LegalEntity</param>
        /// <returns>bool</returns>
        private bool CheckEntityReceivers(Entrepeneur entrepeneur)
        {
            bool result = false;
            foreach (Receiver receiver in CBZ.Receivers)
            {
                if (receiver.Cvr == entrepeneur.Entity.Cvr)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// Method, that checks, whether a LegalEntity exists in tempResult list
        /// </summary>
        /// <param name="entrepeneur">Entrepeneur</param>
        /// <param name="List<Entrepeneur>"></param>
        /// <returns></returns>
        private bool CheckEntrepeneurTempResult(Entrepeneur entrepeneur, List<Entrepeneur> entrepeneurs)
        {
            bool exist = false;
            foreach (Entrepeneur entity in entrepeneurs)
            {
                if (entity.Id == entrepeneur.Id)
                {
                    exist = true;
                    break;
                }
            }
            return exist;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="addressId">int</param>
        /// <returns>Address</returns>
        private Address GetAddress(int addressId)
        {
            Address result = new Address();
            foreach (Address address in CBZ.Addresses)
            {
                if (address.Id == addressId)
                {
                    result = address;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// Method, that returns a Contact
        /// </summary>
        /// <param name="entrepeneur">string</param>
        /// <returns>Contact</returns>
        private Contact GetContact(int entrepeneurId)
        {
            return GetSubEntrepeneur(entrepeneurId).Contact;
        }

        /// <summary>
        /// Method, that retrieves a Contact from Contact List
        /// </summary>
        /// <param name="contactId">int</param>
        /// <returns>Contact</returns>
        private Contact GetContactFromList(int contactId)
        {
            Contact result = new Contact();

            foreach (Contact contact in CBZ.Contacts)
            {
                if (contact.Id == contactId)
                {
                    result = contact;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that finds an email address in list
        /// </summary>
        /// <param name="contactInfoId">int</param>
        /// <returns>string</returns>
        private string GetContactEmail(int contactInfoId)
        {
            string result = "";

            foreach (ContactInfo info in CBZ.ContactInfoList)
            {
                if (info.Id == contactInfoId)
                {
                    result = info.Email;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// Method, that creates a Shipping
        /// </summary>
        /// <param name="shippingId">int</param>
        /// <returns>Shipping</returns>
        private IttLetterShipping GetShipping(int shippingId)
        {
            IttLetterShipping result = new IttLetterShipping();

            foreach (IttLetterShipping shipping in CBZ.IttLetterShippings)
            {
                if (shipping.Id == shippingId)
                {
                    result = shipping;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that retrieves a SubEntrepeneur
        /// </summary>
        /// <param name="entrepeneurId">int</param>
        /// <returns></returns>
        private SubEntrepeneur GetSubEntrepeneur(int entrepeneurId)
        {
            SubEntrepeneur result = new SubEntrepeneur();

            foreach (SubEntrepeneur subEntrepeneur in ProjectSubEntrepeneurs)
            {
                if (subEntrepeneur.Entrepeneur.Id == entrepeneurId)
                {
                    result = subEntrepeneur;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that generates the Project SubEntrepeneurs list
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
        /// Method, that generates List of Receivers
        /// </summary>
        private void RefreshProjectReceivers()
        {
            RefreshProjectShippings();
            ProjectReceivers.Clear();

            foreach (IttLetterShipping shipping in ProjectShippings)
            {
                if (shipping.SubEntrepeneur.Enterprise.Project.Id == CBZ.TempProject.Id)
                {
                    ProjectReceivers.Add(shipping.Receiver);
                }
            }

            if (ProjectReceivers.Count >= 1)
            {
                CheckBoxReceiverListExist.IsChecked = true;
            }
            else
            {
                CheckBoxReceiverListExist.IsChecked = false;
            }

        }

        /// <summary>
        /// Method, that generates List of Receivers
        /// </summary>
        private void RefreshProjectShippings()
        {
            ProjectShippings.Clear();
            CBZ.RefreshList("Shippings");

            foreach (IttLetterShipping shipping in CBZ.IttLetterShippings)
            {
                if (shipping.SubEntrepeneur.Enterprise.Project.Id == CBZ.TempProject.Id)
                {
                    ProjectReceivers.Add(shipping.Receiver);
                }
            }

        }

        /// <summary>
        /// Method, that generates the Project SubEntrepeneurs list
        /// </summary>
        private void RefreshProjectSubEntrepeneurs()
        {
            RefreshProjectEnterprises();
            ProjectSubEntrepeneurs.Clear();
            foreach (Enterprise enterprise in ProjectEnterprises)
            {
                foreach (SubEntrepeneur sub in CBZ.SubEntrepeneurs)
                {
                    if (sub.Enterprise.Id == enterprise.Id)
                    {
                        ProjectSubEntrepeneurs.Add(sub);
                    }
                }
            }
        }

        #endregion

    }
}
