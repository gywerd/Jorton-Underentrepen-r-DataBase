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
        public Bizz Bizz = new Bizz();
        public UcBuilders UcBuilders;
        public UcCommunication UcCommunication;
        public UcContacts UcContacts;
        public UcCraftCategories UcCraftCategories;
        public UcCraftGroups UcCraftGroups;
        public UcEnterpriseForms UcEnterpriseForms;
        public UcEnterprises UcEnterprises;
        public UcJobDescritions UcJobDescritions;
        public UcLogin UcLogin;
        public UcLoginHelp UcLoginHelp;
        public UcProject UcProject;
        public UcProjectStatusList UcProjectStatusList;
        public UcRegions UcRegions;
        public UcSubEntrepeneurList UcSubEntrepeneurList;
        public UcSubEntrepeneurs UcSubEntrepeneurs;
        public UcTenderForms UcTenderForms;
        public UcUsers UcUsers;
        public UcZipList UcZipList;
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            TabOffer.IsEnabled = false;
            TabAdministration.IsEnabled = false;
            HelpData.IsEnabled = false;
            Information.IsEnabled = false;
            OpenUcLoginHelp();
            OpenUcLogin();
        }

        #region Buttons
        private void ButtonAbout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Jorton Underentrepenør Database V. 0.3 ALPHA\n\n©2018 Jorton\n©2018 Daniel Giversen", "Om Jorton Underentrepenør Database", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void ButtonBuilders_Click(object sender, RoutedEventArgs e)
        {
            if (Bizz.UcRightActive)
            {
                if (MessageBox.Show("Vil du åbne 'Kommunikation'. Alt, der ikke er gemt vil blive mistet!", "Åbn Kommunikation", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    Bizz.UcRightActive = true;
                    UcLeft.Content = new UserControl();
                    UcBuilders = new UcBuilders(Bizz, UcRight);
                    UcRight.Content = UcBuilders;
                }
            }
            else
            {
                Bizz.UcRightActive = true;
                UcLeft.Content = new UserControl();
                UcBuilders = new UcBuilders(Bizz, UcRight);
                UcRight.Content = UcBuilders;
            }
        }

        private void ButtonCommunication_Click(object sender, RoutedEventArgs e)
        {
            if (Bizz.UcRightActive)
            {
                if (MessageBox.Show("Vil du åbne 'Kommunikation'. Alt, der ikke er gemt vil blive mistet!", "Åbn Kommunikation", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    Bizz.UcRightActive = false;
                    UcCommunication = new UcCommunication(Bizz, UcLeft, UcRight);
                    UcLeft.Content = UcCommunication;
                    UcRight.Content = new UserControl();
                }
            }
            else
            {
                Bizz.UcRightActive = false;
                UcCommunication = new UcCommunication(Bizz, UcLeft, UcRight);
                UcLeft.Content = UcCommunication;
                UcRight.Content = new UserControl();
            }
        }

        private void ButtonContacts_Click(object sender, RoutedEventArgs e)
        {
            if (Bizz.UcRightActive)
            {
                if (MessageBox.Show("Vil du åbne 'Kommunikation'. Alt, der ikke er gemt vil blive mistet!", "Åbn Kommunikation", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    Bizz.UcRightActive = true;
                    UcLeft.Content = new UserControl();
                    UcContacts = new UcContacts(Bizz, UcRight);
                    UcRight.Content = UcContacts;
                }
            }
            else
            {
                Bizz.UcRightActive = true;
                UcLeft.Content = new UserControl();
                UcContacts = new UcContacts(Bizz, UcRight);
                UcRight.Content = UcContacts;
            }
        }

        private void ButtonCraftCategories_Click(object sender, RoutedEventArgs e)
        {
            if (Bizz.UcRightActive)
            {
                if (MessageBox.Show("Vil du åbne 'Kommunikation'. Alt, der ikke er gemt vil blive mistet!", "Åbn Kommunikation", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    Bizz.UcRightActive = true;
                    UcLeft.Content = new UserControl();
                    UcCraftCategories = new UcCraftCategories(Bizz, UcRight);
                    UcRight.Content = UcCraftCategories;
                }
            }
            else
            {
                Bizz.UcRightActive = true;
                UcLeft.Content = new UserControl();
                UcCraftCategories = new UcCraftCategories(Bizz, UcRight);
                UcRight.Content = UcCraftCategories;
            }
        }

        private void ButtonCraftGroups_Click(object sender, RoutedEventArgs e)
        {
            if (Bizz.UcRightActive)
            {
                if (MessageBox.Show("Vil du åbne 'Kommunikation'. Alt, der ikke er gemt vil blive mistet!", "Åbn Kommunikation", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    Bizz.UcRightActive = true;
                    UcLeft.Content = new UserControl();
                    UcCraftGroups = new UcCraftGroups(Bizz, UcRight);
                    UcRight.Content = UcCraftGroups;
                }
            }
            else
            {
                Bizz.UcRightActive = true;
                UcLeft.Content = new UserControl();
                UcCraftGroups = new UcCraftGroups(Bizz, UcRight);
                UcRight.Content = UcCraftGroups;
            }
        }

        private void ButtonEnterprises_Click(object sender, RoutedEventArgs e)
        {
            if (Bizz.UcRightActive)
            {
                if (MessageBox.Show("Vil du åbne 'Entreprise'. Alt, der ikke er gemt vil blive mistet!", "Åbn Entreprise", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    Bizz.UcRightActive = false;
                    UcEnterprises = new UcEnterprises(Bizz, UcLeft, UcRight);
                    UcLeft.Content = UcEnterprises;
                    UcRight.Content = new UserControl();
                }
            }
            else
            {
                Bizz.UcRightActive = false;
                UcEnterprises = new UcEnterprises(Bizz, UcLeft, UcRight);
                UcLeft.Content = UcEnterprises;
                UcRight.Content = new UserControl();
            }
        }

        private void ButtonEnterpriseForms_Click(object sender, RoutedEventArgs e)
        {
            if (Bizz.UcRightActive)
            {
                if (MessageBox.Show("Vil du åbne 'Kommunikation'. Alt, der ikke er gemt vil blive mistet!", "Åbn Kommunikation", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    Bizz.UcRightActive = true;
                    UcLeft.Content = new UserControl();
                    UcEnterpriseForms = new UcEnterpriseForms(Bizz, UcRight);
                    UcRight.Content = UcEnterpriseForms;
                }
            }
            else
            {
                Bizz.UcRightActive = true;
                UcLeft.Content = new UserControl();
                UcEnterpriseForms = new UcEnterpriseForms(Bizz, UcRight);
                UcRight.Content = UcEnterpriseForms;
            }
        }

        private void ButtonJortonSubEntrepeneurDatabaseV10_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Endnu ikke implementeret", "Jorton Underentrepenør Database 1.0", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ButtonEstimate_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Ikke implementeret. Vil muligvis erstatte overslagsberegninger på excel-ark i en senere version. Særligt velegnet, når tilbud skal gives på skønnede priser.", "Beregninger", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ButtonJobDescritions_Click(object sender, RoutedEventArgs e)
        {
            if (Bizz.UcRightActive)
            {
                if (MessageBox.Show("Vil du åbne 'Jobbeskrivelser'. Alt, der ikke er gemt vil blive mistet!", "Åbn Jobbeskrivelser", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    Bizz.UcRightActive = true;
                    UcLeft.Content = new UserControl();
                    UcJobDescritions = new UcJobDescritions(Bizz, UcRight);
                    UcRight.Content = UcJobDescritions;
                }
            }
            else
            {
                Bizz.UcRightActive = true;
                UcLeft.Content = new UserControl();
                UcJobDescritions = new UcJobDescritions(Bizz, UcRight);
                UcRight.Content = UcJobDescritions;
            }
        }

        private void ButtonHelp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Endnu ikke implementeret", "Hjælp", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ButtonOfferList_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Ikke implementeret. Vil muligvis erstatte tilbudslister på excel-ark i en senere version", "Beregninger", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ButtonOptions_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Endnu ikke implementeret", "Indstillinger", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ButtonProject_Click(object sender, RoutedEventArgs e)
        {
            if (Bizz.UcRightActive)
            {
                if (MessageBox.Show("Vil du åbne 'Projekt'. Alt, der ikke er gemt vil blive mistet!", "Åbn Projekt", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    Bizz.UcRightActive = false;
                    UcProject = new UcProject(Bizz, UcLeft, UcRight);
                    UcLeft.Content = UcProject;
                    UcRight.Content = new UserControl();
                }
            }
            else
            {
                Bizz.UcRightActive = false;
                UcProject = new UcProject(Bizz, UcLeft, UcRight);
                UcLeft.Content = UcProject;
                UcRight.Content = new UserControl();
            }
        }

        private void ButtonProjectStatusList_Click(object sender, RoutedEventArgs e)
        {
            if (Bizz.UcRightActive)
            {
                if (MessageBox.Show("Vil du åbne 'Projektstatusliste'. Alt, der ikke er gemt vil blive mistet!", "Åbn Projektstatusliste", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    Bizz.UcRightActive = true;
                    UcLeft.Content = UcProjectStatusList;
                    UcProjectStatusList = new UcProjectStatusList(Bizz, UcRight);
                    UcRight.Content = new UserControl();
                }
            }
            else
            {
                Bizz.UcRightActive = true;
                UcLeft.Content = UcProjectStatusList;
                UcProjectStatusList = new UcProjectStatusList(Bizz, UcRight);
                UcRight.Content = new UserControl();
            }
        }

        private void ButtonRegions_Click(object sender, RoutedEventArgs e)
        {
            if (Bizz.UcRightActive)
            {
                if (MessageBox.Show("Vil du åbne 'Regioner'. Alt, der ikke er gemt vil blive mistet!", "Åbn Regioner", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    Bizz.UcRightActive = true;
                    UcLeft.Content = new UserControl();
                    UcRegions = new UcRegions(Bizz, UcRight);
                    UcRight.Content = UcRegions;
                }
            }
            else
            {
                Bizz.UcRightActive = true;
                UcLeft.Content = new UserControl();
                UcRegions = new UcRegions(Bizz, UcRight);
                UcRight.Content = UcRegions;
            }
        }

        private void ButtonSubEntrepeneurList_Click(object sender, RoutedEventArgs e)
        {
            if (Bizz.UcRightActive)
            {
                if (MessageBox.Show("Vil du åbne 'Underentrepenørlisten'. Alt, der ikke er gemt vil blive mistet!", "Åbn Underentrepenørliste", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    Bizz.UcRightActive = true;
                    UcLeft.Content = new UserControl();
                    UcSubEntrepeneurList = new UcSubEntrepeneurList(Bizz, UcRight);
                    UcRight.Content = UcSubEntrepeneurList;
                }
            }
            else
            {
                Bizz.UcRightActive = true;
                UcLeft.Content = new UserControl();
                UcSubEntrepeneurList = new UcSubEntrepeneurList(Bizz, UcRight);
                UcRight.Content = UcSubEntrepeneurList;
            }
        }

        private void ButtonSubEntrepeneurs_Click(object sender, RoutedEventArgs e)
        {
            if (Bizz.UcRightActive)
            {
                if (MessageBox.Show("Vil du åbne 'Underentrepenør'. Alt, der ikke er gemt vil blive mistet!", "Åbn Underentrepenør", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    Bizz.UcRightActive = false;
                    UcSubEntrepeneurs = new UcSubEntrepeneurs(Bizz, UcLeft, UcRight);
                    UcLeft.Content = UcSubEntrepeneurs;
                    UcRight.Content = new UserControl();
                }
            }
            else
            {
                Bizz.UcRightActive = false;
                UcSubEntrepeneurs = new UcSubEntrepeneurs(Bizz, UcLeft, UcRight);
                UcLeft.Content = UcSubEntrepeneurs;
                UcRight.Content = new UserControl();
            }
        }

        private void ButtonTenderForms_Click(object sender, RoutedEventArgs e)
        {
            if (Bizz.UcRightActive)
            {
                if (MessageBox.Show("Vil du åbne 'Udbudsformer'. Alt, der ikke er gemt vil blive mistet!", "Åbn Brugere", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    Bizz.UcRightActive = true;
                    UcLeft.Content = new UserControl();
                    UcTenderForms = new UcTenderForms(Bizz, UcRight);
                    UcRight.Content = UcTenderForms;
                }
            }
            else
            {
                Bizz.UcRightActive = true;
                UcLeft.Content = new UserControl();
                UcTenderForms = new UcTenderForms(Bizz, UcRight);
                UcRight.Content = UcTenderForms;
            }
        }

        private void ButtonUsers_Click(object sender, RoutedEventArgs e)
        {
            if (Bizz.UcRightActive)
            {
                if (MessageBox.Show("Vil du åbne 'Brugere'. Alt, der ikke er gemt vil blive mistet!", "Åbn Brugere", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    Bizz.UcRightActive = true;
                    UcLeft.Content = new UserControl();
                    UcUsers = new UcUsers(Bizz, UcRight);
                    UcRight.Content = UcUsers;
                }
            }
            else
            {
                Bizz.UcRightActive = true;
                UcLeft.Content = new UserControl();
                UcUsers = new UcUsers(Bizz, UcRight);
                UcRight.Content = UcUsers;
            }
        }

        private void ButtonZipList_Click(object sender, RoutedEventArgs e)
        {
            if (Bizz.UcRightActive)
            {
                if (MessageBox.Show("Vil du åbne 'Postnummerlisten'. Alt, der ikke er gemt vil blive mistet!", "Åbn Postnummerliste", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    Bizz.UcRightActive = true;
                    UcLeft.Content = new UserControl();
                    UcZipList = new UcZipList(Bizz, UcRight);
                    UcRight.Content = UcZipList;
                }
            }
            else
            {
                Bizz.UcRightActive = true;
                UcLeft.Content = new UserControl();
                UcZipList = new UcZipList(Bizz, UcRight);
                UcRight.Content = UcZipList;
            }
        }

        private void MenuItemQuit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MenuItemLogout_Click(object sender, RoutedEventArgs e)
        {
            TabOffer.IsEnabled = false;
            TabAdministration.IsEnabled = false;
            HelpData.IsEnabled = false;
            Information.IsEnabled = false;
            Bizz = new Bizz();
            UserName.Text = "";
            MenuItemChangePassWord.IsEnabled = false;
            MenuItemLogOut.IsEnabled = false;
            OpenUcLogin();
            OpenUcLoginHelp();
        }

        private void MenuItemChangePassWord_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Endnu ikke implementeret", "Ændre password", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #endregion

        #region Methods
        private void OpenUcLogin()
        {
            Bizz.UcRightActive = true;
            UcLogin = new UcLogin(Bizz, TabOffer, TabAdministration, HelpData, Information, MenuItemChangePassWord, MenuItemLogOut, UserName, UcLeft, UcRight);
            UcRight.Content = UcLogin;
        }

        private void OpenUcLoginHelp()
        {
            UcLoginHelp = new UcLoginHelp();
            UcLeft.Content = UcLoginHelp;
        }

        #endregion

    }
}
