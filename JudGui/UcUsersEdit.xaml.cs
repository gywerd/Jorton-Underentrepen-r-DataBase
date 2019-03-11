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
    /// Interaction logic for UcUsersEdit.xaml
    /// </summary>
    public partial class UcUsersEdit : UserControl
    {
        #region Fields
        public Bizz CBZ;
        public UserControl UcMain;

        public List<IndexedUser> FilteredUsers = new List<IndexedUser>();
        #endregion

        #region Constructors
        public UcUsersEdit(Bizz cbz, UserControl ucMain)
        {
            InitializeComponent();
            this.CBZ = cbz;
            this.UcMain = ucMain;

            CBZ.RefreshIndexedList("IndexedJobDescriptions");
            CBZ.RefreshIndexedList("IndexedUserLevels");
        }


        #endregion

        #region Buttons
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.TempUser != new User())
            {
                //Warning about lost changes before closing
                if (MessageBox.Show("Vil du lukke redigering af Brugere? Ikke gemte data mistes.", "Brugere", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
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

        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            bool result = UpdateUserInDb();

            //Display result
            if (result)
            {
                //Show Confirmation
                MessageBox.Show("Brugeren blev opdateret", "Brugere", MessageBoxButton.OK, MessageBoxImage.Information);

                //Reset Boxes
                ListBoxUsers.SelectedIndex = -1;
                ListBoxUsers.ItemsSource = "";
                TextBoxUserSearch.Text = "";
                TextBoxName.Text = "";
                TextBoxPhone.Text = "";
                TextBoxFax.Text = "";
                TextBoxMobile.Text = "";
                TextBoxEmail.Text = "";
                TextBoxInitials.Text = "";
                ComboBoxJobDescription.SelectedIndex = -1;
                ComboBoxJobDescription.ItemsSource = "";
                ComboBoxUserLevel.SelectedIndex = -1;
                ComboBoxUserLevel.ItemsSource = "";

                //Refresh Users list
                CBZ.RefreshList("Users");
                CBZ.TempUser = new User();
            }
            else
            {
                //Show error
                MessageBox.Show("Databasen returnerede en fejl. Bygherren blev ikke opdateret. Prøv igen.", "Bygherrer", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region Events
        private void ComboBoxJobDescription_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CBZ.TempUser.JobDescription = new JobDescription((JobDescription)ComboBoxJobDescription.SelectedItem);

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void ComboBoxUserLevel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CBZ.TempUser.Authentication.UserLevel = new UserLevel((UserLevel)ComboBoxJobDescription.SelectedItem);

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void ListBoxUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CBZ.TempUser = new User((User)ListBoxUsers.SelectedItem);

            TextBoxName.Text = CBZ.TempUser.Person.Name;
            TextBoxPhone.Text = CBZ.TempUser.Person.ContactInfo.Phone;
            TextBoxFax.Text = CBZ.TempUser.Person.ContactInfo.Fax;
            TextBoxMobile.Text = CBZ.TempUser.Person.ContactInfo.Mobile;
            TextBoxEmail.Text = CBZ.TempUser.Person.ContactInfo.Email;
            TextBoxInitials.Text = CBZ.TempUser.Initials;
            ComboBoxJobDescription.ItemsSource = CBZ.IndexedJobDescriptions;
            ComboBoxJobDescription.SelectedIndex = GetJobDescriptionIndex();
            ComboBoxUserLevel.ItemsSource = CBZ.IndexedUserLevels;
            ComboBoxUserLevel.SelectedIndex = GetUserLevelIndex();

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempUser.Person.ContactInfo.Email = TextBoxEmail.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxFax_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempUser.Person.ContactInfo.Fax = TextBoxFax.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxInitials_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempUser.Initials = TextBoxInitials.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxMobile_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempUser.Person.ContactInfo.Mobile = TextBoxMobile.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxName_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempUser.Person.Name = TextBoxName.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempUser.Person.ContactInfo.Phone = TextBoxPhone.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxUserSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBoxUserSearch.Text.Length >= 3)
            {
                GetFilteredUsers();
                ListBoxUsers.SelectedIndex = -1;
                ListBoxUsers.ItemsSource = "";
                ListBoxUsers.ItemsSource = this.FilteredUsers;
            }


            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Method, that retrieves a list of filtered Users for ListBoxUsers
        /// </summary>
        private void GetFilteredUsers()
        {
            CBZ.RefreshIndexedList("IndexedUsers");
            this.FilteredUsers = new List<IndexedUser>();

            foreach (IndexedUser user in CBZ.IndexedUsers)
            {
                if (user.Person.Name.Remove(3) == TextBoxUserSearch.Text.Remove(3))
                {
                    this.FilteredUsers.Add(user);
                }
            }
        }

        /// <summary>
        /// Method, that returns index for Job Description
        /// </summary>
        /// <returns>int</returns>
        private int GetJobDescriptionIndex()
        {
            int result = 0;

            foreach (IndexedJobDescription description in CBZ.IndexedJobDescriptions)
            {
                if (CBZ.TempUser.JobDescription.Id == description.Id)
                {
                    result = description.Index;

                    break;
                }
            }

            return result;

        }

        /// <summary>
        /// Method, that returns index for User Level
        /// </summary>
        /// <returns>int</returns>
        private int GetUserLevelIndex()
        {
            int result = 0;

            foreach (IndexedUserLevel level in CBZ.IndexedUserLevels)
            {
                if (CBZ.TempUser.Authentication.UserLevel.Id == level.Id)
                {
                    result = level.Index;

                    break;
                }
            }

            return result;

        }

        /// <summary>
        /// Method, that creates Contact Info in Db
        /// </summary>
        /// <returns>bool</returns>
        private bool UpdateContactInfo => CBZ.UpdateInDb(CBZ.TempBuilder.Entity.ContactInfo);

        /// <summary>
        /// Method, that creates a Legal Entity in Db
        /// </summary>
        /// <returns>bool</returns>
        private bool UpdatePerson => CBZ.UpdateInDb(CBZ.TempUser.Person);

        /// <summary>
        /// Method, that creates an Builder in Db
        /// </summary>
        /// <returns>bool</returns>
        private bool UpdateUserInDb()
        {
            bool result = false;

            //Update ContactInfo in Db
            bool contactInfoUpdated = UpdateContactInfo;

            //Update LegalEntity in Db
            bool personUpdated = UpdatePerson;

            //Create Entrepeneur in Db
            if (contactInfoUpdated && personUpdated)
            {
                result = CBZ.UpdateInDb(CBZ.TempUser);
            }

            return result;
        }

        #endregion

    }
}
