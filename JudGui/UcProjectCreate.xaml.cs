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
    /// Interaction logic for UcProjectCreate.xaml
    /// </summary>
    public partial class UcProjectCreate : UserControl
    {
        #region Fields
        public Bizz CBZ;
        public UserControl UcMain;
        #endregion

        #region Constructors
        public UcProjectCreate(Bizz cbz, UserControl ucMain)
        {
            InitializeComponent();
            this.CBZ = cbz;
            this.UcMain = ucMain;
            GenerateComboBoxBuilderItems();
            GenerateComboBoxTenderFormItems();
            GenerateComboBoxEnterpriseFormItems();
            GenerateComboBoxExecutiveItems();
        }

        #endregion

        #region Buttons
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            //Warning about lost changes before closing
            if (MessageBox.Show("Vil du annullere oprettelse af projektet?", "Luk Projekt", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                //Close right UserControl
                CBZ.UcMainEdited = false;
                UcMain.Content = new UserControl();
            }
        }

        private void ButtonCreateClose_Click(object sender, RoutedEventArgs e)
        {
            bool result = false;

            //Code that creates a new project
            Project project = new Project(Convert.ToInt32(TextBoxCaseId.Text), TextBoxCaseName.Text, new Builder((Builder)ComboBoxBuilder.SelectedItem), new ProjectStatus((ProjectStatus)CBZ.GetObject("ProjectStatus", 1)), new TenderForm((TenderForm)ComboBoxTenderForm.SelectedItem), new EnterpriseForm((EnterpriseForm)ComboBoxEnterpriseForm.SelectedItem), new User((User)ComboBoxExecutive.SelectedItem));
            int id = CBZ.CreateInDb(project);
            if (id >= 1)
            {
                result = true;
            }

            if (result)
            {
                //Show Confirmation
                MessageBox.Show("Projektet blev oprettet", "Opret Projekt", MessageBoxButton.OK, MessageBoxImage.Information);

                //Update list of projects
                CBZ.RefreshList("Projects");
                CBZ.RefreshIndexedList("IndexedActiveProjects");
                CBZ.RefreshIndexedList("IndexedProjects");

                //Close right UserControl
                CBZ.UcMainEdited = false;
                UcMain.Content = new UserControl();
            }
            else
            {
                //Show error
                MessageBox.Show("Databasen returnerede en fejl. Projektet blev ikke oprettet. Prøv igen.", "Opret Projekt", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ButtonCreateNew_Click(object sender, RoutedEventArgs e)
        {
            bool result = false;

            //Code that creates a new project
            Project project = new Project(Convert.ToInt32(TextBoxCaseId.Text), TextBoxCaseName.Text, new Builder((Builder)ComboBoxBuilder.SelectedItem), new ProjectStatus((ProjectStatus)CBZ.GetObject("ProjectStatus", 1)), new TenderForm((TenderForm)ComboBoxTenderForm.SelectedItem), new EnterpriseForm((EnterpriseForm)ComboBoxEnterpriseForm.SelectedItem), new User((User)ComboBoxExecutive.SelectedItem));
            int id = CBZ.CreateInDb(project);
            if (id >= 1)
            {
                result = true;
            }

            if (result)
            {
                //Show Confirmation
                MessageBox.Show("Projektet blev oprettet", "Opret Projekt", MessageBoxButton.OK, MessageBoxImage.Information);

                //Reset form
                TextBoxCaseId.Text = "";
                TextBoxCaseName.Text = "";
                ComboBoxBuilder.SelectedIndex = -1;
                ComboBoxTenderForm.SelectedIndex = -1;
                ComboBoxEnterpriseForm.SelectedIndex = -1;
                ComboBoxExecutive.SelectedIndex = -1;

                //Update list of projects
                CBZ.RefreshList("Projects");
                CBZ.RefreshIndexedList("IndexedActiveProjects");
                CBZ.RefreshIndexedList("IndexedProjects");
            }
            else
            {
                //Show error
                MessageBox.Show("Databasen returnerede en fejl. Projektet blev ikke oprettet. Prøv igen.", "Opret Projekt", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        #endregion

        #region Events
        private void TextBoxCaseId_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBoxCaseId.Text.Count() > 6)
            {
                string id = TextBoxCaseId.Text;
                id = id.Remove(id.Length - 1);
                TextBoxCaseId.Text = id;
                TextBoxCaseId.Select(TextBoxCaseId.Text.Length, 0);
            }

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxCaseName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBoxCaseName.Text.Count() > 50)
            {
                string id = TextBoxCaseName.Text;
                id = id.Remove(id.Length - 1);
                TextBoxCaseName.Text = id;
                TextBoxCaseName.Select(TextBoxCaseName.Text.Length, 0);
            }

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        #endregion

        #region Methods
        private void GenerateComboBoxBuilderItems()
        {
            ComboBoxBuilder.Items.Clear();
            foreach (Builder temp in CBZ.Builders)
            {
                ComboBoxBuilder.Items.Add(temp);
            }
        }

        private void GenerateComboBoxTenderFormItems()
        {
            ComboBoxTenderForm.Items.Clear();
            foreach (TenderForm temp in CBZ.TenderForms)
            {
                ComboBoxTenderForm.Items.Add(temp);
            }
        }

        private void GenerateComboBoxEnterpriseFormItems()
        {
            ComboBoxEnterpriseForm.Items.Clear();
            foreach (EnterpriseForm temp in CBZ.EnterpriseForms)
            {
                ComboBoxEnterpriseForm.Items.Add(temp);
            }
        }

        private void GenerateComboBoxExecutiveItems()
        {
            ComboBoxExecutive.Items.Clear();
            foreach (User temp in CBZ.Users)
            {
                ComboBoxExecutive.Items.Add(temp);
            }
        }

        #endregion

    }
}
