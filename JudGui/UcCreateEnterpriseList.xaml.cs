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
    /// Interaction logic for UcCreateEnterpriseList.xaml
    /// </summary>
    public partial class UcCreateEnterpriseList : UserControl
    {
        #region Fields
        public Bizz Bizz;
        public UserControl UcRight;

        private static string strConnection = Bizz.strConnection;
        CraftGroup CCG = new CraftGroup(strConnection);
        Project CCP = new Project(strConnection);

        #endregion

        #region Constructors
        public UcCreateEnterpriseList(Bizz bizz, UserControl ucRight)
        {
            InitializeComponent();
            this.Bizz = bizz;
            this.UcRight = ucRight;
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
                Bizz.UcRightActive = false;
                UcRight.Content = new UserControl();
            }
        }

        private void ButtonCreateClose_Click(object sender, RoutedEventArgs e)
        {
            //Code that creates a new project
            if (Bizz.TempProject.EnterpriseList == false)
            {
                Bizz.TempProject.ToggleEnterpriseList();
                Bizz.PRO.UpdateProject(Bizz.TempProject);
                Bizz.Projects.Clear();
                Bizz.Projects = Bizz.PRO.GetProjects();
                ReloadListActiveProjects();
                ReloadListIndexableProjects();
            }
            bool result = Bizz.ENP.InsertIntoEnterpriseList(Bizz.TempEnterprise);

            if (result)
            {
                //Show Confirmation
                MessageBox.Show("Entrepriselisten blev oprettet", "Opret Entrepriseliste", MessageBoxButton.OK, MessageBoxImage.Information);

                //Update EnterpriseList
                Bizz.EnterpriseList.Clear();
                Bizz.EnterpriseList = Bizz.ENP.GetEnterpriseList();
                Bizz.TempEnterprise = new Enterprise(Bizz.strConnection);

                //Close right UserControl
                Bizz.UcRightActive = false;
                UcRight.Content = new UserControl();
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
            if (Bizz.TempProject.EnterpriseList == false)
            {
                Bizz.TempProject.ToggleEnterpriseList();
                Bizz.PRO.UpdateProject(Bizz.TempProject);
                Bizz.Projects.Clear();
                Bizz.Projects = Bizz.PRO.GetProjects();
                ReloadListActiveProjects();
                ReloadListIndexableProjects();
            }
            bool result = Bizz.ENP.InsertIntoEnterpriseList(Bizz.TempEnterprise);

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
                Bizz.EnterpriseList.Clear();
                Bizz.EnterpriseList = Bizz.ENP.GetEnterpriseList();
                Bizz.TempEnterprise.Name = "";
                Bizz.TempEnterprise.Elaboration = "";
                Bizz.TempEnterprise.OfferList = "";
                Bizz.TempEnterprise.CraftGroup1 = CCG.GetCraftGroup(0);
                Bizz.TempEnterprise.CraftGroup2 = CCG.GetCraftGroup(0);
                Bizz.TempEnterprise.CraftGroup3 = CCG.GetCraftGroup(0);
                Bizz.TempEnterprise.CraftGroup4 = CCG.GetCraftGroup(0);
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
            foreach (IndexedProject temp in Bizz.IndexedActiveProjects)
            {
                if (temp.Index == selectedIndex)
                {
                    Bizz.TempProject = new Project(Bizz.strConnection, temp.Id, temp.CaseId, temp.Name, temp.Builder, temp.Status, temp.TenderForm, temp.EnterpriseForm, temp.Executive, temp.EnterpriseList, temp.Copy);
                }
            }
            TextBoxCaseName.Content = Bizz.TempProject.Name;
            Bizz.TempEnterprise.Project = CCP.GetProject(Bizz.TempProject.Id);
            ComboBoxCraftGroup1.SelectedIndex = 0;
            ComboBoxCraftGroup2.SelectedIndex = 0;
            ComboBoxCraftGroup3.SelectedIndex = 0;
            ComboBoxCraftGroup4.SelectedIndex = 0;
            Bizz.TempEnterprise.CraftGroup1 = CCG.GetCraftGroup(0);
            Bizz.TempEnterprise.CraftGroup2 = CCG.GetCraftGroup(0);
            Bizz.TempEnterprise.CraftGroup3 = CCG.GetCraftGroup(0);
            Bizz.TempEnterprise.CraftGroup4 = CCG.GetCraftGroup(0);
        }

        private void ComboBoxCraftGroup1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Bizz.TempEnterprise.CraftGroup1 = CCG.GetCraftGroup(ComboBoxCraftGroup1.SelectedIndex);
        }

        private void ComboBoxCraftGroup2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Bizz.TempEnterprise.CraftGroup2 = CCG.GetCraftGroup(ComboBoxCraftGroup2.SelectedIndex);
        }

        private void ComboBoxCraftGroup3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Bizz.TempEnterprise.CraftGroup3 = CCG.GetCraftGroup(ComboBoxCraftGroup3.SelectedIndex);
        }

        private void ComboBoxCraftGroup4_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Bizz.TempEnterprise.CraftGroup4 = CCG.GetCraftGroup(ComboBoxCraftGroup4.SelectedIndex);
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

        /// <summary>
        /// Method, that reloads list of active projects
        /// </summary>
        private void ReloadListActiveProjects()
        {
            Bizz.IndexedActiveProjects.Clear();
            int i = 0;
            foreach (Project tempProject in Bizz.Projects)
            {
                if (tempProject.Status == 1)
                {
                    IndexedProject result = new IndexedProject(Bizz.strConnection, i, tempProject);
                    Bizz.IndexedActiveProjects.Add(result);
                    i++;
                }
            }
        }

        /// <summary>
        /// Method, that reloads list of indexable projects
        /// </summary>
        private void ReloadListIndexableProjects()
        {
            Bizz.IndexedProjects.Clear();
            int i = 0;
            foreach (Project temp in Bizz.Projects)
            {
                IndexedProject result = new IndexedProject(Bizz.strConnection, i, temp);
                Bizz.IndexedProjects.Add(result);
                i++;
            }
        }

        #endregion

    }
}
