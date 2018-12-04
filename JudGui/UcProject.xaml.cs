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
    /// Interaction logic for UcProject.xaml
    /// </summary>
    public partial class UcProject : UserControl
    {
        #region Fields
        public Bizz Bizz;
        public UserControl UcRight;
        public UserControl UcLeft;
        #endregion

        public UcProject(Bizz bizz, UserControl ucLeft, UserControl ucRight)
        {
            InitializeComponent();
            this.Bizz = bizz;
            this.UcLeft = ucLeft;
            this.UcRight = ucRight;
        }

        #region Buttons
        private void ButtonCopyProject_Click(object sender, RoutedEventArgs e)
        {
            Bizz.UcRightActive = true;
            UcCopyProject ucCopyProject = new UcCopyProject(Bizz, UcRight);
            UcRight.Content = ucCopyProject;
        }

        private void ButtonCreateProject_Click(object sender, RoutedEventArgs e)
        {
            Bizz.UcRightActive = true;
            UcCreateProject ucCreateProject = new UcCreateProject(Bizz, UcRight);
            UcRight.Content = ucCreateProject;
        }

        private void ButtonChangeCaseId_Click(object sender, RoutedEventArgs e)
        {
            Bizz.UcRightActive = true;
            UcChangeCaseId ucEditCaseId = new UcChangeCaseId(Bizz, UcRight);
            UcRight.Content = ucEditCaseId;
        }

        private void ButtonEditProject_Click(object sender, RoutedEventArgs e)
        {
            Bizz.UcRightActive = true;
            UcEditProject ucEditProject = new UcEditProject(Bizz, UcRight);
            UcRight.Content = ucEditProject;
        }

        private void ButtonChangeProjectStatus_Click(object sender, RoutedEventArgs e)
        {
            Bizz.UcRightActive = true;
            UcChangeProjectStatus ucChangeProjectStatus = new UcChangeProjectStatus(Bizz, UcRight);
            UcRight.Content = ucChangeProjectStatus;
        }

        private void ButtonEraseProject_Click(object sender, RoutedEventArgs e)
        {
            Bizz.UcRightActive = true;
            UcDeleteProject ucDeleteProject = new UcDeleteProject(Bizz, UcRight);
            UcRight.Content = ucDeleteProject;
        }

        #endregion

    }
}
