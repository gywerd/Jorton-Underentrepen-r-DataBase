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
    /// Interaction logic for UcProjectCreate.xaml
    /// </summary>
    public partial class UcProjectCreate : UserControl
    {
        #region Fields
        public Bizz CBZ;
        public UserControl UcMain;

        bool newCaseIdCorrect = false;
        #endregion

        #region Constructors
        public UcProjectCreate(Bizz cbz, UserControl ucMain)
        {
            InitializeComponent();
            this.CBZ = cbz;
            this.UcMain = ucMain;
            GenerateComboBoxBuilderItems();
            GenerateComboBoxTenderFormItems();
            GenerateComboBoxEnterpriseFormItems();
            GenerateComboBoxExecutiveItems();
        }

        #endregion

        #region Buttons
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                //Warning about lost changes before closing
                if (MessageBox.Show("Du er ved at lukke 'Opret Projekt'. Alle ugemte data mistes!", "Projekter", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    CBZ.CloseUcMain(UcMain);
                }
            }
            else
            {
                CBZ.CloseUcMain(UcMain);
            }
        }

        private void ButtonCreate_Click(object sender, RoutedEventArgs e)
        {
            bool result = false;

            //Code that creates a new project
            if (newCaseIdCorrect)
            {
                int id = CBZ.CreateInDb(CBZ.TempProject);
                if (id >= 1)
                {
                    result = true;
                }

            }

            if (result)
            {
                //Show Confirmation
                MessageBox.Show("Projektet blev oprettet", "Projekter", MessageBoxButton.OK, MessageBoxImage.Information);

                //Reset form
                TextBoxCaseId.Text = "";
                TextBoxCaseName.Text = "";
                ComboBoxBuilder.SelectedIndex = -1;
                ComboBoxTenderForm.SelectedIndex = -1;
                ComboBoxEnterpriseForm.SelectedIndex = -1;
                ComboBoxExecutive.SelectedIndex = -1;

                //Update list of projects
                CBZ.RefreshIndexedList("Projects");
            }
            else
            {
                //Show error
                MessageBox.Show("Projektet blev ikke oprettet. Check alle oplysninger og prøv igen.", "Projekter", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        #endregion

        #region Events
        private void ComboBoxBuilder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxBuilder.SelectedIndex >= 0)
            {
                Builder builder = new Builder((Builder)ComboBoxBuilder.SelectedItem);

                if (CBZ.TempProject.Builder.Id != builder.Id)
                {
                    CBZ.TempProject.Builder = builder;

                    //Set CBZ.UcMainEdited
                    if (!CBZ.UcMainEdited)
                    {
                        CBZ.UcMainEdited = true;
                    }
                }
            }
            else
            {
                CBZ.TempProject.Builder = new Builder();
            }
        }

        private void ComboBoxTenderForm_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxTenderForm.SelectedIndex >= 0)
            {
                TenderForm tenderForm = new TenderForm((TenderForm)ComboBoxTenderForm.SelectedItem);
                if (CBZ.TempProject.TenderForm.Id != tenderForm.Id)
                {
                    CBZ.TempProject.TenderForm = tenderForm;

                    //Set CBZ.UcMainEdited
                    if (!CBZ.UcMainEdited)
                    {
                        CBZ.UcMainEdited = true;
                    }
                }
            }
            else
            {
                CBZ.TempProject.TenderForm = new TenderForm();
            }
        }

        private void ComboBoxEnterpriseForm_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxEnterpriseForm.SelectedIndex >= 0)
            {
                EnterpriseForm form = new EnterpriseForm((EnterpriseForm)ComboBoxEnterpriseForm.SelectedItem);
                if (CBZ.TempProject.EnterpriseForm.Id != form.Id)
                {
                    CBZ.TempProject.EnterpriseForm = form;

                    //Set CBZ.UcMainEdited
                    if (!CBZ.UcMainEdited)
                    {
                        CBZ.UcMainEdited = true;
                    }
                }
            }
            else
            {
                CBZ.TempProject.EnterpriseForm = new EnterpriseForm();
            }
        }

        private void ComboBoxExecutive_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxExecutive.SelectedIndex >= 0)
            {
                User user = new User((User)ComboBoxExecutive.SelectedItem);

                if (CBZ.TempProject.Executive.Id != user.Id)
                {
                    CBZ.TempProject.Executive = user;

                    //Set CBZ.UcMainEdited
                    if (!CBZ.UcMainEdited)
                    {
                        CBZ.UcMainEdited = true;
                    }
                }
            }
            else
            {
                CBZ.TempProject.Executive = new User();
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

        private void TextBoxCaseId_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBoxCaseId.Text.Count() > 6)
            {
                string id = TextBoxCaseId.Text;
                id = id.Remove(id.Length - 1);
                TextBoxCaseId.Text = id;
                TextBoxCaseId.Select(TextBoxCaseId.Text.Length, 0);
            }

            if (CBZ.TempProject.Case.ToString() != TextBoxCaseId.Text)
            {
                if (TextBoxCaseId.Text.Count() >= 1)
                {
                    ChecknewCaseIdCorrect();
                    if (newCaseIdCorrect)
                    {
                        Int32.TryParse(TextBoxCaseId.Text, out int caseId);
                        CBZ.TempProject.Case = caseId;
                    }

                    else
                    {
                        CBZ.TempProject.Case = 0;
                    }

                    //Set CBZ.UcMainEdited
                    if (!CBZ.UcMainEdited)
                    {
                        CBZ.UcMainEdited = true;
                    }
                }
            }

        }

        private void TextBoxCaseName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBoxCaseName.Text.Count() > 50)
            {
                string id = TextBoxCaseName.Text;
                id = id.Remove(id.Length - 1);
                TextBoxCaseName.Text = id;
                TextBoxCaseName.Select(TextBoxCaseName.Text.Length, 0);
            }

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
        /// Method, that sets new CaseId indicator
        /// </summary>
        private void ChecknewCaseIdCorrect()
        {
            bool vacantCaseId = CheckVacantCaseId();

            if (vacantCaseId)
            {
                newCaseIdCorrect = true;
                ButtonCaseIdNewIndicator.Background = CBZ.greenIndicator;
            }
            else
            {
                newCaseIdCorrect = false;
                ButtonCaseIdNewIndicator.Background = CBZ.redIndicator;
            }

        }

        /// <summary>
        /// Method, that checks wether a caseId is already used
        /// </summary>
        /// <returns>bool</returns>
        private bool CheckVacantCaseId()
        {
            bool result = true;

            if (TextBoxCaseId.Text.Count() == 6)
            {

                foreach (Project project in CBZ.Projects)
                {
                    if (project.Case.ToString() == TextBoxCaseId.Text)
                    {
                        result = false;
                        break;
                    }
                }

            }


            return result;
        }

        private void GenerateComboBoxBuilderItems()
        {
            ComboBoxBuilder.Items.Clear();
            foreach (Builder temp in CBZ.Builders)
            {
                ComboBoxBuilder.Items.Add(temp);
            }
        }

        private void GenerateComboBoxTenderFormItems()
        {
            ComboBoxTenderForm.Items.Clear();
            foreach (TenderForm temp in CBZ.TenderForms)
            {
                ComboBoxTenderForm.Items.Add(temp);
            }
        }

        private void GenerateComboBoxEnterpriseFormItems()
        {
            ComboBoxEnterpriseForm.Items.Clear();
            foreach (EnterpriseForm temp in CBZ.EnterpriseForms)
            {
                ComboBoxEnterpriseForm.Items.Add(temp);
            }
        }

        private void GenerateComboBoxExecutiveItems()
        {
            ComboBoxExecutive.Items.Clear();
            foreach (User temp in CBZ.Users)
            {
                ComboBoxExecutive.Items.Add(temp);
            }
        }

        #endregion

    }
}
