﻿using JudBizz;
using JudRepository;
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
    /// Interaction logic for UcRequestPrepare.xaml
    /// </summary>
    public partial class UcRequestPrepare : UserControl
    {
        #region Fields
        public Bizz CBZ;
        public UserControl UcMain;

        #endregion

        #region Constructors
        public UcRequestPrepare(Bizz cbz, UserControl ucMain)
        {
            InitializeComponent();
            this.CBZ = cbz;
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