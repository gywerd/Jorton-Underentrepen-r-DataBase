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
    /// Interaction logic for UcProjectCaseIdChange.xaml
    /// </summary>
    public partial class UcProjectCaseIdChange : UserControl
    {
        #region Fields
        public Bizz CBZ;
        public UserControl UcMain;

        bool newCaseIdCorrect = false;

        #endregion

        #region Constructors
        public UcProjectCaseIdChange(Bizz cbz, UserControl ucMain)
        {
            InitializeComponent();
            this.CBZ = cbz;
            this.UcMain = ucMain;

            GenerateComboBoxCaseIdItems();
        }

        #endregion

        #region Buttons
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                //Warning before cancelling
                if (MessageBox.Show("Vil du lukke 'Rediger SagsId'? Alle ugemte data mistes", "Projekter", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
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
            // Code that save changed CaseId to the project
            bool result = CBZ.UpdateInDb(CBZ.TempProject);

            if (result)
            {
                //Show Confirmation
                MessageBox.Show("Sagsnummer blev ændret", "Projekter", MessageBoxButton.OK, MessageBoxImage.Information);

                //Update list of projects
                CBZ.RefreshIndexedList("Projects");

                //ResetForm
                ComboBoxCaseId.SelectedItem = -1;
                CBZ.UcMainEdited = false;
                
            }
            else
            {
                //Show error
                MessageBox.Show("Databasen returnerede en fejl. Sagsnummer blev ikke ændret. Prøv igen.", "Projekter", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        #endregion

        #region Events
        private void ComboBoxCaseId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxCaseId.SelectedIndex == -1)
            {
                CBZ.TempProject = new Project();

                TextBoxName.Text = "";

                //Reset CBZ.UcMainEdited
                if (CBZ.UcMainEdited)
                {
                    CBZ.UcMainEdited = false;
                }
            }
            else
            {
                CBZ.TempProject = new Project((IndexedProject)ComboBoxCaseId.SelectedItem);

                TextBoxName.Text = CBZ.TempProject.Details.Name;

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

            ChecknewCaseIdCorrect();
            if (newCaseIdCorrect)
            {
                Int32.TryParse(TextBoxCaseId.Text, out int caseId);
                CBZ.TempProject.Case = caseId;
            }

            //Check CBZ.UcMainEdited
            if (!CBZ.UcMainEdited && TextBoxCaseId.Text != "")
            {
                CBZ.UcMainEdited = true;
            }
            else if (CBZ.UcMainEdited && TextBoxCaseId.Text == "")
            {
                CBZ.UcMainEdited = false;
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

        private void GenerateComboBoxCaseIdItems()
        {
            CBZ.RefreshIndexedList("Projects");
            ComboBoxCaseId.ItemsSource = "";
            ComboBoxCaseId.ItemsSource = CBZ.IndexedActiveProjects;
        }

        /// <summary>
        /// Method, that reloads list of active projects
        /// </summary>
        private void ReloadListActiveProjects()
        {
            CBZ.IndexedActiveProjects.Clear();
            int i = 0;
            foreach (Project tempProject in CBZ.ActiveProjects)
            {
                if (tempProject.Status.Id == 1)
                {
                    IndexedProject result = new IndexedProject(i, tempProject);
                    CBZ.IndexedActiveProjects.Add(result);
                    i++;
                }
            }
        }

        /// <summary>
        /// Method, that reloads list of indexable projects
        /// </summary>
        private void ReloadListIndexedProjects()
        {
            CBZ.IndexedProjects.Clear();
            int i = 0;
            foreach (Project temp in CBZ.ActiveProjects)
            {
                IndexedProject result = new IndexedProject(i, temp);
                CBZ.IndexedProjects.Add(result);
                i++;
            }
        }

        #endregion

    }
}
