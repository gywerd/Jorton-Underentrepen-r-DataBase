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

        bool oldPassWordCorrect = false;
        bool newPassWordLength = false;
        bool newPassWordrepeatCorrect = false;

        ImageBrush greenIndicator = new ImageBrush();
        ImageBrush redIndicator = new ImageBrush();


        #endregion

        #region Constructors
        public UcChangePassWord(Bizz cbz, UserControl ucMain)
        {
            InitializeComponent();
            this.CBZ = cbz;
            this.UcMain = ucMain;

            SetIndicators();
        }

        #endregion

        #region Buttons
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            if (CBZ.UcMainActive)
            {
                //Warning about lost changes before closing
                if (MessageBox.Show("Passwordet er ikke ændret. Vil du lukke alligevel?", "Password", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    //Close right UserControl
                    CBZ.UcMainActive = false;
                    UcMain.Content = new UserControl();
                }
            }
            else
            {
                //Close main UserControl
                CBZ.UcMainActive = false;
                UcMain.Content = new UserControl();
            }
        }

        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            //Show Errors
            if (!oldPassWordCorrect)
            {
                MessageBox.Show("Det nuværende password er tastet forkert.", "Login", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (!newPassWordLength)
            {
                MessageBox.Show("Det nye password er for kort. Mindst 8 tegn.", "Login", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (!newPassWordrepeatCorrect)
            {
                MessageBox.Show("Nyt Password & Gentaget Nyt Password er ikke ens.", "Login", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            //Change Password in memory
            bool passWordChanged = false;

            if (oldPassWordCorrect && newPassWordLength && newPassWordrepeatCorrect)
            {
                CBZ.CurrentUser.ChangePassword(PasswordBoxOld.Text, PasswordBoxNew.Text);
                passWordChanged = true;
            }

            //Update User in Db
            bool passWordUpdated = false;

            if (passWordChanged)
            {
                passWordUpdated = CBZ.UpdateInDb(CBZ.CurrentUser);
            }

            //Show result
            if (passWordChanged & passWordUpdated)
            {
                MessageBox.Show("Passwordet blev opdateret.", "Login", MessageBoxButton.OK, MessageBoxImage.Information);
                ButtonClose_Click(sender, e);

            }
            else
            {
                MessageBox.Show("Passwordet blev ikke opdateret. Tjek, at du har tastet korrekt: Mindst 8 tegn. Forskel på store og små bogstaver. Ingen mellemrum.", "Login", MessageBoxButton.OK, MessageBoxImage.Error);
                CBZ.CurrentUser.ChangePassword(PasswordBoxNew.Text, PasswordBoxOld.Text);
            }
        }

        #endregion

        #region Events
        private void PasswordBoxNew_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Remove spaces
            string tempPassWord = PasswordBoxNew.Text;
            tempPassWord.Replace(" ", "");
            PasswordBoxNew.Text = tempPassWord;

            //Check if new Password is correct length
            CheckNewPassWordLength();

            //Set CBZ.UcMainActive
            if (!CBZ.UcMainActive)
            {
                CBZ.UcMainActive = true;
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

            //Set CBZ.UcMainActive
            if (!CBZ.UcMainActive)
            {
                CBZ.UcMainActive = true;
            }
        }

        private void PasswordBoxOld_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Remove spaces
            string tempPassWord = PasswordBoxOld.Text;
            tempPassWord.Replace(" ", "");
            PasswordBoxOld.Text = tempPassWord;

            //Check if old Password is correct
            CheckOldPassWordCorrect();

            //Set CBZ.UcMainActive
            if (!CBZ.UcMainActive)
            {
                CBZ.UcMainActive = true;
            }
        }

        #endregion

        #region Methods

        private void CheckNewPassWordRepeat()
        {

            if (PasswordBoxNew.Text == PasswordBoxNewRepeat.Text)
            {
                newPassWordrepeatCorrect = true;
                NewRepeatIndicatorBackGround = greenIndicator;
            }
            else
            {
                newPassWordrepeatCorrect = false;
                NewRepeatIndicatorBackGround = redIndicator;
            }

        }

        private void CheckNewPassWordLength()
        {
            if (PasswordBoxNew.Text.Length >= 8)
            {
                newPassWordLength = true;
                NewIndicatorBackGround = greenIndicator;
            }
            else
            {
                newPassWordLength = false;
                NewIndicatorBackGround = redIndicator;
            }
        }

        private void CheckOldPassWordCorrect()
        {

            if (PasswordBoxOld.Text == CBZ.CurrentUser.Authentication.PassWord)
            {
                oldPassWordCorrect = true;
                OldIndicatorBackGround = greenIndicator;
            }
            else
            {
                oldPassWordCorrect = true;
                OldIndicatorBackGround = redIndicator;
            }

        }

        private void SetIndicators()
        {
            Uri greenUri = new Uri("Images/green-indicator.png", UriKind.Relative);
            Uri redUri = new Uri("Images/red-indicator.png", UriKind.Relative);
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
