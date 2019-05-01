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
    /// Interaction logic for UcIttLettersPrepareCommonLetter.xaml
    /// </summary>
    public partial class UcProjectsElaboration : UserControl
    {
        #region Fields
        public Bizz CBZ;
        public UserControl UcMain;
        public PdfCreator PdfCreator;
        public List<Contact> ProjectContacts = new List<Contact>();
        public List<Enterprise> ProjectEnterprises = new List<Enterprise>();
        public List<Paragraf> ProjectParagrafs = new List<Paragraf>();
        public List<IttLetterShipping> ProjectShippings = new List<IttLetterShipping>();
        public List<SubEntrepeneur> ProjectSubEntrepeneurs = new List<SubEntrepeneur>();
        public List<Receiver> Receivers = new List<Receiver>();

        #endregion

        #region Constructors
        public UcProjectsElaboration(Bizz cbz, UserControl ucMain)
        {
            InitializeComponent();
            this.CBZ = cbz;
            this.UcMain = ucMain;
            CBZ.TempBullet = new Bullet();
            CBZ.TempParagraf = new Paragraf();
            CBZ.TempProject = new Project();

            GenerateComboBoxCaseIdItems();
        }

        #endregion

        #region Buttons
        private void ButtonAddBullet_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBoxParagrafs.SelectedIndex < 0)
            {
                MessageBox.Show("Der er ikke valgt en overskrift. Punktet kan ikke knyttes til overskriften", "Projekter", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (ComboBoxParagrafs.SelectedIndex >= 1 && TextBoxNewBullet.Text == "")
            {
                MessageBox.Show("Der er ikke indtastet et punkt. Punktet kan ikke tilføjes til overskriften", "Projekter", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                int dbAnswer = 0;
                Exception exception = new Exception();
                CBZ.TempBullet = new Bullet(CBZ.TempParagraf, TextBoxNewBullet.Text);
                try
                {
                    dbAnswer = CBZ.CreateInDb(CBZ.TempBullet);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                if (dbAnswer < 1)
                {
                    MessageBox.Show("Databasen meldte en fejl. Punktet blev ikke tilføjet til afsnittet\n" + exception, "", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    RefreshList("Bullets");
                    RefreshIndexedBullets();
                    ListBoxBullets.ItemsSource = CBZ.IndexedBullets;
                    ListBoxBullets.SelectedIndex = 0;
                    TextBoxNewBullet.Text = "";

                    SetUcMainEdited();

                }
            }
        }

        private void ButtonAddParagraf_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBoxCaseId.SelectedIndex < 1)
            {
                MessageBox.Show("Der er ikke valgt et projekt. Overskriften kan ikke tilføjes til projektet", "Projekter", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (ComboBoxCaseId.SelectedIndex >= 1 && TextBoxNewParagraf.Text == "")
            {
                MessageBox.Show("Der er ikke indtastet en overskrift. Overskriften kan ikke tilføjes til projektet", "Projekter", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                int dbAnswer = 0;
                Exception exception = new Exception();
                CBZ.TempParagraf = new Paragraf(CBZ.TempProject, TextBoxNewParagraf.Text);
                try
                {
                    dbAnswer = CBZ.CreateInDb(CBZ.TempParagraf);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
                if (dbAnswer < 1)
                {
                    Exception tempEx = new Exception();
                    if (exception != tempEx)
                    {
                        MessageBox.Show("Databasen meldte en fejl. Overskriften blev ikke tilføjet til afsnittet\n" + exception, "Projekter", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        MessageBox.Show("Databasen meldte en fejl. Overskriften blev ikke tilføjet til afsnittet", "Projekter", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    RefreshList("Paragrafs");
                    RefreshIndexedParagrafs();
                    ComboBoxParagrafs.SelectedIndex = -1;
                    ComboBoxParagrafs.ItemsSource = "";
                    ComboBoxParagrafs.ItemsSource = CBZ.IndexedParagrafs;
                    TextBoxNewParagraf.Text = "";

                    SetUcMainEdited();

                }
            }

        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainEdited)
            {
                //Warning about lost changes before closing
                if (MessageBox.Show("Vil du lukke Uddybning af Projekt? Alle ugemte data mistes.", "Projekter", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    CBZ.CloseUcMain(UcMain);
                }
            }
            else
            {
                CBZ.CloseUcMain(UcMain);
            }

        }

        #endregion

        #region Events
        private void ComboBoxCaseId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CBZ.TempProject = new Project((IndexedProject)ComboBoxCaseId.SelectedItem);

            TextBoxName.Text = CBZ.TempProject.Details.Name;
            RefreshIndexedParagrafs();
            ComboBoxParagrafs.ItemsSource = "";
            ComboBoxParagrafs.ItemsSource = CBZ.IndexedParagrafs;
            ComboBoxParagrafs.SelectedIndex = 0;

        }

        private void ComboBoxParagrafs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CBZ.TempParagraf = new Paragraf((Paragraf)ComboBoxParagrafs.SelectedItem);
            RefreshIndexedBullets();

            ListBoxBullets.ItemsSource = CBZ.IndexedBullets;
            ListBoxBullets.SelectedIndex = -1;

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void ListBoxBullets_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void TextBoxNewParagraf_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetUcMainEdited();
        }

        private void TextBoxNewBullet_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetUcMainEdited();
        }

        #endregion

        #region Methods
        /// <summary>
        /// Method, that checks content of TextBoxBullet
        /// </summary>
        /// <returns>bool</returns>
        private bool CheckTextBoxBulletReset()
        {
            bool result = false;

            if (TextBoxNewBullet.Text.Length < 1 || TextBoxNewBullet.Text == null)
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Method, that checks content of TextBoxParagraf
        /// </summary>
        /// <returns></returns>
        private bool CheckTextBoxParagrafReset()
        {
            bool result = false;

            if (TextBoxNewParagraf.Text.Length < 1 || TextBoxNewParagraf.Text == null)
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Method, that generates Items for ComboBoxCaseId
        /// </summary>
        private void GenerateComboBoxCaseIdItems()
        {
            ComboBoxCaseId.Items.Clear();
            RefreshIndexedList("Projects");

            ComboBoxCaseId.ItemsSource = "";
            ComboBoxCaseId.ItemsSource = CBZ.IndexedActiveProjects;
        }

        /// <summary>
        /// Method that creates an Indexed Bullet List
        /// </summary>
        private void RefreshIndexedBullets()
        {
            RefreshProjectList("Bullets", CBZ.TempProject.Id);
            CBZ.IndexedBullets.Clear();

            int i = 0;

            foreach (Bullet bullet in CBZ.ProjectLists.Bullets)
            {
                if (bullet.Paragraf.Id == CBZ.TempParagraf.Id)
                {
                    CBZ.IndexedBullets.Add(new IndexedBullet(i, bullet));
                }
            }
        }

        /// <summary>
        /// Method that creates an Indexed Enterprise List
        /// </summary>
        private void RefreshIndexedParagrafs()
        {
            RefreshProjectList("Paragrafs", CBZ.TempProject.Id);
            CBZ.IndexedParagrafs.Clear();

            int i = 0;

            foreach (Paragraf paragraf in CBZ.ProjectLists.Paragrafs)
            {
                if (paragraf.Project.Id == this.CBZ.TempProject.Id)
                {
                    IndexedParagraf paragraph = new IndexedParagraf(i, paragraf);
                    CBZ.IndexedParagrafs.Add(paragraph);
                    i++;
                }
            }
        }

        /// <summary>
        /// Method, that refreshes content of a list in CBZ
        /// </summary>
        /// <param name="list">string</param>
        private void RefreshIndexedList(string list) => CBZ.RefreshIndexedList(list);

        /// <summary>
        /// Method, that refreshes content of a list in CBZ
        /// </summary>
        /// <param name="list">string</param>
        private void RefreshList(string list) =>  CBZ.RefreshList(list);

        /// <summary>
        /// Method, that refreshes content of a list in CBZ
        /// </summary>
        /// <param name="list">string</param>
        private void RefreshProjectList(string list, int projectId) => CBZ.RefreshProjectList(list, projectId);

        /// <summary>
        /// Method, sets or resets UcMainEdited
        /// </summary>
        private void SetUcMainEdited()
        {
            //Check TextBox Content
            bool paragrafReset = CheckTextBoxParagrafReset();
            bool bulletReset = CheckTextBoxBulletReset();



            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                if (!paragrafReset || !bulletReset)
                {
                    CBZ.UcMainEdited = true;
                }
            }
            else
            {
                if (paragrafReset && bulletReset)
                {
                    CBZ.UcMainEdited = false;
                }
            }

        }

        #endregion

    }
}
