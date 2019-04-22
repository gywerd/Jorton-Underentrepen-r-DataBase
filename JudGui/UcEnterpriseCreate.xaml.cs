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
    /// Interaction logic for UcEnterpriseCreate.xaml
    /// </summary>
    public partial class UcEnterpriseCreate : UserControl
    {
        #region Fields
        public Bizz CBZ;
        public UserControl UcMain;

        CraftGroup CCG = new CraftGroup();
        Project CCP = new Project();

        #endregion

        #region Constructors
        public UcEnterpriseCreate(Bizz cbz, UserControl ucMain)
        {
            InitializeComponent();
            this.CBZ = cbz;
            this.UcMain = ucMain;
            GenerateComboBoxCaseIdItems();
            GenerateCraftGroupItems();
        }

        #endregion

        #region Buttons
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            //Warning about lost changes before closing
            if (MessageBox.Show("Vil du annullere oprettelse af EntrepriseLister?", "Luk Projekt", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                //Close right UserControl
                CBZ.UcMainEdited = false;
                UcMain.Content = new UserControl();
            }
        }

        private void ButtonCreateClose_Click(object sender, RoutedEventArgs e)
        {
            //Code that creates a new project
            if (CBZ.TempProject.EnterpriseList == false)
            {
                CBZ.TempProject.ToggleEnterpriseList();
                CBZ.UpdateInDb(CBZ.TempProject);
                CBZ.RefreshList("Projects");
                CBZ.RefreshIndexedList("IndexedActiveProjects");
                CBZ.RefreshIndexedList("IndexedProjects");
            }
            bool result = false;
            int id = CBZ.CreateInDb(CBZ.TempEnterprise);
            if (id >= 1)
            {
                result = true;
            }

            if (result)
            {
                //Show Confirmation
                MessageBox.Show("Entreprisen blev oprettet", "Opret Entrepriseliste", MessageBoxButton.OK, MessageBoxImage.Information);

                //Update Enterprises
                CBZ.RefreshList("Enterprises");
                CBZ.TempEnterprise = new Enterprise();

                //Close right UserControl
                CBZ.UcMainEdited = false;
                UcMain.Content = new UserControl();
            }
            else
            {
                //Show error
                MessageBox.Show("Databasen returnerede en fejl. Entrepriselisten blev ikke oprettet. Prøv igen.", "Opret Projekt", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        private void ButtonCreateNew_Click(object sender, RoutedEventArgs e)
        {
            //Code that creates a new project
            if (CBZ.TempProject.EnterpriseList == false)
            {
                CBZ.TempProject.ToggleEnterpriseList();
                CBZ.UpdateInDb(CBZ.TempProject);
                CBZ.RefreshList("Projects");
                CBZ.RefreshIndexedList("IndexedActiveProjects");
                CBZ.RefreshIndexedList("IndexedProjects");
            }
            bool result = false;
            int id = CBZ.CreateInDb(CBZ.TempEnterprise);
            if (id >= 1)
            {
                result = true;
            }

            if (result)
            {
                //Show Confirmation
                MessageBox.Show("Entrepriselisten blev oprettet", "Opret Entrepriselisten", MessageBoxButton.OK, MessageBoxImage.Information);

                //Reset Boxes
                TextBoxCaseName.Content = "";
                TextBoxName.Text = "";
                TextBoxElaboration.Text = "";
                TextBoxOfferList.Text = "";
                ComboBoxCraftGroup1.SelectedIndex = 0;
                ComboBoxCraftGroup2.SelectedIndex = 0;
                ComboBoxCraftGroup3.SelectedIndex = 0;
                ComboBoxCraftGroup4.SelectedIndex = 0;

                //Update Enterprise list
                CBZ.RefreshList("Enterprises");
                CBZ.TempEnterprise.Name = "";
                CBZ.TempEnterprise.Elaboration = "";
                CBZ.TempEnterprise.OfferList = "";
                CBZ.TempEnterprise.CraftGroup1 = new CraftGroup((CraftGroup)CBZ.GetCraftGroup(0));
                CBZ.TempEnterprise.CraftGroup2 = new CraftGroup((CraftGroup)CBZ.GetCraftGroup(0));
                CBZ.TempEnterprise.CraftGroup3 = new CraftGroup((CraftGroup)CBZ.GetCraftGroup(0));
                CBZ.TempEnterprise.CraftGroup4 = new CraftGroup((CraftGroup)CBZ.GetCraftGroup(0));
            }
            else
            {
                //Show error
                MessageBox.Show("Databasen returnerede en fejl. Entrepriselisten blev ikke oprettet. Prøv igen.", "Opret Projekt", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        #endregion

        #region Events
        private void ComboBoxCaseId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = ComboBoxCaseId.SelectedIndex;
            foreach (IndexedProject temp in CBZ.IndexedActiveProjects)
            {
                if (temp.Index == selectedIndex)
                {
                    CBZ.TempProject = new Project(temp.Id, temp.Case, temp.Name, temp.Builder, temp.Status, temp.TenderForm, temp.EnterpriseForm, temp.Executive, temp.EnterpriseList, temp.Copy);
                }
            }
            TextBoxCaseName.Content = CBZ.TempProject.Name;
            CBZ.TempEnterprise.Project = CBZ.TempProject;
            ComboBoxCraftGroup1.SelectedIndex = 0;
            ComboBoxCraftGroup2.SelectedIndex = 0;
            ComboBoxCraftGroup3.SelectedIndex = 0;
            ComboBoxCraftGroup4.SelectedIndex = 0;
            CBZ.TempEnterprise.CraftGroup1 = new CraftGroup((CraftGroup)CBZ.GetCraftGroup(0));
            CBZ.TempEnterprise.CraftGroup2 = new CraftGroup((CraftGroup)CBZ.GetCraftGroup(0));
            CBZ.TempEnterprise.CraftGroup3 = new CraftGroup((CraftGroup)CBZ.GetCraftGroup(0));
            CBZ.TempEnterprise.CraftGroup4 = new CraftGroup((CraftGroup)CBZ.GetCraftGroup(0));

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

        private void TextBoxOfferList_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBoxOfferList.Text.Count() > 255)
            {
                string textBlock = TextBoxOfferList.Text;
                textBlock = textBlock.Remove(textBlock.Length - 1);
                TextBoxOfferList.Text = textBlock;
                TextBoxOfferList.Select(TextBoxOfferList.Text.Length, 0);
            }
            CBZ.TempEnterprise.OfferList = TextBoxOfferList.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        #endregion

        #region Methods
        private void GenerateComboBoxCaseIdItems()
        {
            ComboBoxCaseId.Items.Clear();
            foreach (IndexedProject temp in CBZ.IndexedActiveProjects)
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
            foreach (CraftGroup temp in CBZ.CraftGroups)
            {
                ComboBoxCraftGroup1.Items.Add(temp);
                ComboBoxCraftGroup2.Items.Add(temp);
                ComboBoxCraftGroup3.Items.Add(temp);
                ComboBoxCraftGroup4.Items.Add(temp);
            }
        }

        #endregion

    }
}
