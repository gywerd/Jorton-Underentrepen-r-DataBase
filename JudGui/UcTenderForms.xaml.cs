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
    /// Interaction logic for UcTenderForms.xaml
    /// </summary>
    public partial class UcTenderForms : UserControl
    {
        #region Fields
        public Bizz CBZ;
        public UserControl UcMain;
        public IndexedTenderForm TempNewTenderForm = new IndexedTenderForm();

        public List<IndexedTenderForm> FilteredTenderForms = new List<IndexedTenderForm>();
        #endregion

        #region Constructors
        public UcTenderForms(Bizz cbz, UserControl ucMain)
        {
            InitializeComponent();
            this.CBZ = cbz;
            this.UcMain = ucMain;

            CBZ.TempTenderForm = new TenderForm();

            GetFilteredTenderForms();
            ListBoxTenderForms.ItemsSource = FilteredTenderForms;
            ListBoxTenderForms.SelectedIndex = -1;
        }

        #endregion

        #region Buttons
        private void ButtonAddTenderForm_Click(object sender, RoutedEventArgs e)
        {
            bool result = CreateTenderFormInDb();

            //Display result
            if (result)
            {
                //Show Confirmation
                MessageBox.Show("Udbudsformen blev tilføjet", "Udbudsformer", MessageBoxButton.OK, MessageBoxImage.Information);

                //Reset Boxes
                TextBoxTenderFormSearch.Text = "";
                ListBoxTenderForms.SelectedIndex = -1;
                ListBoxTenderForms.ItemsSource = "";
                ListBoxTenderForms.ItemsSource = FilteredTenderForms;
                TextBoxText.Text = "";
                TextBoxNewText.Text = "";

                //Refresh Users list
                CBZ.RefreshList("TenderForms");
                CBZ.TempTenderForm = new TenderForm();
            }
            else
            {
                //Show error
                MessageBox.Show("Databasen returnerede en fejl. Udbudsformen blev ikke tilføjet. Prøv igen.", "Udbudsformer", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                //Warning about lost changes before closing
                if (MessageBox.Show("Vil du lukke Udbudsformer? Alle ugemte data mistes.", "Udbudsformen", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    CBZ.CloseUcMain(UcMain);
                }
            }
            else
            {
                CBZ.CloseUcMain(UcMain);
            }

        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            bool result = UpdateTenderFormInDb;

            //Display result
            if (result)
            {
                //Show Confirmation
                MessageBox.Show("Udbudsformen blev opdateret", "Udbudsformer", MessageBoxButton.OK, MessageBoxImage.Information);

                //Reset Boxes
                ListBoxTenderForms.SelectedIndex = -1;
                ListBoxTenderForms.ItemsSource = "";
                TextBoxTenderFormSearch.Text = "";
                TextBoxText.Text = "";
                TextBoxNewText.Text = "";

                //Refresh Tender Forms list
                CBZ.RefreshList("TenderForms");
                CBZ.TempTenderForm = new TenderForm();
            }
            else
            {
                //Show error
                MessageBox.Show("Databasen returnerede en fejl. Udbudsformen blev ikke opdateret. Prøv igen.", "Udbudsformer", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        #endregion

        #region Events
        private void ListBoxTenderForms_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CBZ.TempTenderForm = new TenderForm((TenderForm)ListBoxTenderForms.SelectedItem);

            TextBoxText.Text = CBZ.TempTenderForm.Text;

            this.TempNewTenderForm = new IndexedTenderForm();
            TextBoxNewText.Text = "";

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxNewText_TextChanged(object sender, TextChangedEventArgs e)
        {
            TempNewTenderForm.Text = TextBoxNewText.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxTenderFormSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            GetFilteredTenderForms();
            ListBoxTenderForms.SelectedIndex = -1;
            ListBoxTenderForms.ItemsSource = "";
            ListBoxTenderForms.ItemsSource = this.FilteredTenderForms;



            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxText_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempTenderForm.Text = TextBoxText.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Method, that creates a Tender Form in Db
        /// </summary>
        /// <returns>bool</returns>
        private bool CreateTenderFormInDb()
        {
            bool result = false;

            int formId = CBZ.CreateInDb(TempNewTenderForm);

            if (formId >= 1)
            {
                result = true;
            }

            return result;

        }

        /// <summary>
        /// Method, that retrieves a filtered Tender Form list
        /// </summary>
        private void GetFilteredTenderForms()
        {
            CBZ.RefreshIndexedList("TenderForm");
            this.FilteredTenderForms = new List<IndexedTenderForm>();
            int length = TextBoxTenderFormSearch.Text.Length;

            foreach (IndexedTenderForm form in CBZ.IndexedTenderForms)
            {
                if (form.Text == TextBoxTenderFormSearch.Text)
                {
                    this.FilteredTenderForms.Add(form);
                }
            }
        }

        /// <summary>
        /// Method, that updates an Enterprise Form in Db
        /// </summary>
        /// <returns>bool</returns>
        private bool UpdateTenderFormInDb => CBZ.UpdateInDb(CBZ.TempTenderForm);


        #endregion

    }
}
