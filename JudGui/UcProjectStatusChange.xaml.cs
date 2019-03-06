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
    /// Interaction logic for UcProjectStatusChange.xaml
    /// </summary>
    public partial class UcProjectStatusChange : UserControl
    {
        #region Fields
        public Bizz Bizz;
        public UserControl UcMain;

        #endregion

        public UcProjectStatusChange(Bizz bizz, UserControl ucRight)
        {
            InitializeComponent();
            this.Bizz = bizz;
            this.UcMain = ucRight;

            GenerateComboBoxCaseIdItems();
            GenerateComboBoxProjectStatusItems();
        }

        #region Buttons
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Vil du annullere reaktivering af projektet!", "Annuller reaktivering", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
            {
                //Close right UserControl
                UcMain.Content = new UserControl();
                Bizz.UcMainActive = false;
            }
        }

        private void ButtonExecute_Click(object sender, RoutedEventArgs e)
        {
            // Code that changes project status
            bool result = Bizz.UpdateInDb(Bizz.TempProject);

            if (result)
            {
                //Show Confirmation
                MessageBox.Show("Projekstatus blev ændret", "Ændr Projektstatus", MessageBoxButton.OK, MessageBoxImage.Information);

                //Update list of projects
                Bizz.RefreshList("Projects");
                Bizz.RefreshIndexedList("IndexedActiveProjects");
                Bizz.RefreshIndexedList("IndexedProjects");

                //Close right UserControl
                Bizz.UcMainActive = false;
                UcMain.Content = new UserControl();
            }
            else
            {
                //Show error
                MessageBox.Show("Databasen returnerede en fejl. Projektstatus blev ikke ændret. Prøv igen.", "Ændr Projektstatus", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        #endregion

        #region Events
        private void ComboBoxCaseId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = ComboBoxCaseId.SelectedIndex;
            foreach (IndexedProject temp in Bizz.IndexedProjects)
            {
                if (temp.Index == selectedIndex)
                {
                    Bizz.TempProject = new Project(temp.Id, temp.Case, temp.Name, temp.Builder, temp.Status, temp.TenderForm, temp.EnterpriseForm, temp.Executive, temp.EnterprisesList, temp.Copy);
                }
            }
            ComboBoxProjectStatus.SelectedIndex = Bizz.TempProject.Status.Id;
            TextBoxCaseName.Content = Bizz.TempProject.Name;
        }

        private void ComboBoxProjectStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Bizz.TempProject.Status = new ProjectStatus((ProjectStatus)ComboBoxProjectStatus.SelectedItem);
        }

        #endregion

        #region Methods
        private void GenerateComboBoxCaseIdItems()
        {
            ComboBoxCaseId.Items.Clear();
            foreach (IndexedProject temp in Bizz.IndexedProjects)
            {
                ComboBoxCaseId.Items.Add(temp);
            }
        }

        private void GenerateComboBoxProjectStatusItems()
        {
            ComboBoxProjectStatus.Items.Clear();
            foreach (ProjectStatus temp in Bizz.ProjectStatuses)
            {
                ComboBoxProjectStatus.Items.Add(temp);
            }
        }

        /// <summary>
        /// Method, that reloads list of active projects
        /// </summary>
        private void ReloadListActiveProjects()
        {
            Bizz.IndexedActiveProjects.Clear();
            int i = 0;
            foreach (Project tempProject in Bizz.ActiveProjects)
            {
                if (tempProject.Status.Id == 1)
                {
                    IndexedProject result = new IndexedProject(i, tempProject);
                    Bizz.IndexedActiveProjects.Add(result);
                    i++;
                }
            }
        }

        /// <summary>
        /// Method, that reloads list of indexable projects
        /// </summary>
        private void ReloadListIndexedProjects()
        {
            Bizz.IndexedProjects.Clear();
            int i = 0;
            foreach (Project temp in Bizz.ActiveProjects)
            {
                IndexedProject result = new IndexedProject(i, temp);
                Bizz.IndexedProjects.Add(result);
                i++;
            }
        }

        #endregion

    }
}
