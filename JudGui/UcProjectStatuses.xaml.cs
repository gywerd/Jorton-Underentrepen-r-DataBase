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
    /// Interaction logic for UcProjectStatuses.xaml
    /// </summary>
    public partial class UcProjectStatuses : UserControl
    {
        #region Fields
        public Bizz CBZ;
        public UserControl UcMain;
        public ProjectStatus TempNewProjectStatus = new ProjectStatus();

        List<IndexedProjectStatus> FilteredProjectStatusses = new List<IndexedProjectStatus>();
        #endregion

        #region Constructors
        public UcProjectStatuses(Bizz cbz, UserControl ucMain)
        {
            InitializeComponent();
            this.CBZ = cbz;
            this.UcMain = ucMain;
        }

        #endregion

        #region Buttons
        private void ButtonAddCraftGroup_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {

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
            if (TextBoxProjectStatusSearch.Text.Length >= 3)
            {
                GetFilteredProjectStatusses();
                ListBoxProjectStatusses.SelectedIndex = -1;
                ListBoxProjectStatusses.ItemsSource = "";
                ListBoxProjectStatusses.ItemsSource = this.FilteredProjectStatusses;
            }

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

        private void GetFilteredProjectStatusses()
        {
            CBZ.RefreshIndexedList("IndexedProjectStatusses");
            this.FilteredProjectStatusses = new List<IndexedProjectStatus>();

            foreach (IndexedProjectStatus status in CBZ.IndexedProjectStatusses)
            {
                if (status.Text.Remove(3) == TextBoxProjectStatusSearch.Text.Remove(3))
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
