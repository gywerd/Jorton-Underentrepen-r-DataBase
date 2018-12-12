using ClassBizz;
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
    /// Interaction logic for UcIttLetters.xaml
    /// </summary>
    public partial class UcIttLetters : UserControl
    {
        #region Fields
        public Bizz Bizz;
        public UserControl UcLeft;
        public UserControl UcRight;

        #endregion

        public UcIttLetters(Bizz bizz, UserControl ucLeft, UserControl ucRight)
        {
            InitializeComponent();
            this.Bizz = bizz;
            this.UcLeft = ucLeft;
            this.UcRight = ucRight;
        }

        private void ButtonChooseReceivers_Click(object sender, RoutedEventArgs e)
        {
            Bizz.UcRightActive = true;
            UcIttLettersChooseReceivers ucIttLettersChooseReceivers = new UcIttLettersChooseReceivers(Bizz, UcRight);
            UcRight.Content = ucIttLettersChooseReceivers;
        }

        private void ButtonPreparePersonalIttLetters_Click(object sender, RoutedEventArgs e)
        {
            Bizz.UcRightActive = true;
            UcIttLettersPreparePersonalLetters ucIttLettersPreparePersonalLetters = new UcIttLettersPreparePersonalLetters(Bizz, UcRight);
            UcRight.Content = ucIttLettersPreparePersonalLetters;
        }

        private void ButtonPrepareCommonIttLetters_Click(object sender, RoutedEventArgs e)
        {
            Bizz.UcRightActive = true;
            UcIttLettersPrepareCommonLetter ucIttLettersPrepareCommonLetter = new UcIttLettersPrepareCommonLetter(Bizz, UcRight);
            UcRight.Content = ucIttLettersPrepareCommonLetter;
        }

        private void ButtonSendIttLetters_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
