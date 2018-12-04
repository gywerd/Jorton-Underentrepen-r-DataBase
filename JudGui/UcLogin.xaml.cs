using JudBizz;
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
        public Bizz Bizz;
        public RibbonTab TabOffer;
        public RibbonTab TabAdministration;
        public RibbonGroup HelpData;
        public RibbonGroup Information;
        public RibbonApplicationMenuItem MenuItemChangePassWord;
        public RibbonApplicationMenuItem MenuItemLogOut;
        public TextBlock UserName;
        public UserControl UcLeft;
        public UserControl UcRight;

        public static Bizz CBZ = new Bizz();
        #endregion

        public UcLogin(Bizz bizz, RibbonTab tabOffer, RibbonTab tabAdministration, RibbonGroup information, RibbonGroup helpData, RibbonApplicationMenuItem menuitemChangePassWord, RibbonApplicationMenuItem menuItemLogOut, TextBlock userName, UserControl ucLeft, UserControl ucRight)
        {
            InitializeComponent();
            this.Bizz = bizz;
            this.TabOffer = tabOffer;
            this.TabAdministration = tabAdministration;
            this.HelpData = helpData;
            this.Information = information;
            this.MenuItemChangePassWord = menuitemChangePassWord;
            this.MenuItemLogOut = menuItemLogOut;
            this.UserName = userName;
            this.UcLeft = ucLeft;
            this.UcRight = ucRight;
        }

        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.CheckCredentials(Bizz, UserName, MenuItemChangePassWord, MenuItemLogOut, TextBoxInitials.Text, TextBoxPassword.Password))
            {
                TabOffer.IsEnabled = true;
                TabAdministration.IsEnabled = true;
                if (Bizz.CurrentUser.Administrator)
                {
                    HelpData.IsEnabled = true;
                }
                else
                {
                    HelpData.IsEnabled = false;
                }
                Information.IsEnabled = true;
                UcLeft.Content = new UserControl();
                UcRight.Content = new UserControl();
                Bizz.UcRightActive = false;
            }
            else
            {
                MessageBox.Show("Initialer eller password er forkert.");
            }
        }
    }
}
