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
    /// Interaction logic for UcEditProject.xaml
    /// </summary>
    public partial class UcEditProject : UserControl
    {
        #region Fields
        public Bizz Bizz;
        public UserControl UcRight;

        #endregion

        public UcEditProject(Bizz bizz, UserControl ucRight)
        {
            InitializeComponent();
            this.Bizz = bizz;
            this.UcRight = ucRight;
            GenerateComboBoxCaseIdItems();
            GenerateComboBoxBuilderItems();
            GenerateComboBoxProjectStatusItems();
            GenerateComboBoxTenderFormItems();
            ComboBoxExecutive.ItemsSource = Bizz.IndexableUsers;
            ComboBoxProjectStatus.ItemsSource = Bizz.IndexedEnterpriseForms;
            GenerateComboBoxExecutiveItems();
        }

        #region Buttons
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            //Warning about lost changes before closing
            if (MessageBox.Show("Du er ved at lukke projektet. Alt, der ikke er gemt vil blive mistet!", "Luk Projekt", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
            {
                //Close right UserControl
                UcRight.Content = new UserControl();
                Bizz.UcRightActive = false;
            }
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            // Code that save changes to the project
            bool result = Bizz.UpdateInDb(Bizz.TempProject);

            if (result)
            {
                //Show Confirmation
                MessageBox.Show("Projektet blev rettet", "Ret Projekt", MessageBoxButton.OK, MessageBoxImage.Information);

                //Update Projects lists
                Bizz.RefreshList("Projects");
                Bizz.RefreshIndexedList("IndexedActiveProjects");
                Bizz.RefreshIndexedList("IndexedProjects");

                //Close right UserControl
                UcRight.Content = new UserControl();
                Bizz.UcRightActive = false;
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
            foreach (IndexedProject temp in Bizz.IndexedActiveProjects)
            {
                if (temp.Index == selectedIndex)
                {
                    Bizz.TempProject = new Project(temp.Id, temp.CaseId, temp.Name, temp.Builder, temp.Status, temp.TenderForm, temp.EnterpriseForm, temp.Executive, temp.EnterpriseList, temp.Copy);
                }
            }
            TextBoxCaseName.Text = Bizz.TempProject.Name;
            ComboBoxBuilder.SelectedIndex = Bizz.TempProject.Builder.Id;
            ComboBoxProjectStatus.SelectedIndex = GetProjectStatusIndex(Bizz.TempProject.Status.Id);
            ComboBoxTenderForm.SelectedIndex = Bizz.TempProject.TenderForm.Id;
            ComboBoxEnterpriseForm.SelectedIndex = GetEnterPriseFormIndex(Bizz.TempProject.EnterpriseForm);
            ComboBoxExecutive.SelectedIndex = Bizz.TempProject.Executive.Id;
        }

        private void TextBoxCaseName_TextChanged(object sender, RoutedEventArgs e)
        {
            Bizz.TempProject.Name = TextBoxCaseName.Text;
        }

        private void ComboBoxBuilder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Bizz.TempProject.Builder = GetBuilder(ComboBoxBuilder.SelectedIndex);
        }

        private void ComboBoxProjectStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Bizz.TempProject.Status = GetProjectStatus(ComboBoxProjectStatus.SelectedIndex);
        }

        private void ComboBoxTenderForm_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Bizz.TempProject.TenderForm = GetTenderform(ComboBoxTenderForm.SelectedIndex);
        }

        private void ComboBoxEnterpriseForm_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Bizz.TempProject.EnterpriseForm = GetEnterpriseForm(ComboBoxEnterpriseForm.SelectedIndex);
        }

        private void ComboBoxExecutive_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Bizz.TempProject.Executive = GetUser(ComboBoxExecutive.SelectedIndex);
        }

        #endregion

        #region Methods
        private void GenerateComboBoxCaseIdItems()
        {
            ComboBoxCaseId.Items.Clear();
            foreach (IndexedProject temp in Bizz.IndexedActiveProjects)
            {
                ComboBoxCaseId.Items.Add(temp);
            }
        }

        private void GenerateComboBoxBuilderItems()
        {
            ComboBoxBuilder.Items.Clear();
            foreach (Builder temp in Bizz.Builders)
            {
                ComboBoxBuilder.Items.Add(temp);
            }
        }

        private void GenerateComboBoxProjectStatusItems()
        {
            ComboBoxProjectStatus.Items.Clear();
            foreach (ProjectStatus temp in Bizz.ProjectStatusList)
            {
                ComboBoxProjectStatus.Items.Add(temp);
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

        private void GenerateComboBoxExecutiveItems()
        {
            ComboBoxExecutive.Items.Clear();
            foreach (User temp in Bizz.Users)
            {
                ComboBoxExecutive.Items.Add(temp);
            }
        }

        private Builder GetBuilder(int selectedIndex)
        {
            Builder result = new Builder();

            int i = 0;
            foreach (Builder builder in Bizz.Builders)
            {
                if (i == selectedIndex)
                {
                    result = builder;
                    break;
                }

                i++;
            }

            return result;
        }

        private int GetEnterPriseFormIndex(EnterpriseForm enterpriseForm)
        {
            int result = 0;

            foreach (IndexedEnterpriseForm indexForm in ComboBoxExecutive.Items)
            {
                if (indexForm.Name == enterpriseForm.Name)
                {
                    result = indexForm.Index;
                }
            }

            return result;
        }

        private EnterpriseForm GetEnterpriseForm(int selectedIndex)
        {
            EnterpriseForm result = new EnterpriseForm();

            foreach (IndexedEnterpriseForm form in Bizz.IndexedEnterpriseForms)
            {
                if (form.Index == selectedIndex)
                {
                    result = form;
                    break;
                }

            }

            return result;
        }

        private ProjectStatus GetProjectStatus(int selectedIndex)
        {
            IndexedProjectStatus result = new IndexedProjectStatus();
            int i = 0;

            foreach (IndexedProjectStatus status in Bizz.IndexedProjectStatusList)
            {
                if (status.Index == selectedIndex)
                {
                    result = status;
                    break;
                }

                i++;
            }

            return new ProjectStatus(result.Id, result.Description);
        }

        private int GetProjectStatusIndex(int id)
        {
            int result = 0;

            foreach (IndexedProjectStatus indexProjectStatus in Bizz.IndexedProjectStatusList)
            {
                if (indexProjectStatus.Id == id)
                {
                    result = indexProjectStatus.Index;
                    break;
                }
            }

            return result;
        }

        private TenderForm GetTenderform(int selectedIndex)
        {
            TenderForm result = new TenderForm();

            foreach (IndexedTenderForm form in Bizz.IndexedTenderForms)
            {
                if (form.Index == selectedIndex)
                {
                    result = form;
                    break;
                }

            }

            return new TenderForm(result.Id, result.Description);
        }

        private User GetUser(int selectedIndex)
        {
            User result = new User();
            int i = 0;

            foreach (User user in Bizz.Users)
            {
                if (i == selectedIndex)
                {
                    result = user;
                    break;
                }

                i++;
            }

            return result;
        }

        #endregion

    }
}
