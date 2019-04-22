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

        #region Constructors
        public UcProjectsEdit(Bizz cbz, UserControl ucMain)
        {
            InitializeComponent();
            this.CBZ = cbz;
            this.UcMain = ucMain;
            CBZ.RefreshList("Projects");
            ComboBoxCaseId.ItemsSource = CBZ.ActiveProjects;
            CBZ.RefreshIndexedList("IndexedBuilders");
            ComboBoxBuilder.ItemsSource = CBZ.ActiveBuilders;
            CBZ.RefreshIndexedList("IndexedProjectStatuses");
            ComboBoxProjectStatus.ItemsSource = CBZ.IndexedProjectStatuses;
            CBZ.RefreshIndexedList("IndexedTenderForms");
            ComboBoxTenderForm.ItemsSource = CBZ.IndexedTenderForms;
            CBZ.RefreshIndexedList("IndexedEnterpriseForms");
            ComboBoxEnterpriseForm.ItemsSource = CBZ.IndexedEnterpriseForms;
            CBZ.RefreshIndexedList("IndexedUsers");
            ComboBoxExecutive.ItemsSource = CBZ.ActiveUsers;
        }

        #endregion

        #region Buttons
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            //Warning about lost changes before closing
            if (MessageBox.Show("Du er ved at lukke projektet. Alt, der ikke er gemt vil blive mistet!", "Luk Projekt", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
            {
                //Close right UserControl
                UcMain.Content = new UserControl();
                CBZ.UcMainEdited = false;
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
                CBZ.UcMainEdited = false;
            }
            else
            {
                //Show error
                MessageBox.Show("Databasen returnerede en fejl. Projektet blev ikke rettet. Prøv igen.", "Ret Projekt", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        #endregion

        #region Events
        private void ComboBoxBuilder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CBZ.TempProject.Builder = new Builder((Builder)ComboBoxBuilder.SelectedItem);


            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void ComboBoxCaseId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CBZ.TempProject = new Project((Project)ComboBoxCaseId.SelectedItem);

            TextBoxCaseName.Text = CBZ.TempProject.Name;
            ComboBoxBuilder.SelectedIndex = GetBuilderIndex();
            ComboBoxProjectStatus.SelectedIndex = GetProjectStatusIndex();
            ComboBoxTenderForm.SelectedIndex = GetTenderFormIndex();
            ComboBoxEnterpriseForm.SelectedIndex = GetEnterPriseFormIndex();
            ComboBoxExecutive.SelectedIndex = GetExecutiveIndex();

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }

        }

        private void ComboBoxEnterpriseForm_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CBZ.TempProject.EnterpriseForm = new EnterpriseForm((EnterpriseForm)ComboBoxEnterpriseForm.SelectedItem);

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void ComboBoxExecutive_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CBZ.TempProject.Executive = new User((User)ComboBoxExecutive.SelectedItem);

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void ComboBoxProjectStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CBZ.TempProject.Status = new ProjectStatus((ProjectStatus)ComboBoxProjectStatus.SelectedItem);

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void ComboBoxTenderForm_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CBZ.TempProject.TenderForm = new TenderForm((TenderForm)ComboBoxTenderForm.SelectedItem);

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxCaseName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CBZ.TempProject.Name != TextBoxCaseName.Text)
            {
                CBZ.TempProject.Name = TextBoxCaseName.Text;
            }

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Method, that retrieves the index of a Builder
        /// </summary>
        /// <returns>int</returns>
        private int GetBuilderIndex()
        {
            int result = 0;

            CBZ.RefreshIndexedList("IndexedBuilders");

            foreach (IndexedBuilder builder in CBZ.IndexedBuilders)
            {
                if (builder.Id == CBZ.TempProject.Builder.Id)
                {
                    result = builder.Index;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that retrieves the index of an EnterPrise Form
        /// </summary>
        /// <returns>int</returns>
        private int GetEnterPriseFormIndex()
        {
            int result = 0;

            CBZ.RefreshIndexedList("IndexedEnterpriseForms");

            foreach (IndexedEnterpriseForm form in CBZ.IndexedEnterpriseForms)
            {
                if (form.Id == CBZ.TempProject.Status.Id)
                {
                    result = form.Index;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that retrieves the index of an Executive
        /// </summary>
        /// <returns>int</returns>
        private int GetExecutiveIndex()
        {
            int result = 0;

            CBZ.RefreshIndexedList("IndexedUsers");

            foreach (IndexedUser user in CBZ.IndexedUsers)
            {
                if (user.Id == CBZ.TempProject.Executive.Id)
                {
                    result = user.Index;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that retrieves the index of a Project Status
        /// </summary>
        /// <returns>int</returns>
        private int GetProjectStatusIndex()
        {
            int result = 0;

            CBZ.RefreshIndexedList("IndexedProjectStatuses");

            foreach (IndexedProjectStatus status in CBZ.IndexedProjectStatuses)
            {
                if (status.Id == CBZ.TempProject.Status.Id)
                {
                    result = status.Index;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that retrieves the index of a Tender Form
        /// </summary>
        /// <returns>int</returns>
        private int GetTenderFormIndex()
        {
            int result = 0;

            CBZ.RefreshIndexedList("IndexedTenderForms");

            foreach (IndexedTenderForm form in CBZ.IndexedTenderForms)
            {
                if (form.Id == CBZ.TempProject.TenderForm.Id)
                {
                    result = form.Index;
                    break;
                }
            }

            return result;
        }

        #endregion

    }
}
