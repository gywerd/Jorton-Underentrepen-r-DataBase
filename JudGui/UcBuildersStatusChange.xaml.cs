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

        #endregion

        #region Constructors
        public UcBuildersStatusChange(Bizz cbz, UserControl ucRight)
        {
            InitializeComponent();
            this.CBZ = cbz;
            this.UcMain = ucRight;

        }

        #endregion

        #region Buttons
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.TempBuilder != new Builder())
            {
                //Warning about lost changes before closing
                if (MessageBox.Show("Vil du lukke redigering af Entrepenører? Ikke gemte data mistes.", "Entrepenører", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    //Close right UserControl
                    CBZ.UcMainActive = false;
                    UcMain.Content = new UserControl();
                }
            }
            else
            {
                //Close right UserControl
                CBZ.UcMainActive = false;
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
            }

            else if (!CBZ.TempBuilder.Active && CheckBoxActive.IsChecked == true)
            {
                CBZ.TempBuilder.ToggleActive();
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

        }

        #endregion

        #region Methods

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
