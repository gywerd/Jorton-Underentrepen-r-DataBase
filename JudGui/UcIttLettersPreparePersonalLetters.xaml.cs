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
        public UserControl UcRight;
        public Shipping Shipping = new Shipping();
        public PdfCreator PdfCreator;
        public static string macAddress;

        public List<Contact> ProjectContacts = new List<Contact>();
        public List<Enterprise> ProjectEnterprises = new List<Enterprise>();
        public List<Receiver> ProjectReceivers = new List<Receiver>();
        public List<Shipping> ProjectShippings = new List<Shipping>();
        public List<SubEntrepeneur> ProjectSubEntrepeneurs = new List<SubEntrepeneur>();

        #endregion

        public UcIttLettersPreparePersonalLetters(Bizz cbz, UserControl ucRight)
        {
            InitializeComponent();
            this.CBZ = cbz;
            macAddress = GetMacAddress();
            this.UcRight = ucRight;
            GenerateComboBoxCaseIdItems();
            ComboBoxCaseId.SelectedIndex = 0;
        }

        #region Buttons
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            //Warning about lost changes before closing
            if (MessageBox.Show("Vil du lukke klargøring af Udbudsbrev ?", "Luk Klargør Udbudsbrev", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
            {
                //Close right UserControl
                UcRight.Content = new UserControl();
                CBZ.UcRightActive = false;
            }
        }

        private void ButtonPrepare_Click(object sender, RoutedEventArgs e)
        {
            //Code, that prepares 
            CBZ.TempLetterData = new LetterData(CBZ.TempProject.Name, CBZ.TempBuilder.Entity.Name, TextBoxAnswerDate.Text, TextBoxQuestionDate.Text, TextBoxCorrectionSheetDate.Text, Convert.ToInt32(TextBoxTimeSpan.Text), TextBoxMaterialUrl.Text, TextBoxConditionUrl.Text, TextBoxPassword.Text);

            try
            {
                // Code that save changes to the PdfData
                int result = CBZ.CreateInDb(CBZ.TempLetterData);

                if (result > 0)
                {

                    foreach (Shipping shipping in ProjectShippings)
                    {
                        CBZ.TempShipping = shipping;
                        CBZ.TempShipping.PdfPath = PdfCreator.GenerateIttLetterCompanyPdf(CBZ, shipping);
                        CBZ.UpdateInDb(CBZ.TempShipping);

                    }

                    //Show Confirmation
                    MessageBox.Show("Personlig del af Udbudsbrevet blev oprettet", "Forbered Udbudsbrev", MessageBoxButton.OK, MessageBoxImage.Information);

                    //Update PdfData List
                    CBZ.RefreshList("PdfDataList");

                    //Reset UcIttLetterPreparePersonal
                    ComboBoxCaseId.SelectedIndex = 0;
                    CBZ.RefreshList("ShippingList");
                    RefreshShippingList();
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

                }
                else
                {
                    throw new OperationCanceledException();
                }

            }
            catch (OperationCanceledException)
            {
                //Show error
                MessageBox.Show("Databasen returnerede en fejl. Personlig del af Tilbudsbrev blev ikke oprettet. Prøv igen.", "Forbered UdbudsBrev", MessageBoxButton.OK, MessageBoxImage.Information);
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
            if (selectedIndex >= 0)
            {
                foreach (IndexedProject temp in CBZ.IndexedActiveProjects)
                {
                    if (temp.Index == selectedIndex)
                    {
                        CBZ.TempProject = new Project(temp.Id, temp.Case, temp.Name, temp.Builder, temp.Status, temp.TenderForm, temp.EnterpriseForm, temp.Executive, temp.EnterprisesList, temp.Copy);
                        break;
                    }
                }
                TextBoxName.Text = CBZ.TempProject.Name;
                GetProjectSubEntrepeneurs();
                GetProjectReceivers();
            }
            else
            {
                TextBoxName.Text = "";
                ProjectSubEntrepeneurs.Clear();
                ProjectEnterprises.Clear();
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Method that adds a Shipping to Project Shipping List
        /// </summary>
        /// <param name="id">int</param>
        private void AddShipping(int id)
        {
            Shipping shipping = GetShipping(id);
            ProjectShippings.Add(shipping);
        }

        /// <summary>
        /// Method, that checks, whether a LegalEntity exists in Receivers list
        /// </summary>
        /// <param name="entity">LegalEntity</param>
        /// <returns>bool</returns>
        private bool CheckEntityReceivers(LegalEntity entity)
        {
            bool result = false;
            foreach (Receiver receiver in CBZ.Receivers)
            {
                if (receiver.Cvr == entity.Cvr)
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
        /// <param name="tempEntity">LegalEntity</param>
        /// <param name="sub">SubEntrepeneur</param>
        /// <param name="List<LegalEntity>"></param>
        /// <returns></returns>
        private bool CheckEntityTempResult(LegalEntity tempEntity, List<LegalEntity> tempResult)
        {
            bool exist = false;
            foreach (LegalEntity entity in tempResult)
            {
                if (entity.Id == tempEntity.Id)
                {
                    exist = true;
                    break;
                }
            }
            return exist;
        }

        /// <summary>
        /// Method, that generate content of ComboBoxCaseId
        /// </summary>
        private void GenerateComboBoxCaseIdItems()
        {
            ComboBoxCaseId.Items.Clear();
            foreach (IndexedProject temp in CBZ.IndexedActiveProjects)
            {
                ComboBoxCaseId.Items.Add(temp);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>Address</returns>
        private Address GetAddress(int id)
        {
            Address result = new Address();
            foreach (Address temp in CBZ.Addresses)
            {
                if (temp.Id == id)
                {
                    result = temp;
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
        /// <param name="sub">int</param>
        /// <returns>Contact</returns>
        private Contact GetContactFromList(int id)
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

        /// <summary>
        /// Method, that finds an email address in list
        /// </summary>
        /// <returns>string</returns>
        private string GetContactEmail(int id)
        {
            string result = "";
            foreach (ContactInfo info in CBZ.ContactInfoList)
            {
                if (info.Id == id)
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
        private Shipping GetShipping(int shippingId)
        {
            Shipping result = new Shipping();

            foreach (Shipping shipping in CBZ.Shippings)
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
        /// Method, that generates List of Receivers
        /// </summary>
        private void GetProjectReceivers()
        {
            ProjectReceivers.Clear();
            ProjectShippings.Clear();
            CBZ.RefreshList("Receivers");
            CBZ.RefreshList("ShippingList");

            foreach (Shipping shipping in CBZ.Shippings)
            {
                if (shipping.Project.Id == CBZ.TempProject.Id)
                {
                    ProjectReceivers.Add(shipping.Receiver);
                    ProjectShippings.Add(shipping);
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
        /// Method, that retrieves the MacAddress
        /// </summary>
        /// <returns></returns>
        private string GetMacAddress()
        {
            return CBZ.GetMacAddress();
        }

        /// <summary>
        /// Method, that generates the Project SubEntrepeneurs list
        /// </summary>
        private void GetProjectSubEntrepeneurs()
        {
            ProjectEnterprises.Clear();
            ProjectSubEntrepeneurs.Clear();
            foreach (Enterprise enterprise in CBZ.Enterprises)
            {
                if (enterprise.Project.Id == CBZ.TempProject.Id)
                {
                    ProjectEnterprises.Add(enterprise);
                    foreach (SubEntrepeneur sub in CBZ.SubEntrepeneurs)
                    {
                        if (sub.Enterprise.Id == enterprise.Id)
                        {
                            ProjectSubEntrepeneurs.Add(sub);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Method, that retrieves a SubEntrepeneur
        /// </summary>
        /// <param name="entrepeneurId">int</param>
        /// <returns></returns>
        private SubEntrepeneur GetSubEntrepeneur(int entrepeneurId)
        {
            SubEntrepeneur result = new SubEntrepeneur();

            foreach (SubEntrepeneur sub in ProjectSubEntrepeneurs)
            {
                if (sub.Entrepeneur.Id == entrepeneurId)
                {
                    result = sub;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that refreshes content of the Shipping List
        /// </summary>
        private void RefreshShippingList()
        {
            ProjectShippings.Clear();
            foreach (Receiver receiver in ProjectReceivers)
            {
                AddShipping(receiver.Id);
            }
        }

        #endregion

    }
}
