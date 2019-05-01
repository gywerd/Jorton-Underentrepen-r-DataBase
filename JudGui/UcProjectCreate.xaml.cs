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
            int id = CBZ.CreateInDb(CBZ.TempProject);
            if (id >= 1)
            {
                result = true;
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
                MessageBox.Show("Databasen returnerede en fejl. Projektet blev ikke oprettet. Prøv igen.", "Projekter", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        #endregion

        #region Events
        private void ComboBoxBuilder_SelectionChanged(object sender, SelectionChangedEventArgs e)
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

        private void ComboBoxTenderForm_SelectionChanged(object sender, SelectionChangedEventArgs e)
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

        private void ComboBoxEnterpriseForm_SelectionChanged(object sender, SelectionChangedEventArgs e)
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

        private void ComboBoxExecutive_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
                try
                {
                    CBZ.TempProject.Case = Convert.ToInt32(TextBoxCaseId.Text);
                }
                catch (Exception)
                {
                    MessageBox.Show("Sagsnummer skal være et tal!", "Projekter", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
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

        #endregion

        #region Methods
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
