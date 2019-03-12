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
    /// Interaction logic for UcEnterpriseForms.xaml
    /// </summary>
    public partial class UcEnterpriseForms : UserControl
    {
        #region Fields
        public Bizz CBZ;
        public UserControl UcMain;
        public IndexedEnterpriseForm TempNewEnterpriseForm = new IndexedEnterpriseForm();

        public List<IndexedEnterpriseForm> FilteredEnterpriseForms = new List<IndexedEnterpriseForm>();
        #endregion

        #region Constructors
        public UcEnterpriseForms(Bizz cbz, UserControl ucMain)
        {
            InitializeComponent();
            this.CBZ = cbz;
            this.UcMain = ucMain;

            CBZ.TempEnterpriseForm = new EnterpriseForm();

            GetFilteredEnterpriseForms();
            ListBoxEnterpriseForms.ItemsSource = FilteredEnterpriseForms;

        }

        #endregion

        #region Buttons
        private void ButtonAddEnterpriseForm_Click(object sender, RoutedEventArgs e)
        {
            bool result = CreateEnterpriseFormInDb();

            //Display result
            if (result)
            {
                //Show Confirmation
                MessageBox.Show("Entrepriseformen blev tilføjet", "Entrepriseformer", MessageBoxButton.OK, MessageBoxImage.Information);

                //Reset Boxes
                TextBoxEnterpriseFormSearch.Text = "";
                ListBoxEnterpriseForms.SelectedIndex = -1;
                ListBoxEnterpriseForms.ItemsSource = "";
                ListBoxEnterpriseForms.ItemsSource = FilteredEnterpriseForms;
                TextBoxText.Text = "";
                TextBoxNewText.Text = "";

                //Refresh Users list
                CBZ.RefreshList("EnterpriseForms");
                CBZ.TempEnterpriseForm = new EnterpriseForm();
            }
            else
            {
                //Show error
                MessageBox.Show("Databasen returnerede en fejl. Entrepriseformen blev ikke tilføjet. Prøv igen.", "Entrepriseformer", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                //Warning about lost changes before closing
                if (MessageBox.Show("Vil du lukke Entrepriseformer? Ikke gemte data mistes.", "Entrepriseformen", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
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

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            bool result = UpdateEnterpriseFormInDb;

            //Display result
            if (result)
            {
                //Show Confirmation
                MessageBox.Show("Entrepriseformen blev opdateret", "Entrepriseformer", MessageBoxButton.OK, MessageBoxImage.Information);

                //Reset Boxes
                ListBoxEnterpriseForms.SelectedIndex = -1;
                ListBoxEnterpriseForms.ItemsSource = "";
                TextBoxEnterpriseFormSearch.Text = "";
                TextBoxText.Text = "";
                TextBoxNewText.Text = "";

                //Refresh Users list
                CBZ.RefreshList("EnterpriseForms");
                CBZ.TempEnterpriseForm = new EnterpriseForm();
            }
            else
            {
                //Show error
                MessageBox.Show("Databasen returnerede en fejl. Entrepriseformen blev ikke opdateret. Prøv igen.", "Entrepriseformer", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        #endregion

        #region Events
        private void TextBoxEnterpriseFormSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            GetFilteredEnterpriseForms();
            ListBoxEnterpriseForms.SelectedIndex = -1;
            ListBoxEnterpriseForms.ItemsSource = "";
            ListBoxEnterpriseForms.ItemsSource = this.FilteredEnterpriseForms;


            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxNewText_TextChanged(object sender, TextChangedEventArgs e)
        {
            TempNewEnterpriseForm.Text = TextBoxNewText.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxText_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempEnterpriseForm.Text = TextBoxText.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        #endregion

        #region Methods
        private bool CreateEnterpriseFormInDb()
        {
            bool result = false;

            int formId = CBZ.CreateInDb(TempNewEnterpriseForm);

            if (formId >= 1)
            {
                result = true;
            }

            return result;

        }

        private void GetFilteredEnterpriseForms()
        {
            CBZ.RefreshIndexedList("IndexedEnterpriseForms");
            this.FilteredEnterpriseForms = new List<IndexedEnterpriseForm>();
            int length = TextBoxEnterpriseFormSearch.Text.Length;

            foreach (IndexedEnterpriseForm form in CBZ.IndexedEnterpriseForms)
            {
                if (form.Text == TextBoxEnterpriseFormSearch.Text)
                {
                    this.FilteredEnterpriseForms.Add(form);
                }
            }
        }

        /// <summary>
        /// Method, that updates an Enterprise Form in Db
        /// </summary>
        /// <returns>bool</returns>
        private bool UpdateEnterpriseFormInDb => CBZ.UpdateInDb(CBZ.TempEnterpriseForm);

        #endregion

    }
}
