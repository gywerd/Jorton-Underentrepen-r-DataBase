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
    /// Interaction logic for UcCraftGroups.xaml
    /// </summary>
    public partial class UcCraftGroups : UserControl
    {
        #region Fields
        public Bizz CBZ;
        public UserControl UcMain;
        public CraftGroup TempNewCraftGroup = new CraftGroup();

        public List<IndexedCraftGroup> FilteredCraftGroups = new List<IndexedCraftGroup>();

        #endregion

        #region Constructors
        public UcCraftGroups(Bizz cbz, UserControl ucMain)
        {
            InitializeComponent();
            this.CBZ = cbz;
            this.UcMain = ucMain;

            CBZ.TempCraftGroup = new CraftGroup();

            GetFilteredCraftGroups();
            ListBoxCraftGroups.ItemsSource = FilteredCraftGroups;
            ListBoxCraftGroups.SelectedIndex = -1;

            CBZ.RefreshIndexedList("IndexedCategories");
            ComboBoxCategory.ItemsSource = CBZ.IndexedCategories;
            ComboBoxCategory.SelectedIndex = -1;
        }

        #endregion

        #region Buttons
        private void ButtonAddCraftGroup_Click(object sender, RoutedEventArgs e)
        {
            bool result = CreateCraftGroupInDb();

            //Display result
            if (result)
            {
                //Show Confirmation
                MessageBox.Show("Faggruppen blev tilføjet", "Faggrupper", MessageBoxButton.OK, MessageBoxImage.Information);

                //Reset Boxes
                ListBoxCraftGroups.SelectedIndex = -1;
                ListBoxCraftGroups.ItemsSource = "";
                CBZ.RefreshIndexedList("IndexedCraftGroups");
                ListBoxCraftGroups.ItemsSource = CBZ.IndexedCraftGroups;
                TextBoxCraftGroupSearch.Text = "";
                TextBoxDesignation.Text = "";
                TextBoxDescription.Text = "";
                ComboBoxCategory.SelectedIndex = -1;
                ComboBoxCategory.ItemsSource = "";
                TextBoxNewDesignation.Text = "";
                TextBoxNewDescription.Text = "";
                ComboBoxNewCategory.SelectedIndex = -1;
                ComboBoxNewCategory.ItemsSource = "";

                //Refresh Users list
                CBZ.RefreshList("CraftGroups");
                CBZ.TempCraftGroup = new CraftGroup();
            }
            else
            {
                //Show error
                MessageBox.Show("Databasen returnerede en fejl. Faggruppen blev ikke tilføjet. Prøv igen.", "Faggrupper", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                //Warning about lost changes before closing
                if (MessageBox.Show("Vil du lukke Faggrupper? Ikke gemte data mistes.", "Faggrupper", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
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
            bool result = UpdateCraftGroupInDb;

            //Display result
            if (result)
            {
                //Show Confirmation
                MessageBox.Show("Faggruppen blev opdateret", "Faggrupper", MessageBoxButton.OK, MessageBoxImage.Information);

                //Reset Boxes
                ListBoxCraftGroups.SelectedIndex = -1;
                ListBoxCraftGroups.ItemsSource = "";
                TextBoxCraftGroupSearch.Text = "";
                TextBoxDesignation.Text = "";
                TextBoxDescription.Text = "";
                ComboBoxCategory.SelectedIndex = -1;
                ComboBoxCategory.ItemsSource = "";
                TextBoxNewDesignation.Text = "";
                TextBoxNewDescription.Text = "";
                ComboBoxNewCategory.SelectedIndex = -1;
                ComboBoxNewCategory.ItemsSource = "";

                //Refresh CraftGroups list
                CBZ.RefreshList("CraftGroups");
                CBZ.TempCraftGroup = new CraftGroup();
            }
            else
            {
                //Show error
                MessageBox.Show("Databasen returnerede en fejl. Faggruppen blev ikke opdateret. Prøv igen.", "Faggrupper", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region Events
        private void ComboBoxCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CBZ.TempCraftGroup.Category = new Category((Category)ComboBoxCategory.SelectedItem);

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void ComboBoxNewCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TempNewCraftGroup.Category = new Category((Category)ComboBoxNewCategory.SelectedItem);

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void ListBoxCraftGroups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CBZ.TempCraftGroup = new CraftGroup((CraftGroup)ListBoxCraftGroups.SelectedItem);

            TextBoxDesignation.Text = CBZ.TempCraftGroup.Designation;
            TextBoxDescription.Text = CBZ.TempCraftGroup.Description;
            ComboBoxCategory.ItemsSource = CBZ.IndexedCraftGroups;
            ComboBoxCategory.SelectedIndex = GetCategoryIndex();

            this.TempNewCraftGroup = new CraftGroup();
            TextBoxNewDesignation.Text = "";
            TextBoxNewDescription.Text = "";
            ComboBoxNewCategory.SelectedIndex = -1;
            ComboBoxNewCategory.ItemsSource = "";

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxCraftGroupSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            GetFilteredCraftGroups();
            ListBoxCraftGroups.SelectedIndex = -1;
            ListBoxCraftGroups.ItemsSource = "";
            ListBoxCraftGroups.ItemsSource = this.FilteredCraftGroups;


            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempCraftGroup.Description = TextBoxDescription.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxDesignation_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempCraftGroup.Designation = TextBoxDesignation.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxNewDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            TempNewCraftGroup.Description = TextBoxNewDescription.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxNewDesignation_TextChanged(object sender, TextChangedEventArgs e)
        {
            TempNewCraftGroup.Designation = TextBoxNewDesignation.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Method, that creates a CraftGroup in Db
        /// </summary>
        /// <returns>bool</returns>
        private bool CreateCraftGroupInDb()
        {
            bool result = false;

            int craftGroupId = CBZ.CreateInDb(TempNewCraftGroup);
            
            if (craftGroupId >= 1)
            {
                result = true;
            }

            return result;

        }

        /// <summary>
        /// Method, that returns index for Job Description
        /// </summary>
        /// <returns>int</returns>
        private int GetCategoryIndex()
        {
            int result = 0;

            foreach (IndexedCategory category in CBZ.IndexedCategories)
            {
                if (CBZ.TempCategory.Id == category.Id)
                {
                    result = category.Index;

                    break;
                }
            }

            return result;

        }

        /// <summary>
        /// Method, that retrieves a list of filtered CraftGroups for ListBoxCraftGroups
        /// </summary>
        private void GetFilteredCraftGroups()
        {
            CBZ.RefreshIndexedList("IndexedCraftGroups");
            this.FilteredCraftGroups = new List<IndexedCraftGroup>();
            int length = TextBoxCraftGroupSearch.Text.Length;

            foreach (IndexedCraftGroup group in CBZ.IndexedCraftGroups)
            {
                if (group.Designation.Remove(length) == TextBoxCraftGroupSearch.Text)
                {
                    this.FilteredCraftGroups.Add(group);
                }
            }
        }

        /// <summary>
        /// Method, that updates an Craft Group in Db
        /// </summary>
        /// <returns>bool</returns>
        private bool UpdateCraftGroupInDb => CBZ.UpdateInDb(CBZ.TempCraftGroup);

        #endregion

    }
}
