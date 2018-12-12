using ClassBizz;
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
        public Bizz CBZ;
        public UserControl UcRight;
        public IttLetterShipping Shipping = new IttLetterShipping();
        public static string macAddress;

        public List<Contact> ProjectContacts = new List<Contact>();
        public List<Enterprise> ProjectEnterprises = new List<Enterprise>();
        public List<IttLetterReceiver> ProjectIttLetterReceivers = new List<IttLetterReceiver>();
        public List<SubEntrepeneur> ProjectSubEntrepeneurs = new List<SubEntrepeneur>();

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
            if (MessageBox.Show("Du er ved at lukke projektet. Alt, der ikke er gemt vil blive mistet!", "Luk Projekt", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
            {
                //Close right UserControl
                UcRight.Content = new UserControl();
                CBZ.UcRightActive = false;
            }
        }

        private void ButtonPrepare_Click(object sender, RoutedEventArgs e)
        {
            //Code, that prepares 
            CBZ.TempIttLetterPdfData = new IttLetterPdfData(CBZ.TempProject, CBZ.TempBuilder, TextBoxAnswerDate.Text, TextBoxQuestionDate.Text, TextBoxCorrectionSheetDate.Text, Convert.ToInt32(TextBoxTimeSpan.Text), TextBoxMaterialUrl.Text, TextBoxConditionUrl.Text, TextBoxPassword.Text);

            // Code that save changes to the IttLetter PdfData
            int result = CBZ.CreateInDbReturnInt(CBZ.TempIttLetterPdfData);

            if (result > 0)
            {
                //Show Confirmation
                MessageBox.Show("Personlig del af Udbudsbrevet blev rettet", "Forbered Udbudsbrev", MessageBoxButton.OK, MessageBoxImage.Information);

                //Update IttLetter PdfData List
                CBZ.RefreshList("IttLetterPdfDataList");

                //Reset UcIttLetterPreparePersonal
                ComboBoxCaseId.SelectedIndex = 0;
                CBZ.TempProject = new Project();
                CBZ.TempBuilder = new Builder();
                CBZ.TempIttLetterPdfData = new IttLetterPdfData();

            }
            else
            {
                //Show error
                MessageBox.Show("Databasen returnerede en fejl. Personlig del af Tilbudsbrev blev ikke oprettet. Prøv igen.", "Forbered UdbudsBrev", MessageBoxButton.OK, MessageBoxImage.Information);
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
                        CBZ.TempProject = new Project(temp.Id, temp.CaseId, temp.Name, temp.Builder, temp.Status, temp.TenderForm, temp.EnterpriseForm, temp.Executive, temp.EnterprisesList, temp.Copy);
                        break;
                    }
                }
                TextBoxName.Text = CBZ.TempProject.Name;
                GetProjectSubEntrepeneurs();
                GetProjectIttLetterReceivers();
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
        /// Method, that checks, whether a LegalEntity exists in IttLetter Receivers list
        /// </summary>
        /// <param name="entity">LegalEntity</param>
        /// <returns>bool</returns>
        private bool CheckEntityIttLetterReceivers(LegalEntity entity)
        {
            bool result = false;
            foreach (IttLetterReceiver receiver in CBZ.IttLetterReceivers)
            {
                if (receiver.CompanyId == entity.Id)
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
        private Contact GetContact(string entrepeneur)
        {
            return GetSubEntrepeneur(entrepeneur).Contact;
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
        /// Method, that creates a IttLetterShipping
        /// </summary>
        /// <returns></returns>
        private void GetIttLetterShipping()
        {
            this.Shipping = new IttLetterShipping(this.CBZ.TempProject, @"PDF_Documents\", macAddress);
            try
            {
                int id = CBZ.CreateInDbReturnInt(Shipping);
                Shipping.SetId(id);
                Shipping.PdfPath = "";
                CBZ.UpdateInDb(Shipping);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Databasen returnerede en fejl. Forsendelsen blev ikke opdateret.\n" + ex, "Opdater forsendelse", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Method, that generates List of IttLetterReceivers
        /// </summary>
        private void GetProjectIttLetterReceivers()
        {
            ProjectIttLetterReceivers.Clear();
            CBZ.RefreshList("IttLetterReceivers");

            foreach (IttLetterReceiver receiver in CBZ.IttLetterReceivers)
            {
                if (receiver.Project.Id == CBZ.TempProject.Id)
                {
                    ProjectIttLetterReceivers.Add(receiver);
                }
            }

            if (ProjectIttLetterReceivers.Count >= 1)
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
        /// <param name="entrepeneur"></param>
        /// <returns></returns>
        private SubEntrepeneur GetSubEntrepeneur(string entrepeneur)
        {
            SubEntrepeneur result = new SubEntrepeneur();

            foreach (SubEntrepeneur sub in ProjectSubEntrepeneurs)
            {
                if (sub.Entrepeneur.Id == entrepeneur)
                {
                    result = sub;
                    break;
                }
            }

            return result;
        }

        #endregion

    }
}
