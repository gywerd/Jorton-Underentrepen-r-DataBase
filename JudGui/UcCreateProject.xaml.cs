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
    /// Interaction logic for UcCreateProject.xaml
    /// </summary>
    public partial class UcCreateProject : UserControl
    {
        #region Fields
        public Bizz Bizz;
        public UserControl UcRight;
        #endregion

        public UcCreateProject(Bizz bizz, UserControl ucRight)
        {
            InitializeComponent();
            this.Bizz = bizz;
            this.UcRight = ucRight;
            GenerateComboBoxBuilderItems();
            GenerateComboBoxTenderFormItems();
            GenerateComboBoxEnterpriseFormItems();
            GenerateComboBoxExecutiveItems();
        }

        #region Buttons
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            //Warning about lost changes before closing
            if (MessageBox.Show("Vil du annullere oprettelse af projektet?", "Luk Projekt", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                //Close right UserControl
                Bizz.UcRightActive = false;
                UcRight.Content = new UserControl();
            }
        }

        private void ButtonCreateClose_Click(object sender, RoutedEventArgs e)
        {
            //Code that creates a new project
            Project project = new Project(Bizz.strConnection, Convert.ToInt32(TextBoxCaseId.Text), TextBoxCaseName.Text, ComboBoxBuilder.SelectedIndex, 1, ComboBoxTenderForm.SelectedIndex, ComboBoxEnterpriseForm.SelectedIndex, ComboBoxExecutive.SelectedIndex);
            bool result = Bizz.PRO.InsertIntoProject(project);

            if (result)
            {
                //Show Confirmation
                MessageBox.Show("Projektet blev oprettet", "Opret Projekt", MessageBoxButton.OK, MessageBoxImage.Information);

                //Update list of projects
                Bizz.Projects.Clear();
                Bizz.Projects = Bizz.PRO.GetProjects();
                Bizz.IndexedActiveProjects.Clear();
                Bizz.IndexedActiveProjects = Bizz.RefreshIndexedActiveProjects();
                Bizz.IndexedProjects.Clear();
                Bizz.IndexedProjects = Bizz.RefreshIndexedProjects();

                //Close right UserControl
                Bizz.UcRightActive = false;
                UcRight.Content = new UserControl();
            }
            else
            {
                //Show error
                MessageBox.Show("Databasen returnerede en fejl. Projektet blev ikke oprettet. Prøv igen.", "Opret Projekt", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ButtonCreateNew_Click(object sender, RoutedEventArgs e)
        {
            //Code that creates a new project
            Project project = new Project(Bizz.strConnection, Convert.ToInt32(TextBoxCaseId.Text), TextBoxCaseName.Text, ComboBoxBuilder.SelectedIndex, 1, ComboBoxTenderForm.SelectedIndex, ComboBoxEnterpriseForm.SelectedIndex, ComboBoxExecutive.SelectedIndex);
            bool result = Bizz.PRO.InsertIntoProject(project);

            if (result)
            {
                //Show Confirmation
                MessageBox.Show("Projektet blev oprettet", "Opret Projekt", MessageBoxButton.OK, MessageBoxImage.Information);

                //Reset form
                TextBoxCaseId.Text = "";
                TextBoxCaseName.Text = "";
                ComboBoxBuilder.SelectedIndex = -1;
                ComboBoxTenderForm.SelectedIndex = -1;
                ComboBoxEnterpriseForm.SelectedIndex = -1;
                ComboBoxExecutive.SelectedIndex = -1;

                //Update list of projects
                Bizz.Projects.Clear();
                Bizz.Projects = Bizz.PRO.GetProjects();
                Bizz.IndexedActiveProjects.Clear();
                Bizz.IndexedActiveProjects = Bizz.RefreshIndexedActiveProjects();
                Bizz.IndexedProjects.Clear();
                Bizz.IndexedProjects = Bizz.RefreshIndexedProjects();
            }
            else
            {
                //Show error
                MessageBox.Show("Databasen returnerede en fejl. Projektet blev ikke oprettet. Prøv igen.", "Opret Projekt", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        #endregion

        #region Events
        private void TextBoxCaseId_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (TextBoxCaseId.Text.Count() > 6)
            {
                string id = TextBoxCaseId.Text;
                id = id.Remove(id.Length - 1);
                TextBoxCaseId.Text = id;
                TextBoxCaseId.Select(TextBoxCaseId.Text.Length, 0);
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
        }

        #endregion

        #region Methods
        private void GenerateComboBoxBuilderItems()
        {
            ComboBoxBuilder.Items.Clear();
            foreach (Builder temp in Bizz.Builders)
            {
                ComboBoxBuilder.Items.Add(temp);
            }
        }

        private void GenerateComboBoxTenderFormItems()
        {
            ComboBoxTenderForm.Items.Clear();
            foreach (TenderForm temp in Bizz.TenderForms)
            {
                ComboBoxTenderForm.Items.Add(temp);
            }
        }

        private void GenerateComboBoxEnterpriseFormItems()
        {
            ComboBoxEnterpriseForm.Items.Clear();
            foreach (EnterpriseForm temp in Bizz.EnterpriseForms)
            {
                ComboBoxEnterpriseForm.Items.Add(temp);
            }
        }

        private void GenerateComboBoxExecutiveItems()
        {
            ComboBoxExecutive.Items.Clear();
            foreach (User temp in Bizz.Users)
            {
                ComboBoxExecutive.Items.Add(temp);
            }
        }

        #endregion

    }
}
