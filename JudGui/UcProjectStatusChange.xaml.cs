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
        public Bizz CBZ;
        public UserControl UcMain;

        #endregion

        #region Constructors
        public UcProjectStatusChange(Bizz cbz, UserControl ucMain)
        {
            InitializeComponent();
            this.CBZ = cbz;
            this.UcMain = ucMain;

            GenerateComboBoxCaseIdItems();
            GenerateComboBoxProjectStatusItems();
        }

        #endregion

        #region Buttons
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Vil du annullere reaktivering af projektet!", "Annuller reaktivering", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
            {
                //Close right UserControl
                UcMain.Content = new UserControl();
                CBZ.UcMainEdited = false;
            }
        }

        private void ButtonExecute_Click(object sender, RoutedEventArgs e)
        {
            // Code that changes project status
            bool result = CBZ.UpdateInDb(CBZ.TempProject);

            if (result)
            {
                //Show Confirmation
                MessageBox.Show("Projekstatus blev ændret", "Ændr Projektstatus", MessageBoxButton.OK, MessageBoxImage.Information);

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
                MessageBox.Show("Databasen returnerede en fejl. Projektstatus blev ikke ændret. Prøv igen.", "Ændr Projektstatus", MessageBoxButton.OK, MessageBoxImage.Information);
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
                    CBZ.TempProject = new Project(temp.Id, temp.Case, temp.Name, temp.Builder, temp.Status, temp.TenderForm, temp.EnterpriseForm, temp.Executive, temp.EnterpriseList, temp.Copy);
                }
            }
            ComboBoxProjectStatus.SelectedIndex = CBZ.TempProject.Status.Id;
            TextBoxCaseName.Content = CBZ.TempProject.Name;

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

        private void GenerateComboBoxProjectStatusItems()
        {
            ComboBoxProjectStatus.Items.Clear();
            foreach (ProjectStatus temp in CBZ.ProjectStatuses)
            {
                ComboBoxProjectStatus.Items.Add(temp);
            }
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
