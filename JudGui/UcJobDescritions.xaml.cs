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
    /// Interaction logic for UcJobDescritions.xaml
    /// </summary>
    public partial class UcJobDescritions : UserControl
    {
        #region Fields
        public Bizz CBZ;
        public UserControl UcMain;
        public IndexedJobDescription TempNewJobDescription = new IndexedJobDescription();

        List<IndexedJobDescription> FilteredJobDescriptions = new List<IndexedJobDescription>();
        #endregion

        #region Constructors
        public UcJobDescritions(Bizz cbz, UserControl ucMain)
        {
            InitializeComponent();
            this.CBZ = cbz;
            this.UcMain = ucMain;

            CBZ.TempJobDescription = new JobDescription();

            GetFilteredJobDescriptions();
            ListBoxJobDescriptions.ItemsSource = FilteredJobDescriptions;
            ListBoxJobDescriptions.SelectedIndex = -1;
        }

        #endregion

        #region Buttons
        private void ButtonAddJobDescription_Click(object sender, RoutedEventArgs e)
        {
            bool result = CreateJobDescriptionInDb();

            //Display result
            if (result)
            {
                //Show Confirmation
                MessageBox.Show("Jobbeskrivelsen blev tilføjet", "Jobbeskrivelser", MessageBoxButton.OK, MessageBoxImage.Information);

                //Reset Boxes
                ListBoxJobDescriptions.SelectedIndex = -1;
                ListBoxJobDescriptions.ItemsSource = "";
                CBZ.RefreshIndexedList("JobDescriptions");
                ListBoxJobDescriptions.ItemsSource = CBZ.IndexedJobDescriptions;
                TextBoxJobDescriptionSearch.Text = "";
                TextBoxArea.Text = "";
                TextBoxOccupation.Text = "";
                CheckBoxProcuration.IsChecked = null;
                TextBoxNewArea.Text = "";
                TextBoxNewOccupation.Text = "";
                CheckBoxNewProcuration.IsChecked = null;

                //Refresh JobDescriptions list
                CBZ.RefreshList("JobDescriptions");
                CBZ.TempJobDescription = new JobDescription();
                TempNewJobDescription = new IndexedJobDescription();
            }
            else
            {
                //Show error
                MessageBox.Show("Databasen returnerede en fejl. Jobbeskrivelsen blev ikke tilføjet. Prøv igen.", "Jobbeskrivelser", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                //Warning about lost changes before closing
                if (MessageBox.Show("Vil du lukke Jobbeskrivelser? Alle ugemte data mistes.", "Jobbeskrivelser", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
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
            bool result = UpdateJobDescriptioninDb();

            //Display result
            if (result)
            {
                //Show Confirmation
                MessageBox.Show("Jobbeskrivelsen blev opdateret", "Jobbeskrivelser", MessageBoxButton.OK, MessageBoxImage.Information);

                //Reset Boxes
                ListBoxJobDescriptions.SelectedIndex = -1;
                ListBoxJobDescriptions.ItemsSource = "";
                TextBoxJobDescriptionSearch.Text = "";
                TextBoxOccupation.Text = "";
                TextBoxArea.Text = "";
                CheckBoxProcuration.IsChecked = null;
                TextBoxNewOccupation.Text = "";
                TextBoxNewArea.Text = "";
                CheckBoxNewProcuration.IsChecked = null;

                //Refresh JobDescription list
                CBZ.RefreshList("JobDescriptions");
                CBZ.TempJobDescription = new JobDescription();
                TempNewJobDescription = new IndexedJobDescription();
            }
            else
            {
                //Show error
                MessageBox.Show("Databasen returnerede en fejl. Jobbeskrivelsen blev ikke opdateret. Prøv igen.", "Jobbeskrivelser", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region Events
        private void CheckBoxNewProcuration_Checked(object sender, RoutedEventArgs e)
        {
            if (CheckBoxNewProcuration.IsChecked == true && !TempNewJobDescription.Procuration)
            {
                TempNewJobDescription.ToggleProcuration();
            }
            else if (CheckBoxNewProcuration.IsChecked == false && TempNewJobDescription.Procuration)
            {
                TempNewJobDescription.ToggleProcuration();
            }

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void CheckBoxProcuration_Checked(object sender, RoutedEventArgs e)
        {
            if (CheckBoxNewProcuration.IsChecked == true && !CBZ.TempJobDescription.Procuration)
            {
                CBZ.TempJobDescription.ToggleProcuration();
            }
            else if (CheckBoxNewProcuration.IsChecked == false && CBZ.TempJobDescription.Procuration)
            {
                CBZ.TempJobDescription.ToggleProcuration();
            }

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void ListBoxJobDescriptions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CBZ.TempJobDescription = new JobDescription((JobDescription)ListBoxJobDescriptions.SelectedItem);

            TextBoxOccupation.Text = CBZ.TempJobDescription.Occupation;
            TextBoxArea.Text = CBZ.TempJobDescription.Area;
            CheckBoxProcuration.IsChecked = CBZ.TempJobDescription.Procuration;

            this.TempNewJobDescription = new IndexedJobDescription();
            TextBoxNewOccupation.Text = "";
            TextBoxNewArea.Text = "";
            CheckBoxProcuration.IsChecked = null;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxJobDescriptionSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            GetFilteredJobDescriptions();
            ListBoxJobDescriptions.SelectedIndex = -1;
            ListBoxJobDescriptions.ItemsSource = "";
            ListBoxJobDescriptions.ItemsSource = this.FilteredJobDescriptions;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxArea_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempJobDescription.Area = TextBoxArea.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxNewArea_TextChanged(object sender, TextChangedEventArgs e)
        {
            TempNewJobDescription.Area = TextBoxNewArea.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxNewOccupation_TextChanged(object sender, TextChangedEventArgs e)
        {
            TempNewJobDescription.Occupation = TextBoxNewOccupation.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void TextBoxOccupation_TextChanged(object sender, TextChangedEventArgs e)
        {
            CBZ.TempJobDescription.Occupation = TextBoxOccupation.Text;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        #endregion

        #region Methods
        private bool CreateJobDescriptionInDb()
        {
            bool result = false;

            int jobDescriptionId = CBZ.CreateInDb(TempNewJobDescription);

            if (jobDescriptionId >= 1)
            {
                result = true;
            }

            return result;

        }

        private void GetFilteredJobDescriptions()
        {
            CBZ.RefreshIndexedList("JobDescriptions");
            this.FilteredJobDescriptions = new List<IndexedJobDescription>();
            int length = TextBoxJobDescriptionSearch.Text.Length;

            foreach (IndexedJobDescription description in CBZ.IndexedJobDescriptions)
            {
                if (description.Occupation.Remove(length) == TextBoxJobDescriptionSearch.Text)
                {
                    this.FilteredJobDescriptions.Add(description);
                }
            }
        }

        private bool UpdateJobDescriptioninDb() => CBZ.UpdateInDb(CBZ.TempJobDescription);

        #endregion

    }
}
