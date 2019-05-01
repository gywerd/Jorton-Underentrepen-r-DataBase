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
            CBZ.RefreshIndexedList("Builders");
            ComboBoxBuilder.ItemsSource = CBZ.ActiveBuilders;
            CBZ.RefreshIndexedList("ProjectStatuses");
            ComboBoxProjectStatus.ItemsSource = CBZ.IndexedProjectStatuses;
            CBZ.RefreshIndexedList("TenderForms");
            ComboBoxTenderForm.ItemsSource = CBZ.IndexedTenderForms;
            CBZ.RefreshIndexedList("EnterpriseForms");
            ComboBoxEnterpriseForm.ItemsSource = CBZ.IndexedEnterpriseForms;
            CBZ.RefreshIndexedList("Users");
            ComboBoxExecutive.ItemsSource = CBZ.ActiveUsers;
        }

        #endregion

        #region Buttons
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                //Warning about lost changes before closing
                if (MessageBox.Show("Du er ved at lukke projektet. Alle ugemte data mistes!", "Projekter", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    CBZ.CloseUcMain(UcMain);
                }
            }
            else
            {
                CBZ.CloseUcMain(UcMain);
            }
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            // Code that save changes to the project
            bool result = CBZ.UpdateInDb(CBZ.TempProject);

            if (result)
            {
                //Show Confirmation
                MessageBox.Show("Projektet blev rettet", "Projekter", MessageBoxButton.OK, MessageBoxImage.Information);

                //Update Projects lists
                CBZ.RefreshList("Projects");
                CBZ.RefreshIndexedList("Projects");

                //Close right UserControl
                UcMain.Content = new UserControl();
                CBZ.UcMainEdited = false;
            }
            else
            {
                //Show error
                MessageBox.Show("Databasen returnerede en fejl. Projektet blev ikke rettet. Prøv igen.", "Projekter", MessageBoxButton.OK, MessageBoxImage.Information);
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

            TextBoxCaseName.Text = CBZ.TempProject.Details.Name;
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
            if (CBZ.TempProject.Details.Name != TextBoxCaseName.Text)
            {
                CBZ.TempProject.Details.Name = TextBoxCaseName.Text;

                //Set CBZ.UcMainEdited
                if (!CBZ.UcMainEdited)
                {
                    CBZ.UcMainEdited = true;
                }
            }

        }

        private void TextBoxDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CBZ.TempProject.Details.Description != TextBoxDescription.Text)
            {
                CBZ.TempProject.Details.Description = TextBoxDescription.Text;

                //Set CBZ.UcMainEdited
                if (!CBZ.UcMainEdited)
                {
                    CBZ.UcMainEdited = true;
                }
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

            CBZ.RefreshIndexedList("Builders");

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

            CBZ.RefreshIndexedList("EnterpriseForms");

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

            CBZ.RefreshIndexedList("Users");

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

            CBZ.RefreshIndexedList("ProjectStatuses");

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

            CBZ.RefreshIndexedList("TenderForms");

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
