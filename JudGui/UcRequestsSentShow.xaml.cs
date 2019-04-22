using JudBizz;
using JudRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
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
    /// Interaction logic for UcRequestsSentShow.xaml
    /// </summary>
    public partial class UcRequestsSentShow : UserControl
    {
        #region Fields
        public Bizz CBZ = new Bizz();
        public bool result;
        public UserControl UcMain;
        public static string macAddress;

        public Shipping Shipping = new Shipping();
        public List<RequestData> ProjectRequestData = new List<RequestData>();

        #endregion

        #region Constructors
        public UcRequestsSentShow(Bizz cbz, UserControl ucMain)
        {
            InitializeComponent();
            this.CBZ = cbz;
            CBZ.TempProject = new Project();
            CBZ.TempRequestData = new RequestData();
            macAddress = CBZ.MacAddress;
            this.UcMain = ucMain;
            GenerateComboBoxCaseIdItems();
        }

        #endregion

        #region Buttons
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            //Warning about lost changes before closing
            if (CBZ.UcMainEdited)
            {
                if (MessageBox.Show("Vil du lukke Vælg Modtagere?", "Luk Vælg Modtagere", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    //Close right UserControl
                    CBZ.UcMainEdited = false;
                    UcMain.Content = new UserControl();
                }
            }
            else
            {
                //Close right UserControl
                CBZ.UcMainEdited = false;
                UcMain.Content = new UserControl();
            }
        }

        private void ButtonShow_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(CBZ.TempRequestData.RequestUrl);

        }

        #endregion

        #region Events
        private void ComboBoxCaseId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxCaseId.SelectedIndex >= 0)
            {
                CBZ.TempProject = new Project((Project)ComboBoxCaseId.SelectedItem);
                TextBoxName.Text = CBZ.TempProject.Name;
                CBZ.TempRequestData = new RequestData();
                GetProjectRequestData();
                ListBoxEntrepeneurs.ItemsSource = "";
                ListBoxEntrepeneurs.ItemsSource = ProjectRequestData;
                ListBoxEntrepeneurs.SelectedIndex = -1;
            }
            else
            {
                TextBoxName.Text = "";
                CBZ.TempProject = new Project();
                CBZ.TempRequestData = new RequestData();
                ProjectRequestData.Clear();
                ListBoxEntrepeneurs.ItemsSource = "";
                ListBoxEntrepeneurs.SelectedIndex = -1;

                //Set CBZ.UcMainEdited
                if (CBZ.UcMainEdited)
                {
                    CBZ.UcMainEdited = false;
                }

            }

        }

        private void ListBoxEntrepeneurs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CBZ.TempRequestData = new RequestData((RequestData)ListBoxEntrepeneurs.SelectedItem);

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }

        }

        #endregion

        #region Methods
        /// <summary>
        /// Method, that generates Items for ComboBoxCaseId
        /// </summary>
        private void GenerateComboBoxCaseIdItems()
        {
            ComboBoxCaseId.Items.Clear();
            ComboBoxCaseId.ItemsSource = "";
            ComboBoxCaseId.ItemsSource = CBZ.IndexedActiveProjects;
        }

        /// <summary>
        /// Method, that generates List of ProjectSubEntrepeneurs
        /// </summary>
        /// <returns></returns>
        private void GetProjectRequestData()
        {
            ProjectRequestData.Clear();
            CBZ.RefreshList("Projects");
            CBZ.RefreshList("LetterDataList");
            foreach (RequestData data in CBZ.RequestDataList)
            {
                if (data.Project.Id == CBZ.TempProject.Id)
                {
                    ProjectRequestData.Add(data);
                }
            }
        }

        #endregion

    }
}
