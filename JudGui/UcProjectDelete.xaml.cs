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
        public Bizz Bizz;
        public UserControl UcMain;

        #endregion

        public UcProjectDelete(Bizz bizz, UserControl ucRight)
        {
            InitializeComponent();
            this.Bizz = bizz;
            this.UcMain = ucRight;

            GenerateComboBoxCaseIdItems();
        }

        #region Buttons
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Vil du annullere reaktivering af projektet!", "Annuller reaktivering", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
            {
                //Close right UserControl
                UcMain.Content = new UserControl();
                Bizz.UcMainActive = false;
            }
        }

        private void ButtonErase_Click(object sender, RoutedEventArgs e)
        {
            if (CheckBoxEraseProject.IsChecked == true)
            {
                if (MessageBox.Show("Er du sikker på, at du vil slette projektet? Alle data vil gå tabt!", "Slet Projekt", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    // Code that changes project status
                    bool result = Bizz.DeleteFromDb("Projects", Bizz.TempProject.Id.ToString());

                    if (result)
                    {
                        foreach (Enterprise enterprise in Bizz.Enterprises)
                        {
                            if (enterprise.Project.Id == Bizz.TempProject.Case)
                            {
                                foreach (SubEntrepeneur subEntrepeneur in Bizz.SubEntrepeneurs)
                                {
                                    if (subEntrepeneur.Enterprise.Id == enterprise.Id)
                                    {
                                        Bizz.DeleteFromDb("Requests", subEntrepeneur.Request.Id.ToString());
                                        Bizz.DeleteFromDb("IttLetters", subEntrepeneur.IttLetter.Id.ToString());
                                        Bizz.DeleteFromDb("Offers", subEntrepeneur.Offer.Id.ToString());
                                        Bizz.DeleteFromDb("SubEntrepeneurs", subEntrepeneur.Id.ToString());
                                    }
                                }
                                Bizz.DeleteFromDb("Enterprises", enterprise.Id.ToString());
                            }
                        }

                        //Show Confirmation
                        MessageBox.Show("Projektet blev slettet", "Slet Projekt", MessageBoxButton.OK, MessageBoxImage.Information);

                        //Update list of projects
                        Bizz.RefreshList("Projects");
                        Bizz.RefreshIndexedList("IndexedActiveProjects");
                        Bizz.RefreshIndexedList("IndexedProjects");

                        //Close right UserControl
                        Bizz.UcMainActive = false;
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
            foreach (IndexedProject temp in Bizz.IndexedProjects)
            {
                if (temp.Index == selectedIndex)
                {
                    Bizz.TempProject = new Project(temp.Id, temp.Case, temp.Name, temp.Builder, temp.Status, temp.TenderForm, temp.EnterpriseForm, temp.Executive, temp.EnterprisesList, temp.Copy);
                }
            }
        }

        #endregion

        #region Methods
        private void GenerateComboBoxCaseIdItems()
        {
            ComboBoxCaseId.Items.Clear();
            foreach (IndexedProject temp in Bizz.IndexedProjects)
            {
                ComboBoxCaseId.Items.Add(temp);
            }
        }

        #endregion

    }
}
