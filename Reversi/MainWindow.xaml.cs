using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Threading;

namespace Reversi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Grid myGrid;
        string date = null;

        public delegate void RefreshGame();
        public event RefreshGame RefreshGameEvent;

        // 0 -> EMPTY   1 -> BLACK  2 -> WHITE
        int[,] startMatrix = new int[8, 8]
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
        int[,] gridMatrix = new int[8, 8];

        int whitePoints = 0;
        int blackPoints = 0;

        int playerTurn = 0; //0 -> WHITE's    1-> BLACK's
        string playerOneName = null;
        string playerTwoName = null;
        bool ai = false;

        public bool isStarted { get; set; }

        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        int ticked = 0;

        string gameOverTime = null;
        
        public MainWindow()
        {
            InitializeComponent();

            date = System.DateTime.Now.Date.ToString();

            isStarted = false;

            dispatcherTimer.Tick += new EventHandler(DispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
        }

        //Game initialization
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
            RefreshUI();

            if (playerTurn == 0)
            {
                MessageBox.Show($"WHITE ({playerOneName}) starts!");
            }
            else
            {
                MessageBox.Show($"BLACK ({playerTwoName}) starts!");
            }

            ticked = 0;
            dispatcherTimer.Start();
        }

        private void CreateGrid()
        {
            Array.Copy(startMatrix, 0, gridMatrix, 0, startMatrix.Length);
            
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

            //Update Counters
            blackPoints = 0;
            whitePoints = 0;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (gridMatrix[i, j] == 1)
                    {
                        blackPoints++;
                    }
                    else if (gridMatrix[i, j] == 2)
                    {
                        whitePoints++;
                    }
                }
            }

            BlackPointsCounter.Content = blackPoints.ToString();
            WhitePointsCounter.Content = whitePoints.ToString();
        }

        private void GameOver()
        {
            dispatcherTimer.Stop();
            gameOverTime = StopWatchLabel.Content.ToString();
            SaveResult();

            if (MessageBox.Show("Do you want to start a new game? ", "New Game", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                myGrid.Children.Clear();
                isStarted = false;
                OpenNewGameWindow();
            }
            else
            {
                this.Close();
            }
        }

        private void SaveResult()
        {
            SQLiteDataAccess.AddHighscore(new Highscore(playerOneName, playerTwoName, gameOverTime, whitePoints, blackPoints, date));
        }

        private void OpenNewGameWindow()
        {
            NewGameWindow newGameWindow = new NewGameWindow();
            RefreshGameEvent += new RefreshGame(RefreshUI);       //event initialization
            newGameWindow.RefreshGame = RefreshGameEvent;        //assigning event to the Delegate
            newGameWindow.ShowDialog();
        }

        //***************** EVENTS *****************
        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            int r = Grid.GetRow(sender as Canvas);
            int c = Grid.GetColumn(sender as Canvas);

            if (gridMatrix[r,c].Equals(0))
            {
                Circle newCircle = new Circle(25);
                if (playerTurn == 0)    //0 -> WHITE's    1-> BLACK's
                {
                    //TODO validMove
                    gridMatrix[r, c] = 2;
                    newCircle.Draw(sender as Canvas, Brushes.White);
                    playerTurn = 1;
                }
                else
                {
                    if (ai)
                    {
                        //TODO
                    }
                    else
                    {
                        //TODO validMove
                        gridMatrix[r, c] = 1;
                        newCircle.Draw(sender as Canvas, Brushes.Black);
                        playerTurn = 0;
                    }
                }
                RefreshUI();
            }
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

        private void Canvas_MouseLeave(object sender, MouseEventArgs e)
        {
            Canvas canvas = (sender as Canvas);
            canvas.Children.Remove(canvas.Children.OfType<Rectangle>().FirstOrDefault());
        }

        //Stopwatch
        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            ticked++;
            TimeSpan time = TimeSpan.FromSeconds(ticked);
            string stopWatchTime = time.ToString(@"hh\:mm\:ss");

            StopWatchLabel.Content = stopWatchTime;

            // Forcing the CommandManager to raise the RequerySuggested event
            CommandManager.InvalidateRequerySuggested();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to quit? All unsaved progress will be lost.", "Quit", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                dispatcherTimer.Stop();
                e.Cancel = true;
            }
        }

        //***************** BUTTONS *****************
        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            if (!isStarted)
            {
                OpenNewGameWindow();
            }
            else
            {
                if (MessageBox.Show("Do you want to start a new game? All unsaved progress will be lost.", "New Game", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    dispatcherTimer.Stop();
                    myGrid.Children.Clear();
                    isStarted = false;
                    OpenNewGameWindow();
                }
            }
        }

        private void HighscoresButton_Click(object sender, RoutedEventArgs e)
        {
            HighscoresWindow highscoresWindow = new HighscoresWindow();
            highscoresWindow.ShowDialog();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}