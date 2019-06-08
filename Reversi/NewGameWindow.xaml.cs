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
using System.Windows.Shapes;

namespace Reversi
{
    /// <summary>
    /// Interaction logic for NewGameWindow.xaml
    /// </summary>
    public partial class NewGameWindow : Window
    {
        public NewGameWindow()
        {
            InitializeComponent();
            AICheckBox.IsChecked = true;
        }

        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

        public Delegate RefreshGame;

        int playerTurn = 0; //0 -> WHITE's    1-> BLACK's
        string playerOneName = null;
        string playerTwoName = null;
        bool ai = false;

        //BUTTONS
        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            playerOneName = PlayerOneTextBox.Text;
            playerTwoName = PlayerTwoTextBox.Text;

            if(string.IsNullOrEmpty(PlayerOneTextBox.Text))
            {
                MessageBox.Show("Player One's name is missing!");
                return;
            }

            if (AICheckBox.IsChecked.Value == false)
            {
                if (string.IsNullOrEmpty(PlayerTwoTextBox.Text))
                {
                    MessageBox.Show("Player Two's name is missing!");
                    return;
                }
            }
                
            if (AICheckBox.IsChecked.Value == true)
            {
                playerTwoName = "AI";
            }

            Random r = new Random();
            playerTurn = r.Next(1, 3);  //2 -> WHITE's    1-> BLACK's

            mainWindow.InitGame(playerTurn, playerOneName, playerTwoName, ai);
            mainWindow.isStarted = true;
            RefreshGame.DynamicInvoke();
            this.Close();
        }

        //EVENTS
        private void AICheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ai = true;
            PlayerCheckBox.IsChecked = false;
            PlayerTwoTextBox.IsEnabled = false;
            PlayerTwoTextBox.Background = Brushes.DarkGray;
            PlayerTwoTextBox.Text = null;
        }

        private void PlayerCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ai = false;
            AICheckBox.IsChecked = false;
            PlayerTwoTextBox.IsEnabled = true;
            PlayerTwoTextBox.Background = Brushes.White;
        }

        private void AICheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ai = false;
            PlayerCheckBox.IsChecked = true;
        }

        private void PlayerCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ai = true;
            AICheckBox.IsChecked = true;
        }
    }
}
