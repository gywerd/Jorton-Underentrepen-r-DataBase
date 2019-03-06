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
    /// Interaction logic for UcEntrepeneurEdit.xaml
    /// </summary>
    public partial class UcEntrepeneurEdit : UserControl
    {
        #region Fields
        public Bizz CBZ;
        public UserControl UcMain;
        public CraftGroup CCG = new CraftGroup();



        #endregion

        #region Constructors
        public UcEntrepeneurEdit(Bizz cbz, UserControl ucMain)
        {
            InitializeComponent();
            this.CBZ = cbz;
            this.UcMain = ucMain;

            CBZ.RefreshIndexedList("IndexedEntrepeneurs");
            ListBoxEntrepeneurs.ItemsSource = CBZ.IndexedEntrepeneurs;

            ComboBoxCraftGroup1.ItemsSource = CBZ.IndexedCraftGroups;
            ComboBoxCraftGroup2.ItemsSource = CBZ.IndexedCraftGroups;
            ComboBoxCraftGroup3.ItemsSource = CBZ.IndexedCraftGroups;
            ComboBoxCraftGroup4.ItemsSource = CBZ.IndexedCraftGroups;
            ComboBoxRegion.ItemsSource = CBZ.IndexedRegions;

            ComboBoxCraftGroup1.SelectedIndex = 0;
            ComboBoxCraftGroup2.SelectedIndex = 0;
            ComboBoxCraftGroup3.SelectedIndex = 0;
            ComboBoxCraftGroup4.SelectedIndex = 0;
            ComboBoxRegion.SelectedIndex = 0;

        }

        #endregion

        #region Events
        private void ComboBoxCraftGroup1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CBZ.TempEntrepeneur.CraftGroup1 = new CraftGroup((CraftGroup)ComboBoxCraftGroup1.SelectedItem);
        }

        private void ComboBoxCraftGroup2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CBZ.TempEntrepeneur.CraftGroup2 = new CraftGroup((CraftGroup)ComboBoxCraftGroup2.SelectedItem);
        }

        private void ComboBoxCraftGroup3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CBZ.TempEntrepeneur.CraftGroup3 = new CraftGroup((CraftGroup)ComboBoxCraftGroup3.SelectedItem);
        }

        private void ComboBoxCraftGroup4_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CBZ.TempEntrepeneur.CraftGroup4 = new CraftGroup((CraftGroup)ComboBoxCraftGroup4.SelectedItem);
        }

        private void ComboBoxRegion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CBZ.TempEntrepeneur.Region = new Region((Region)ComboBoxRegion.SelectedItem);
        }

        private void ListBoxEntrepeneurs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CBZ.TempEntrepeneur = new Entrepeneur((Entrepeneur)ListBoxEntrepeneurs.SelectedItem);
            TextBoxName.Text = CBZ.TempEntrepeneur.Entity.Name;
            TextBoxPhone.Text = CBZ.TempEntrepeneur.Entity.ContactInfo.Phone;
            TextBoxFax.Text = CBZ.TempEntrepeneur.Entity.ContactInfo.Fax;
            TextBoxMobile.Text = CBZ.TempEntrepeneur.Entity.ContactInfo.Mobile;
            TextBoxEmail.Text = CBZ.TempEntrepeneur.Entity.ContactInfo.Email;

            ComboBoxCraftGroup1.SelectedIndex = GetCraftGroupIndexFromId(CBZ.TempEntrepeneur.CraftGroup1.Id);
            ComboBoxCraftGroup2.SelectedIndex = GetCraftGroupIndexFromId(CBZ.TempEntrepeneur.CraftGroup2.Id);
            ComboBoxCraftGroup3.SelectedIndex = GetCraftGroupIndexFromId(CBZ.TempEntrepeneur.CraftGroup3.Id);
            ComboBoxCraftGroup4.SelectedIndex = GetCraftGroupIndexFromId(CBZ.TempEntrepeneur.CraftGroup4.Id);
            ComboBoxRegion.SelectedIndex = GetRegionIndexFromId(CBZ.TempEntrepeneur.Region.Id);
            if (CBZ.TempEntrepeneur.CountryWide)
            {
                RadioButtonCountryWideNo.IsChecked = false;
                RadioButtonCountryWideYes.IsChecked = true;
            }
            else
            {
                RadioButtonCountryWideNo.IsChecked = true;
                RadioButtonCountryWideYes.IsChecked = false;
            }

        }

        private void RadioButtonCountryWideYes_Checked(object sender, RoutedEventArgs e)
        {
            RadioButtonCountryWideNo.IsChecked = false;
            RadioButtonCountryWideYes.IsChecked = true;

            if (!CBZ.TempEntrepeneur.CountryWide)
            {
                CBZ.TempEntrepeneur.ToggleCountryWide();
            }

        }

        private void RadioButtonCountryWideNo_Checked(object sender, RoutedEventArgs e)
        {
            RadioButtonCountryWideNo.IsChecked = true;
            RadioButtonCountryWideYes.IsChecked = false;

            if (CBZ.TempEntrepeneur.CountryWide)
            {
                CBZ.TempEntrepeneur.ToggleCountryWide();
            }

        }

        private void TextBoxName_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempEntrepeneur.Entity.Name = TextBoxName.Text;
        }

        private void TextBoxEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempEntrepeneur.Entity.ContactInfo.Email = TextBoxEmail.Text;
        }

        private void TextBoxFax_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempEntrepeneur.Entity.ContactInfo.Fax = TextBoxFax.Text;
        }

        private void TextBoxMobile_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempEntrepeneur.Entity.ContactInfo.Mobile = TextBoxMobile.Text;
        }

        private void TextBoxPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempEntrepeneur.Entity.ContactInfo.Phone = TextBoxPhone.Text;
        }

        #endregion

        #region Buttons
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.TempEntrepeneur != new Entrepeneur())
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
            bool result = UpdateEntrepeneurInDb();

            //Display result
            if (result)
            {
                //Show Confirmation
                MessageBox.Show("Entrepenøren blev opdateret", "Entrepenører", MessageBoxButton.OK, MessageBoxImage.Information);

                //Reset Boxes
                TextBoxName.Text = "";
                TextBoxPhone.Text = "";
                TextBoxFax.Text = "";
                TextBoxMobile.Text = "";
                TextBoxEmail.Text = "";
                ComboBoxCraftGroup1.SelectedIndex = 0;
                ComboBoxCraftGroup2.SelectedIndex = 0;
                ComboBoxCraftGroup3.SelectedIndex = 0;
                ComboBoxCraftGroup4.SelectedIndex = 0;
                ComboBoxRegion.SelectedIndex = 0;

                //Refresh Entrepeneurs list
                CBZ.RefreshList("Entrepeneurs");
                CBZ.TempEntrepeneur = new Entrepeneur();
            }
            else
            {
                //Show error
                MessageBox.Show("Databasen returnerede en fejl. Entrepenøren blev ikke opdateret. Prøv igen.", "Entrepenører", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonUpdateCvr_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.TempEntrepeneur != new Entrepeneur())
            {
                CBZ.CvrApi.UpdateCvrData(CBZ.TempEntrepeneur);
                CBZ.TempEntrepeneur.Entity.Address = CBZ.TempLegalEntity.Address;
                CBZ.TempEntrepeneur.Entity.ContactInfo = CBZ.TempLegalEntity.ContactInfo;
                TextBoxName.Text = CBZ.TempLegalEntity.Name;
                TextBoxPhone.Text = CBZ.TempBuilder.Entity.ContactInfo.Phone;
                TextBoxFax.Text = CBZ.TempBuilder.Entity.ContactInfo.Fax;
                TextBoxMobile.Text = CBZ.TempBuilder.Entity.ContactInfo.Mobile;
                TextBoxEmail.Text = CBZ.TempBuilder.Entity.ContactInfo.Email;
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Method, that retrieves an index from CraftGroup id
        /// </summary>
        /// <param name="craftGroupId"></param>
        /// <returns>int</returns>
        private int GetCraftGroupIndexFromId(int craftGroupId)
        {
            int result = 0;

            foreach (IndexedCraftGroup group in CBZ.IndexedCraftGroups)
            {
                if (group.Id == craftGroupId)
                {
                    result = group.Index;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that retrieves an index from Region id
        /// </summary>
        /// <param name="craftGroupId"></param>
        /// <returns>int</returns>
        private int GetRegionIndexFromId(int regionId)
        {
            int result = 0;

            foreach (IndexedRegion region in CBZ.IndexedRegions)
            {
                if (region.Id == regionId)
                {
                    result = region.Index;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that creates Contact Info in Db
        /// </summary>
        /// <returns>bool</returns>
        private bool UpdateContactInfo => CBZ.UpdateInDb(CBZ.TempEntrepeneur.Entity.ContactInfo);

        /// <summary>
        /// Method, that creates an Entrepeneur in Db
        /// </summary>
        /// <returns>bool</returns>
        private bool UpdateEntrepeneurInDb()
        {
            //Code that creates a new Entrepeneur
            bool result = false;

            //Update ContactInfo in Db
            bool contactInfoUpdated = UpdateContactInfo;

            //Update LegalEntity in Db
            bool legalEntityUpdated = UpdateLegalEntity;

            //Create Entrepeneur in Db
            if (contactInfoUpdated && legalEntityUpdated)
            {
                result = CBZ.UpdateInDb(CBZ.TempEntrepeneur);
            }

            return result;
        }

        /// <summary>
        /// Method, that creates a Legal Entity in Db
        /// </summary>
        /// <returns>bool</returns>
        private bool UpdateLegalEntity => CBZ.UpdateInDb(CBZ.TempEntrepeneur.Entity);

        #endregion

    }
}
