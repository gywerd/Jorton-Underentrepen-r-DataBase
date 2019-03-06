﻿using JudBizz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
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
    /// Interaction logic for UcLogin.xaml
    /// </summary>
    public partial class UcLogin : UserControl
    {
        #region Fields
        public Bizz CBZ;
        public RibbonTab TabOffer;
        public RibbonTab TabMaintenance;
        public RibbonTab TabAdministration;
        public RibbonGroup Data;
        public RibbonGroup Users;
        public RibbonApplicationMenuItem ButtonChangePassWord;
        public RibbonApplicationMenuItem ButtonLogOut;
        public TextBlock UserName;
        public UserControl UcMain;

        #endregion

        public UcLogin(Bizz cbz, RibbonTab tabOffer, RibbonTab tabMaintenance, RibbonTab tabAdministration, RibbonGroup data, RibbonGroup users, RibbonApplicationMenuItem buttonChangePassWord, RibbonApplicationMenuItem buttonLogOut, TextBlock userName, UserControl ucMain)
        {
            InitializeComponent();
            this.CBZ = cbz;
            this.TabOffer = tabOffer;
            this.TabMaintenance = tabMaintenance;
            this.TabAdministration = tabAdministration;
            this.Data = data;
            this.Users = users;
            this.ButtonChangePassWord = buttonChangePassWord;
            this.ButtonLogOut = buttonLogOut;
            this.UserName = userName;
            this.UcMain = ucMain;
        }

        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.CheckCredentials(UserName, ButtonChangePassWord, ButtonLogOut, TextBoxInitials.Text, TextBoxPassword.Password))
            {
                switch (CBZ.CurrentUser.Authentication.UserLevel.Id)
                {
                    case 1:
                        ButtonChangePassWord.IsEnabled = true;
                        ButtonLogOut.IsEnabled = true;
                        TabOffer.IsEnabled = true;
                        TabMaintenance.IsEnabled = false;
                        TabAdministration.IsEnabled = false;
                        Data.IsEnabled = false;
                        Users.IsEnabled = false;
                        break;
                    case 2:
                        ButtonChangePassWord.IsEnabled = true;
                        ButtonLogOut.IsEnabled = true;
                        TabOffer.IsEnabled = true;
                        TabMaintenance.IsEnabled = true;
                        TabAdministration.IsEnabled = false;
                        Data.IsEnabled = false;
                        Users.IsEnabled = false;
                        break;
                    case 3:
                        ButtonChangePassWord.IsEnabled = true;
                        ButtonLogOut.IsEnabled = true;
                        TabOffer.IsEnabled = true;
                        TabMaintenance.IsEnabled = true;
                        TabAdministration.IsEnabled = true;
                        Data.IsEnabled = true;
                        Users.IsEnabled = false;
                        break;
                    case 4:
                        ButtonChangePassWord.IsEnabled = true;
                        ButtonLogOut.IsEnabled = true;
                        TabOffer.IsEnabled = true;
                        TabMaintenance.IsEnabled = true;
                        TabAdministration.IsEnabled = true;
                        Data.IsEnabled = true;
                        Users.IsEnabled = true;
                        break;
                    default:
                        ButtonChangePassWord.IsEnabled = true;
                        ButtonLogOut.IsEnabled = true;
                        TabOffer.IsEnabled = false;
                        TabMaintenance.IsEnabled = false;
                        TabAdministration.IsEnabled = false;
                        Data.IsEnabled = false;
                        Users.IsEnabled = false;
                        break;
                }
                UcMain.Content = new UserControl();
                CBZ.UcMainActive = false;
            }
            else
            {
                MessageBox.Show("Initialer eller password er forkert.", "Login",MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
