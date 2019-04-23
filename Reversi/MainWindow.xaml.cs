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

namespace Reversi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Grid myGrid;

        public delegate void RefreshGame();
        public event RefreshGame RefreshGameEvent;

        // 0 -> EMPTY   1 -> BLACK  2 -> WHITE
        int[,] gridMatrix = new int[8, 8]
        {
                {0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0},
                {0,0,0,2,1,0,0,0},
                {0,0,0,1,2,0,0,0},
                {0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0},
        };  
    
        int whitePoints = 0;
        int blackPoints = 0;

        int playerTurn = 0; //0 -> WHITE's    1-> BLACK's
        string playerOneName = null;
        string playerTwoName = null;
        bool ai = false;

        public bool isStarted { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            isStarted = false;
        }

        public void InitGame(int playerTurn, string playerOneName, string playerTwoName, bool ai)
        {
            this.playerTurn = playerTurn;
            this.playerOneName = playerOneName;
            this.playerTwoName = playerTwoName;
            this.ai = ai;

            whitePoints = 0;
            blackPoints = 0;

            isStarted = true;

            WhitePointsLabel.Content = $"White({playerOneName}):";
            BlackPointsLabel.Content = $"Black({playerTwoName}):";

            CreateGrid();
        }

        private void CreateGrid()
        {
            myGrid = GameGrid;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Canvas canvas = new Canvas();
                    canvas.MouseDown += Canvas_MouseDown;
                    canvas.MouseEnter += Canvas_MouseEnter;
                    canvas.MouseLeave += Canvas_MouseLeave;

                    canvas.Background = Brushes.ForestGreen;
                    Grid.SetRow(canvas, i);
                    Grid.SetColumn(canvas, j);
                    myGrid.Children.Add(canvas);

                    Circle newCircle = new Circle(25);
                    if (gridMatrix[i,j] == 1)
                    {                        
                        newCircle.Draw(canvas, Brushes.Black);
                    }
                    else if(gridMatrix[i, j] == 2)
                    {
                        newCircle.Draw(canvas, Brushes.White);
                    }
                }
            }
        }

        //Update Black's and White's Counters
        void UpdateCounters()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (gridMatrix[i, j] == 1)
                    {
                        blackPoints++;
                    }
                    else if(gridMatrix[i, j] == 2)
                    {
                        whitePoints++;
                    }
                }
            }
            // TODO +2???

            BlackPointsCounter.Content = blackPoints.ToString();
            WhitePointsCounter.Content = whitePoints.ToString();
        }

       public void RefreshUI()
        {
            WhitePointsLabel.Content = $"White({playerOneName}):";
            BlackPointsLabel.Content = $"Black({playerTwoName}):";
            BlackPointsCounter.Content = blackPoints.ToString();
            WhitePointsCounter.Content = whitePoints.ToString();

            if (playerTurn == 0)
            {
                TurnLabel.Content = "WHITE's turn!";
                TurnLabel.Background = Brushes.White;
                TurnLabel.Foreground = Brushes.Black;
             
            }
            else
            {
                TurnLabel.Content = "BLACK's turn!";
                TurnLabel.Background = Brushes.Black;
                TurnLabel.Foreground = Brushes.White;
            }

            UpdateCounters();
        }

        //EVENTS
        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //e.getPosition TODO get grid position

            Circle newCircle = new Circle(25);
            if (playerTurn == 0)    //0 -> WHITE's    1-> BLACK's
            {
                newCircle.Draw(sender as Canvas, Brushes.White);
                playerTurn = 1;
            }
            else
            {
                newCircle.Draw(sender as Canvas, Brushes.Black);
                playerTurn = 0;
            }

            RefreshUI();
        }

        private void Canvas_MouseEnter(object sender, MouseEventArgs e)
        {
            Canvas canvas = (sender as Canvas);
            if (canvas.Children.Count < 1)
            {
                Rectangle rect = new Rectangle();
                rect.Width = canvas.ActualWidth;
                rect.Height = canvas.ActualHeight;
                rect.Stroke = Brushes.Red;
                canvas.Children.Add(rect);
            }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to quit? All unsaved progress will be lost.", "Quit", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }

        private void Canvas_MouseLeave(object sender, MouseEventArgs e)
        {
            Canvas canvas = (sender as Canvas);
            canvas.Children.Remove(canvas.Children.OfType<Rectangle>().FirstOrDefault());
        }

        //BUTTONS
        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            if (!isStarted)
            {
                NewGameWindow newGameWindow = new NewGameWindow();
                RefreshGameEvent += new RefreshGame(RefreshUI);       //event initialization
                newGameWindow.RefreshGame = RefreshGameEvent;        //assigning event to the Delegate
                newGameWindow.ShowDialog();
            }
            else
            {
                if (MessageBox.Show("Do you want to start a new game? All unsaved progress will be lost.", "New Game", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    //TODO init new game
                }
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
