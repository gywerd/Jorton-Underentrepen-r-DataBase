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
        public UcCraftCategories UcCraftCategories;
        public UcCraftGroups UcCraftGroups;
        public UcEnterpriseCreate UcEnterpriseCreate;
        public UcEnterpriseForms UcEnterpriseForms;
        public UcEnterprisesEdit UcEnterprisesEdit;
        public UcEnterprisesView UcEnterprisesView;
        public UcEntrepeneurCreate UcEntrepeneurCreate;
        public UcEntrepeneurEdit UcEntrepeneurEdit;
        public UcEntrepeneursStatusChange UcEntrepeneursStatusChange;
        public UcIttLetters UcIttLetters;
        public UcIttLettersShow UcIttLettersShow;
        public UcJobDescritions UcJobDescritions;
        public UcLogin UcLogin;
        public UcProjectCreate UcProjectCreate;
        public UcProjectCaseIdChange UcProjectCaseIdChange;
        public UcProjectCopy UcProjectCopy;
        public UcProjectDelete UcProjectDelete;
        public UcProjectsEdit UcProjectsEdit;
        public UcProjectsElaboration UcProjectsElaboration;
        public UcProjectStatusChange UcProjectStatusChange;
        public UcProjectStatuses UcProjectStatuses;
        public UcRequests UcRequests;
        public UcRequestsSentShow UcRequestsSentShow;
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
            TabOffer.Visibility = Visibility.Hidden;
            TabCalculation.Visibility = Visibility.Hidden;
            TabNews.Visibility = Visibility.Hidden;
            TabMaintenance.Visibility = Visibility.Hidden;
            TabAdministration.Visibility = Visibility.Hidden;
            Data.Visibility = Visibility.Hidden;
            Users.Visibility = Visibility.Hidden;
            OpenUcLogin();
        }

        #endregion

        #region Buttons
        private void ButtonAbout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Jorton Underentrepenør Database V. 0.9 BETA\n\n©2019 Jorton\n©2019 Daniel Giversen", "Om Jorton Underentrepenør Database", MessageBoxButton.OK, MessageBoxImage.Information);
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
                if (MessageBox.Show("Vil du åbne 'Opret Bygherre'. Alle ugemte data mistes!", "Bygherrer", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
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
                if (MessageBox.Show("Vil du åbne 'Rediger Bygherrer'. Alle ugemte data mistes!", "Bygherrer", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
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
                if (MessageBox.Show("Vil du åbne 'Rediger Status for Bygherrer'. Alle ugemte data mistes!", "Bygherrer", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
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
                if (MessageBox.Show("Vil du åbne 'Ændr Password'. Alle ugemte data mistes!", "Password", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
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
                if (MessageBox.Show("Vil du åbne 'Opret Kontakt'. Alle ugemte data mistes!", "Kontakter", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
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
                if (MessageBox.Show("Vil du åbne 'Slet Kontakt'. Alle ugemte data mistes!", "Kontakter", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
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
                if (MessageBox.Show("Vil du åbne 'Rediger Kontakt'. Alle ugemte data mistes!", "Kontakter", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
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
                if (MessageBox.Show("Vil du åbne 'Fagkategorier'. Alle ugemte data mistes!", "Fagkategorier", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    CBZ.UcMainEdited = false;
                    UcCraftCategories = new UcCraftCategories(CBZ, UcMain);
                    UcMain.Content = UcCraftCategories;
                }
            }
            else
            {
                UcCraftCategories = new UcCraftCategories(CBZ, UcMain);
                UcMain.Content = UcCraftCategories;
            }
        }

        private void ButtonUcCraftGroups_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                if (MessageBox.Show("Vil du åbne 'Faggrupper'. Alle ugemte data mistes!", "Faggrupper", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
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

        private void ButtonEnterpriseForms_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                if (MessageBox.Show("Vil du åbne 'Entrepriseformer'. Alle ugemte data mistes!", "Entrepriseformer", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
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

        private void ButtonEnterprisesCreate_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                if (MessageBox.Show("Vil du åbne 'Opret Entrepriser'. Alle ugemte data mistes!", "Entrepriser", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
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
                if (MessageBox.Show("Vil du åbne 'Rediger Entrepriser'. Alle ugemte data mistes!", "Entrepriser", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
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
                if (MessageBox.Show("Vil du åbne 'Vis Entrepriser'. Alle ugemte data mistes!", "Entrepriser", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
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

        private void ButtonEntrepeneurCreate_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                if (MessageBox.Show("Vil du åbne 'Opret Entrepenør'. Alle ugemte data mistes!", "Entrepenører", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
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
                if (MessageBox.Show("Vil du åbne 'Rediger Entrepenører'. Alle ugemte data mistes!", "Entrepenører", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
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
                if (MessageBox.Show("Vil du åbne 'Rediger Status for Entrepenører'. Alle ugemte data mistes!", "Entrepenører", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
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

        private void ButtonIttLettersSend_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                if (MessageBox.Show("Vil du åbne 'Vælg Modtagere'. Alle ugemte data mistes!", "Tilbudsbreve", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    CBZ.UcMainEdited = false;
                    UcIttLetters = new UcIttLetters(CBZ, UcMain);
                    UcMain.Content = UcIttLetters;
                }
            }
            else
            {
                UcIttLetters = new UcIttLetters(CBZ, UcMain);
                UcMain.Content = UcIttLetters;
            }
        }

        private void ButtonIttLettersShow_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                if (MessageBox.Show("Vil du åbne 'Afsend'. Alle ugemte data mistes!", "Tilbudsbreve", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    CBZ.UcMainEdited = false;
                    UcIttLettersShow = new UcIttLettersShow(CBZ, UcMain);
                    UcMain.Content = UcIttLettersShow;
                }
            }
            else
            {
                UcIttLettersShow = new UcIttLettersShow(CBZ, UcMain);
                UcMain.Content = UcIttLettersShow;
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
                if (MessageBox.Show("Vil du åbne 'Jobbeskrivelser'. Alle ugemte data mistes!", "Jobbeskrivelser", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
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
            UserName.Content = "";
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
                if (MessageBox.Show("Vil du åbne 'Kopier Projekt'. Alle ugemte data mistes!", "Projekter", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
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
                if (MessageBox.Show("Vil du åbne 'Opret Projekt'. Alle ugemte data mistes!", "Projekter", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    CBZ.UcMainEdited = false;
                    UcProjectCreate = new UcProjectCreate(CBZ, UcMain);
                    UcMain.Content = UcProjectCreate;
                }
            }
            else
            {
                UcProjectCreate = new UcProjectCreate(CBZ, UcMain);
                UcMain.Content = UcProjectCreate;
            }
        }

        private void ButtonProjectDelete_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                if (MessageBox.Show("Vil du åbne 'Slet Projekt'. Alle ugemte data mistes!", "Projekter", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    CBZ.UcMainEdited = false;
                    UcProjectDelete = new UcProjectDelete(CBZ, UcMain);
                    UcMain.Content = UcProjectDelete;
                }
            }
            else
            {
                UcProjectDelete = new UcProjectDelete(CBZ, UcMain);
                UcMain.Content = UcProjectDelete;
            }
        }

        private void ButtonProjectEdit_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                if (MessageBox.Show("Vil du åbne 'Rediger Projekt'. Alle ugemte data mistes!", "Projekter", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
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

        private void ButtonProjectElaborate_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                if (MessageBox.Show("Vil du åbne 'Uddybning af Projekt'. Alle ugemte data mistes!", "Projekter", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    CBZ.UcMainEdited = false;
                    UcProjectsElaboration = new UcProjectsElaboration(CBZ, UcMain);
                    UcMain.Content = UcProjectsElaboration;
                }
            }
            else
            {
                UcProjectsElaboration = new UcProjectsElaboration(CBZ, UcMain);
                UcMain.Content = UcProjectsElaboration;
            }

        }

        private void ButtonProjectChangeCaseId_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                if (MessageBox.Show("Vil du åbne 'Skift Sagsnummer'. Alle ugemte data mistes!", "Projekter", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
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
                if (MessageBox.Show("Vil du åbne 'Skift Status'. Alle ugemte data mistes!", "Projekter", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
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
                if (MessageBox.Show("Vil du åbne 'Projektstatuser'. Alle ugemte data mistes!", "Projektstatuser", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
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

        private void ButtonRequestsSend_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                if (MessageBox.Show("Vil du åbne 'Vælg Modtagere'. Alle ugemte data mistes!", "Forespørgsler", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    CBZ.UcMainEdited = false;
                    UcRequests = new UcRequests(CBZ, UcMain);
                    UcMain.Content = UcRequests;
                }
            }
            else
            {
                UcRequests = new UcRequests(CBZ, UcMain);
                UcMain.Content = UcRequests;
            }
        }

        private void ButtonRequestsShow_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                if (MessageBox.Show("Vil du åbne 'Vis Forespørgsler'. Alle ugemte data mistes!", "Forespørgsler", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    CBZ.UcMainEdited = false;
                    UcRequestsSentShow = new UcRequestsSentShow(CBZ, UcMain);
                    UcMain.Content = UcRequestsSentShow;
                }
            }
            else
            {
                UcRequestsSentShow = new UcRequestsSentShow(CBZ, UcMain);
                UcMain.Content = UcRequestsSentShow;
            }
        }

        private void ButtonRegions_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                if (MessageBox.Show("Vil du åbne 'Regioner'. Alle ugemte data mistes!", "Regioner", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
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
                if (MessageBox.Show("Vil du åbne 'Vælg Underentrepenører'. Alle ugemte data mistes!", "Underentrepenører", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
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
                if (MessageBox.Show("Vil du åbne 'Rediger Underentrepenører'. Alle ugemte data mistes!", "Underentrepenører", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
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
                if (MessageBox.Show("Vil du åbne 'Vis Underentrepenører'. Alle ugemte data mistes!", "Underentrepenører", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
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
                if (MessageBox.Show("Vil du åbne 'Udbudsformer'. Alle ugemte data mistes!", "Udbudsformer", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
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
                if (MessageBox.Show("Vil du åbne 'Opret Bruger'. Alle ugemte data mistes!", "Brugere", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
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
                if (MessageBox.Show("Vil du åbne 'Rediger Brugere'. Alle ugemte data mistes!", "Brugere", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
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
                if (MessageBox.Show("Vil du åbne 'Rediger Status for Brugere'. Alle ugemte data mistes!", "Brugere", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
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
                if (MessageBox.Show("Vil du åbne 'Postnummerliste'. Alle ugemte data mistes!", "Postnummerliste", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
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
            UcLogin = new UcLogin(CBZ, TabOffer, TabNews, TabMaintenance, TabAdministration, Data, Users, ButtonChangePassWord, ButtonLogOut, UserName, UcMain);
            UcMain.Content = UcLogin;
        }

        #endregion

    }
}
