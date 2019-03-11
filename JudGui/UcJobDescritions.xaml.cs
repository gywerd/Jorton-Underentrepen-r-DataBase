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
    /// Interaction logic for UcJobDescritions.xaml
    /// </summary>
    public partial class UcJobDescritions : UserControl
    {
        #region Fields
        public Bizz Bizz;
        public UserControl UcMain;

        #endregion

        #region Constructors
        public UcJobDescritions(Bizz bizz, UserControl ucMain)
        {
            InitializeComponent();
            this.Bizz = bizz;
            this.UcMain = ucMain;
        }

        #endregion

        #region Buttons

        #endregion

        #region Events


        //Set CBZ.UcMainEdited
        //if (!CBZ.UcMainEdited)
        //{
        //    CBZ.UcMainEdited = true;
        //}
        #endregion

        #region Methods

        #endregion
    }
}
