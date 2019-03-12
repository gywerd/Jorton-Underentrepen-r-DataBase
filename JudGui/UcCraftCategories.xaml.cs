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
    /// Interaction logic for UcCraftCategories.xaml
    /// </summary>
    public partial class UcCraftCategories : UserControl
    {
        #region Fields
        public Bizz CBZ;
        public UserControl UcMain;
        public Category TempNewCategory = new Category();

        public List<IndexedCategory> FilteredCategories = new List<IndexedCategory>();
        #endregion

        #region Constructors
        public UcCraftCategories(Bizz cbz, UserControl ucMain)
        {
            InitializeComponent();
            this.CBZ = cbz;
            this.UcMain = ucMain;

            GetFilteredCategories();
            ListBoxCraftCategories.ItemsSource = FilteredCategories;
        }

        #endregion

        #region Buttons
        private void ButtonAddCraftCategories_Click(object sender, RoutedEventArgs e)
        {
            bool result = CreateCategoryInDb();

            //Display result
            if (result)
            {
                //Show Confirmation
                MessageBox.Show("Kategorien blev tilføjet", "Fagkategorier", MessageBoxButton.OK, MessageBoxImage.Information);

                //Reset Boxes
                ListBoxCraftCategories.SelectedIndex = -1;
                ListBoxCraftCategories.ItemsSource = "";
                CBZ.RefreshIndexedList("IndexedCategories");
                ListBoxCraftCategories.ItemsSource = CBZ.IndexedCraftGroups;
                TextBoxCraftCategorySearch.Text = "";
                TextBoxText.Text = "";
                TextBoxNewText.Text = "";

                //Refresh Users list
                CBZ.RefreshList("ProjectStatusses");
                CBZ.TempProjectStatus = new ProjectStatus();
            }
            else
            {
                //Show error
                MessageBox.Show("Databasen returnerede en fejl. Kategorien blev ikke tilføjet. Prøv igen.", "Fagkategorier", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                //Warning about lost changes before closing
                if (MessageBox.Show("Vil du lukke Fagkategorier? Ikke gemte data mistes.", "Fagkategorier", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
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
            bool result = UpdateCategoryInDb;

            //Display result
            if (result)
            {
                //Show Confirmation
                MessageBox.Show("Projektstatussen blev opdateret", "Projektstatusser", MessageBoxButton.OK, MessageBoxImage.Information);

                //Reset Boxes
                ListBoxCraftCategories.SelectedIndex = -1;
                ListBoxCraftCategories.ItemsSource = "";
                TextBoxCraftCategorySearch.Text = "";
                TextBoxText.Text = "";
                TextBoxNewText.Text = "";

                //Refresh Users list
                CBZ.RefreshList("Categories");
                CBZ.TempCategory = new Category();
            }
            else
            {
                //Show error
                MessageBox.Show("Databasen returnerede en fejl. Projektstatussen blev ikke opdateret. Prøv igen.", "Projektstatusser", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        #endregion

        #region Events
        private void ListBoxCraftCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                CBZ.TempCategory = new Category((Category)ListBoxCraftCategories.SelectedItem);

                TextBoxText.Text = CBZ.TempCategory.Text;

                this.TempNewCategory = new IndexedCategory();
                TextBoxNewText.Text = "";

                //Set CBZ.UcMainEdited
                if (!CBZ.UcMainEdited)
                {
                    CBZ.UcMainEdited = true;
                }
            }

        private void TextBoxCraftCategorySearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            GetFilteredCategories();
            ListBoxCraftCategories.SelectedIndex = -1;
            ListBoxCraftCategories.ItemsSource = "";
            ListBoxCraftCategories.ItemsSource = this.FilteredCategories;


            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxNewText_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.TempNewCategory.Text = TextBoxNewText.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxText_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempCategory.Text = TextBoxText.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Method, that creates a Project Status in Db
        /// </summary>
        /// <returns>bool</returns>
        private bool CreateCategoryInDb()
        {
            bool result = false;

            int projectStatusId = CBZ.CreateInDb(TempNewCategory);

            if (projectStatusId >= 1)
            {
                result = true;
            }

            return result;

        }

        /// <summary>
        /// Method, that retrieves a list of filtered Categories for ListBoxCraftCategories
        /// </summary>
        private void GetFilteredCategories()
        {
            CBZ.RefreshIndexedList("IndexedCategories");
            this.FilteredCategories = new List<IndexedCategory>();
            int length = TextBoxCraftCategorySearch.Text.Length;

            foreach (IndexedCategory category in CBZ.IndexedCategories)
            {
                if (category.Text.Remove(length) == TextBoxCraftCategorySearch.Text.Remove(length))
                {
                    this.FilteredCategories.Add(category);
                }
            }
        }

        /// <summary>
        /// Method, that updates an Category in Db
        /// </summary>
        /// <returns>bool</returns>
        private bool UpdateCategoryInDb => CBZ.UpdateInDb(CBZ.TempCategory);

        #endregion

    }
}
