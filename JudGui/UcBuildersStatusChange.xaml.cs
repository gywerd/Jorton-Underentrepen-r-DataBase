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
    /// Interaction logic for UcBuildersStatusChange.xaml
    /// </summary>
    public partial class UcBuildersStatusChange : UserControl
    {
        #region Fields
        public Bizz CBZ;
        public UserControl UcMain;

        public List<Builder> FilteredBuilders = new List<Builder>();

        #endregion

        #region Constructors
        public UcBuildersStatusChange(Bizz cbz, UserControl ucMain)
        {
            InitializeComponent();
            this.CBZ = cbz;
            this.UcMain = ucMain;

            GetFilteredBuilders();
            ListBoxBuilders.ItemsSource = FilteredBuilders;

        }

        #endregion

        #region Buttons
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                //Warning about lost changes before closing
                if (MessageBox.Show("Vil du lukke redigering af Entrepenører? Ikke gemte data mistes.", "Entrepenører", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
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
            bool result = UpdateBuilderInDb();

            //Display result
            if (result)
            {
                //Show Confirmation
                MessageBox.Show("Bygherren blev opdateret", "Bygherrer", MessageBoxButton.OK, MessageBoxImage.Information);

                //Reset Boxes
                ListBoxBuilders.SelectedIndex = 0;
                CheckBoxActive.IsChecked = false;

                //Refresh Entrepeneurs list
                CBZ.RefreshList("Builders");
                CBZ.TempBuilder = new Builder();
            }
            else
            {
                //Show error
                MessageBox.Show("Databasen returnerede en fejl. Bygherren blev ikke opdateret. Prøv igen.", "Bygherrer", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region Events
        private void CheckBoxActive_Checked(object sender, RoutedEventArgs e)
        {
            if (CBZ.TempBuilder.Active && CheckBoxActive.IsChecked == false)
            {
                CBZ.TempBuilder.ToggleActive();

                //Set CBZ.UcMainEdited
                if (!CBZ.UcMainEdited)
                {
                    CBZ.UcMainEdited = true;
                }
            }

            else if (!CBZ.TempBuilder.Active && CheckBoxActive.IsChecked == true)
            {
                CBZ.TempBuilder.ToggleActive();

                //Set CBZ.UcMainEdited
                if (!CBZ.UcMainEdited)
                {
                    CBZ.UcMainEdited = true;
                }
            }


        }

        private void ListBoxBuilders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CBZ.TempBuilder = new Builder((Builder)ListBoxBuilders.SelectedItem);
            if (CBZ.TempBuilder.Active)
            {
                CheckBoxActive.IsChecked = true;
            }
            else
            {
                CheckBoxActive.IsChecked = false;
            }


            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            GetFilteredBuilders();
            ListBoxBuilders.SelectedIndex = -1;
            ListBoxBuilders.ItemsSource = "";
            ListBoxBuilders.ItemsSource = this.FilteredBuilders;

        }

        #endregion

        #region Methods

        /// <summary>
        /// Method, that retrieves a list of filtered Builders for ListBoxBuilders
        /// </summary>
        private void GetFilteredBuilders()
        {
            int length = TextBoxSearch.Text.Length;

            if (length > 0)
            {
                CBZ.RefreshList("Builders");
                this.FilteredBuilders.Clear();
                foreach (Builder builder in CBZ.Builders)
                {
                    if (builder.Entity.Name.Remove(length).ToLower() == TextBoxSearch.Text.ToLower())
                    {
                        this.FilteredBuilders.Add(builder);
                    }
                }

            }
            else
            {
                CBZ.RefreshList("Builders");
                this.FilteredBuilders.Clear();
                foreach (Builder builder in CBZ.Builders)
                {
                    this.FilteredBuilders.Add(builder);
                }
            }
        }

        /// <summary>
        /// Method, that creates an Builder in Db
        /// </summary>
        /// <returns>bool</returns>
        private bool UpdateBuilderInDb()
        {
            return CBZ.UpdateInDb(CBZ.TempBuilder);
        }

        #endregion

    }
}
