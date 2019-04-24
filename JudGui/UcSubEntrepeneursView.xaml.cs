using JudBizz;
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
    /// Interaction logic for UcSubEntrepeneursView.xaml
    /// </summary>
    public partial class UcSubEntrepeneursView : UserControl
    {
        #region Fields
        public Bizz CBZ;
        public UserControl UcMain;
        public List<int> EnterpriseIds = new List<int>();
        public List<Enterprise> IndexedEnterprises = new List<Enterprise>();
        public List<IndexedSubEntrepeneur> IndexedSubEntrepeneurs = new List<IndexedSubEntrepeneur>();
        public List<IndexedSubEntrepeneur> OpenIndexedSubEntrepeneurs = new List<IndexedSubEntrepeneur>();
        public List<IndexedSubEntrepeneur> ChosenIndexedSubEntrepeneurs = new List<IndexedSubEntrepeneur>();
        public List<IndexedSubEntrepeneur> YesReceivedChosenIndexedSubEntrepeneurs = new List<IndexedSubEntrepeneur>();

        #endregion

        #region Constructors
        public UcSubEntrepeneursView(Bizz cbz, UserControl ucMain)
        {
            InitializeComponent();
            this.CBZ = cbz;
            this.UcMain = ucMain;
            ComboBoxCaseId.ItemsSource = CBZ.IndexedActiveProjects;
        }

        #endregion

        #region Buttons
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            //Close right UserControl
            MessageBox.Show("Visning af Underentrepenører lukkes.", "Luk Underentrepenører", MessageBoxButton.OK, MessageBoxImage.Information);
            CBZ.UcMainEdited = false;
            UcMain.Content = new UserControl();
        }

        private void ButtonGeneratePdf_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBoxCaseId.SelectedIndex == -1)
            {
                MessageBox.Show("Du har ikke valgt en sag. Der kan ikke genereres en PDF.", "Fejl Generer PDF", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string path = "";
            PdfCreator pdfCreator = new PdfCreator(CBZ.StrConnection);
            if (RadioButtonShowAll.IsChecked.Value)
            {
                path = pdfCreator.GenerateSubEntrepeneursPdf(CBZ, IndexedSubEntrepeneurs);
            }
            if (RadioButtonShowOpen.IsChecked.Value)
            {
                path = pdfCreator.GenerateSubEntrepeneursPdf(CBZ, OpenIndexedSubEntrepeneurs);
            }
            if (RadioButtonShowChosen.IsChecked.Value)
            {
                path = pdfCreator.GenerateSubEntrepeneursPdf(CBZ, ChosenIndexedSubEntrepeneurs);
            }
            if (RadioButtonShowYesReceivedChosen.IsChecked.Value)
            {
                path = pdfCreator.GenerateSubEntrepeneursPdf(CBZ, YesReceivedChosenIndexedSubEntrepeneurs);
            }
            if (RadioButtonShowAgreement.IsChecked.Value)
            {
                path = pdfCreator.GenerateSubEntrepeneursPdfForAgreement(CBZ, ChosenIndexedSubEntrepeneurs);
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
                    CBZ.TempProject = new Project(temp);
                }
            }
            TextBoxCaseName.Text = CBZ.TempProject.Details.Name;
            IndexedEnterprises = GetIndexedEnterprises();
            IndexedSubEntrepeneurs = GetIndexedSubEntrepeneurs();
            RadioButtonShowAll.IsChecked = true;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }

        }

        private void RadioButtonShowAll_Checked(object sender, RoutedEventArgs e)
        {
            RadioButtonShowAll.IsChecked = true;
            RadioButtonShowOpen.IsChecked = false;
            RadioButtonShowChosen.IsChecked = false;
            RadioButtonShowYesReceivedChosen.IsChecked = false;
            RadioButtonShowAgreement.IsChecked = false;
            UpdateIndexedLists();

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }

        }

        private void RadioButtonShowOpen_Checked(object sender, RoutedEventArgs e)
        {
            RadioButtonShowAll.IsChecked = false;
            RadioButtonShowOpen.IsChecked = true;
            RadioButtonShowChosen.IsChecked = false;
            RadioButtonShowYesReceivedChosen.IsChecked = false;
            RadioButtonShowAgreement.IsChecked = false;
            UpdateIndexedLists();
            OpenIndexedSubEntrepeneurs.Clear();
            OpenIndexedSubEntrepeneurs = FilterOpen();

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }

        }

        private void RadioButtonShowChosen_Checked(object sender, RoutedEventArgs e)
        {
            RadioButtonShowAll.IsChecked = false;
            RadioButtonShowOpen.IsChecked = false;
            RadioButtonShowChosen.IsChecked = true;
            RadioButtonShowYesReceivedChosen.IsChecked = false;
            RadioButtonShowAgreement.IsChecked = false;
            UpdateIndexedLists();
            ChosenIndexedSubEntrepeneurs.Clear();
            ChosenIndexedSubEntrepeneurs = FilterChosen();

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }

        }

        private void RadioButtonShowYesReceivedChosen_Checked(object sender, RoutedEventArgs e)
        {
            RadioButtonShowAll.IsChecked = false;
            RadioButtonShowOpen.IsChecked = false;
            RadioButtonShowChosen.IsChecked = false;
            RadioButtonShowYesReceivedChosen.IsChecked = true;
            RadioButtonShowAgreement.IsChecked = false;
            UpdateIndexedLists();
            YesReceivedChosenIndexedSubEntrepeneurs.Clear();
            YesReceivedChosenIndexedSubEntrepeneurs = FilterYesReceivedChosen();

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }

        }

        private void RadioButtonShowAgreement_Checked(object sender, RoutedEventArgs e)
        {
            RadioButtonShowAll.IsChecked = false;
            RadioButtonShowOpen.IsChecked = false;
            RadioButtonShowChosen.IsChecked = false;
            RadioButtonShowYesReceivedChosen.IsChecked = false;
            RadioButtonShowAgreement.IsChecked = true;
            UpdateIndexedLists();
            ChosenIndexedSubEntrepeneurs.Clear();
            ChosenIndexedSubEntrepeneurs = FilterChosen();

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }

        }
        #endregion

        #region Methods
        /// <summary>
        /// Method, that returns list of chosen Subentrepeneurs from IndexedSubEntrepeneur
        /// </summary>
        /// <returns>List<IndexedSubEntrepeneur></returns>
        private List<IndexedSubEntrepeneur> FilterChosen()
        {
            List<IndexedSubEntrepeneur> result = new List<IndexedSubEntrepeneur>();
            foreach (int id in EnterpriseIds)
            {
                foreach (IndexedSubEntrepeneur entrepeneur in IndexedSubEntrepeneurs)
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
                foreach (IndexedSubEntrepeneur entrepeneur in IndexedSubEntrepeneurs)
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
                foreach (IndexedSubEntrepeneur entrepeneur in IndexedSubEntrepeneurs)
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

        private List<Enterprise> GetIndexedEnterprises()
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
        private List<IndexedSubEntrepeneur> GetIndexedSubEntrepeneurs()
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
        /// Method, that reloads IndexedEnterprises & IndexedSubEntrepeneurs
        /// </summary>
        private void UpdateIndexedLists()
        {
            IndexedEnterprises.Clear();
            IndexedEnterprises = GetIndexedEnterprises();
            IndexedSubEntrepeneurs.Clear();
            IndexedSubEntrepeneurs = GetIndexedSubEntrepeneurs();
        }

        #endregion

    }
}
