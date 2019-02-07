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
        public Bizz Bizz;
        public UserControl UcRight;

        #endregion

        public UcZipList(Bizz bizz, UserControl ucRight)
        {
            InitializeComponent();
            this.Bizz = bizz;
            this.UcRight = ucRight;
            ListBoxZipList.ItemsSource = Bizz.ZipTownList;
        }

        #region Buttons
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            //Update lists and fields
            Bizz.TempZipTown = new ZipTown();
            Bizz.RefreshList("ZipTownList");

            //Close right UserControl
            Bizz.UcRightActive = false;
            UcRight.Content = new UserControl();

        }

        private void ButtonCreate_Click(object sender, RoutedEventArgs e)
        {
            //Code that creates a new project
            bool result = false;
            int id = Bizz.CreateInDb(Bizz.TempZipTown);
            if (id >= 1)
            {
                result = true;
            }

            if (result)
            {
                //Show Confirmation
                MessageBox.Show("Postnummeret blev oprettet", "Opret Postnummer", MessageBoxButton.OK, MessageBoxImage.Information);

                //Update lists and fields
                Bizz.TempZipTown = new ZipTown();
                Bizz.RefreshList("ZipTownList");

                //Close right UserControl
                Bizz.UcRightActive = false;
                UcRight.Content = new UserControl();

            }
            else
            {
                //Show error
                MessageBox.Show("Databasen returnerede en fejl. Postnummeret blev ikke oprettet. Prøv igen.", "Opret Postnummer", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region Events
        private void ListBoxZipList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Bizz.TempZipTown = new ZipTown((ZipTown)ListBoxZipList.SelectedItem);
            TextBoxZip.Text = Bizz.TempZipTown.Zip;
            TextBoxTown.Text = Bizz.TempZipTown.Town;
        }

        private void TextBoxZip_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBoxZip.Text.Count() > 10)
            {
                string id = TextBoxZip.Text;
                id = id.Remove(id.Length - 1);
                TextBoxZip.Text = id;
                TextBoxZip.Select(TextBoxZip.Text.Length, 0);
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
            }
        }

        private void CheckBoxAddNewZipCode_Checked(object sender, RoutedEventArgs e)
        {
            ListBoxZipList.SelectedIndex = -1;
            Bizz.TempZipTown = new ZipTown();
            TextBoxZip.Text = "";
            TextBoxTown.Text = "";
            ButtonCreate.Visibility = Visibility.Visible;
            ButtonDelete.Visibility = Visibility.Hidden;
            ButtonEdit.Visibility = Visibility.Hidden;
            CheckBoxAddNewZipCode.IsEnabled = true;
            CheckBoxDeleteZipCode.IsEnabled = false;
            CheckBoxEditZipCode.IsEnabled = false;
            TextBoxZip.IsEnabled = true;
        }

        private void CheckBoxAddNewZipCode_Unchecked(object sender, RoutedEventArgs e)
        {
            ListBoxZipList.SelectedIndex = -1;
            Bizz.TempZipTown = new ZipTown();
            TextBoxZip.Text = "";
            TextBoxTown.Text = "";
            ButtonCreate.Visibility = Visibility.Hidden;
            ButtonDelete.Visibility = Visibility.Hidden;
            ButtonEdit.Visibility = Visibility.Visible;
            CheckBoxAddNewZipCode.IsEnabled = true;
            CheckBoxDeleteZipCode.IsEnabled = true;
            CheckBoxEditZipCode.IsEnabled = true;
            TextBoxZip.IsEnabled = false;
        }

        private void CheckBoxDeleteZipCode_Checked(object sender, RoutedEventArgs e)
        {
            if (ListBoxZipList.SelectedIndex != -1)
            {
                ButtonCreate.Visibility = Visibility.Hidden;
                ButtonDelete.Visibility = Visibility.Visible;
                ButtonEdit.Visibility = Visibility.Hidden;
                CheckBoxAddNewZipCode.IsEnabled = false;
                CheckBoxDeleteZipCode.IsEnabled = true;
                CheckBoxEditZipCode.IsEnabled = false;
                TextBoxZip.IsEnabled = false;
            }
            else
            {
                CheckBoxDeleteZipCode.IsChecked = false;
            }
        }

        private void CheckBoxDeleteZipCode_Unchecked(object sender, RoutedEventArgs e)
        {
            ListBoxZipList.SelectedIndex = -1;
            Bizz.TempZipTown = new ZipTown();
            TextBoxZip.Text = "";
            TextBoxTown.Text = "";
            ButtonCreate.Visibility = Visibility.Hidden;
            ButtonDelete.Visibility = Visibility.Hidden;
            ButtonEdit.Visibility = Visibility.Visible;
            CheckBoxAddNewZipCode.IsEnabled = true;
            CheckBoxDeleteZipCode.IsEnabled = true;
            CheckBoxEditZipCode.IsEnabled = true;
            TextBoxZip.IsEnabled = false;
        }

        private void CheckBoxEditZipCode_Checked(object sender, RoutedEventArgs e)
        {
            if (ListBoxZipList.SelectedIndex != -1)
            {
                ButtonCreate.Visibility = Visibility.Hidden;
                ButtonEdit.Visibility = Visibility.Visible;
                ButtonDelete.Visibility = Visibility.Hidden;
                CheckBoxAddNewZipCode.IsEnabled = false;
                CheckBoxDeleteZipCode.IsEnabled = false;
                CheckBoxEditZipCode.IsEnabled = true;
                TextBoxZip.IsEnabled = true;
            }
            else
            {
                CheckBoxEditZipCode.IsChecked = false;
            }
        }

        private void CheckBoxEditZipCode_Unchecked(object sender, RoutedEventArgs e)
        {
            ListBoxZipList.SelectedIndex = -1;
            Bizz.TempZipTown = new ZipTown();
            TextBoxZip.Text = "";
            TextBoxTown.Text = "";
            ButtonCreate.Visibility = Visibility.Hidden;
            ButtonDelete.Visibility = Visibility.Hidden;
            ButtonEdit.Visibility = Visibility.Visible;
            CheckBoxAddNewZipCode.IsEnabled = true;
            CheckBoxDeleteZipCode.IsEnabled = true;
            CheckBoxEditZipCode.IsEnabled = true;
            TextBoxZip.IsEnabled = false;
        }

        #endregion

        #region Methods

        #endregion

    }
}
