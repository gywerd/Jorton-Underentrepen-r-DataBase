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
    /// Interaction logic for UcZipList.xaml
    /// </summary>
    public partial class UcZipList : UserControl
    {
        #region Fields
        public Bizz CBZ;
        public UserControl UcMain;

        public List<IndexedZipTown> FilteredZipTowns = new List<IndexedZipTown>();
        #endregion

        #region Constructors
        public UcZipList(Bizz cbz, UserControl ucMain)
        {
            InitializeComponent();
            this.CBZ = cbz;
            this.UcMain = ucMain;

            GetFilteredZipTowns();

            ListBoxZipTowns.SelectedIndex = -1;
            ListBoxZipTowns.ItemsSource = FilteredZipTowns;
            CBZ.TempZipTown = new ZipTown();
            TextBoxZip.Text = "";
            TextBoxTown.Text = "";
            ButtonCreate.Visibility = Visibility.Hidden;
            ButtonDelete.Visibility = Visibility.Hidden;
            ButtonUpdate.Visibility = Visibility.Hidden;
            CheckBoxAddNewZipCode.IsEnabled = true;
            CheckBoxDeleteZipCode.IsEnabled = true;
            CheckBoxEditZipCode.IsEnabled = true;
            TextBoxZip.IsEnabled = false;
            TextBoxTown.IsEnabled = false;

        }

        #endregion

        #region Buttons
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                //Warning about lost changes before closing
                if (MessageBox.Show("Vil du lukke Vælg Modtagere?", "Luk Vælg Modtagere", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    //Update lists and fields
                    CBZ.TempZipTown = new ZipTown();
                    CBZ.RefreshList("ZipTowns");

                    CBZ.CloseUcMain(UcMain);
                }
            }
            else
            {
                //Update lists and fields
                CBZ.TempZipTown = new ZipTown();
                CBZ.RefreshList("ZipTowns");

                CBZ.CloseUcMain(UcMain);
            }

        }

        private void ButtonCreate_Click(object sender, RoutedEventArgs e)
        {
            //Code that creates a new project
            bool result = false;
            int id = CBZ.CreateInDb(CBZ.TempZipTown);
            if (id >= 1)
            {
                result = true;
            }

            if (result)
            {
                //Show Confirmation
                MessageBox.Show("Postnummeret blev oprettet", "Opret Postnummer", MessageBoxButton.OK, MessageBoxImage.Information);

                //Update lists and fields
                CBZ.TempZipTown = new ZipTown();
                CBZ.RefreshList("ZipTowns");

                //Close right UserControl
                CBZ.UcMainEdited = false;
                UcMain.Content = new UserControl();

            }
            else
            {
                //Show error
                MessageBox.Show("Databasen returnerede en fejl. Postnummeret blev ikke oprettet. Prøv igen.", "Opret Postnummer", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region Events
        private void CheckBoxAddNewZipCode_Checked(object sender, RoutedEventArgs e)
        {
            if (CheckBoxAddNewZipCode.IsEnabled == true)
            {
                ListBoxZipTowns.SelectedIndex = -1;
                CBZ.TempZipTown = new ZipTown();
                TextBoxZip.Text = "";
                TextBoxTown.Text = "";
                ButtonCreate.Visibility = Visibility.Visible;
                ButtonDelete.Visibility = Visibility.Hidden;
                ButtonUpdate.Visibility = Visibility.Hidden;
                CheckBoxAddNewZipCode.IsEnabled = true;
                CheckBoxDeleteZipCode.IsEnabled = false;
                CheckBoxEditZipCode.IsEnabled = false;
                TextBoxZip.IsEnabled = true;
                TextBoxTown.IsEnabled = true;
            }
            else
            {
                ListBoxZipTowns.SelectedIndex = -1;
                CBZ.TempZipTown = new ZipTown();
                TextBoxZip.Text = "";
                TextBoxTown.Text = "";
                ButtonCreate.Visibility = Visibility.Hidden;
                ButtonDelete.Visibility = Visibility.Hidden;
                ButtonUpdate.Visibility = Visibility.Hidden;
                CheckBoxAddNewZipCode.IsEnabled = true;
                CheckBoxDeleteZipCode.IsEnabled = true;
                CheckBoxEditZipCode.IsEnabled = true;
                TextBoxZip.IsEnabled = false;
                TextBoxTown.IsEnabled = false;
            }

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }

        }

        private void CheckBoxDeleteZipCode_Checked(object sender, RoutedEventArgs e)
        {
            if (CheckBoxDeleteZipCode.IsEnabled == true)
            {
                if (ListBoxZipTowns.SelectedIndex >= 0)
                {
                    ButtonCreate.Visibility = Visibility.Hidden;
                    ButtonDelete.Visibility = Visibility.Visible;
                    ButtonUpdate.Visibility = Visibility.Hidden;
                    CheckBoxAddNewZipCode.IsEnabled = false;
                    CheckBoxDeleteZipCode.IsEnabled = true;
                    CheckBoxEditZipCode.IsEnabled = false;
                    TextBoxZip.IsEnabled = false;
                    TextBoxTown.IsEnabled = false;
                }
                else
                {
                    CheckBoxDeleteZipCode.IsChecked = false;
                }
            }
            else
            {
                ListBoxZipTowns.SelectedIndex = -1;
                CBZ.TempZipTown = new ZipTown();
                TextBoxZip.Text = "";
                TextBoxTown.Text = "";
                ButtonCreate.Visibility = Visibility.Hidden;
                ButtonDelete.Visibility = Visibility.Hidden;
                ButtonUpdate.Visibility = Visibility.Hidden;
                CheckBoxAddNewZipCode.IsEnabled = true;
                CheckBoxDeleteZipCode.IsEnabled = true;
                CheckBoxEditZipCode.IsEnabled = true;
                TextBoxZip.IsEnabled = false;
                TextBoxTown.IsEnabled = false;
            }

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void CheckBoxEditZipCode_Checked(object sender, RoutedEventArgs e)
        {
            if (CheckBoxEditZipCode.IsEnabled == true)
            {
                if (ListBoxZipTowns.SelectedIndex >= 0)
                {
                    ButtonCreate.Visibility = Visibility.Hidden;
                    ButtonUpdate.Visibility = Visibility.Visible;
                    ButtonDelete.Visibility = Visibility.Hidden;
                    CheckBoxAddNewZipCode.IsEnabled = false;
                    CheckBoxDeleteZipCode.IsEnabled = false;
                    CheckBoxEditZipCode.IsEnabled = true;
                    TextBoxZip.IsEnabled = false;
                    TextBoxTown.IsEnabled = true;
                }
                else
                {
                    CheckBoxEditZipCode.IsChecked = false;
                }
            }
            else
            {
                ListBoxZipTowns.SelectedIndex = -1;
                CBZ.TempZipTown = new ZipTown();
                TextBoxZip.Text = "";
                TextBoxTown.Text = "";
                ButtonCreate.Visibility = Visibility.Hidden;
                ButtonDelete.Visibility = Visibility.Hidden;
                ButtonUpdate.Visibility = Visibility.Hidden;
                CheckBoxAddNewZipCode.IsEnabled = true;
                CheckBoxDeleteZipCode.IsEnabled = true;
                CheckBoxEditZipCode.IsEnabled = true;
                TextBoxZip.IsEnabled = false;
                TextBoxTown.IsEnabled = false;

            }

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }

        }

        private void ListBoxZipTowns_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBoxZipTowns.SelectedIndex >= 0)
            {
                CBZ.TempZipTown = new ZipTown((ZipTown)ListBoxZipTowns.SelectedItem);
                TextBoxZip.Text = CBZ.TempZipTown.Zip;
                TextBoxTown.Text = CBZ.TempZipTown.Town;
            }
            else
            {
                CBZ.TempZipTown = new ZipTown();
                TextBoxZip.Text = "";
                TextBoxTown.Text = "";
            }

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxZip_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBoxZip.Text.Count() > 10)
            {
                string id = TextBoxZip.Text;
                id = id.Remove(id.Length - 1);
                TextBoxZip.Text = id;
                TextBoxZip.Select(TextBoxZip.Text.Length, 0);
                CBZ.TempZipTown.Zip = TextBoxZip.Text;
            }

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxTown_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBoxTown.Text.Count() > 50)
            {
                string id = TextBoxTown.Text;
                id = id.Remove(id.Length - 1);
                TextBoxTown.Text = id;
                TextBoxTown.Select(TextBoxTown.Text.Length, 0);
                CBZ.TempZipTown.Town = TextBoxTown.Text;
            }

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxZipSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            GetFilteredZipTowns();
            ListBoxZipTowns.SelectedIndex = -1;
            ListBoxZipTowns.ItemsSource = "";
            ListBoxZipTowns.ItemsSource = this.FilteredZipTowns;

        }

        #endregion

        #region Methods
        public bool CreateZipTownInDb()
        {
            bool result = false;

            int zipTownId = CBZ.CreateInDb(CBZ.TempZipTown);

            if (zipTownId > 0)
            {
                result = true;
            }
            return result;
        }

        private void GetFilteredZipTowns()
        {
            CBZ.RefreshIndexedList("ZipTowns");
            this.FilteredZipTowns = new List<IndexedZipTown>();
            int length = TextBoxZipSearch.Text.Length;

            foreach (IndexedZipTown zipTown in CBZ.IndexedZipTowns)
            {
                if (zipTown.Zip.Remove(length) == TextBoxZipSearch.Text || zipTown.Town.Remove(length) == TextBoxZipSearch.Text)
                {
                    this.FilteredZipTowns.Add(zipTown);
                }
            }
        }

        public bool UpdateZipTownInDb => CBZ.UpdateInDb(CBZ.TempZipTown);
        #endregion

    }
}
