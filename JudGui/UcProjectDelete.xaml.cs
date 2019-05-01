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
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                if (MessageBox.Show("Du er ved at lukke 'Slet Projekt'.", "Projekter", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
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
            bool allSubEntrepeneursDeleted = true;
            bool allRequestsDeleted = true;
            bool allIttLettersDeleted = true;
            bool allOffersDeleted = true;
            bool allEnterprisesDeleted = true;
            bool someSubEntrepeneursDeleted = false;
            bool someRequestsDeleted = false;
            bool someIttLettersDeleted = false;
            bool someOffersDeleted = false;
            bool someEnterprisesDeleted = false;
            bool result = false;

            if (CheckBoxEraseProject.IsChecked == true)
            {
                if (MessageBox.Show("Er du sikker på, at du vil slette projektet? Alle projektets data inkl. entrepriser & underentrepenører vil gå tabt!", "Projekter", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                { 

                    try
                    {
                        foreach (Enterprise enterprise in CBZ.Enterprises)
                        {
                            if (enterprise.Project.Id == CBZ.TempProject.Case)
                            {
                                foreach (SubEntrepeneur subEntrepeneur in CBZ.SubEntrepeneurs)
                                {
                                    if (subEntrepeneur.Enterprise.Id == enterprise.Id)
                                    {
                                        SubEntrepeneur sub = new SubEntrepeneur(subEntrepeneur);
                                        // Code that deletes SubEntrepeneur 
                                        bool subEntrepeneurDeleted = CBZ.DeleteFromDb("SubEntrepeneurs", sub.Id.ToString());
                                        if (subEntrepeneurDeleted && !someSubEntrepeneursDeleted)
                                        {
                                            someSubEntrepeneursDeleted = true;

                                            // Code that deletes Request
                                            bool requestDeleted = CBZ.DeleteFromDb("Requests", sub.Request.Id.ToString());
                                            if (requestDeleted && !someRequestsDeleted)
                                            {
                                                someRequestsDeleted = true;
                                            }
                                            else if (!requestDeleted && allRequestsDeleted)
                                            {
                                                allRequestsDeleted = false;
                                            }

                                            // Code that deletes IttLetter
                                            bool ittLetterDeleted = CBZ.DeleteFromDb("IttLetters", sub.IttLetter.Id.ToString());
                                            if (ittLetterDeleted && !someIttLettersDeleted)
                                            {
                                                someIttLettersDeleted = true;
                                            }
                                            else if (!ittLetterDeleted && allIttLettersDeleted)
                                            {
                                                allIttLettersDeleted = false;
                                            }

                                            // Code that deletes Offer
                                            bool offerDeleted = CBZ.DeleteFromDb("Offers", sub.Offer.Id.ToString());
                                            if (offerDeleted && !someOffersDeleted)
                                            {
                                                someOffersDeleted = true;
                                            }
                                            else if (!offerDeleted && allOffersDeleted)
                                            {
                                                allOffersDeleted = false;
                                            }
                                        }
                                        else if (!someSubEntrepeneursDeleted && allSubEntrepeneursDeleted)
                                        {
                                            allSubEntrepeneursDeleted = false;
                                            allRequestsDeleted = false;
                                            allIttLettersDeleted = false;
                                            allOffersDeleted = false;
                                        }

                                    }
                                }

                                // Code that deletes Enterprise
                                if (someSubEntrepeneursDeleted)
                                {
                                    bool enterpriseDeleted = CBZ.DeleteFromDb("Enterprises", enterprise.Id.ToString());
                                    if (enterpriseDeleted && !someEnterprisesDeleted)
                                    {
                                        someEnterprisesDeleted = true;
                                    }
                                    else if (!enterpriseDeleted && allEnterprisesDeleted)
                                    {
                                        allEnterprisesDeleted = false;
                                    }
                                }
                            }

                            if (someEnterprisesDeleted)
                            {
                                // Code that deletes Project
                                result = CBZ.DeleteFromDb("Projects", CBZ.TempProject.Id.ToString());
                                if (result)
                                {
                                    //Show results
                                    ShowDependencystatus(allSubEntrepeneursDeleted, allRequestsDeleted, allIttLettersDeleted, allOffersDeleted, allEnterprisesDeleted, someSubEntrepeneursDeleted, someRequestsDeleted, someIttLettersDeleted, someOffersDeleted, someEnterprisesDeleted);

                                    MessageBox.Show("Projektet blev slettet", "Projekter", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                                else
                                {
                                    //Show results
                                    ShowDependencystatus(allSubEntrepeneursDeleted, allRequestsDeleted, allIttLettersDeleted, allOffersDeleted, allEnterprisesDeleted, someSubEntrepeneursDeleted, someRequestsDeleted, someIttLettersDeleted, someOffersDeleted, someEnterprisesDeleted);

                                    MessageBox.Show("Projektet blev ikke slettet", "Projekter", MessageBoxButton.OK, MessageBoxImage.Error);
                                }

                            }

                            //Reset form
                            ComboBoxCaseId.SelectedIndex = -1;
                            CheckBoxEraseProject.IsChecked = false;

                            //Update list of projects
                            CBZ.RefreshIndexedList("SubEntrepeneurs");

                        }

                    }
                    catch (Exception ex)
                    {
                        //Show results
                        ShowDependencystatus(allSubEntrepeneursDeleted, allRequestsDeleted, allIttLettersDeleted, allOffersDeleted, allEnterprisesDeleted, someSubEntrepeneursDeleted, someRequestsDeleted, someIttLettersDeleted, someOffersDeleted, someEnterprisesDeleted);

                        MessageBox.Show("Databasen returnerede en fejl. Projektet blev ikke slettet. Prøv igen.\n" + ex, "Projekter", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                //Show error
                MessageBox.Show("Du har glemt at markere 'Godkend sletning af projekt'.", "Projekter", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region Events
        private void ComboBoxCaseId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxCaseId.SelectedIndex >= 0)
            {
                CBZ.TempProject = new Project((IndexedProject)ComboBoxCaseId.SelectedItem);
                TextBoxName.Text = CBZ.TempProject.Details.Name;

                //Set CBZ.UcMainEdited
                if (!CBZ.UcMainEdited)
                {
                    CBZ.UcMainEdited = true;
                }

            }
            else
            {
                CBZ.TempProject = new Project();
                TextBoxName.Text = "";

                //Set CBZ.UcMainEdited
                if (CBZ.UcMainEdited)
                {
                    CBZ.UcMainEdited = false;
                }

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

        private void ShowDependencystatus(bool allSubEntrepeneursDeleted, bool allRequestsDeleted, bool allIttLettersDeleted, bool allOffersDeleted, bool allEnterprisesDeleted, bool someSubEntrepeneursDeleted, bool someRequestsDeleted, bool someIttLettersDeleted, bool someOffersDeleted, bool someEnterprisesDeleted)
        {
            //Show status for Requests
            if (allRequestsDeleted)
            {
                MessageBox.Show("Alle tilhørende forespørgsler blev slettet.", "Projekter", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            else if (!allRequestsDeleted && someSubEntrepeneursDeleted)
            {
                MessageBox.Show("Nogle tilhørende forespørgsler blev slettet. De tilbageværende forespørgsler kan være knyttet til andre underentrepenører.", "Projekter", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (!allRequestsDeleted && !someSubEntrepeneursDeleted)
            {
                MessageBox.Show("Ingen tilhørende forespørgsler blev slettet. Evt. tilbageværende forespørgsler kan være knyttet til andre underentrepenører.", "Projekter", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            //Show status for IttLetters
            if (allIttLettersDeleted)
            {
                MessageBox.Show("Alle tilhørende udbudsbreve blev slettet.", "Projekter", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (!allIttLettersDeleted && someSubEntrepeneursDeleted)
            {
                MessageBox.Show("Nogle tilhørende udbudsbreve blev slettet. De tilbageværende udbudsbreve kan være knyttet til andre underentrepenører.", "Projekter", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (!allIttLettersDeleted && !someSubEntrepeneursDeleted)
            {
                MessageBox.Show("Ingen tilhørende udbudsbreve blev slettet. Evt. tilbageværende udbudsbreve kan være knyttet til andre underentrepenører.", "Projekter", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            //Show status for Offers
            if (allOffersDeleted)
            {
                MessageBox.Show("Alle tilhørende tilbud blev slettet.", "Projekter", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (!allOffersDeleted && someSubEntrepeneursDeleted)
            {
                MessageBox.Show("Nogle tilhørende tilbud blev slettet. De tilbageværende tilbud kan være knyttet til andre underentrepenører.", "Projekter", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (!allOffersDeleted && !someSubEntrepeneursDeleted)
            {
                MessageBox.Show("Ingen tilhørende tilbud blev slettet. Evt. tilbageværende tilbud kan være knyttet til andre underentrepenører.", "Projekter", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            //Show status for SubEntrepeneurs
            if (allSubEntrepeneursDeleted)
            {
                MessageBox.Show("Alle tilhørende underentrepenører blev slettet.", "Projekter", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (!allSubEntrepeneursDeleted && someSubEntrepeneursDeleted)
            {
                MessageBox.Show("Nogle tilhørende underentrepenører blev slettet. De tilbageværende underentrepenører kan være knyttet til andre entrepriser.", "Projekter", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (!allSubEntrepeneursDeleted && !someSubEntrepeneursDeleted)
            {
                MessageBox.Show("Ingen tilhørende underentrepenører blev slettet. Evt. tilbageværende underentrepenører kan være knyttet til andre entrepriser.", "Projekter", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            //Show status for Enterprises
            if (allEnterprisesDeleted)
            {
                MessageBox.Show("Alle tilhørende entrepriser blev slettet.", "Projekter", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (!allEnterprisesDeleted && someSubEntrepeneursDeleted)
            {
                MessageBox.Show("Nogle tilhørende entrepriser blev slettet. De tilbageværende entrepriser kan være knyttet til andre projekter.", "Projekter", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (!allEnterprisesDeleted && !someSubEntrepeneursDeleted)
            {
                MessageBox.Show("Ingen tilhørende entrepriser blev slettet. Evt. tilbageværende entrepriser kan være knyttet til andre projekter.", "Projekter", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        #endregion

    }
}
