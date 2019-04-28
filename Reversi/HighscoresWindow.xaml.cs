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
    /// Interaction logic for HighscoresWindow.xaml
    /// </summary>
    public partial class HighscoresWindow : Window
    {
        List<Highscore> highscores = new List<Highscore>();

        public HighscoresWindow()
        {
            InitializeComponent();
            ShowHighscores();
        }

        private void ShowHighscores()
        {
            highscores = SQLiteDataAccess.LoadHighscores();
            ListView.ItemsSource = null;
            ListView.ItemsSource = highscores;
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do you want to clear the High Scores? ", "Clear - High Scores", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                SQLiteDataAccess.DeleteHighscores();
                ShowHighscores();
            }
        }
    }
}
