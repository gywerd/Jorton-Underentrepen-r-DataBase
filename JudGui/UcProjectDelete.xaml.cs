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
    /// Interaction logic for UcProjectDelete.xaml
    /// </summary>
    public partial class UcProjectDelete : UserControl
    {
        #region Fields
        public Bizz CBZ;
        public UserControl UcMain;

        #endregion

        #region Constructors
        public UcProjectDelete(Bizz cbz, UserControl ucMain)
        {
            InitializeComponent();
            this.CBZ = cbz;
            this.UcMain = ucMain;

            GenerateComboBoxCaseIdItems();
        }

        #endregion

        #region Buttons
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Vil du annullere reaktivering af projektet!", "Annuller reaktivering", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
            {
                //Close right UserControl
                UcMain.Content = new UserControl();
                CBZ.UcMainEdited = false;
            }
        }

        private void ButtonErase_Click(object sender, RoutedEventArgs e)
        {
            if (CheckBoxEraseProject.IsChecked == true)
            {
                if (MessageBox.Show("Er du sikker på, at du vil slette projektet? Alle data vil gå tabt!", "Slet Projekt", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    // Code that changes project status
                    bool result = CBZ.DeleteFromDb("Projects", CBZ.TempProject.Id.ToString());

                    if (result)
                    {
                        foreach (Enterprise enterprise in CBZ.Enterprises)
                        {
                            if (enterprise.Project.Id == CBZ.TempProject.Case)
                            {
                                foreach (SubEntrepeneur subEntrepeneur in CBZ.SubEntrepeneurs)
                                {
                                    if (subEntrepeneur.Enterprise.Id == enterprise.Id)
                                    {
                                        CBZ.DeleteFromDb("Requests", subEntrepeneur.Request.Id.ToString());
                                        CBZ.DeleteFromDb("IttLetters", subEntrepeneur.IttLetter.Id.ToString());
                                        CBZ.DeleteFromDb("Offers", subEntrepeneur.Offer.Id.ToString());
                                        CBZ.DeleteFromDb("SubEntrepeneurs", subEntrepeneur.Id.ToString());
                                    }
                                }
                                CBZ.DeleteFromDb("Enterprises", enterprise.Id.ToString());
                            }
                        }

                        //Show Confirmation
                        MessageBox.Show("Projektet blev slettet", "Slet Projekt", MessageBoxButton.OK, MessageBoxImage.Information);

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
                        MessageBox.Show("Databasen returnerede en fejl. Projektet blev ikke slettet. Prøv igen.", "Slet Projekt", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            else
            {
                //Show error
                MessageBox.Show("Du har glemt at markere 'Godkend sletning af projekt'.", "Slet Projekt", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        #endregion

        #region Events
        private void ComboBoxCaseId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = ComboBoxCaseId.SelectedIndex;
            foreach (IndexedProject temp in CBZ.IndexedProjects)
            {
                if (temp.Index == selectedIndex)
                {
                    CBZ.TempProject = new Project(temp.Id, temp.Case, temp.Name, temp.Builder, temp.Status, temp.TenderForm, temp.EnterpriseForm, temp.Executive, temp.EnterpriseList, temp.Copy);
                }
            }

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
            foreach (IndexedProject temp in CBZ.IndexedProjects)
            {
                ComboBoxCaseId.Items.Add(temp);
            }
        }

        #endregion

    }
}
