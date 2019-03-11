using CvrApiServices;
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
    /// Interaction logic for UcEntrepeneurCreate.xaml
    /// </summary>
    public partial class UcEntrepeneurCreate : UserControl
    {
        #region Fields
        public Bizz CBZ;
        public UserControl UcMain;

        #endregion

        #region Constructors
        public UcEntrepeneurCreate(Bizz cbz, UserControl ucMain)
        {
            InitializeComponent();
            this.CBZ = cbz;
            this.UcMain = ucMain;
            CBZ.TempEntrepeneur = new Entrepeneur();
        }

        #endregion

        #region Events
        private void ComboBoxCraftGroup1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CBZ.TempEntrepeneur.CraftGroup1 = new CraftGroup((CraftGroup)ComboBoxCraftGroup1.SelectedItem);

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void ComboBoxCraftGroup2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CBZ.TempEntrepeneur.CraftGroup2 = new CraftGroup((CraftGroup)ComboBoxCraftGroup2.SelectedItem);

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void ComboBoxCraftGroup3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CBZ.TempEntrepeneur.CraftGroup3 = new CraftGroup((CraftGroup)ComboBoxCraftGroup3.SelectedItem);

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void ComboBoxCraftGroup4_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CBZ.TempEntrepeneur.CraftGroup4 = new CraftGroup((CraftGroup)ComboBoxCraftGroup4.SelectedItem);

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void ComboBoxRegion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CBZ.TempEntrepeneur.Region = new Region((Region)ComboBoxRegion.SelectedItem);

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
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

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
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

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }

        }

        private void TextBoxCvr_TextChanged(object sender, TextChangedEventArgs e)
        {
                CBZ.TempEntrepeneur.Entity.Cvr = TextBoxCvr.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxName_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempEntrepeneur.Entity.Name = TextBoxName.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxCoName_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempEntrepeneur.Entity.CoName = TextBoxCoName.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempEntrepeneur.Entity.ContactInfo.Email = TextBoxEmail.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxFax_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempEntrepeneur.Entity.ContactInfo.Fax = TextBoxFax.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxMobile_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempEntrepeneur.Entity.ContactInfo.Mobile = TextBoxMobile.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempEntrepeneur.Entity.ContactInfo.Phone = TextBoxPhone.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxPlace_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempEntrepeneur.Entity.Address.Place = TextBoxPlace.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxStreet_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempEntrepeneur.Entity.Address.Street = TextBoxStreet.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxZip_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.RetrieveZipTownFromZip(TextBoxZip.Text);
            if(CBZ.TempZipTown.Id != 0)
            {
                CBZ.TempEntrepeneur.Entity.Address.ZipTown = CBZ.TempZipTown;
            }
            TextBoxTown.Text = CBZ.RetrieveTownFromZip(TextBoxZip.Text);

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        #endregion

        #region Buttons
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.TempEntrepeneur != new Entrepeneur())
            {
                //Warning about lost changes before closing
                if (MessageBox.Show("Vil du annullere oprettelse af Entrepenør? Ikke gemte data mistes.", "Entrepenører", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    //Refresh Entrepeneurs list
                    CBZ.RefreshList("Entrepeneurs");
                    CBZ.TempEnterprise = new Enterprise();

                    //Close right UserControl
                    CBZ.UcMainEdited = false;
                    UcMain.Content = new UserControl();
                }
            }
            else
            {
                //Refresh Entrepeneurs list
                CBZ.RefreshList("Entrepeneurs");
                CBZ.TempEnterprise = new Enterprise();

                //Close main UserControl
                CBZ.UcMainEdited = false;
                UcMain.Content = new UserControl();
            }

        }

        private void ButtonCreateClose_Click(object sender, RoutedEventArgs e)
        {
            //Code that creates a new Entrepeneur
            bool result = CreateEntrepeneurInDb();

            //Display result
            if (result)
            {
                //Show Confirmation
                MessageBox.Show("Entrepenøren blev oprettet", "Entrepenører", MessageBoxButton.OK, MessageBoxImage.Information);

                //Refresh Entrepeneurs list
                CBZ.RefreshList("Entrepeneurs");
                CBZ.TempEnterprise = new Enterprise();

                //Close right UserControl
                CBZ.UcMainEdited = false;
                UcMain.Content = new UserControl();
            }
            else
            {
                //Show error
                MessageBox.Show("Databasen returnerede en fejl. Entrepenøren blev ikke oprettet. Prøv igen.", "Entrepenører", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void ButtonCreateNew_Click(object sender, RoutedEventArgs e)
        {
            bool result = CreateEntrepeneurInDb();

            //Display result
            if (result)
            {
                //Show Confirmation
                MessageBox.Show("Entrepenøren blev oprettet", "Entrepenører", MessageBoxButton.OK, MessageBoxImage.Information);

                //Refresh Entrepeneurs list
                CBZ.RefreshList("Entrepeneurs");
                CBZ.TempEnterprise = new Enterprise();

                //Reset Boxes
                TextBoxName.Text = "";
                TextBoxCoName.Text = "";
                TextBoxStreet.Text = "";
                TextBoxPlace.Text = "";
                TextBoxZip.Text = "";
                TextBoxTown.Text = "";
                TextBoxPhone.Text = "";
                TextBoxFax.Text = "";
                TextBoxMobile.Text = "";
                TextBoxEmail.Text = "";
                ComboBoxCraftGroup1.SelectedIndex = 0;
                ComboBoxCraftGroup2.SelectedIndex = 0;
                ComboBoxCraftGroup3.SelectedIndex = 0;
                ComboBoxCraftGroup4.SelectedIndex = 0;
                ComboBoxRegion.SelectedIndex = 0;

            }
            else
            {
                //Show error
                MessageBox.Show("Databasen returnerede en fejl. Entreprenøren blev ikke oprettet. Prøv igen.", "Entrepenører", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void ButtonSearchCvr_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxCvr.Text != "")
            {
                if (TextBoxCvr.Text.Length == 8 || TextBoxCvr.Text.Length == 10)
                {
                    CBZ.CvrApi.GetCvrData(TextBoxCvr.Text, CBZ.TempLegalEntity);
                    CBZ.TempEntrepeneur.Entity = CBZ.TempLegalEntity;
                    TextBoxName.Text = CBZ.TempEntrepeneur.Entity.Name;
                    TextBoxCoName.Text = CBZ.TempEntrepeneur.Entity.CoName;
                    TextBoxStreet.Text = CBZ.TempEntrepeneur.Entity.Address.Street;
                    TextBoxPlace.Text = CBZ.TempEntrepeneur.Entity.Address.Place;
                    TextBoxZip.Text = CBZ.TempEntrepeneur.Entity.Address.ZipTown.Zip;
                    TextBoxPhone.Text = CBZ.TempEntrepeneur.Entity.ContactInfo.Phone;
                    TextBoxFax.Text = CBZ.TempEntrepeneur.Entity.ContactInfo.Fax;
                    TextBoxMobile.Text = CBZ.TempEntrepeneur.Entity.ContactInfo.Mobile;
                    TextBoxEmail.Text = CBZ.TempEntrepeneur.Entity.ContactInfo.Email;
                }
            }
        }

        #endregion

        #region Properties

        #endregion

        #region Methods
        /// <summary>
        /// Method, that creates an Address in Db
        /// </summary>
        /// <returns>bool</returns>
        private bool CreateAddressInDb()
        {
            bool result = false;

            try
            {
                int addressId = CBZ.CreateInDb(CBZ.TempEntrepeneur.Entity.Address);
                CBZ.TempEntrepeneur.Entity.Address.SetId(addressId);
                result = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Addressen blev ikke gemt\n" + ex, "Opret Entrepriseliste", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return result;
        }

        /// <summary>
        /// Method, that creates Contact Info in Db
        /// </summary>
        /// <returns>bool</returns>
        private bool CreateContactInfo()
        {
            bool result = false;

            try
            {
                int contactInfoId = CBZ.CreateInDb(CBZ.TempEntrepeneur.Entity.ContactInfo);
                CBZ.TempEntrepeneur.Entity.ContactInfo.SetId(contactInfoId);
                result = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kontaktoplysningerne blev ikke gemt\n" + ex, "Opret Entrepriseliste", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return result;
        }

        /// <summary>
        /// Method, that creates an Entrepeneur in Db
        /// </summary>
        /// <returns>bool</returns>
        private bool CreateEntrepeneurInDb()
        {
            //Code that creates a new Entrepeneur
            bool result = false;

            //Create Address in Db
            bool addressExist = CreateAddressInDb();


            //Create ContactInfo in Db
            bool contactInfoExist = CreateContactInfo();

            //Create LegalEntity in Db
            bool legalEntityExists = false;

            if (addressExist && contactInfoExist)
            {
                legalEntityExists = CreateLegalEntity();
            }

            //Create Entrepeneur in Db
            int entrepeneurId = 0;

            if (legalEntityExists && addressExist && contactInfoExist)
            {
                entrepeneurId = CBZ.CreateInDb(CBZ.TempEntrepeneur);
            }

            //Check result
            if (entrepeneurId >= 1)
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Method, that creates a Legal Entity in Db
        /// </summary>
        /// <returns>bool</returns>
        private bool CreateLegalEntity()
        {
            bool result = false;

            try
            {
                int entityId = CBZ.CreateInDb(CBZ.TempEntrepeneur.Entity);
                CBZ.TempEntrepeneur.Entity.SetId(entityId);
                result = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Firmaet blev ikke gemt\n" + ex, "Opret Entrepriseliste", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return result;
        }

        #endregion

    }
}
