using JudDataAccess;
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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MyEntityFrameWork MEFW = new MyEntityFrameWork(); 

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool contactsSorted = SortContacts();
            if (contactsSorted)
            {
                MessageBox.Show("Kontakterne blev opdateret og gemt i databasen", "Opdater Kontakter", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Kontakterne blev ikke opdateret", "Opdater Kontakter", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            bool subEntrepeneursSorted = SortSubEntrepeneurs();
            if (subEntrepeneursSorted)
            {
                MessageBox.Show("Underentrepenørerne blev opdateret og gemt i databasen", "Opdater Kontakter", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Underentrepenørerne blev ikke opdateret", "Opdater Kontakter", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private bool SortContacts()
        {
            bool result = false;

            try
            {
                foreach (LegalEntity entity in MEFW.LegalEntities)
                {
                    int entityCvr = Convert.ToInt32(entity.Cvr);
                    int entityId = entity.Id;
                    foreach (Contact contact in MEFW.Contacts)
                    {
                        bool saved = false;
                        if (contact.Entity == entityCvr)
                        {
                            Contact tempContact = new Contact(contact);
                            tempContact.Entity = entityId;
                            saved = MEFW.UpdateInDb(tempContact);
                        }
                        if (!result)
                        {
                            result = saved;
                        }
                    }

                }
            }
            catch (Exception)
            {
            }

            return result;
        }

        private bool SortSubEntrepeneurs()
        {
            bool result = false;
            try
            {
                foreach (LegalEntity entity in MEFW.LegalEntities)
                {
                    int entityCvr = Convert.ToInt32(entity.Cvr);
                    int entityId = entity.Id;
                    bool saved = false;
                    foreach (SubEntrepeneur subEntrepeneur in MEFW.SubEntrepeneurs)
                    {
                        if (subEntrepeneur.Entrepeneur == entityCvr)
                        {
                            SubEntrepeneur tempSubEntrepeneur = new SubEntrepeneur(subEntrepeneur);
                            tempSubEntrepeneur.Entrepeneur = entityId;
                            MEFW.UpdateInDb(tempSubEntrepeneur);
                        }
                        if (!result)
                        {
                            result = saved;
                        }
                    }

                }
            }
            catch (Exception)
            {
            }

            return result;
        }

    }
}
