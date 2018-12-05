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
    /// Interaction logic for UcViewEnterpriseList.xaml
    /// </summary>
    public partial class UcViewEnterpriseList : UserControl
    {
        #region Fields
        public Bizz CBZ;
        public UserControl UcRight;
        public List<Enterprise> IndexableEnterpriseList = new List<Enterprise>();

        #endregion

        public UcViewEnterpriseList(Bizz cbz, UserControl ucRight)
        {
            InitializeComponent();
            this.CBZ = cbz;
            this.UcRight = ucRight;
            ComboBoxCaseId.ItemsSource = CBZ.IndexedActiveProjects;
        }

        #region Buttons
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            //Close right UserControl
            MessageBox.Show("Visning af Entrepriselisten lukkes.", "Luk Entrepriseliste", MessageBoxButton.OK, MessageBoxImage.Information);
            CBZ.UcRightActive = false;
            UcRight.Content = new UserControl();
        }

        private void ButtonGeneratePdf_Click(object sender, RoutedEventArgs e)
        {
            PdfCreator pdfCreator = new PdfCreator(GetStrConnection());
            string path = pdfCreator.GenerateEnterpriseListPdf(CBZ, IndexableEnterpriseList, CBZ.Users);
            System.Diagnostics.Process.Start(path);
        }

        #endregion

        #region Events
        private void ComboBoxCaseId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = ComboBoxCaseId.SelectedIndex;
            foreach (IndexedProject temp in CBZ.IndexedActiveProjects)
            {
                if (temp.Index == selectedIndex)
                {
                    CBZ.TempProject = new Project(temp.Id, temp.CaseId, temp.Name, temp.Builder, temp.Status, temp.TenderForm, temp.EnterpriseForm, temp.Executive, temp.EnterpriseList, temp.Copy);
                }
            }
            TextBoxCaseName.Text = CBZ.TempProject.Name;
            IndexableEnterpriseList = GetIndexableEnterpriseList();
        }

        #endregion

        #region Methods
        //private void GenerateComboBoxCaseIdItems()
        //{
        //    ComboBoxCaseId.Items.Clear();
        //    foreach (IndexableProject temp in Bizz.ActiveProjects)
        //    {
        //        ComboBoxCaseId.Items.Add(temp);
        //    }
        //}

        private List<Enterprise> GetIndexableEnterpriseList()
        {
            List<Enterprise> result = new List<Enterprise>();
            int i = 0;
            foreach (Enterprise enterprise in CBZ.EnterpriseList)
            {
                if (enterprise.Project.Id == CBZ.TempProject.Id)
                {
                    IndexedEnterprise temp = new IndexedEnterprise(i, enterprise);
                    result.Add(temp);
                }
                i++;
            }
            return result;
        }

        /// <summary>
        /// Method, that retrieves the strConnection
        /// </summary>
        /// <returns></returns>
        private string GetStrConnection()
        {
            return CBZ.GetStrConnection();
        }

        #endregion

    }
}
