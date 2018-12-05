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
    /// Interaction logic for UcEditEnterpriseList.xaml
    /// </summary>
    public partial class UcEditEnterpriseList : UserControl
    {
        #region Fields
        public Bizz Bizz;
        public UserControl UcRight;
        public List<Enterprise> IndexableEnterpriseList = new List<Enterprise>();

        CraftGroup CCG = new CraftGroup();

        #endregion

        public UcEditEnterpriseList(Bizz bizz, UserControl ucRight)
        {
            InitializeComponent();
            this.Bizz = bizz;
            this.UcRight = ucRight;
            GenerateComboBoxCaseIdItems();
            GenerateCraftGroupItems();
        }

        #region Buttons
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            //Warning about lost changes before closing
            if (MessageBox.Show("Vil du lukke Redigering af Entrepriselisten?", "Luk Entrepriseliste", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                //Close right UserControl
                Bizz.UcRightActive = false;
                UcRight.Content = new UserControl();
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            //Code that deletes a project from Db
            bool result = Bizz.DeleteFromDb("EnterpriseList", Bizz.TempEnterprise.Id.ToString());
            Bizz.TempEnterprise = new Enterprise();

            if (result)
            {
                //Show Confirmation
                MessageBox.Show("Entrepriselisten blev slettet", "Slet Entrepriseliste", MessageBoxButton.OK, MessageBoxImage.Information);
                //Reset Boxes
                TextBoxName.Text = "";
                TextBoxElaboration.Text = "";
                TextBoxOfferList.Text = "";
                ComboBoxCraftGroup1.SelectedIndex = -1;
                ComboBoxCraftGroup2.SelectedIndex = -1;
                ComboBoxCraftGroup3.SelectedIndex = -1;
                ComboBoxCraftGroup4.SelectedIndex = -1;

                //Update Enterprise List
                Bizz.RefreshList("EnterpriseList");
                IndexableEnterpriseList.Clear();
                IndexableEnterpriseList = GetIndexableEnterpriseList();
                ListBoxEnterpriseList.ItemsSource = IndexableEnterpriseList;
            }
            else
            {
                //Show error
                MessageBox.Show("Databasen returnerede en fejl. Entrepriselisten blev ikke slettet. Prøv igen.", "Slet Projekt", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            //Code that edits a project
            bool result = Bizz.UpdateInDb(Bizz.TempEnterprise);

            if (result)
            {
                //Show Confirmation
                MessageBox.Show("Entrepriselisten blev redigeret", "Rediger Entrepriseliste", MessageBoxButton.OK, MessageBoxImage.Information);

                //Reset Boxes
                TextBoxName.Text = "";
                TextBoxElaboration.Text = "";
                TextBoxOfferList.Text = "";
                ComboBoxCraftGroup1.SelectedIndex = -1;
                ComboBoxCraftGroup2.SelectedIndex = -1;
                ComboBoxCraftGroup3.SelectedIndex = -1;
                ComboBoxCraftGroup4.SelectedIndex = -1;

                //Update Enterprise List
                Bizz.RefreshList("EnterpriseList");
                IndexableEnterpriseList.Clear();
                IndexableEnterpriseList = GetIndexableEnterpriseList();
                ListBoxEnterpriseList.ItemsSource = IndexableEnterpriseList;
            }
            else
            {
                //Show error
                MessageBox.Show("Databasen returnerede en fejl. Entrepriselisten blev ikke redigeret. Prøv igen.", "Rediger Projekt", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        #endregion

        #region Events
        private void ComboBoxCaseId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = ComboBoxCaseId.SelectedIndex;
            foreach (IndexedProject temp in Bizz.IndexedActiveProjects)
            {
                if (temp.Index == selectedIndex)
                {
                    Bizz.TempProject = new Project(temp.Id, temp.CaseId, temp.Name, temp.Builder, temp.Status, temp.TenderForm, temp.EnterpriseForm, temp.Executive, temp.EnterpriseList, temp.Copy);
                }
            }
            IndexableEnterpriseList = GetIndexableEnterpriseList();
            ListBoxEnterpriseList.ItemsSource = IndexableEnterpriseList;
        }

        private void ComboBoxCraftGroup1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Bizz.TempEnterprise.CraftGroup1 = new CraftGroup((CraftGroup)ComboBoxCraftGroup1.SelectedItem);
        }

        private void ComboBoxCraftGroup2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Bizz.TempEnterprise.CraftGroup2 = new CraftGroup((CraftGroup)ComboBoxCraftGroup2.SelectedItem);
        }

        private void ComboBoxCraftGroup3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Bizz.TempEnterprise.CraftGroup3 = new CraftGroup((CraftGroup)ComboBoxCraftGroup3.SelectedItem);
        }

        private void ComboBoxCraftGroup4_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Bizz.TempEnterprise.CraftGroup4 = new CraftGroup((CraftGroup)ComboBoxCraftGroup4.SelectedItem);
        }

        private void ListBoxEnterpriseList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Enterprise temp = new Enterprise((Enterprise)ListBoxEnterpriseList.SelectedItem);
            Bizz.TempEnterprise = temp;
            TextBoxName.Text = temp.Name;
            TextBoxElaboration.Text = temp.Elaboration;
            TextBoxOfferList.Text = temp.OfferList;
            ComboBoxCraftGroup1.SelectedIndex = temp.CraftGroup1.Id;
            ComboBoxCraftGroup2.SelectedIndex = temp.CraftGroup2.Id;
            ComboBoxCraftGroup3.SelectedIndex = temp.CraftGroup3.Id;
            ComboBoxCraftGroup4.SelectedIndex = temp.CraftGroup4.Id;
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
            Bizz.TempEnterprise.Name = TextBoxName.Text;
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
            Bizz.TempEnterprise.Elaboration = TextBoxElaboration.Text;
        }

        private void TextBoxOfferList_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBoxOfferList.Text.Count() > 255)
            {
                string textBlock = TextBoxOfferList.Text;
                textBlock = textBlock.Remove(textBlock.Length - 1);
                TextBoxOfferList.Text = textBlock;
                TextBoxOfferList.Select(TextBoxOfferList.Text.Length, 0);
            }
            Bizz.TempEnterprise.OfferList = TextBoxOfferList.Text;
        }

        #endregion

        #region Methods
        private void GenerateComboBoxCaseIdItems()
        {
            ComboBoxCaseId.Items.Clear();
            foreach (IndexedProject temp in Bizz.IndexedActiveProjects)
            {
                ComboBoxCaseId.Items.Add(temp);
            }
        }

        private void GenerateCraftGroupItems()
        {
            ComboBoxCraftGroup1.Items.Clear();
            ComboBoxCraftGroup2.Items.Clear();
            ComboBoxCraftGroup3.Items.Clear();
            ComboBoxCraftGroup4.Items.Clear();
            foreach (CraftGroup temp in Bizz.CraftGroups)
            {
                ComboBoxCraftGroup1.Items.Add(temp);
                ComboBoxCraftGroup2.Items.Add(temp);
                ComboBoxCraftGroup3.Items.Add(temp);
                ComboBoxCraftGroup4.Items.Add(temp);
            }
        }

        private List<Enterprise> GetIndexableEnterpriseList()
        {
            List<Enterprise> result = new List<Enterprise>();
            int i = 0;
            foreach (Enterprise enterprise in Bizz.EnterpriseList)
            {
                if (enterprise.Project.Id == Bizz.TempProject.Id)
                {
                    IndexedEnterprise temp = new IndexedEnterprise(i, enterprise);
                    result.Add(temp);
                }
                i++;
            }
            return result;
        }

        #endregion

    }
}
