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
    /// Interaction logic for UcUsersStatusChange.xaml
    /// </summary>
    public partial class UcUsersStatusChange : UserControl
    {
        #region Fields
        public Bizz CBZ;
        public UserControl UcMain;

        public List<IndexedUser> FilteredUsers = new List<IndexedUser>();

        #endregion

        #region Constructors
        public UcUsersStatusChange(Bizz cbz, UserControl ucMain)
        {
            InitializeComponent();
            this.CBZ = cbz;
            this.UcMain = ucMain;

            GetFilteredUsers();
            ListBoxUsers.ItemsSource = FilteredUsers;
            ListBoxUsers.SelectedIndex = -1;
            ComboBoxUserAccess.ItemsSource = CBZ.IndexedUserLevels;
            ComboBoxUserAccess.SelectedIndex = -1;


        }

        #endregion

        #region Buttons
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                //Warning about lost changes before closing
                if (MessageBox.Show("Vil du lukke 'Ændr Brugeradgang'? Ikke gemte data mistes.", "Brugere", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
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
            bool result = UpdateAuthenticationInDb;

            //Display result
            if (result)
            {
                //Show Confirmation
                MessageBox.Show("Brugeradgangen blev opdateret", "Brugere", MessageBoxButton.OK, MessageBoxImage.Information);

                //Reset Boxes
                ListBoxUsers.SelectedIndex = -1;
                ListBoxUsers.ItemsSource = "";
                ComboBoxUserAccess.SelectedItem = -1;
                TextBoxUserSearch.Text = "";
                TextBoxName.Text = "";
                
                //Refresh Users list
                CBZ.RefreshList("Authentications");
                CBZ.TempUser = new User();
            }
            else
            {
                //Show error
                MessageBox.Show("Databasen returnerede en fejl. Brugeradgangen blev ikke opdateret. Prøv igen.", "Brugere", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region Events
        private void ComboBoxUserAccess_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CBZ.TempUser.Authentication = new Authentication((Authentication)ComboBoxUserAccess.SelectedItem);

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
            ComboBoxUserAccess.SelectedIndex = GetUserAccessIndex();

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxUserSearch_TextChanged(object sender, TextChangedEventArgs e)
        {

            GetFilteredUsers();
            ListBoxUsers.SelectedIndex = -1;
            ListBoxUsers.ItemsSource = "";
            ListBoxUsers.ItemsSource = this.FilteredUsers;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Method, that retrieves a list of filtered Categories for ListBoxCraftCategories
        /// </summary>
        private void GetFilteredUsers()
        {
            CBZ.RefreshIndexedList("IndexedUsers");
            this.FilteredUsers = new List<IndexedUser>();
            int length = TextBoxUserSearch.Text.Length;

            foreach (IndexedUser user in CBZ.IndexedUsers)
            {
                if (user.Person.Name.Remove(length) == TextBoxUserSearch.Text.Remove(length))
                {
                    this.FilteredUsers.Add(user);
                }
            }
        }

        /// <summary>
        /// Method, that retrieves a UserLevel index for a selected User
        /// </summary>
        /// <returns></returns>
        private int GetUserAccessIndex()
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
        /// Method, that updates an Authentication in Db
        /// </summary>
        /// <returns>bool</returns>
        private bool UpdateAuthenticationInDb => CBZ.UpdateInDb(CBZ.TempUser.Authentication);


        #endregion

    }
}
