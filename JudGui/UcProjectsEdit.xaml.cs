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
    /// Interaction logic for UcProjectsEdit.xaml
    /// </summary>
    public partial class UcProjectsEdit : UserControl
    {
        #region Fields
        public Bizz CBZ;
        public UserControl UcMain;

        #endregion

        public UcProjectsEdit(Bizz cbz, UserControl ucRight)
        {
            InitializeComponent();
            this.CBZ = cbz;
            this.UcMain = ucRight;
            ComboBoxCaseId.ItemsSource = CBZ.ActiveProjects;
            ComboBoxBuilder.ItemsSource = CBZ.ActiveBuilders;
            ComboBoxTenderForm.ItemsSource = CBZ.IndexedTenderForms;
            ComboBoxProjectStatus.ItemsSource = CBZ.IndexedEnterpriseForms;
            ComboBoxExecutive.ItemsSource = CBZ.ActiveUsers;
        }

        #region Buttons
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            //Warning about lost changes before closing
            if (MessageBox.Show("Du er ved at lukke projektet. Alt, der ikke er gemt vil blive mistet!", "Luk Projekt", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
            {
                //Close right UserControl
                UcMain.Content = new UserControl();
                CBZ.UcMainActive = false;
            }
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            // Code that save changes to the project
            bool result = CBZ.UpdateInDb(CBZ.TempProject);

            if (result)
            {
                //Show Confirmation
                MessageBox.Show("Projektet blev rettet", "Ret Projekt", MessageBoxButton.OK, MessageBoxImage.Information);

                //Update Projects lists
                CBZ.RefreshList("Projects");
                CBZ.RefreshIndexedList("IndexedActiveProjects");
                CBZ.RefreshIndexedList("IndexedProjects");

                //Close right UserControl
                UcMain.Content = new UserControl();
                CBZ.UcMainActive = false;
            }
            else
            {
                //Show error
                MessageBox.Show("Databasen returnerede en fejl. Projektet blev ikke rettet. Prøv igen.", "Ret Projekt", MessageBoxButton.OK, MessageBoxImage.Information);
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
            TextBoxCaseName.Text = CBZ.TempProject.Name;
            ComboBoxBuilder.SelectedIndex = CBZ.TempProject.Builder.Id;
            ComboBoxProjectStatus.SelectedIndex = GetProjectStatusIndex(CBZ.TempProject.Status.Id);
            ComboBoxTenderForm.SelectedIndex = CBZ.TempProject.TenderForm.Id;
            ComboBoxEnterpriseForm.SelectedIndex = GetEnterPriseFormIndex(CBZ.TempProject.EnterpriseForm);
            ComboBoxExecutive.SelectedIndex = CBZ.TempProject.Executive.Id;
        }

        private void ComboBoxBuilder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CBZ.TempProject.Builder = new Builder((Builder)ComboBoxBuilder.SelectedItem);
        }

        private void ComboBoxProjectStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CBZ.TempProject.Status = new ProjectStatus((ProjectStatus)ComboBoxProjectStatus.SelectedItem);
        }

        private void ComboBoxTenderForm_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CBZ.TempProject.TenderForm = new TenderForm((TenderForm)ComboBoxTenderForm.SelectedItem);
        }

        private void ComboBoxEnterpriseForm_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CBZ.TempProject.EnterpriseForm = new EnterpriseForm((EnterpriseForm)ComboBoxEnterpriseForm.SelectedItem);
        }

        private void ComboBoxExecutive_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CBZ.TempProject.Executive = new User((User)ComboBoxExecutive.SelectedItem);
        }

        private void TextBoxCaseName_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempProject.Name = TextBoxCaseName.Text;
        }

        #endregion

        #region Methods

        private int GetEnterPriseFormIndex(EnterpriseForm enterpriseForm)
        {
            int result = 0;

            foreach (IndexedEnterpriseForm indexForm in ComboBoxExecutive.Items)
            {
                if (indexForm.Text == enterpriseForm.Text)
                {
                    result = indexForm.Index;
                }
            }

            return result;
        }

        private int GetProjectStatusIndex(int id)
        {
            int result = 0;

            foreach (IndexedProjectStatus indexProjectStatus in CBZ.IndexedProjectStatuses)
            {
                if (indexProjectStatus.Id == id)
                {
                    result = indexProjectStatus.Index;
                    break;
                }
            }

            return result;
        }

        #endregion

    }
}
