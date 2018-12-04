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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JudGui
{
    /// <summary>
    /// Interaction logic for UcSubentrepeneurs.xaml
    /// </summary>
    public partial class UcSubEntrepeneurs : UserControl
    {
        #region Fields
        public Bizz Bizz;
        public UserControl UcRight;
        public UserControl UcLeft;

        #endregion

        #region Constructors
        public UcSubEntrepeneurs(Bizz bizz, UserControl ucLeft, UserControl ucRight)
        {
            InitializeComponent();
            this.Bizz = bizz;
            this.UcLeft = ucLeft;
            this.UcRight = ucRight;
        }

        #endregion

        #region Buttons
        private void ButtonChooseSubEntrepeneurs_Click(object sender, RoutedEventArgs e)
        {
            Bizz.UcRightActive = true;
            UcChooseSubEntrepeneurs ucChooseSubEntrepeneur = new UcChooseSubEntrepeneurs(Bizz, UcRight);
            UcRight.Content = ucChooseSubEntrepeneur;
        }

        private void ButtonEditSubEntrepeneurs_Click(object sender, RoutedEventArgs e)
        {
            Bizz.UcRightActive = true;
            UcEditSubEntrepeneurs ucEditSubEntrepeneur = new UcEditSubEntrepeneurs(Bizz, UcRight);
            UcRight.Content = ucEditSubEntrepeneur;
        }

        private void ButtonViewSubEntrepeneurs_Click(object sender, RoutedEventArgs e)
        {
            Bizz.UcRightActive = true;
            UcViewSubEntrepeneurs ucViewSubEntrepeneurs = new UcViewSubEntrepeneurs(Bizz, UcRight);
            UcRight.Content = ucViewSubEntrepeneurs;
        }

        #endregion
    }
}
