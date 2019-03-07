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
    /// Interaction logic for UcProjectCopy.xaml
    /// </summary>
    public partial class UcProjectCopy : UserControl
    {
        #region Fields
        public Bizz CBZ;
        public UserControl UcMain;

        #endregion

        #region Constructors
        public UcProjectCopy(Bizz cbz, UserControl ucRight)
        {
            InitializeComponent();
            this.CBZ = cbz;
            this.UcMain = ucRight;

            GenerateComboBoxCaseIdItems();
        }

        #endregion

        #region Buttons
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Vil du annullere kopiering af projektet?", "Annuller kopiering", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                //Close right UserControl
                UcMain.Content = new UserControl();
                CBZ.UcMainEdited = false;
            }
        }

        private void ButtonCopy_Click(object sender, RoutedEventArgs e)
        {
            bool result = false;
            // Code that copies the current project into a new project
            Project project = new Project(CBZ.TempProject.Case, CBZ.TempProject.Name, CBZ.TempProject.Builder, new ProjectStatus((ProjectStatus)CBZ.GetObject("ProjectStatus", 1)), CBZ.TempProject.TenderForm, CBZ.TempProject.EnterpriseForm, CBZ.TempProject.Executive);
            int id = CBZ.CreateInDb(CBZ.TempProject);
            if (id >= 1)
            {
                result = true;
            }

            if (result)
            {
                //Show Confirmation
                MessageBox.Show("Projektet blev kopieret", "Kopier projekt", MessageBoxButton.OK, MessageBoxImage.Information);

                //Update list of projects
                CBZ.RefreshList("Projects");
                CBZ.RefreshIndexedList("IndexedActiveProjects");
                CBZ.RefreshIndexedList("IndexedProjects");

                //Close right UserControl
                CBZ.UcMainEdited = false;
                UcMain.Content = new UserControl();
            }
            else
            {
                //Show error
                MessageBox.Show("Databasen returnerede en fejl. Projektet blev ikke kopieret. Prøv igen.", "Kopier Projekt", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        #endregion

        #region Events
        private void ComboBoxCaseId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = ComboBoxCaseId.SelectedIndex;
            foreach (IndexedProject temp in CBZ.IndexedProjects)
            {
                if (temp.Index == selectedIndex)
                {
                    CBZ.TempProject = new Project(temp.Id, temp.Case, temp.Name, temp.Builder, temp.Status, temp.TenderForm, temp.EnterpriseForm, temp.Executive, temp.EnterprisesList, temp.Copy);
                }
            }
            TextBoxCaseName.Text = CBZ.TempProject.Name;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
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
            CBZ.TempProject.Case = Convert.ToInt32(TextBoxCaseId.Text);

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxCaseName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBoxCaseId.Text.Count() > 50)
            {
                string id = TextBoxCaseName.Text;
                id = id.Remove(id.Length - 1);
                TextBoxCaseName.Text = id;
                TextBoxCaseName.Select(TextBoxCaseName.Text.Length, 0);
            }
            CBZ.TempProject.Name = TextBoxCaseName.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        #endregion

        #region Methods
        private void GenerateComboBoxCaseIdItems()
        {
            ComboBoxCaseId.Items.Clear();
            foreach (IndexedProject temp in CBZ.IndexedProjects)
            {
                ComboBoxCaseId.Items.Add(temp);
            }
        }

        #endregion

    }
}
