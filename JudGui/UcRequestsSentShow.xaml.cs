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

        public IttLetterShipping Shipping = new IttLetterShipping();
        public List<RequestShipping> ProjectRequestData = new List<RequestShipping>();

        #endregion

        #region Constructors
        public UcRequestsSentShow(Bizz cbz, UserControl ucMain)
        {
            InitializeComponent();
            this.CBZ = cbz;
            CBZ.TempProject = new Project();
            CBZ.TempRequestShipping = new RequestShipping();
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
                if (MessageBox.Show("Vil du lukke Vælg Modtagere? Alle ugemte data mistes.", "Forespørgsler", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    CBZ.CloseUcMain(UcMain);
                }
            }
            else
            {
                CBZ.CloseUcMain(UcMain);
            }
        }

        private void ButtonShow_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(CBZ.TempRequestShipping.RequestPdfPath);

        }

        #endregion

        #region Events
        private void ComboBoxCaseId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxCaseId.SelectedIndex >= 0)
            {
                CBZ.TempProject = new Project((Project)ComboBoxCaseId.SelectedItem);
                TextBoxName.Text = CBZ.TempProject.Details.Name;
                CBZ.TempRequestShipping = new RequestShipping();
                GetProjectRequestData();
                ListBoxEntrepeneurs.ItemsSource = "";
                ListBoxEntrepeneurs.ItemsSource = ProjectRequestData;
                ListBoxEntrepeneurs.SelectedIndex = -1;
            }
            else
            {
                TextBoxName.Text = "";
                CBZ.TempProject = new Project();
                CBZ.TempRequestShipping = new RequestShipping();
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
            CBZ.TempRequestShipping = new RequestShipping((RequestShipping)ListBoxEntrepeneurs.SelectedItem);

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
            foreach (RequestShipping data in CBZ.RequestShippings)
            {
                if (data.SubEntrepeneur.Id == CBZ.TempProject.Id)
                {
                    ProjectRequestData.Add(data);
                }
            }
        }

        #endregion

    }
}
