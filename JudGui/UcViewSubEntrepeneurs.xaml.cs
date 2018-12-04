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
    /// Interaction logic for UcViewSubEntrepeneurs.xaml
    /// </summary>
    public partial class UcViewSubEntrepeneurs : UserControl
    {
        #region Fields
        public Bizz Bizz;
        public UserControl UcRight;
        public List<int> EnterpriseIds = new List<int>();
        public List<Enterprise> IndexableEnterpriseList = new List<Enterprise>();
        public List<IndexedSubEntrepeneur> IndexableSubEntrepeneurs = new List<IndexedSubEntrepeneur>();
        public List<IndexedSubEntrepeneur> OpenIndexableSubEntrepeneurs = new List<IndexedSubEntrepeneur>();
        public List<IndexedSubEntrepeneur> ChosenIndexableSubEntrepeneurs = new List<IndexedSubEntrepeneur>();
        public List<IndexedSubEntrepeneur> YesReceivedChosenIndexableSubEntrepeneurs = new List<IndexedSubEntrepeneur>();

        #endregion

        #region Constructors
        public UcViewSubEntrepeneurs(Bizz bizz, UserControl ucRight)
        {
            InitializeComponent();
            this.Bizz = bizz;
            this.UcRight = ucRight;
            ComboBoxCaseId.ItemsSource = Bizz.IndexedActiveProjects;
        }

        #endregion

        #region Buttons
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            //Close right UserControl
            MessageBox.Show("Visning af Underentrepenører lukkes.", "Luk Underentrepenører", MessageBoxButton.OK, MessageBoxImage.Information);
            Bizz.UcRightActive = false;
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
            PdfCreator pdfCreator = new PdfCreator(Bizz.MEFW.ObtainStrConnection());
            if (RadioButtonShowAll.IsChecked.Value)
            {
                path = pdfCreator.GenerateSubEntrepeneursPdf(Bizz, IndexableEnterpriseList, IndexableSubEntrepeneurs, Bizz.Users);
            }
            if (RadioButtonShowOpen.IsChecked.Value)
            {
                path = pdfCreator.GenerateSubEntrepeneursPdf(Bizz, IndexableEnterpriseList, OpenIndexableSubEntrepeneurs, Bizz.Users);
            }
            if (RadioButtonShowChosen.IsChecked.Value)
            {
                path = pdfCreator.GenerateSubEntrepeneursPdf(Bizz, IndexableEnterpriseList, ChosenIndexableSubEntrepeneurs, Bizz.Users);
            }
            if (RadioButtonShowYesReceivedChosen.IsChecked.Value)
            {
                path = pdfCreator.GenerateSubEntrepeneursPdf(Bizz, IndexableEnterpriseList, YesReceivedChosenIndexableSubEntrepeneurs, Bizz.Users);
            }
            if (RadioButtonShowAgreement.IsChecked.Value)
            {
                path = pdfCreator.GenerateSubEntrepeneursPdfForAgreement(Bizz, IndexableEnterpriseList, ChosenIndexableSubEntrepeneurs, Bizz.Users);
            }
            Process.Start(path);
        }

        #endregion

        #region Events
        private void ComboBoxCaseId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = ComboBoxCaseId.SelectedIndex;
            foreach (IndexedProject temp in Bizz.IndexedActiveProjects)
            {
                if (temp.Index == selectedIndex)
                {
                    Bizz.TempProject = new Project(temp.Id, temp.CaseId, temp.Name, temp.Builder, temp.Status, temp.TenderForm, temp.EnterpriseForm, temp.Executive, temp.EnterpriseList, temp.Copy);
                }
            }
            TextBoxCaseName.Text = Bizz.TempProject.Name;
            IndexableEnterpriseList = GetIndexableEnterpriseList();
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
                    if (entrepeneur.EnterpriseList.Id == id)
                    {
                        foreach (Offer offer in Bizz.Offers)
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
                    if (entrepeneur.EnterpriseList.Id == id)
                    {
                        foreach (Request request in Bizz.Requests)
                        {
                            if (entrepeneur.Request.Id == request.Id && request.Status.Id == 2)
                            {
                                foreach (Offer offer in Bizz.Offers)
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

        private List<Enterprise> GetIndexableEnterpriseList()
        {
            List<Enterprise> result = new List<Enterprise>();
            EnterpriseIds.Clear();
            int i = 0;
            foreach (Enterprise enterprise in Bizz.EnterpriseList)
            {
                if (enterprise.Project.Id == Bizz.TempProject.Id)
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
            foreach (SubEntrepeneur entrepeneur in Bizz.SubEntrepeneurs)
            {
                foreach (int id in EnterpriseIds)
                {
                    if (entrepeneur.EnterpriseList.Id == id)
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
        /// Method, that reloads IndexableEnterpriseList & IndexableSubEntrepeneurs
        /// </summary>
        private void UpdateIndexableLists()
        {
            IndexableEnterpriseList.Clear();
            IndexableEnterpriseList = GetIndexableEnterpriseList();
            IndexableSubEntrepeneurs.Clear();
            IndexableSubEntrepeneurs = GetIndexableSubEntrepeneurs();
        }

        #endregion

    }
}
