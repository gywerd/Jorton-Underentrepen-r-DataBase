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
    /// Interaction logic for UcRegions.xaml
    /// </summary>
    public partial class UcRegions : UserControl
    {
        #region Fields
        public Bizz Bizz;
        public UserControl UcRight;

        #endregion

        public UcRegions(Bizz bizz, UserControl ucRight)
        {
            InitializeComponent();
            this.Bizz = bizz;
            this.UcRight = ucRight;
        }
    }
}
