using JudBizz;
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
    /// Interaction logic for UcEnterprisesView.xaml
    /// </summary>
    public partial class UcEnterprisesView : UserControl
    {
        #region Fields
        public Bizz CBZ;
        public UserControl UcMain;
        #endregion

        #region Constructors
        public UcEnterprisesView(Bizz cbz, UserControl ucMain)
        {
            InitializeComponent();
            this.CBZ = cbz;
            this.UcMain = ucMain;
            ComboBoxCaseId.ItemsSource = CBZ.IndexedActiveProjects;
        }

        #endregion

        #region Buttons
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Vil du lukke Visning af Entrepriselisten.", "Entrepriser", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                CBZ.CloseUcMain(UcMain);
            }
        }

        private void ButtonGeneratePdf_Click(object sender, RoutedEventArgs e)
        {
            PdfCreator pdfCreator = new PdfCreator(CBZ.StrConnection);
            string path = pdfCreator.GenerateEnterprisesPdf(CBZ);
            System.Diagnostics.Process.Start(path);
        }

        #endregion

        #region Events
        private void ComboBoxCaseId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CBZ.TempProject = new Project((IndexedProject)ComboBoxCaseId.SelectedItem);

            TextBoxCaseName.Text = CBZ.TempProject.Details.Name;
            RefreshesIndexedEnterprises();

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        #endregion

        #region Methods
        private void RefreshesIndexedEnterprises()
        {
            CBZ.IndexedEnterprises.Clear();
            CBZ.IndexedEnterprises.Add(new IndexedEnterprise(0, CBZ.Enterprises[0]));

            int i = 1;
            foreach (Enterprise enterprise in CBZ.Enterprises)
            {
                if (enterprise.Project.Id == CBZ.TempProject.Id)
                {
                    CBZ.IndexedEnterprises.Add(new IndexedEnterprise(i, enterprise));
                }
                i++;
            }
        }

        #endregion

    }
}
