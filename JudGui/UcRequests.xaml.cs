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
    /// Interaction logic for UcRequests.xaml
    /// </summary>
    public partial class UcRequests : UserControl
    {
        public Bizz Bizz;
        public UserControl UcLeft;
        public UserControl UcRight;

        public UcRequests(Bizz bizz, UserControl ucLeft, UserControl ucRight)
        {
            InitializeComponent();
            this.Bizz = bizz;
            this.UcLeft = ucLeft;
            this.UcRight = ucRight;
        }

        private void ButtonChooseReceivers_Click(object sender, RoutedEventArgs e)
        {
            Bizz.UcRightActive = true;
            UcRequestsChooseReceivers ucRequestsChooseReceivers = new UcRequestsChooseReceivers(Bizz, UcRight);
            UcRight.Content = ucRequestsChooseReceivers;
        }

        private void ButtonPrepareRequests_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonSendRequests_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
