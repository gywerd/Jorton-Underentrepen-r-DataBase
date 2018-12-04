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
    /// Interaction logic for UcEnterprices.xaml
    /// </summary>
    public partial class UcEnterprises : UserControl
    {
        #region Fields
        public Bizz Bizz;
        public UserControl UcRight;
        public UserControl UcLeft;

        #endregion

        public UcEnterprises(Bizz bizz, UserControl ucLeft, UserControl ucRight)
        {
            InitializeComponent();
            this.Bizz = bizz;
            this.UcLeft = ucLeft;
            this.UcRight = ucRight;
        }

        #region Buttons
        private void ButtonCreateEnterpriseListe_Click(object sender, RoutedEventArgs e)
        {
            Bizz.UcRightActive = true;
            UcCreateEnterpriseList ucCreateEnterpriseList = new UcCreateEnterpriseList(Bizz, UcRight);
            UcRight.Content = ucCreateEnterpriseList;
        }

        private void ButtonEditEnterpriseList_Click(object sender, RoutedEventArgs e)
        {
            Bizz.UcRightActive = true;
            UcEditEnterpriseList ucEditEnterpriseList = new UcEditEnterpriseList(Bizz, UcRight);
            UcRight.Content = ucEditEnterpriseList;
        }

        private void ButtonViewEnterpriseList_Click(object sender, RoutedEventArgs e)
        {
            Bizz.UcRightActive = true;
            UcViewEnterpriseList ucViewEnterpriseList = new UcViewEnterpriseList(Bizz, UcRight);
            UcRight.Content = ucViewEnterpriseList;
        }

        #endregion

    }
}
