using ClassBizz;
using JudRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
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
    /// Interaction logic for UcViewSubEntrepeneurs.xaml
    /// </summary>
    public partial class UcViewSubEntrepeneurs : UserControl
    {
        #region Fields
        public Bizz CBZ;
        public UserControl UcRight;
        public List<int> EnterpriseIds = new List<int>();
        public List<Enterprise> IndexableEnterprises = new List<Enterprise>();
        public List<IndexedSubEntrepeneur> IndexableSubEntrepeneurs = new List<IndexedSubEntrepeneur>();
        public List<IndexedSubEntrepeneur> OpenIndexableSubEntrepeneurs = new List<IndexedSubEntrepeneur>();
        public List<IndexedSubEntrepeneur> ChosenIndexableSubEntrepeneurs = new List<IndexedSubEntrepeneur>();
        public List<IndexedSubEntrepeneur> YesReceivedChosenIndexableSubEntrepeneurs = new List<IndexedSubEntrepeneur>();

        #endregion

        #region Constructors
        public UcViewSubEntrepeneurs(Bizz cbz, UserControl ucRight)
        {
            InitializeComponent();
            this.CBZ = cbz;
            this.UcRight = ucRight;
            ComboBoxCaseId.ItemsSource = CBZ.IndexedActiveProjects;
        }

        #endregion

        #region Buttons
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            //Close right UserControl
            MessageBox.Show("Visning af Underentrepenører lukkes.", "Luk Underentrepenører", MessageBoxButton.OK, MessageBoxImage.Information);
            CBZ.UcRightActive = false;
            UcRight.Content = new UserControl();
        }

        private void ButtonGeneratePdf_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBoxCaseId.SelectedIndex == -1)
            {
                MessageBox.Show("Du har ikke valgt en sag. Der kan ikke genereres en PDF.", "Fejl Generer PDF", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string path = "";
            PdfCreator pdfCreator = new PdfCreator(GetStrConnection());
            if (RadioButtonShowAll.IsChecked.Value)
            {
                path = pdfCreator.GenerateSubEntrepeneursPdf(CBZ, IndexableEnterprises, IndexableSubEntrepeneurs, CBZ.Users);
            }
            if (RadioButtonShowOpen.IsChecked.Value)
            {
                path = pdfCreator.GenerateSubEntrepeneursPdf(CBZ, IndexableEnterprises, OpenIndexableSubEntrepeneurs, CBZ.Users);
            }
            if (RadioButtonShowChosen.IsChecked.Value)
            {
                path = pdfCreator.GenerateSubEntrepeneursPdf(CBZ, IndexableEnterprises, ChosenIndexableSubEntrepeneurs, CBZ.Users);
            }
            if (RadioButtonShowYesReceivedChosen.IsChecked.Value)
            {
                path = pdfCreator.GenerateSubEntrepeneursPdf(CBZ, IndexableEnterprises, YesReceivedChosenIndexableSubEntrepeneurs, CBZ.Users);
            }
            if (RadioButtonShowAgreement.IsChecked.Value)
            {
                path = pdfCreator.GenerateSubEntrepeneursPdfForAgreement(CBZ, IndexableEnterprises, ChosenIndexableSubEntrepeneurs, CBZ.Users);
            }
            Process.Start(path);
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
                    CBZ.TempProject = new Project(temp.Id, temp.CaseId, temp.Name, temp.Builder, temp.Status, temp.TenderForm, temp.EnterpriseForm, temp.Executive, temp.EnterprisesList, temp.Copy);
                }
            }
            TextBoxCaseName.Text = CBZ.TempProject.Name;
            IndexableEnterprises = GetIndexableEnterprises();
            IndexableSubEntrepeneurs = GetIndexableSubEntrepeneurs();
            RadioButtonShowAll.IsChecked = true;
        }

        private void RadioButtonShowAll_Checked(object sender, RoutedEventArgs e)
        {
            RadioButtonShowAll.IsChecked = true;
            RadioButtonShowOpen.IsChecked = false;
            RadioButtonShowChosen.IsChecked = false;
            RadioButtonShowYesReceivedChosen.IsChecked = false;
            RadioButtonShowAgreement.IsChecked = false;
            UpdateIndexableLists();
        }

        private void RadioButtonShowOpen_Checked(object sender, RoutedEventArgs e)
        {
            RadioButtonShowAll.IsChecked = false;
            RadioButtonShowOpen.IsChecked = true;
            RadioButtonShowChosen.IsChecked = false;
            RadioButtonShowYesReceivedChosen.IsChecked = false;
            RadioButtonShowAgreement.IsChecked = false;
            UpdateIndexableLists();
            OpenIndexableSubEntrepeneurs.Clear();
            OpenIndexableSubEntrepeneurs = FilterOpen();
        }

        private void RadioButtonShowChosen_Checked(object sender, RoutedEventArgs e)
        {
            RadioButtonShowAll.IsChecked = false;
            RadioButtonShowOpen.IsChecked = false;
            RadioButtonShowChosen.IsChecked = true;
            RadioButtonShowYesReceivedChosen.IsChecked = false;
            RadioButtonShowAgreement.IsChecked = false;
            UpdateIndexableLists();
            ChosenIndexableSubEntrepeneurs.Clear();
            ChosenIndexableSubEntrepeneurs = FilterChosen();
        }

        private void RadioButtonShowYesReceivedChosen_Checked(object sender, RoutedEventArgs e)
        {
            RadioButtonShowAll.IsChecked = false;
            RadioButtonShowOpen.IsChecked = false;
            RadioButtonShowChosen.IsChecked = false;
            RadioButtonShowYesReceivedChosen.IsChecked = true;
            RadioButtonShowAgreement.IsChecked = false;
            UpdateIndexableLists();
            YesReceivedChosenIndexableSubEntrepeneurs.Clear();
            YesReceivedChosenIndexableSubEntrepeneurs = FilterYesReceivedChosen();
        }

        private void RadioButtonShowAgreement_Checked(object sender, RoutedEventArgs e)
        {
            RadioButtonShowAll.IsChecked = false;
            RadioButtonShowOpen.IsChecked = false;
            RadioButtonShowChosen.IsChecked = false;
            RadioButtonShowYesReceivedChosen.IsChecked = false;
            RadioButtonShowAgreement.IsChecked = true;
            UpdateIndexableLists();
            ChosenIndexableSubEntrepeneurs.Clear();
            ChosenIndexableSubEntrepeneurs = FilterChosen();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Method, that returns list of chosen Subentrepeneurs from IndexableSubEntrepeneur
        /// </summary>
        /// <returns>List<IndexableSubEntrepeneur></returns>
        private List<IndexedSubEntrepeneur> FilterChosen()
        {
            List<IndexedSubEntrepeneur> result = new List<IndexedSubEntrepeneur>();
            foreach (int id in EnterpriseIds)
            {
                foreach (IndexedSubEntrepeneur entrepeneur in IndexableSubEntrepeneurs)
                {
                    if (entrepeneur.Enterprise.Id == id)
                    {
                        foreach (Offer offer in CBZ.Offers)
                        {
                            if (entrepeneur.Offer.Id == offer.Id && offer.Chosen)
                            {
                                result.Add(entrepeneur);
                            }

                        }
                    }
                }
            }
            return result;
        }

        private List<IndexedSubEntrepeneur> FilterOpen()
        {
            List<IndexedSubEntrepeneur> result = new List<IndexedSubEntrepeneur>();
            foreach (int id in EnterpriseIds)
            {
                foreach (IndexedSubEntrepeneur entrepeneur in IndexableSubEntrepeneurs)
                {
                    if (entrepeneur.Active)
                    {
                        result.Add(entrepeneur);
                    }
                }
            }
            return result;
        }

        private List<IndexedSubEntrepeneur> FilterYesReceivedChosen()
        {
            List<IndexedSubEntrepeneur> result = new List<IndexedSubEntrepeneur>();
            foreach (int id in EnterpriseIds)
            {
                foreach (IndexedSubEntrepeneur entrepeneur in IndexableSubEntrepeneurs)
                {
                    if (entrepeneur.Enterprise.Id == id)
                    {
                        foreach (Request request in CBZ.Requests)
                        {
                            if (entrepeneur.Request.Id == request.Id && request.Status.Id == 2)
                            {
                                foreach (Offer offer in CBZ.Offers)
                                {
                                    if (entrepeneur.Offer.Id == offer.Id && offer.Received && offer.Chosen)
                                    {
                                        result.Add(entrepeneur);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        private List<Enterprise> GetIndexableEnterprises()
        {
            List<Enterprise> result = new List<Enterprise>();
            EnterpriseIds.Clear();
            int i = 0;
            foreach (Enterprise enterprise in CBZ.Enterprises)
            {
                if (enterprise.Project.Id == CBZ.TempProject.Id)
                {
                    IndexedEnterprise temp = new IndexedEnterprise(i, enterprise);
                    result.Add(temp);
                    EnterpriseIds.Add(temp.Id);
                }
                i++;
            }
            return result;
        }

        /// <summary>
        /// Method, that load add an index to S
        /// </summary>
        /// <returns></returns>
        private List<IndexedSubEntrepeneur> GetIndexableSubEntrepeneurs()
        {
            List<IndexedSubEntrepeneur> result = new List<IndexedSubEntrepeneur>();
            int i = 0;
            foreach (SubEntrepeneur entrepeneur in CBZ.SubEntrepeneurs)
            {
                foreach (int id in EnterpriseIds)
                {
                    if (entrepeneur.Enterprise.Id == id)
                    {
                        IndexedSubEntrepeneur temp = new IndexedSubEntrepeneur(i, entrepeneur);
                        result.Add(temp);
                    }
                    i++;
                }
            }
            return result;
        }

        /// <summary>
        /// Method, that retrieves the strConnection
        /// </summary>
        /// <returns></returns>
        private string GetStrConnection()
        {
            return CBZ.GetStrConnection();
        }

        /// <summary>
        /// Method, that reloads IndexableEnterprises & IndexableSubEntrepeneurs
        /// </summary>
        private void UpdateIndexableLists()
        {
            IndexableEnterprises.Clear();
            IndexableEnterprises = GetIndexableEnterprises();
            IndexableSubEntrepeneurs.Clear();
            IndexableSubEntrepeneurs = GetIndexableSubEntrepeneurs();
        }

        #endregion

    }
}
