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
using System.Windows.Resources;
using System.Windows.Shapes;

namespace JudGui
{
    /// <summary>
    /// Interaction logic for UcChangePassWord.xaml
    /// </summary>
    public partial class UcChangePassWord : UserControl
    {
        #region Fields
        public Bizz CBZ;
        public UserControl UcMain;
        public string TempNewPassWord = "";

        public bool ForcedPassWordChange = false;
        public bool OldPassWordCorrect = false;
        public bool NewPassWordLength = false;
        public bool NewPassWordrepeatCorrect = false;

        public ImageBrush greenIndicator = new ImageBrush();
        public ImageBrush redIndicator = new ImageBrush();


        #endregion

        #region Constructors
        public UcChangePassWord(Bizz cbz, UserControl ucMain, bool forcedPassWordChange = false)
        {
            InitializeComponent();
            this.CBZ = cbz;
            this.UcMain = ucMain;
            this.ForcedPassWordChange = forcedPassWordChange;

            SetIndicators();
        }

        #endregion

        #region Buttons
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            if (ForcedPassWordChange)
            {
                MessageBox.Show("Passwordet skal ændres, før du kan benytte programmet", "Password", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (CBZ.UcMainEdited && !ForcedPassWordChange)
            {
                //Warning about lost changes before closing
                if (MessageBox.Show("Passwordet er ikke ændret. Vil du lukke alligevel?", "Password", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    //Close right UserControl
                    CBZ.UcMainEdited = false;
                    UcMain.Content = new UserControl();
                }
            }
            else
            {
                //Close main UserControl
                CBZ.UcMainEdited = false;
                UcMain.Content = new UserControl();
            }
        }

        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            //Show Errors
            if (!OldPassWordCorrect)
            {
                MessageBox.Show("Det nuværende password er tastet forkert.", "Login", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (!NewPassWordLength)
            {
                MessageBox.Show("Det nye password er for kort. Mindst 8 tegn.", "Login", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (!NewPassWordrepeatCorrect)
            {
                MessageBox.Show("Nyt Password & Gentaget Nyt Password er ikke ens.", "Login", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            //Update Password
            bool passWordChanged = false;

            if (OldPassWordCorrect && NewPassWordLength && NewPassWordrepeatCorrect)
            {
                passWordChanged = CBZ.ChangePassword(PasswordBoxOld.Password, PasswordBoxNew.Password);
            }

            //
           //Show result
            if (passWordChanged)
            {
                MessageBox.Show("Passwordet blev opdateret.", "Login", MessageBoxButton.OK, MessageBoxImage.Information);
                ForcedPassWordChange = false;
                CBZ.UcMainEdited = false;
                ButtonClose_Click(sender, e);
            }
            else
            {
                MessageBox.Show("Passwordet blev ikke opdateret. Tjek, at du har tastet korrekt: Mindst 8 tegn. Forskel på store og små bogstaver. Ingen mellemrum.", "Login", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region Events
        private void PasswordBoxNew_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Remove spaces
            TempNewPassWord = PasswordBoxNew.Text;
            TempNewPassWord.Replace(" ", "");
            PasswordBoxNew.Text = TempNewPassWord;

            //Check if new Password is correct length
            CheckNewPassWordLength();

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void PasswordBoxNewRepeat_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Remove spaces
            string tempPassWord = PasswordBoxNewRepeat.Text;
            tempPassWord.Replace(" ", "");
            PasswordBoxNewRepeat.Text = tempPassWord;

            //Check if new Password repeat matches
            CheckNewPassWordRepeat();

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        private void PasswordBoxOld_TextChanged(object sender, TextChangedEventArgs e)
            {
            //Remove spaces
            string tempPassWord = PasswordBoxOld.Password;
            tempPassWord.Replace(" ", "");
            PasswordBoxOld.Text = tempPassWord;

            //Check if old Password is correct
            CheckOldPassWordCorrect();

            //Set CBZ.UcMainEdited
            if (!CBZ.UcMainEdited)
            {
                CBZ.UcMainEdited = true;
            }
        }

        #endregion

        #region Methods

        private void CheckNewPassWordRepeat()
        {

            if (PasswordBoxNew.Text == PasswordBoxNewRepeat.Text)
            {
                NewPassWordrepeatCorrect = true;
                ButtonPasswordNewRepeatIndicator.Background = greenIndicator;
            }
            else
            {
                NewPassWordrepeatCorrect = false;
                ButtonPasswordNewRepeatIndicator.Background = redIndicator;
            }

        }

        private void CheckNewPassWordLength()
        {
            if (PasswordBoxNew.Text.Length >= 8)
            {
                NewPassWordLength = true;
                ButtonPasswordNewIndicator.Background = greenIndicator;
            }
            else
            {
                NewPassWordLength = false;
                ButtonPasswordNewIndicator.Background = redIndicator;
            }
        }

        private void CheckOldPassWordCorrect()
        {

            if (CBZ.CheckLogin(CBZ.CurrentUser.Initials, PasswordBoxOld.Text))
            {
                OldPassWordCorrect = true;
                ButtonPasswordOldIndicator.Background = greenIndicator;
            }
            else
            {
                OldPassWordCorrect = false;
                ButtonPasswordOldIndicator.Background = redIndicator; 
            }

        }

        private void SetIndicators()
        {
            Uri greenUri = new Uri(@"Images\green-indicator.png", UriKind.Relative);
            Uri redUri = new Uri(@"Images\red-indicator.png", UriKind.Relative);
            StreamResourceInfo greenStreamInfo = Application.GetResourceStream(greenUri);
            StreamResourceInfo redStreamInfo = Application.GetResourceStream(redUri);

            BitmapFrame tempGreen = BitmapFrame.Create(greenStreamInfo.Stream);
            BitmapFrame tempRed = BitmapFrame.Create(redStreamInfo.Stream);
            greenIndicator.ImageSource = tempGreen;
            redIndicator.ImageSource = tempRed;

        }

        #endregion

    }
}
