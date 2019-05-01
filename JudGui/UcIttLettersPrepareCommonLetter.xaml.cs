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
        public List<Contact> ProjectContacts = new List<Contact>();
        public List<Enterprise> ProjectEnterprises = new List<Enterprise>();
        public List<Paragraf> ProjectParagrafs = new List<Paragraf>();
        public List<IttLetterShipping> ProjectShippings = new List<IttLetterShipping>();
        public List<SubEntrepeneur> ProjectSubEntrepeneurs = new List<SubEntrepeneur>();
        public List<Receiver> Receivers = new List<Receiver>();

        #endregion

        #region Constructors
        public UcIttLettersPrepareCommonLetter(Bizz cbz, UserControl ucMain)
        {
            InitializeComponent();
            this.CBZ = cbz;
            this.UcMain = ucMain;
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
                CBZ.TempBullet = new Bullet(CBZ.TempParagraf, TextBoxNewBullet.Text);
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
                        MessageBox.Show("Databasen meldte en fejl. Linjen blev ikke tilføjet til afsnittet\n" + exception, "Udbudsbreve", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        MessageBox.Show("Databasen meldte en fejl. Linjen blev ikke tilføjet til afsnittet", "Udbudsbreve", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    RefreshList("Bullets");
                    GetIndexedBullets();
                    ListBoxBullets.ItemsSource = CBZ.IndexedBullets;
                    ListBoxBullets.SelectedIndex = 0;
                    TextBoxNewBullet.Text = "";
                }
            }
        }

        private void ButtonAddParagraf_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxNewBullet.Text != "")
            {
                int dbAnswer = 0;
                Exception exception = new Exception();
                this.CBZ.TempParagraf = new JudRepository.Paragraf(this.CBZ.TempProject, this.TextBoxNewParagraf.Text);
                try
                {
                    dbAnswer = CBZ.CreateInDb(CBZ.TempParagraf);
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
                        MessageBox.Show("Databasen meldte en fejl. Linjen blev ikke tilføjet til afsnittet\n" + exception, "Udbudsbreve", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        MessageBox.Show("Databasen meldte en fejl. Linjen blev ikke tilføjet til afsnittet", "Udbudsbreve", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    RefreshList("Paragrafs");
                    GetIndexedParagrafs();
                    ComboBoxParagrafs.ItemsSource = "";
                    ComboBoxParagrafs.ItemsSource = CBZ.IndexedParagrafs;
                    ComboBoxParagrafs.SelectedIndex = 0;
                    TextBoxNewParagraf.Text = "";
                }
            }
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                //Warning about lost changes before closing
                if (MessageBox.Show("Vil du lukke klargøring af Udbudsbrev? Alle ugemte data mistes.", "Udbudsbreve", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    CBZ.CloseUcMain(UcMain);
                }
            }
            else
            {
                CBZ.CloseUcMain(UcMain);
            }
        }

        private void ButtonPrepare_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckReceiversListExist();
                CheckParagrafsExist();
                CheckParagrafBulletsExist(CBZ.TempParagraf.Id);

                string commonPdfPath = GetCommonPdfPath();

                UpdateCommonPdfPathInShippingList(commonPdfPath);

                RefreshList("ShippingList");
                RefreshProjectShippings();
                ComboBoxCaseId.SelectedIndex = 0;
                ComboBoxParagrafs.ItemsSource = "";
                ComboBoxParagrafs.SelectedIndex = -1;
                ListBoxBullets.ItemsSource = "";
                ListBoxBullets.SelectedIndex = -1;

            }
            catch (ArgumentNullException ane)
            {
                switch (ane.ParamName)
                {
                    case "missingBullet":
                        MessageBox.Show("Mindst et afsnit mangler underpunkter. Fælles del af Udbudsbrev kan ikke genereres.", "Udbudsbreve", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                    case "pdf":
                        MessageBox.Show("Pdf'en med Fælles del af Udbudsbrev kunne ikke genereres.\n" + ane.ToString(), "Udbudsbreve", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                    case "shipping":
                        MessageBox.Show("Forsendelseslisten kunne ikke opdateres.\n" + ane.ToString(), "Udbudsbreve", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                    case "noParagrafs":
                        MessageBox.Show("Der er ingen afsnit. Fælles del af Udbudsbrev kan ikke genereres.", "Udbudsbreve", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                    case "noReceivers":
                        MessageBox.Show("Der er ingen modtagerliste. Fælles del af Udbudsbrev kan ikke genereres.", "Udbudsbreve", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                }
            }
        }

        #endregion

        #region Events
        private void ComboBoxCaseId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CBZ.TempProject = new Project((IndexedProject)ComboBoxCaseId.SelectedItem);

            TextBoxName.Text = CBZ.TempProject.Details.Name;
            GetProjectDetails();
            SetCheckBoxReceiversListExist();
            ComboBoxParagrafs.ItemsSource = CBZ.IndexedParagrafs;
            ComboBoxParagrafs.SelectedIndex = 0;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }

        }

        private void ComboBoxParagrafs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = ComboBoxParagrafs.SelectedIndex;
            if (CBZ.IndexedParagrafs.Count < 1 || CBZ.IndexedParagrafs == null)
            {
                GetIndexedParagrafs();
            }
            foreach (IndexedParagraf temp in CBZ.IndexedParagrafs)
            {
                if (temp.Index == selectedIndex)
                {
                    this.CBZ.TempParagraf = new Paragraf(temp.Id, temp.Project, temp.Text);
                    break;
                }
            }
            RefreshList("Bullets");
            CBZ.IndexedBullets.Clear();
            GetIndexedBullets();
            ListBoxBullets.ItemsSource = CBZ.IndexedBullets;
            ListBoxBullets.SelectedIndex = 0;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void ListBoxBullets_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Method, that checks, wether a Contact has already been added to the IndexedBullets list
        /// </summary>
        /// <param name="bulletId"></param>
        /// <returns></returns>
        private bool CheckBulletExist(int bulletId)
        {
            bool result = false;

            foreach (Bullet bullet in CBZ.Bullets)
            {
                if (bullet.Id == bulletId)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that checks, wether a Contact has already been added to the ProjectContacts list
        /// </summary>
        /// <param name="contactId">int</param>
        /// <returns>bool</returns>
        private bool CheckContactExist(int contactId)
        {
            bool result = false;

            if (ProjectContacts.Count > 0)
            {
                foreach (Contact tempContact in ProjectContacts)
                {
                    if (tempContact.Id == contactId)
                    {
                        result = true;
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that checks, whether an Entrepeneur has already been added to the Entrepeneurs list
        /// </summary>
        /// <param name="entrepeneur">LegalEntity</param>
        /// <returns>bool</returns>
        private bool CheckEntrepeneurExist(Entrepeneur entrepeneur, List<Entrepeneur> list)
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
        /// Method, that checks, whether an Entrepeneur has already been added to the IndexedEntrepeneurs list
        /// </summary>
        /// <param name="entrepeneur">Entrepeneur</param>
        /// <returns>bool</returns>
        private bool CheckIndexedEntrepeneurÉxist(int entrepeneurId, List<IndexedEntrepeneur> list)
        {
            bool result = false;

            foreach (IndexedEntrepeneur indexedEntrepeneur in list)
            {
                if (indexedEntrepeneur.Id == entrepeneurId)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that checks, whether Bullets for a Paragraf exist
        /// </summary>
        /// <param name="paragraphId"></param>
        /// <returns></returns>
        private bool CheckParagrafBulletsExist(int paragraphId)
        {
            bool result = false;

            foreach (Bullet bullet in CBZ.Bullets)
            {
                if (bullet.Paragraf.Id == paragraphId)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method that checks wether a Paragraf has already been added to the Paragrafs list
        /// </summary>
        private void CheckParagrafsExist()
        {
            bool paragraphsExist = false;

            foreach (Paragraf temp in this.CBZ.Paragrafs)
            {
                if (temp.Project.Id == this.CBZ.TempProject.Id)
                {
                    paragraphsExist = true;
                    break;
                }

            }

            if (!paragraphsExist)
            {
                throw new ArgumentNullException("noParagrafs");
            }
        }

        /// <summary>
        /// Method, that checks, wether the Receivers list Exist
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
                result = PdfCreator.GenerateIttLetterCommonPdf(CBZ);
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
            RefreshList("Bullets");
            CBZ.IndexedBullets.Clear();
            CBZ.IndexedBullets.Add(new IndexedBullet(0, CBZ.Bullets[0]));

            int i = 1;

            foreach (Bullet temp in CBZ.Bullets)
            {
                if (temp.Paragraf.Id == CBZ.TempParagraf.Id)
                {
                    IndexedBullet other = new IndexedBullet(i, temp);
                    CBZ.IndexedBullets.Add(other);
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

            RefreshList("Entrepeneurs");
            CBZ.IndexedEntrepeneurs.Clear();
            CBZ.IndexedEntrepeneurs.Add(temp);

            foreach (Enterprise enterprise in CBZ.Enterprises)
            {
                if (enterprise.Project.Id == CBZ.TempProject.Id)
                {
                    foreach (SubEntrepeneur subEntrepeneur in CBZ.SubEntrepeneurs)
                    {
                        if (subEntrepeneur.Enterprise.Id == enterprise.Id)
                        {
                            tempResult.Add(subEntrepeneur.Entrepeneur);
                        }
                    }
                }
            }

            int i = 1;

            foreach (Entrepeneur sub in tempResult)
            {
                bool entrepeneurExist = CheckIndexedEntrepeneurÉxist(sub.Id, CBZ.IndexedEntrepeneurs);

                if (!entrepeneurExist)
                {
                    CBZ.IndexedEntrepeneurs.Add(new IndexedEntrepeneur(i, sub));
                    i++;
                }
            }
        }

        /// <summary>
        /// Method that creates an Indexed Enterprise List
        /// </summary>
        private void GetIndexedParagrafs()
        {
            RefreshList("Paragrafs");
            CBZ.IndexedParagrafs.Clear();
            CBZ.IndexedParagrafs.Add(new IndexedParagraf(0, CBZ.Paragrafs[0]));

            int i = 1;

            foreach (Paragraf temp in this.CBZ.Paragrafs)
            {
                if (temp.Project.Id == this.CBZ.TempProject.Id)
                {
                    IndexedParagraf paragraph = new IndexedParagraf(i, temp);
                    CBZ.IndexedParagrafs.Add(paragraph);
                    i++;
                }
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
            CBZ.IndexedEnterprises.Clear();
            Receivers.Clear();
            ProjectShippings.Clear();
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
                        foreach (IttLetterShipping shipping in CBZ.IttLetterShippings)
                        {
                            if (shipping.SubEntrepeneur.Id == sub.Id)
                            {
                                Receivers.Add(shipping.Receiver);
                                ProjectShippings.Add(shipping);
                            }
                        }
                    }
                    ProjectEnterprises.Add(enterprise);
                }
            }
            GetIndexedParagrafs();
            GetIndexedEntrepeneurs();
        }

        /// <summary>
        /// Method, that refreshes content of a list in CBZ
        /// </summary>
        /// <param name="list">string</param>
        private void RefreshList(string list) =>  CBZ.RefreshList(list);

        /// <summary>
        /// Method, that adds a Shipping to Shippings list
        /// </summary>
        private void RefreshProjectContacts()
        {
            ProjectContacts.Clear();

            foreach (SubEntrepeneur subEntrepeneur in ProjectSubEntrepeneurs)
            {
                bool contactExist = CheckContactExist(subEntrepeneur.Contact.Id);

                if (!contactExist)
                {
                    ProjectContacts.Add(subEntrepeneur.Contact);
                }
            }
        }

        /// <summary>
        /// Method, that adds a Shipping to ProjectEnterprises list, based on Receiver Id
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
        /// Method, that adds a Shipping to Shippings list, based on Receiver Id
        /// </summary>
        private void RefreshProjectParagrafs()
        {
            ProjectParagrafs.Clear();

            foreach (Paragraf paragraph in CBZ.Paragrafs)
            {
                if (paragraph.Project.Id == CBZ.TempProject.Id)
                {
                    ProjectParagrafs.Add(paragraph);
                }
            }
        }

        /// <summary>
        /// Method, that refreshes ProjectShippings list
        /// </summary>
        private void RefreshProjectShippings()
        {
            ProjectShippings.Clear();

            foreach (IttLetterShipping shipping in CBZ.IttLetterShippings)
            {
                if (shipping.SubEntrepeneur.Enterprise.Project.Id == CBZ.TempProject.Id)
                {
                    ProjectShippings.Add(shipping);
                }
            }
        }

        /// <summary>
        /// Method, that refreshes ProjectShippings list
        /// </summary>
        private void RefreshProjectSubEntrepeneurs()
        {
            ProjectSubEntrepeneurs.Clear();
            foreach (Enterprise enterprise in ProjectEnterprises)
            {
                foreach (SubEntrepeneur subEntrepeneur in CBZ.SubEntrepeneurs)
                {
                    if (subEntrepeneur.Enterprise.Id == enterprise.Id)
                    {
                        ProjectSubEntrepeneurs.Add(subEntrepeneur);
                    }
                }
            }
        }

        /// <summary>
        /// Method, that determines, whether CheckBoxReceiversList should be checked
        /// </summary>
        private void SetCheckBoxReceiversListExist()
        {
            if (Receivers.Count >= 1)
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
                foreach (IttLetterShipping shipping in ProjectShippings)
                {
                    CBZ.TempIttLetterShipping = shipping;
                    CBZ.TempIttLetterShipping.CommonPdfPath = commonPdfPath;
                    CBZ.UpdateInDb(CBZ.TempIttLetterShipping);
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
