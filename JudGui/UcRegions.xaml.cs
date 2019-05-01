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
    /// Interaction logic for UcRegions.xaml
    /// </summary>
    public partial class UcRegions : UserControl
    {
        #region Fields
        public Bizz CBZ;
        public UserControl UcMain;
        public IndexedRegion TempNewRegion = new IndexedRegion();

        public List<IndexedRegion> FilteredRegions = new List<IndexedRegion>();
        #endregion

        #region Constructors
        public UcRegions(Bizz cbz, UserControl ucMain)
        {
            InitializeComponent();
            this.CBZ = cbz;
            this.UcMain = ucMain;

            CBZ.TempRegion = new Region();

            GetFilteredRegions();
            ListBoxRegions.ItemsSource = FilteredRegions;
            ListBoxRegions.SelectedIndex = -1;

        }


        #endregion

        #region Buttons
        private void ButtonAddRegion_Click(object sender, RoutedEventArgs e)
        {
            bool result = CreateRegionInDb();

            //Display result
            if (result)
            {
                //Show Confirmation
                MessageBox.Show("Regionen blev tilføjet", "Regioner", MessageBoxButton.OK, MessageBoxImage.Information);

                //Reset Boxes
                ListBoxRegions.SelectedIndex = -1;
                ListBoxRegions.ItemsSource = "";
                CBZ.RefreshIndexedList("Regtions");
                ListBoxRegions.ItemsSource = CBZ.IndexedJobDescriptions;
                TextBoxRegionSearch.Text = "";
                TextBoxText.Text = "";
                TextBoxZips.Text = "";
                TextBoxNewText.Text = "";
                TextBoxNewZips.Text = "";

                //Refresh JobDescriptions list
                CBZ.RefreshList("JobDescriptions");
                CBZ.TempRegion = new Region();
                TempNewRegion = new IndexedRegion();
            }
            else
            {
                //Show error
                MessageBox.Show("Databasen returnerede en fejl. Regionen blev ikke tilføjet. Prøv igen.", "Regioner", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                //Warning about lost changes before closing
                if (MessageBox.Show("Vil du lukke Regioner? Alle ugemte data mistes.", "Regioner", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
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
            bool result = UpdateRegionInDb;

            //Display result
            if (result)
            {
                //Show Confirmation
                MessageBox.Show("Regionen blev opdateret", "Regioner", MessageBoxButton.OK, MessageBoxImage.Information);

                //Reset Boxes
                ListBoxRegions.SelectedIndex = -1;
                ListBoxRegions.ItemsSource = "";
                TextBoxRegionSearch.Text = "";
                TextBoxText.Text = "";
                TextBoxZips.Text = "";
                TextBoxNewText.Text = "";
                TextBoxNewZips.Text = "";

                //Refresh JobDescription list
                CBZ.RefreshList("Regions");
                CBZ.TempRegion = new Region();
                TempNewRegion = new IndexedRegion();
            }
            else
            {
                //Show error
                MessageBox.Show("Databasen returnerede en fejl. Regionen blev ikke opdateret. Prøv igen.", "Regioner", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region Events
        private void ListBoxRegions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CBZ.TempRegion = new Region((Region)ListBoxRegions.SelectedItem);

            TextBoxText.Text = CBZ.TempRegion.Text;
            TextBoxZips.Text = CBZ.TempRegion.Zips;

            this.TempNewRegion = new IndexedRegion();
            TextBoxText.Text = "";
            TextBoxZips.Text = "";

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxRegionSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            GetFilteredRegions();
            ListBoxRegions.SelectedIndex = -1;
            ListBoxRegions.ItemsSource = "";
            ListBoxRegions.ItemsSource = this.FilteredRegions;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxNewText_TextChanged(object sender, TextChangedEventArgs e)
        {
            TempNewRegion.Text = TextBoxNewText.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxNewZips_TextChanged(object sender, TextChangedEventArgs e)
        {
            TempNewRegion.Zips = TextBoxNewZips.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxText_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempRegion.Text = TextBoxText.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxZips_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempRegion.Zips = TextBoxZips.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        #endregion

        #region Methods
        public bool CreateRegionInDb()
        {
            bool result = false;

            int regionId = CBZ.CreateInDb(TempNewRegion);

            if (regionId >= 1)
            {
                result = true;
            }

            return result;

        }

        public void GetFilteredRegions()
        {
            CBZ.RefreshIndexedList("Regions");
            this.FilteredRegions = new List<IndexedRegion>();
            int length = TextBoxRegionSearch.Text.Length;

            foreach (IndexedRegion region in CBZ.IndexedRegions)
            {
                if (region.Text == TextBoxRegionSearch.Text)
                {
                    this.FilteredRegions.Add(region);
                }
            }
        }

        public bool UpdateRegionInDb => CBZ.UpdateInDb(CBZ.TempRegion);
        #endregion

    }
}
