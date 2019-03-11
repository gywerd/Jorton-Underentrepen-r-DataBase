using JudBizz;
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
using System.Windows.Shapes;

namespace JudGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Fields
        public Bizz CBZ;

        public UcBuilderCreate UcBuilderCreate;
        public UcBuildersEdit UcBuildersEdit;
        public UcBuildersStatusChange UcBuildersStatusChange;
        public UcChangePassWord UcChangePassWord;
        public UcContactCreate UcContactCreate;
        public UcContactsDelete UcContactsDelete;
        public UcContactsEdit UcContactsEdit;
        public UcCraftGroupCategories UcCraftGroupCategories;
        public UcCraftGroups UcCraftGroups;
        public UcEnterpriseCreate UcEnterpriseCreate;
        public UcEnterpriseForms UcEnterpriseForms;
        public UcEnterprisesEdit UcEnterprisesEdit;
        public UcEnterprisesView UcEnterprisesView;
        public UcEntrepeneurCreate UcEntrepeneurCreate;
        public UcEntrepeneurEdit UcEntrepeneurEdit;
        public UcEntrepeneursStatusChange UcEntrepeneursStatusChange;
        public UcIttLettersPrepareCommonLetter UcIttLettersPrepareCommonLetter;
        public UcIttLettersPreparePersonalLetters UcIttLettersPreparePersonalLetters;
        public UcIttLettersChooseReceivers UcIttLettersChooseReceivers;
        public UcIttLettersSendLetters UcIttLettersSendLetters;
        public UcJobDescritions UcJobDescritions;
        public UcLogin UcLogin;
        public UcProjectCreate UcProjectCreate;
        public UcProjectCaseIdChange UcProjectCaseIdChange;
        public UcProjectCopy UcProjectCopy;
        public UcProjectsEdit UcProjectsEdit;
        public UcProjectStatusChange UcProjectStatusChange;
        public UcProjectStatuses UcProjectStatuses;
        public UcRequestsChooseReceivers UcRequestsChooseReceivers;
        public UcRequestPrepare UcRequestPrepare;
        public UcRequestSend UcRequestSend;
        public UcRegions UcRegions;
        public UcSubEntrepeneursChoose UcSubEntrepeneursChoose;
        public UcSubEntrepeneursEdit UcSubEntrepeneursEdit;
        public UcSubEntrepeneursView UcSubEntrepeneursView;
        public UcTenderForms UcTenderForms;
        public UcUserCreate UcUserCreate; 
        public UcUsersEdit UcUsersEdit;
        public UcUsersStatusChange UcUsersStatusChange;
        public UcZipList UcZipList;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor, that initialises the Main Window
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            CBZ = new Bizz();
            ButtonChangePassWord.IsEnabled = false;
            ButtonLogOut.IsEnabled = false;
            TabOffer.IsEnabled = false;
            TabMaintenance.IsEnabled = false;
            TabAdministration.IsEnabled = false;
            Data.IsEnabled = false;
            Users.IsEnabled = false;
            OpenUcLogin();
        }

        #endregion

        #region Buttons
        private void ButtonAbout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Jorton Underentrepenør Database V. 0.4 ALPHA\n\n©2018 Jorton\n©2018 Daniel Giversen", "Om Jorton Underentrepenør Database", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ButtonBicV115_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(@"Documentation\BIC Nyheder version 1.1.5.pdf");
        }

        private void ButtonBicV116_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(@"Documentation\BIC Nyheder version 1.1.6.pdf");
        }

        private void ButtonBicV117_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(@"Documentation\BIC Nyheder version 1.1.7.pdf");
        }

        private void ButtonBuilderCreate_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                if (MessageBox.Show("Vil du åbne 'Opret Bygherre'. Alt, der ikke er gemt vil blive mistet!", "Bygherrer", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    CBZ.UcMainEdited = false;
                    UcBuilderCreate = new UcBuilderCreate(CBZ, UcMain);
                    this.UcMain.Content = UcBuilderCreate;
                }
            }
            else
            {
                UcBuilderCreate = new UcBuilderCreate(CBZ, UcMain);
                UcMain.Content = UcBuilderCreate;
            }
        }

        private void ButtonBuildersEdit_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                if (MessageBox.Show("Vil du åbne 'Rediger Bygherrer'. Alt, der ikke er gemt vil blive mistet!", "Bygherrer", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    CBZ.UcMainEdited = false;
                    UcBuildersEdit = new UcBuildersEdit(CBZ, UcMain);
                    this.UcMain.Content = UcBuildersEdit;
                }
            }
            else
            {
                UcBuildersEdit = new UcBuildersEdit(CBZ, UcMain);
                UcMain.Content = UcBuildersEdit;
            }
        }

        private void ButtonBuildersStatusChange_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                if (MessageBox.Show("Vil du åbne 'Rediger Status for Bygherrer'. Alt, der ikke er gemt vil blive mistet!", "Bygherrer", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    CBZ.UcMainEdited = false;
                    UcBuildersStatusChange = new UcBuildersStatusChange(CBZ, UcMain);
                    this.UcMain.Content = UcBuildersStatusChange;
                }
            }
            else
            {
                UcBuildersStatusChange = new UcBuildersStatusChange(CBZ, UcMain);
                UcMain.Content = UcBuildersStatusChange;
            }
        }

        private void ButtonChangePassWord_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                if (MessageBox.Show("Vil du åbne 'Ændr Password'. Alt, der ikke er gemt vil blive mistet!", "Password", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    CBZ.UcMainEdited = false;
                    UcChangePassWord = new UcChangePassWord(CBZ, UcMain);
                    this.UcMain.Content = UcChangePassWord;
                }
            }
            else
            {
                UcChangePassWord = new UcChangePassWord(CBZ, UcMain);
                UcMain.Content = UcChangePassWord;
            }
        }

        private void ButtonContactCreate_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                if (MessageBox.Show("Vil du åbne 'Opret Kontakt'. Alt, der ikke er gemt vil blive mistet!", "Kontakter", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    CBZ.UcMainEdited = false;
                    UcContactCreate = new UcContactCreate(CBZ, UcMain);
                    UcMain.Content = UcContactCreate;
                }
            }
            else
            {
                UcContactCreate = new UcContactCreate(CBZ, UcMain);
                UcMain.Content = UcContactCreate;
            }
        }

        private void ButtonContactsDelete_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                if (MessageBox.Show("Vil du åbne 'Slet Kontakt'. Alt, der ikke er gemt vil blive mistet!", "Kontakter", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    CBZ.UcMainEdited = false;
                    UcContactsDelete = new UcContactsDelete(CBZ, UcMain);
                    UcMain.Content = UcContactsDelete;
                }
            }
            else
            {
                UcContactsDelete = new UcContactsDelete(CBZ, UcMain);
                UcMain.Content = UcContactsDelete;
            }
        }

        private void ButtonContactsEdit_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                if (MessageBox.Show("Vil du åbne 'Rediger Kontakt'. Alt, der ikke er gemt vil blive mistet!", "Kontakter", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    CBZ.UcMainEdited = false;
                    UcContactsEdit = new UcContactsEdit(CBZ, UcMain);
                    UcMain.Content = UcContactsEdit;
                }
            }
            else
            {
                UcContactsEdit = new UcContactsEdit(CBZ, UcMain);
                UcMain.Content = UcContactsEdit;
            }
        }

        private void ButtonCraftCategories_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                if (MessageBox.Show("Vil du åbne 'Fagkategorier'. Alt, der ikke er gemt vil blive mistet!", "Fagkategorier", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    CBZ.UcMainEdited = false;
                    UcCraftGroupCategories = new UcCraftGroupCategories(CBZ, UcMain);
                    UcMain.Content = UcCraftGroupCategories;
                }
            }
            else
            {
                UcCraftGroupCategories = new UcCraftGroupCategories(CBZ, UcMain);
                UcMain.Content = UcCraftGroupCategories;
            }
        }

        private void ButtonUcCraftGroups_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                if (MessageBox.Show("Vil du åbne 'Faggrupper'. Alt, der ikke er gemt vil blive mistet!", "Faggrupper", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    CBZ.UcMainEdited = false;
                    UcCraftGroups = new UcCraftGroups(CBZ, UcMain);
                    UcMain.Content = UcCraftGroups;
                }
            }
            else
            {
                UcCraftGroups = new UcCraftGroups(CBZ, UcMain);
                UcMain.Content = UcCraftGroups;
            }
        }

        private void ButtonEnterprisesCreate_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                if (MessageBox.Show("Vil du åbne 'Opret Entrepriser'. Alt, der ikke er gemt vil blive mistet!", "Entrepriser", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    CBZ.UcMainEdited = false;
                    UcEnterpriseCreate = new UcEnterpriseCreate(CBZ, UcMain);
                    UcMain.Content = UcEnterpriseCreate;
                }
            }
            else
            {
                UcEnterpriseCreate = new UcEnterpriseCreate(CBZ, UcMain);
                UcMain.Content = UcEnterpriseCreate;
            }
        }

        private void ButtonEnterprisesEdit_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                if (MessageBox.Show("Vil du åbne 'Rediger Entrepriser'. Alt, der ikke er gemt vil blive mistet!", "Entrepriser", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    CBZ.UcMainEdited = false;
                    UcEnterprisesEdit = new UcEnterprisesEdit(CBZ, UcMain);
                    UcMain.Content = UcEnterprisesEdit;
                }
            }
            else
            {
                UcEnterprisesEdit = new UcEnterprisesEdit(CBZ, UcMain);
                UcMain.Content = UcEnterprisesEdit;
            }
        }

        private void ButtonEnterprisesView_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                if (MessageBox.Show("Vil du åbne 'Vis Entrepriser'. Alt, der ikke er gemt vil blive mistet!", "Entrepriser", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    CBZ.UcMainEdited = false;
                    UcEnterprisesView = new UcEnterprisesView(CBZ, UcMain);
                    UcMain.Content = UcEnterprisesView;
                }
            }
            else
            {
                UcEnterprisesView = new UcEnterprisesView(CBZ, UcMain);
                UcMain.Content = UcEnterprisesView;
            }
        }

        private void ButtonEnterpriseForms_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                if (MessageBox.Show("Vil du åbne 'Entrepriseformer'. Alt, der ikke er gemt vil blive mistet!", "Entrepriseformer", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    CBZ.UcMainEdited = false;
                    UcEnterpriseForms = new UcEnterpriseForms(CBZ, UcMain);
                    UcMain.Content = UcEnterpriseForms;
                }
            }
            else
            {
                UcEnterpriseForms = new UcEnterpriseForms(CBZ, UcMain);
                UcMain.Content = UcEnterpriseForms;
            }
        }

        private void ButtonEntrepeneurCreate_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                if (MessageBox.Show("Vil du åbne 'Opret Entrepenør'. Alt, der ikke er gemt vil blive mistet!", "Entrepenører", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    CBZ.UcMainEdited = false;
                    UcEntrepeneurCreate = new UcEntrepeneurCreate(CBZ, UcMain);
                    UcMain.Content = UcEntrepeneurCreate;
                }
            }
            else
            {
                UcEntrepeneurCreate = new UcEntrepeneurCreate(CBZ, UcMain);
                UcMain.Content = UcEntrepeneurCreate;
            }
        }

        private void ButtonEntrepeneursEdit_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                if (MessageBox.Show("Vil du åbne 'Rediger Entrepenører'. Alt, der ikke er gemt vil blive mistet!", "Entrepenører", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    CBZ.UcMainEdited = false;
                    UcEntrepeneurEdit = new UcEntrepeneurEdit(CBZ, UcMain);
                    UcMain.Content = UcEntrepeneurEdit;
                }
            }
            else
            {
                UcEntrepeneurEdit = new UcEntrepeneurEdit(CBZ, UcMain);
                UcMain.Content = UcEntrepeneurEdit;
            }
        }

        private void ButtonEntrepeneursStatusChange_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                if (MessageBox.Show("Vil du åbne 'Rediger Status for Entrepenører'. Alt, der ikke er gemt vil blive mistet!", "Entrepenører", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    CBZ.UcMainEdited = false;
                    UcEntrepeneursStatusChange = new UcEntrepeneursStatusChange(CBZ, UcMain);
                    UcMain.Content = UcEntrepeneursStatusChange;
                }
            }
            else
            {
                UcEntrepeneursStatusChange = new UcEntrepeneursStatusChange(CBZ, UcMain);
                UcMain.Content = UcEntrepeneursStatusChange;
            }
        }

        private void ButtonEstimate_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Ikke implementeret. Vil muligvis erstatte overslagsberegninger på excel-ark i en senere version. Særligt velegnet, når tilbud skal gives på skønnede priser.", "Beregninger", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ButtonHelp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Endnu ikke implementeret", "Hjælp", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ButtonIttLetterChooseReceivers_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                if (MessageBox.Show("Vil du åbne 'Vælg Modtagere'. Alt, der ikke er gemt vil blive mistet!", "Tilbudsbreve", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    CBZ.UcMainEdited = false;
                    UcIttLettersChooseReceivers = new UcIttLettersChooseReceivers(CBZ, UcMain);
                    UcMain.Content = UcIttLettersChooseReceivers;
                }
            }
            else
            {
                UcIttLettersChooseReceivers = new UcIttLettersChooseReceivers(CBZ, UcMain);
                UcMain.Content = UcIttLettersChooseReceivers;
            }
        }

        private void ButtonIttLetterPrepareCommon_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                if (MessageBox.Show("Vil du åbne 'Klargør Fælles Brevdel'. Alt, der ikke er gemt vil blive mistet!", "Tilbudsbreve", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    CBZ.UcMainEdited = false;
                    UcIttLettersPrepareCommonLetter = new UcIttLettersPrepareCommonLetter(CBZ, UcMain);
                    UcMain.Content = UcIttLettersPrepareCommonLetter;
                }
            }
            else
            {
                UcIttLettersPrepareCommonLetter = new UcIttLettersPrepareCommonLetter(CBZ, UcMain);
                UcMain.Content = UcIttLettersPrepareCommonLetter;
            }
        }

        private void ButtonIttLetterPreparePersonal_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                if (MessageBox.Show("Vil du åbne 'Klargør Personlig Brevdel'. Alt, der ikke er gemt vil blive mistet!", "Tilbudsbreve", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    CBZ.UcMainEdited = false;
                    UcIttLettersPreparePersonalLetters = new UcIttLettersPreparePersonalLetters(CBZ, UcMain);
                    UcMain.Content = UcIttLettersPreparePersonalLetters;
                }
            }
            else
            {
                UcIttLettersPreparePersonalLetters = new UcIttLettersPreparePersonalLetters(CBZ, UcMain);
                UcMain.Content = UcIttLettersPreparePersonalLetters;
            }
        }

        private void ButtonIttLetterSend_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                if (MessageBox.Show("Vil du åbne 'Afsend'. Alt, der ikke er gemt vil blive mistet!", "Tilbudsbreve", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    CBZ.UcMainEdited = false;
                    UcIttLettersSendLetters = new UcIttLettersSendLetters(CBZ, UcMain);
                    UcMain.Content = UcIttLettersSendLetters;
                }
            }
            else
            {
                UcIttLettersSendLetters = new UcIttLettersSendLetters(CBZ, UcMain);
                UcMain.Content = UcIttLettersSendLetters;
            }

        }

        private void ButtonIttList_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Ikke implementeret. Vil muligvis erstatte tilbudslister på excel-ark i en senere version", "Beregninger", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ButtonJobDescritions_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                if (MessageBox.Show("Vil du åbne 'Jobbeskrivelser'. Alt, der ikke er gemt vil blive mistet!", "Jobbeskrivelser", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    CBZ.UcMainEdited = false;
                    UcJobDescritions = new UcJobDescritions(CBZ, UcMain);
                    UcMain.Content = UcJobDescritions;
                }
            }
            else
            {
                UcJobDescritions = new UcJobDescritions(CBZ, UcMain);
                UcMain.Content = UcJobDescritions;
            }
        }

        private void ButtonJortonSubEntrepeneurDatabaseV10_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Endnu ikke implementeret", "Jorton Underentrepenør Database 1.0", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ButtonLogOut_Click(object sender, RoutedEventArgs e)
        {
            ButtonChangePassWord.IsEnabled = false;
            ButtonLogOut.IsEnabled = false;
            TabOffer.IsEnabled = false;
            TabMaintenance.IsEnabled = false;
            Data.IsEnabled = false;
            Users.IsEnabled = false;
            CBZ = new Bizz();
            UserName.Text = "";
            OpenUcLogin();
        }

        private void ButtonOptions_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Endnu ikke implementeret", "Indstillinger", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ButtonProjectCopy_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                if (MessageBox.Show("Vil du åbne 'Kopier Projekt'. Alt, der ikke er gemt vil blive mistet!", "Projekter", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    CBZ.UcMainEdited = false;
                    UcProjectCopy = new UcProjectCopy(CBZ, UcMain);
                    UcMain.Content = UcProjectCopy;
                }
            }
            else
            {
                UcProjectCopy = new UcProjectCopy(CBZ, UcMain);
                UcMain.Content = UcProjectCopy;
            }

        }

        private void ButtonProjectCreate_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                if (MessageBox.Show("Vil du åbne 'Opret Projekt'. Alt, der ikke er gemt vil blive mistet!", "Projekter", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    CBZ.UcMainEdited = false;
                    UcProjectCreate = new UcProjectCreate(CBZ, UcMain);
                    UcMain.Content = new UserControl();
                }
            }
            else
            {
                UcProjectCreate = new UcProjectCreate(CBZ, UcMain);
                UcMain.Content = new UserControl();
            }
        }

        private void ButtonProjectEdit_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                if (MessageBox.Show("Vil du åbne 'Rediger Projekter'. Alt, der ikke er gemt vil blive mistet!", "Projekter", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    CBZ.UcMainEdited = false;
                    UcProjectsEdit = new UcProjectsEdit(CBZ, UcMain);
                    UcMain.Content = UcProjectsEdit;
                }
            }
            else
            {
                UcProjectsEdit = new UcProjectsEdit(CBZ, UcMain);
                UcMain.Content = UcProjectsEdit;
            }

        }

        private void ButtonProjectChangeCaseId_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                if (MessageBox.Show("Vil du åbne 'Skift Sagsnummer'. Alt, der ikke er gemt vil blive mistet!", "Projekter", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    CBZ.UcMainEdited = false;
                    UcProjectCaseIdChange = new UcProjectCaseIdChange(CBZ, UcMain);
                    UcMain.Content = UcProjectCaseIdChange;
                }
            }
            else
            {
                UcProjectCaseIdChange = new UcProjectCaseIdChange(CBZ, UcMain);
                UcMain.Content = UcProjectCaseIdChange;
            }

        }

        private void ButtonProjectStatusChange_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                if (MessageBox.Show("Vil du åbne 'Skift Status'. Alt, der ikke er gemt vil blive mistet!", "Projekter", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    CBZ.UcMainEdited = false;
                    UcProjectStatusChange = new UcProjectStatusChange(CBZ, UcMain);
                    UcMain.Content = UcProjectStatusChange;
                }
            }
            else
            {
                UcProjectStatusChange = new UcProjectStatusChange(CBZ, UcMain);
                UcMain.Content = UcProjectStatusChange;
            }

        }

        private void ButtonProjectStatuses_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                if (MessageBox.Show("Vil du åbne 'Projektstatuser'. Alt, der ikke er gemt vil blive mistet!", "Projektstatuser", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    CBZ.UcMainEdited = false;
                    UcProjectStatuses = new UcProjectStatuses(CBZ, UcMain);
                    UcMain.Content = UcProjectStatuses;
                }
            }
            else
            {
                UcProjectStatuses = new UcProjectStatuses(CBZ, UcMain);
                UcMain.Content = UcProjectStatuses;
            }
        }

        private void ButtonQuit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ButtonRequestChooseReceivers_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                if (MessageBox.Show("Vil du åbne 'Vælg Modtagere'. Alt, der ikke er gemt vil blive mistet!", "Forespørgsler", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    CBZ.UcMainEdited = false;
                    UcRequestsChooseReceivers = new UcRequestsChooseReceivers(CBZ, UcMain);
                    UcMain.Content = UcRequestsChooseReceivers;
                }
            }
            else
            {
                UcRequestsChooseReceivers = new UcRequestsChooseReceivers(CBZ, UcMain);
                UcMain.Content = UcRequestsChooseReceivers;
            }
        }

        private void ButtonRequestPrepare_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                if (MessageBox.Show("Vil du åbne 'Klargør forespørgsel'. Alt, der ikke er gemt vil blive mistet!", "Forespørgsler", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    CBZ.UcMainEdited = false;
                    UcRequestPrepare = new UcRequestPrepare(CBZ, UcMain);
                    UcMain.Content = UcRequestPrepare;
                }
            }
            else
            {
                UcRequestPrepare = new UcRequestPrepare(CBZ, UcMain);
                UcMain.Content = UcRequestPrepare;
            }
        }

        private void ButtonRequestSend_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                if (MessageBox.Show("Vil du åbne 'Afsend'. Alt, der ikke er gemt vil blive mistet!", "Forespørgsler", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    CBZ.UcMainEdited = false;
                    UcRequestSend = new UcRequestSend(CBZ, UcMain);
                    UcMain.Content = UcRequestSend;
                }
            }
            else
            {
                UcRequestSend = new UcRequestSend(CBZ, UcMain);
                UcMain.Content = UcRequestSend;
            }
        }

        private void ButtonRegions_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                if (MessageBox.Show("Vil du åbne 'Regioner'. Alt, der ikke er gemt vil blive mistet!", "Regioner", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    CBZ.UcMainEdited = false;
                    UcRegions = new UcRegions(CBZ, UcMain);
                    UcMain.Content = UcRegions;
                }
            }
            else
            {
                UcRegions = new UcRegions(CBZ, UcMain);
                UcMain.Content = UcRegions;
            }
        }

        private void ButtonSubEntrepeneursChoose_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                if (MessageBox.Show("Vil du åbne 'Vælg Underentrepenører'. Alt, der ikke er gemt vil blive mistet!", "Underentrepenører", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    CBZ.UcMainEdited = false;
                    UcSubEntrepeneursChoose = new UcSubEntrepeneursChoose(CBZ, UcMain);
                    UcMain.Content = UcSubEntrepeneursChoose;
                }
            }
            else
            {
                UcSubEntrepeneursChoose = new UcSubEntrepeneursChoose(CBZ, UcMain);
                UcMain.Content = UcSubEntrepeneursChoose;
            }
        }
        
        private void ButtonSubEntrepeneursEdit_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                if (MessageBox.Show("Vil du åbne 'Rediger Underentrepenører'. Alt, der ikke er gemt vil blive mistet!", "Underentrepenører", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    CBZ.UcMainEdited = false;
                    UcSubEntrepeneursEdit = new UcSubEntrepeneursEdit(CBZ, UcMain);
                    UcMain.Content = UcSubEntrepeneursEdit;
                }
            }
            else
            {
                UcSubEntrepeneursEdit = new UcSubEntrepeneursEdit(CBZ, UcMain);
                UcMain.Content = UcSubEntrepeneursEdit;
            }

        }
        
        private void ButtonSubEntrepeneursView_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                if (MessageBox.Show("Vil du åbne 'Vis Underentrepenører'. Alt, der ikke er gemt vil blive mistet!", "Underentrepenører", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    CBZ.UcMainEdited = false;
                    UcSubEntrepeneursView = new UcSubEntrepeneursView(CBZ, UcMain);
                    UcMain.Content = UcSubEntrepeneursView;
                }
            }
            else
            {
                UcSubEntrepeneursView = new UcSubEntrepeneursView(CBZ, UcMain);
                UcMain.Content = UcSubEntrepeneursView;
            }
        }

        private void ButtonTenderForms_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                if (MessageBox.Show("Vil du åbne 'Udbudsformer'. Alt, der ikke er gemt vil blive mistet!", "Udbudsformer", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    CBZ.UcMainEdited = false;
                    UcTenderForms = new UcTenderForms(CBZ, UcMain);
                    UcMain.Content = UcTenderForms;
                }
            }
            else
            {
                UcTenderForms = new UcTenderForms(CBZ, UcMain);
                UcMain.Content = UcTenderForms;
            }
        }

        private void ButtonUserCreate_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                if (MessageBox.Show("Vil du åbne 'Opret Bruger'. Alt, der ikke er gemt vil blive mistet!", "Brugere", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    CBZ.UcMainEdited = false;
                    UcUserCreate = new UcUserCreate(CBZ, UcMain);
                    UcMain.Content = UcUserCreate;
                }
            }
            else
            {
                UcUserCreate = new UcUserCreate(CBZ, UcMain);
                UcMain.Content = UcUserCreate;
            }
        }

        private void ButtonUsersEdit_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                if (MessageBox.Show("Vil du åbne 'Rediger Brugere'. Alt, der ikke er gemt vil blive mistet!", "Brugere", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    CBZ.UcMainEdited = false;
                    UcUsersEdit = new UcUsersEdit(CBZ, UcMain);
                    UcMain.Content = UcUsersEdit;
                }
            }
            else
            {
                UcUsersEdit = new UcUsersEdit(CBZ, UcMain);
                UcMain.Content = UcUsersEdit;
            }
        }

        private void ButtonUsersStatusChange_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                if (MessageBox.Show("Vil du åbne 'Rediger Status for Brugere'. Alt, der ikke er gemt vil blive mistet!", "Brugere", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    CBZ.UcMainEdited = false;
                    UcUsersStatusChange = new UcUsersStatusChange(CBZ, UcMain);
                    UcMain.Content = UcUsersStatusChange;
                }
            }
            else
            {
                UcUsersStatusChange = new UcUsersStatusChange(CBZ, UcMain);
                UcMain.Content = UcUsersStatusChange;
            }
        }

        private void ButtonZipList_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                if (MessageBox.Show("Vil du åbne 'Postnummerliste'. Alt, der ikke er gemt vil blive mistet!", "Postnummerliste", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    CBZ.UcMainEdited = false;
                    UcZipList = new UcZipList(CBZ, UcMain);
                    UcMain.Content = UcZipList;
                }
            }
            else
            {
                UcZipList = new UcZipList(CBZ, UcMain);
                UcMain.Content = UcZipList;
            }
        }

        #endregion

        #region Methods
        private void OpenUcLogin()
        {
            CBZ.UcMainEdited = true;
            UcLogin = new UcLogin(CBZ, TabOffer, TabMaintenance, TabAdministration, Data, Users, ButtonChangePassWord, ButtonLogOut, UserName, UcMain);
            UcMain.Content = UcLogin;
        }

        #endregion

    }
}
