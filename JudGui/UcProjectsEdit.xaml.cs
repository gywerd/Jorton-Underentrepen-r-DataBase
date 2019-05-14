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
            CBZ.RefreshIndexedList("Projects"); //Refreshes Indexed Projects list and all dependent lists
            ComboBoxCaseId.ItemsSource = CBZ.IndexedActiveProjects;
            ComboBoxBuilder.ItemsSource = CBZ.IndexedActiveBuilders;
            ComboBoxTenderForm.ItemsSource = CBZ.IndexedTenderForms;
            ComboBoxEnterpriseForm.ItemsSource = CBZ.IndexedEnterpriseForms;
            ComboBoxExecutive.ItemsSource = CBZ.IndexedActiveUsers;
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
                CBZ.RefreshIndexedList("Projects"); //Refreshes Indexed Projects list and all dependent lists

                //Reset form
                ComboBoxCaseId.SelectedIndex = -1;
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
            if (ComboBoxEnterpriseForm.SelectedIndex >= 0)
            {
                CBZ.TempProject.Builder = new Builder((Builder)ComboBoxBuilder.SelectedItem);


                //Set CBZ.UcMainEdited
                if (!CBZ.UcMainEdited)
                {
                    CBZ.UcMainEdited = true;
                }
            }
            else
            {
                CBZ.TempProject.Builder = new Builder();
            }
        }

        private void ComboBoxCaseId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxCaseId.SelectedIndex >= 0)
            {
                CBZ.TempProject = new Project((Project)ComboBoxCaseId.SelectedItem);

                TextBoxCaseName.Text = CBZ.TempProject.Details.Name;
                ComboBoxBuilder.SelectedIndex = GetBuilderIndex();
                ComboBoxTenderForm.SelectedIndex = GetTenderFormIndex();
                ComboBoxEnterpriseForm.SelectedIndex = GetEnterPriseFormIndex();
                ComboBoxExecutive.SelectedIndex = GetExecutiveIndex();
                TextBoxDescription.Text = CBZ.TempProject.Details.Description;
                TextBoxPeriod.Text = CBZ.TempProject.Details.Period;
                TextBoxAnswerDate.Text = CBZ.TempProject.Details.AnswerDate;

            }
            else
            {
                ResetForm();
            }

        }

        private void ComboBoxEnterpriseForm_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxEnterpriseForm.SelectedIndex >= 0)
            {
                CBZ.TempProject.EnterpriseForm = new EnterpriseForm((EnterpriseForm)ComboBoxEnterpriseForm.SelectedItem);

                //Set CBZ.UcMainEdited
                if (!CBZ.UcMainEdited)
                {
                    CBZ.UcMainEdited = true;
                }
            }
            else
            {
                CBZ.TempProject.EnterpriseForm = new EnterpriseForm();
            }
        }

        private void ComboBoxExecutive_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxEnterpriseForm.SelectedIndex >= 0)
            {
                CBZ.TempProject.Executive = new User((User)ComboBoxExecutive.SelectedItem);

                //Set CBZ.UcMainEdited
                if (!CBZ.UcMainEdited)
                {
                    CBZ.UcMainEdited = true;
                }
            }
            else
            {
                CBZ.TempProject.Executive = new User();
            }
        }

        private void ComboBoxTenderForm_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxEnterpriseForm.SelectedIndex >= 0)
            {
                CBZ.TempProject.TenderForm = new TenderForm((TenderForm)ComboBoxTenderForm.SelectedItem);

                //Set CBZ.UcMainEdited
                if (!CBZ.UcMainEdited)
                {
                    CBZ.UcMainEdited = true;
                }
            }
            else
            {
                CBZ.TempProject.TenderForm = new TenderForm();
            }
        }

        private void TextBoxAnswerDate_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CBZ.TempProject.Details.AnswerDate != TextBoxDescription.Text)
            {
                CBZ.TempProject.Details.AnswerDate = TextBoxAnswerDate.Text;

                //Set CBZ.UcMainEdited
                if (!CBZ.UcMainEdited)
                {
                    CBZ.UcMainEdited = true;
                }
            }
        }

        private void TextBoxCaseName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBoxCaseName.Text.Count() >= 1 && CBZ.TempProject.Details.Name != TextBoxCaseName.Text)
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
            if (TextBoxDescription.Text.Count() >= 1 && CBZ.TempProject.Details.Description != TextBoxDescription.Text)
            {
                CBZ.TempProject.Details.Description = TextBoxDescription.Text;

                //Set CBZ.UcMainEdited
                if (!CBZ.UcMainEdited)
                {
                    CBZ.UcMainEdited = true;
                }
            }
        }

        private void TextBoxPeriod_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CBZ.TempProject.Details.Period != TextBoxPeriod.Text)
            {
                CBZ.TempProject.Details.Period = TextBoxPeriod.Text;

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

            foreach (IndexedBuilder builder in CBZ.IndexedActiveBuilders)
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

            foreach (IndexedUser user in CBZ.IndexedActiveUsers)
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

        /// <summary>
        /// Method, that resets ComboBoxes - except ComboBoxCaseId - and TextBoces
        /// </summary>
        private void ResetForm()
        {
            CBZ.TempProject = new Project();

            TextBoxCaseName.Text = "";
            ComboBoxBuilder.SelectedIndex = -1;
            ComboBoxTenderForm.SelectedIndex = -1;
            ComboBoxEnterpriseForm.SelectedIndex = -1;
            ComboBoxExecutive.SelectedIndex = -1;
            TextBoxDescription.Text = "";
            TextBoxPeriod.Text = "";
            TextBoxAnswerDate.Text = "";

            //Reset CBZ.UcMainEdited
            if (CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = false;
            }
        }

        #endregion

    }
}
