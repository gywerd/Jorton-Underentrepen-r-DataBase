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
        public Bizz Bizz;
        public UserControl UcRight;
        public IttLetterShipping Shipping = new IttLetterShipping();

        public List<Contact> ProjectContacts = new List<Contact>();
        public List<Enterprise> ProjectEnterpriseList = new List<Enterprise>();
        public List<IttLetterReceiver> ProjectIttLetterReceivers = new List<IttLetterReceiver>();
        public List<SubEntrepeneur> ProjectSubEntrepeneurs = new List<SubEntrepeneur>();

        public UcIttLettersPreparePersonalLetters(Bizz bizz, UserControl ucRight)
        {
            InitializeComponent();
            this.Bizz = bizz;
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
                Bizz.UcRightActive = false;
            }
        }

        private void ButtonPrepare_Click(object sender, RoutedEventArgs e)
        {
            //Code, that prepares 
            Bizz.TempIttLetterPdfData = new IttLetterPdfData(Bizz.TempProject, Bizz.TempBuilder, TextBoxAnswerDate.Text, TextBoxQuestionDate.Text, TextBoxCorrectionSheetDate.Text, Convert.ToInt32(TextBoxTimeSpan.Text), TextBoxMaterialUrl.Text, TextBoxConditionUrl.Text, TextBoxPassword.Text);

            // Code that save changes to the IttLetter PdfData
            int result = Bizz.CreateInDbReturnInt("IttLetterPdfData", Bizz.TempIttLetterPdfData);

            if (result > 0)
            {
                //Show Confirmation
                MessageBox.Show("Personlig del af Udbudsbrevet blev rettet", "Forbered Udbudsbrev", MessageBoxButton.OK, MessageBoxImage.Information);

                //Update IttLetter PdfData List
                Bizz.RefreshIttLetterPdfDataList();

                //Reset UcIttLetterPreparePersonal
                ComboBoxCaseId.SelectedIndex = 0;
                Bizz.TempProject = new Project();
                Bizz.TempBuilder = new Builder();
                Bizz.TempIttLetterPdfData = new IttLetterPdfData();

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
                foreach (IndexedProject temp in Bizz.IndexedActiveProjects)
                {
                    if (temp.Index == selectedIndex)
                    {
                        Bizz.TempProject = new Project(temp.Id, temp.CaseId, temp.Name, temp.Builder, temp.Status, temp.TenderForm, temp.EnterpriseForm, temp.Executive, temp.EnterpriseList, temp.Copy);
                        break;
                    }
                }
                TextBoxName.Text = Bizz.TempProject.Name;
                GetProjectSubEntrepeneurs();
                GetProjectIttLetterReceivers();
            }
            else
            {
                TextBoxName.Text = "";
                ProjectSubEntrepeneurs.Clear();
                ProjectEnterpriseList.Clear();
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
            foreach (IttLetterReceiver receiver in Bizz.IttLetterReceivers)
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

        private void GenerateComboBoxCaseIdItems()
        {
            ComboBoxCaseId.Items.Clear();
            foreach (IndexedProject temp in Bizz.IndexedActiveProjects)
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
            foreach (Address temp in Bizz.Addresses)
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

        /// <summary>
        /// Method, that finds an email address in list
        /// </summary>
        /// <returns>string</returns>
        private string GetContactEmail(int id)
        {
            string result = "";
            foreach (ContactInfo info in Bizz.ContactInfoList)
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
            Shipping = new IttLetterShipping(Bizz.TempProject, @"PDF_Documents\", Bizz.MacAdresss);
            try
            {
                int id = Bizz.CreateInDbReturnInt("IttLetterShipping", Shipping);
                Shipping.SetId(id);
                Shipping.PdfPath = "";
                Shipping.UpdateIttLetterShipping(Shipping);
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
            Bizz.RefreshIttLetterReceivers();

            foreach (IttLetterReceiver receiver in Bizz.IttLetterReceivers)
            {
                if (receiver.Project.Id == Bizz.TempProject.Id)
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
        /// Method, that generates List of ProjectSubEntrepeneurs
        /// </summary>
        private void GetProjectSubEntrepeneurs()
        {
            ProjectEnterpriseList.Clear();
            ProjectSubEntrepeneurs.Clear();
            foreach (Enterprise enterprise in Bizz.EnterpriseList)
            {
                if (enterprise.Project.Id == Bizz.TempProject.Id)
                {
                    ProjectEnterpriseList.Add(enterprise);
                    foreach (SubEntrepeneur sub in Bizz.SubEntrepeneurs)
                    {
                        if (sub.EnterpriseList.Id == enterprise.Id)
                        {
                            ProjectSubEntrepeneurs.Add(sub);
                        }
                    }
                }
            }
        }

        private SubEntrepeneur GetSubEntrepeneur(string entrepeneur)
        {
            SubEntrepeneur result = new SubEntrepeneur();
            foreach (SubEntrepeneur sub in ProjectSubEntrepeneurs)
            {
                if (sub.Entrepeneur.Id == entrepeneur)
                {
                    result = sub;
                }
            }
            return result;
        }

        #endregion

    }
}
