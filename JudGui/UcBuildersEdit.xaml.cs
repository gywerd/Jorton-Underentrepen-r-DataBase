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
    /// Interaction logic for UcBuildersEdit.xaml
    /// </summary>
    public partial class UcBuildersEdit : UserControl
    {
        #region Fields
        public Bizz CBZ;
        public UserControl UcMain;

        public List<Builder> FilteredBuilders = new List<Builder>();

        #endregion

        #region Constructors
        public UcBuildersEdit(Bizz cbz, UserControl ucMain)
        {
            InitializeComponent();
            this.CBZ = cbz;
            this.UcMain = ucMain;
            CBZ.TempBuilder = new Builder();

            GetFilteredBuilders();
            ListBoxBuilders.ItemsSource = FilteredBuilders;

        }

        #endregion

        #region Buttons
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.TempBuilder != new Builder())
            {
                //Warning about lost changes before closing
                if (MessageBox.Show("Vil du lukke redigering af Bygherrer? Alle ugemte data mistes.", "Bygherrer", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
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
            bool result = UpdateBuilderInDb();

            //Display result
            if (result)
            {
                //Show Confirmation
                MessageBox.Show("Bygherren blev opdateret", "Bygherrer", MessageBoxButton.OK, MessageBoxImage.Information);

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

        private void ButtonUpdateCvr_Click(object sender, RoutedEventArgs e)
        {
            UpdateData updatedData = new UpdateData();
            if (CBZ.TempBuilder != new Builder())
            {
                updatedData = CBZ.CvrApi.UpdateCvrData(CBZ.TempBuilder.Entity);
                CBZ.TempBuilder.Entity = updatedData.Entity;
                bool updated = CBZ.UpdateInDb(updatedData.Entity);

                if (updated)
                {
                    int index = ListBoxBuilders.SelectedIndex;
                    ListBoxBuilders.SelectedIndex = -1;
                    GetFilteredBuilders();
                    ListBoxBuilders.SelectedIndex = index;
                }
            }

        }

        #endregion

        #region Events
        private void ListBoxBuilders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBoxBuilders.SelectedIndex >= 0)
            {
                CBZ.TempBuilder = new Builder((Builder)ListBoxBuilders.SelectedItem);
                TextBoxName.Text = CBZ.TempBuilder.Entity.Name;
                TextBoxCoName.Text = CBZ.TempBuilder.Entity.CoName;
                TextBoxStreet.Text = CBZ.TempBuilder.Entity.Address.Street;
                TextBoxPlace.Text = CBZ.TempBuilder.Entity.Address.Place;
                TextBoxZip.Text = CBZ.TempBuilder.Entity.Address.ZipTown.Zip;
                TextBoxTown.Text = CBZ.TempBuilder.Entity.Address.ZipTown.Town;
                TextBoxPhone.Text = CBZ.TempBuilder.Entity.ContactInfo.Phone;
                TextBoxFax.Text = CBZ.TempBuilder.Entity.ContactInfo.Fax;
                TextBoxMobile.Text = CBZ.TempBuilder.Entity.ContactInfo.Mobile;
                TextBoxEmail.Text = CBZ.TempBuilder.Entity.ContactInfo.Email;

                //Set CBZ.UcMainEdited
                if (!CBZ.UcMainEdited)
                {
                    CBZ.UcMainEdited = true;
                }

            }
            else
            {
                CBZ.TempBuilder = new Builder();
                TextBoxName.Text = "";
                TextBoxCoName.Text = "";
                TextBoxStreet.Text = "";
                TextBoxPlace.Text = "";
                TextBoxZip.Text = "";
                TextBoxTown.Text = "";
                TextBoxPhone.Text = CBZ.TempBuilder.Entity.ContactInfo.Phone;
                TextBoxFax.Text = "";
                TextBoxMobile.Text = "";
                TextBoxEmail.Text = "";

                //Set CBZ.UcMainEdited
                if (CBZ.UcMainEdited)
                {
                    CBZ.UcMainEdited = false;
                }

            }

        }

        private void TextBoxName_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempBuilder.Entity.Name = TextBoxName.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }

        }

        private void TextBoxCoName_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempBuilder.Entity.CoName = TextBoxCoName.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }

        }

        private void TextBoxStreet_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempBuilder.Entity.Address.Street = TextBoxStreet.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }

        }

        private void TextBoxPlace_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempBuilder.Entity.Address.Place = TextBoxPlace.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }

        }

        private void TextBoxZip_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempZipTown = GetZipTown(TextBoxZip.Text);
            CBZ.TempBuilder.Entity.Address.ZipTown = CBZ.TempZipTown;
            TextBoxTown.Text = CBZ.TempZipTown.Town;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }

        }

        private void TextBoxEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempBuilder.Entity.ContactInfo.Email = TextBoxEmail.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxFax_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempBuilder.Entity.ContactInfo.Fax = TextBoxFax.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxMobile_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempBuilder.Entity.ContactInfo.Mobile = TextBoxMobile.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempBuilder.Entity.ContactInfo.Phone = TextBoxPhone.Text;

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
        /// Method, that retrieves a ZipTown
        /// </summary>
        /// <param name="zip"></param>
        /// <returns></returns>
        private ZipTown GetZipTown(string zip)
        {
            ZipTown result = new ZipTown();

            foreach (ZipTown zipTown in CBZ.ZipTowns)
            {
                if (zipTown.Zip == zip)
                {
                    result = zipTown;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that creates Address in Db
        /// </summary>
        /// <returns>bool</returns>
        private bool UpdateAddress => CBZ.UpdateInDb(CBZ.TempBuilder.Entity.Address);

        /// <summary>
        /// Method, that creates Contact Info in Db
        /// </summary>
        /// <returns>bool</returns>
        private bool UpdateContactInfo => CBZ.UpdateInDb(CBZ.TempBuilder.Entity.ContactInfo);

        /// <summary>
        /// Method, that creates an Builder in Db
        /// </summary>
        /// <returns>bool</returns>
        private bool UpdateBuilderInDb()
        {
            bool result = false;

            //Update Address in Db
            bool addressUpdated = UpdateAddress;

            //Update ContactInfo in Db
            bool contactInfoUpdated = UpdateContactInfo;

            //Update LegalEntity in Db
            bool legalEntityUpdated = UpdateLegalEntity;

            //Create Entrepeneur in Db
            if (addressUpdated && contactInfoUpdated && legalEntityUpdated)
            {
                result = CBZ.UpdateInDb(CBZ.TempBuilder);
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
