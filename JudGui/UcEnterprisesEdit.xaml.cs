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
    /// Interaction logic for UcEnterprisesEdit.xaml
    /// </summary>
    public partial class UcEnterprisesEdit : UserControl
    {
        #region Fields
        public Bizz CBZ;
        public UserControl UcMain;
        public CraftGroup CCG = new CraftGroup();

        public List<Enterprise> IndexedEnterprises = new List<Enterprise>();

        #endregion

        #region Constructors
        public UcEnterprisesEdit(Bizz cbz, UserControl ucMain)
        {
            InitializeComponent();
            this.CBZ = cbz;
            this.UcMain = ucMain;
            GenerateComboBoxCaseIdItems();
            GenerateCraftGroupItems();
        }

        #endregion

        #region Buttons
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                //Warning about lost changes before closing
                if (MessageBox.Show("Vil du lukke Redigering af Entrepriselisten? Alle ugemte data mistes.", "Entrepriser", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    CBZ.CloseUcMain(UcMain);
                }
            }
            else
            {
                CBZ.CloseUcMain(UcMain);
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            //Code that deletes a project from Db
            bool result = CBZ.DeleteFromDb("Enterprises", CBZ.TempEnterprise.Id.ToString());
            CBZ.TempEnterprise = new Enterprise();

            if (result)
            {
                //Show Confirmation
                MessageBox.Show("Entrepriselisten blev slettet", "Entrepriser", MessageBoxButton.OK, MessageBoxImage.Information);
                //Reset Boxes
                TextBoxName.Text = "";
                TextBoxElaboration.Text = "";
                TextBoxOfferList.Text = "";
                ComboBoxCraftGroup1.SelectedIndex = -1;
                ComboBoxCraftGroup2.SelectedIndex = -1;
                ComboBoxCraftGroup3.SelectedIndex = -1;
                ComboBoxCraftGroup4.SelectedIndex = -1;

                //Update Enterprise List
                RefreshIndexedEnterprises();
                ListBoxEnterprises.ItemsSource = "";
                ListBoxEnterprises.ItemsSource = IndexedEnterprises;
            }
            else
            {
                //Show error
                MessageBox.Show("Databasen returnerede en fejl. Entrepriselisten blev ikke slettet. Prøv igen.", "Entrepriser", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            //Code that edits a project
            bool result = CBZ.UpdateInDb(CBZ.TempEnterprise);

            if (result)
            {
                //Show Confirmation
                MessageBox.Show("Entrepriselisten blev redigeret", "Entrepriser", MessageBoxButton.OK, MessageBoxImage.Information);

                //Reset Boxes
                TextBoxName.Text = "";
                TextBoxElaboration.Text = "";
                TextBoxOfferList.Text = "";
                ComboBoxCraftGroup1.SelectedIndex = -1;
                ComboBoxCraftGroup2.SelectedIndex = -1;
                ComboBoxCraftGroup3.SelectedIndex = -1;
                ComboBoxCraftGroup4.SelectedIndex = -1;

                //Update Enterprise List
                RefreshIndexedEnterprises();
                ListBoxEnterprises.ItemsSource = "";
                ListBoxEnterprises.ItemsSource = IndexedEnterprises;
            }
            else
            {
                //Show error
                MessageBox.Show("Databasen returnerede en fejl. Entrepriselisten blev ikke redigeret. Prøv igen.", "Entrepriser", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        #endregion

        #region Events
        private void ComboBoxCaseId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CBZ.TempProject = new Project((IndexedProject)ComboBoxCaseId.SelectedItem);

            RefreshIndexedEnterprises();

            ListBoxEnterprises.ItemsSource = "";
            ListBoxEnterprises.ItemsSource = IndexedEnterprises;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }

        }

        private void ComboBoxCraftGroup1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CBZ.TempEnterprise.CraftGroup1 = new CraftGroup((CraftGroup)ComboBoxCraftGroup1.SelectedItem);

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void ComboBoxCraftGroup2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CBZ.TempEnterprise.CraftGroup2 = new CraftGroup((CraftGroup)ComboBoxCraftGroup2.SelectedItem);

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void ComboBoxCraftGroup3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CBZ.TempEnterprise.CraftGroup3 = new CraftGroup((CraftGroup)ComboBoxCraftGroup3.SelectedItem);

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void ComboBoxCraftGroup4_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CBZ.TempEnterprise.CraftGroup4 = new CraftGroup((CraftGroup)ComboBoxCraftGroup4.SelectedItem);

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void ListBoxEnterprises_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Enterprise temp = new Enterprise((Enterprise)ListBoxEnterprises.SelectedItem);
            CBZ.TempEnterprise = temp;
            TextBoxName.Text = temp.Name;
            TextBoxElaboration.Text = temp.Elaboration;
            TextBoxOfferList.Text = temp.OfferList;
            ComboBoxCraftGroup1.SelectedIndex = temp.CraftGroup1.Id;
            ComboBoxCraftGroup2.SelectedIndex = temp.CraftGroup2.Id;
            ComboBoxCraftGroup3.SelectedIndex = temp.CraftGroup3.Id;
            ComboBoxCraftGroup4.SelectedIndex = temp.CraftGroup4.Id;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }

        }

        private void TextBoxName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBoxName.Text.Count() > 255)
            {
                string textBlock = TextBoxName.Text;
                textBlock = textBlock.Remove(textBlock.Length - 1);
                TextBoxName.Text = textBlock;
                TextBoxName.Select(TextBoxName.Text.Length, 0);
            }
            CBZ.TempEnterprise.Name = TextBoxName.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }

        }

        private void TextBoxElaboration_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBoxElaboration.Text.Count() > 255)
            {
                string textBlock = TextBoxElaboration.Text;
                textBlock = textBlock.Remove(textBlock.Length - 1);
                TextBoxElaboration.Text = textBlock;
                TextBoxElaboration.Select(TextBoxElaboration.Text.Length, 0);
            }
            CBZ.TempEnterprise.Elaboration = TextBoxElaboration.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }

        }

        #endregion

        #region Methods
        /// <summary>
        /// Method, that populates the CaseId ComboBox
        /// </summary>
        private void GenerateComboBoxCaseIdItems()
        {
            ComboBoxCaseId.ItemsSource = "";
            ComboBoxCaseId.ItemsSource = CBZ.IndexedActiveProjects;
        }

        /// <summary>
        /// Method, that populate the CraftGroups ComboBoxes
        /// </summary>
        private void GenerateCraftGroupItems()
        {
            ComboBoxCraftGroup1.ItemsSource = "";
            ComboBoxCraftGroup2.ItemsSource = "";
            ComboBoxCraftGroup3.ItemsSource = "";
            ComboBoxCraftGroup4.ItemsSource = "";

            ComboBoxCraftGroup1.ItemsSource = CBZ.IndexedCraftGroups;
            ComboBoxCraftGroup2.ItemsSource = CBZ.IndexedCraftGroups;
            ComboBoxCraftGroup3.ItemsSource = CBZ.IndexedCraftGroups;
            ComboBoxCraftGroup4.ItemsSource = CBZ.IndexedCraftGroups;
        }

        /// <summary>
        /// Method, that refreshes Indexed Enterprises with content from Project Enterprises list
        /// </summary>
        private void RefreshIndexedEnterprises()
        {
            CBZ.RefreshProjectList("All", CBZ.TempProject.Id);

            if (CBZ.IndexedEnterprises != null)
            {
                CBZ.IndexedEnterprises.Clear();
            }

            int i = 0;

            foreach (Enterprise enterprise in CBZ.Enterprises)
            {
                if (enterprise.Project.Id == CBZ.TempProject.Id)
                {
                    CBZ.IndexedEnterprises.Add(new IndexedEnterprise(i, enterprise));
                }
                i++;
            }

        }

        #endregion

    }
}
