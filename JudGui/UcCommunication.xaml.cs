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
    /// Interaction logic for UcCommunication.xaml
    /// </summary>
    public partial class UcCommunication : UserControl
    {
        #region Fields
        public Bizz Bizz;
        public UserControl UcLeft;
        public UserControl UcRight;

        #endregion

        #region Constructors
        public UcCommunication(Bizz bizz, UserControl ucLeft, UserControl ucRight)
        {
            InitializeComponent();
            this.Bizz = bizz;
            this.UcLeft = ucLeft;
            this.UcRight = ucRight;
        }

        #endregion

        #region Buttons
        private void ButtonRequests_Click(object sender, RoutedEventArgs e)
        {
            Bizz.UcRightActive = true;
            UcRequests ucRequests = new UcRequests(Bizz, UcLeft, UcRight);
            UcLeft.Content = ucRequests;
        }

        private void ButtonIttLetters_Click(object sender, RoutedEventArgs e)
        {
            Bizz.UcRightActive = false;
            UcIttLetters ucIttLetters = new UcIttLetters(Bizz, UcLeft, UcRight);
            UcLeft.Content = ucIttLetters;
        }

        #endregion

    }
}
