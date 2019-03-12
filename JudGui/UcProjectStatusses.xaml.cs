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
    /// Interaction logic for UcProjectStatusses.xaml
    /// </summary>
    public partial class UcProjectStatusses : UserControl
    {
        #region Fields
        public Bizz CBZ;
        public UserControl UcMain;
        public ProjectStatus TempNewProjectStatus = new ProjectStatus();

        List<IndexedProjectStatus> FilteredProjectStatusses = new List<IndexedProjectStatus>();
        #endregion

        #region Constructors
        public UcProjectStatusses(Bizz cbz, UserControl ucMain)
        {
            InitializeComponent();
            this.CBZ = cbz;
            this.UcMain = ucMain;

            CBZ.TempProjectStatus = new ProjectStatus();

            GetFilteredProjectStatusses();
            ListBoxProjectStatusses.ItemsSource = FilteredProjectStatusses;
            ListBoxProjectStatusses.SelectedIndex = -1;

        }

        #endregion

        #region Buttons
        private void ButtonAddProjectStatus_Click(object sender, RoutedEventArgs e)
        {
            bool result = CreateProjectStatusInDb();

            //Display result
            if (result)
            {
                //Show Confirmation
                MessageBox.Show("Projektstatussen blev tilføjet", "Projektstatusser", MessageBoxButton.OK, MessageBoxImage.Information);

                //Reset Boxes
                ListBoxProjectStatusses.SelectedIndex = -1;
                ListBoxProjectStatusses.ItemsSource = "";
                CBZ.RefreshIndexedList("IndexedProjectStatusses");
                ListBoxProjectStatusses.ItemsSource = CBZ.IndexedCraftGroups;
                TextBoxProjectStatusSearch.Text = "";
                TextBoxText.Text = "";
                TextBoxNewText.Text = "";

                //Refresh Users list
                CBZ.RefreshList("ProjectStatusses");
                CBZ.TempProjectStatus = new ProjectStatus();
            }
            else
            {
                //Show error
                MessageBox.Show("Databasen returnerede en fejl. Projektstatussen blev ikke tilføjet. Prøv igen.", "Projektstatusser", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                //Warning about lost changes before closing
                if (MessageBox.Show("Vil du lukke Projektstatusser? Ikke gemte data mistes.", "Projektstatusser", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    //Close right UserControl
                    CBZ.UcMainEdited = false;
                    UcMain.Content = new UserControl();
                }
            }
            else
            {
                //Close right UserControl
                CBZ.UcMainEdited = false;
                UcMain.Content = new UserControl();

            }
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            bool result = UpdateProjectStatusInDb;

            //Display result
            if (result)
            {
                //Show Confirmation
                MessageBox.Show("Projektstatussen blev opdateret", "Projektstatusser", MessageBoxButton.OK, MessageBoxImage.Information);

                //Reset Boxes
                ListBoxProjectStatusses.SelectedIndex = -1;
                ListBoxProjectStatusses.ItemsSource = "";
                TextBoxProjectStatusSearch.Text = "";
                TextBoxText.Text = "";
                TextBoxNewText.Text = "";

                //Refresh Users list
                CBZ.RefreshList("ProjectStatusses");
                CBZ.TempProjectStatus = new ProjectStatus();
            }
            else
            {
                //Show error
                MessageBox.Show("Databasen returnerede en fejl. Projektstatussen blev ikke opdateret. Prøv igen.", "Projektstatusser", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        #endregion

        #region Events
        private void ListBoxProjectStatusses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CBZ.TempProjectStatus = new ProjectStatus((ProjectStatus)ListBoxProjectStatusses.SelectedItem);

            TextBoxText.Text = CBZ.TempProjectStatus.Text;

            this.TempNewProjectStatus = new IndexedProjectStatus();
            TextBoxNewText.Text = "";

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxNewText_TextChanged(object sender, TextChangedEventArgs e)
        {
            TempNewProjectStatus.Text = TextBoxNewText.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxProjectStatusSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            GetFilteredProjectStatusses();
            ListBoxProjectStatusses.SelectedIndex = -1;
            ListBoxProjectStatusses.ItemsSource = "";
            ListBoxProjectStatusses.ItemsSource = this.FilteredProjectStatusses;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxText_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempProjectStatus.Text = TextBoxText.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Method, that creates a Project Status in Db
        /// </summary>
        /// <returns>bool</returns>
        private bool CreateProjectStatusInDb()
        {
            bool result = false;

            int projectStatusId = CBZ.CreateInDb(TempNewProjectStatus);

            if (projectStatusId >= 1)
            {
                result = true;
            }

            return result;

        }

        /// <summary>
        /// Method, that retrieves a list of filtered Project Statusses for ListBoxProjectStatusses
        /// </summary>
        private void GetFilteredProjectStatusses()
        {
            CBZ.RefreshIndexedList("IndexedProjectStatusses");
            this.FilteredProjectStatusses = new List<IndexedProjectStatus>();
            int length = TextBoxProjectStatusSearch.Text.Length;

            foreach (IndexedProjectStatus status in CBZ.IndexedProjectStatusses)
            {
                if (status.Text == TextBoxProjectStatusSearch.Text)
                {
                    this.FilteredProjectStatusses.Add(status);
                }
            }
        }

        /// <summary>
        /// Method, that updates an Craft Group in Db
        /// </summary>
        /// <returns>bool</returns>
        private bool UpdateProjectStatusInDb => CBZ.UpdateInDb(CBZ.TempProjectStatus);

        #endregion

    }
}
