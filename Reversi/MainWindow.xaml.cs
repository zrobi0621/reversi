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

        enum Cell { Empty, Black, White }
        enum Direction { North, NorthEast, East, SouthEast, South, SouthWest, West, NorthWest }

        List<Direction> validDirections = new List<Direction>();

        struct FCell
        {
            public int Row;
            public int Column;
            public int FlipCount;

            public FCell(int row, int column)
            {
                this.Row = row;
                this.Column = column;
                this.FlipCount = 0;
            }

            public FCell(int row, int column, int flipCount)
            {
                this.Row = row;
                this.Column = column;
                this.FlipCount = flipCount;
            }
        }

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
        int WP = 0;
        int BP = 0;

        public int playerTurn = 2; //2 -> WHITE's    1-> BLACK's
        string playerOneName = null;
        string playerTwoName = null;

        bool ai = false;
        public bool isStarted { get; set; }

        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        int ticked = 0;
        string gameOverTime = null;

        int flipCount = 0;        
        List<FCell> flipCells = new List<FCell>();

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
            WP = 0;
            BP = 0;
       
            isStarted = true;

            WhitePointsLabel.Content = $"White({playerOneName}):";
            BlackPointsLabel.Content = $"Black({playerTwoName}):";

            CreateGrid();
            RefreshUI();

            if (playerTurn == 2)
            {
                MessageBox.Show($"WHITE ({playerOneName}) starts!");
            }
            else
            {
                MessageBox.Show($"BLACK ({playerTwoName}) starts!");                              
            }

            ticked = 0;
            dispatcherTimer.Start();

            if (ai && playerTurn == 1)
            {
                List<FCell> posCells = GetAIPossibleCells();
                FCell best = GetBestCell(posCells);
                int flipC = best.FlipCount;

                if (IsValidMove(best.Row, best.Column))
                {                                       
                    Flip(flipCells, flipC);
                    Canvas canv = new Canvas();
                    Grid.SetRow(canv, best.Row);
                    Grid.SetColumn(canv, best.Column);
                    myGrid.Children.Add(canv);
                    Circle newCircle = new Circle(25);
                    newCircle.Draw(canv, Brushes.Blue);
                    gridMatrix[best.Row, best.Column] = (int)Cell.Black;
                }
                playerTurn = 2;
            }
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

                    if (gridMatrix[i, j] == (int)Cell.Black)
                    {
                        newCircle.Draw(canvas, Brushes.Black);
                    }
                    else if (gridMatrix[i, j] == (int)Cell.White)
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

            if (playerTurn == 2)
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
                    if (gridMatrix[i, j] == (int)Cell.Black)
                    {
                        blackPoints++;
                    }
                    else if (gridMatrix[i, j] == (int)Cell.White)
                    {
                        whitePoints++;
                    }
                }
            }

            BlackPointsCounter.Content = blackPoints.ToString();
            WhitePointsCounter.Content = whitePoints.ToString();

            if (blackPoints + whitePoints == 64)
            {
                BP = blackPoints;
                WP = whitePoints;
                if (blackPoints > whitePoints)
                {
                    TurnLabel.Content = "BLACK WON!";
                    MessageBox.Show("BLACK WON!");
                }
                else if(whitePoints > blackPoints)
                {
                    TurnLabel.Content = "WHITE WON!";
                    MessageBox.Show("WHITE WON!");
                }
                else
                {
                    TurnLabel.Content = "TIE!";
                    MessageBox.Show("TIE!");
                }
                GameOver();
            }
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
            SQLiteDataAccess.AddHighscore(new Highscore(playerOneName, playerTwoName, gameOverTime, WP, BP, date));
        }

        private void OpenNewGameWindow()
        {
            NewGameWindow newGameWindow = new NewGameWindow();
            RefreshGameEvent += new RefreshGame(RefreshUI);
            newGameWindow.RefreshGame = RefreshGameEvent;
            newGameWindow.ShowDialog();
        }

        private void Flip(List<FCell> cells, int count)
        {
            if (cells.Count > 0)
            {
                for (int i = 0; i < cells.Count; i++)
                {
                    Circle c = new Circle(25);

                    Canvas can = (Canvas)myGrid.Children.Cast<UIElement>().First(e => Grid.GetRow(e) == cells[i].Row && Grid.GetColumn(e) == cells[i].Column);

                    if (playerTurn == 2)    //2 -> WHITE's    1-> BLACK's
                    {                                             
                        gridMatrix[cells[i].Row,cells[i].Column] = 2;
                        c.Draw(can, Brushes.White);
                    }
                    else
                    {
                        gridMatrix[cells[i].Row, cells[i].Column] = 1;
                        c.Draw(can, Brushes.Black);
                    }
                }
            }
        }

        private List<int> GetPossibleDirections(int row, int column, int playerTurn)
        {
            List<int> possibleDirections = new List<int>();

            //0 1 2
            //3 X 4 -> X = Selected cell
            //5 6 7

            if ((row - 1 >= 0 && row - 1 <= 7) && (column - 1 >= 0 && column - 1 <= 7))
            {
                if (gridMatrix[row - 1, column - 1] != (int)Cell.Empty && gridMatrix[row - 1, column - 1] != playerTurn)
                {
                    int p = 0;
                    int r = row;
                    int c = column - 1;
                    for (int i = r - 1; i >= 0; i--)
                    {
                        int cc = c--;
                        if (i >= 0 && c >= 0)
                        {
                            if (gridMatrix[i, cc].Equals(playerTurn))
                            {
                                p++;
                            }
                            else if (gridMatrix[i, cc].Equals((int)Cell.Empty))
                            {
                                break;
                            }
                        }
                    }

                    if (p > 0)
                    {
                        possibleDirections.Add(0); //UP_LEFT
                    }
                }                                
            }
            if ((row - 1 >= 0 && row - 1 <= 7) && (column >= 0 && column <= 7))
            {
                if (gridMatrix[row - 1, column - 0] != (int)Cell.Empty && gridMatrix[row - 1, column - 0] != playerTurn)
                {
                    int p = 0;
                    for (int i = row - 1; i >= 0; i--)
                    {
                        if (gridMatrix[i, column].Equals(playerTurn))
                        {
                            p++;
                        }
                        else if (gridMatrix[i, column].Equals((int)Cell.Empty))
                        {
                            break;
                        }
                    }

                    if (p > 0)
                    {
                        possibleDirections.Add(1);  //UP
                    }
                }
            }
            if ((row - 1 >= 0 && row - 1 <= 7) && (column + 1 >= 0 && column + 1 <= 7))
            {
                if (gridMatrix[row - 1, column + 1] != (int)Cell.Empty && gridMatrix[row - 1, column + 1] != playerTurn)
                {
                    int p = 0;
                    int r = row;
                    int c = column + 1;
                    for (int i = r - 1; i >= 0; i--)
                    {
                        int cc = c++;
                        if (i >= 0 && c < 8)
                        {
                            if (gridMatrix[i, cc].Equals(playerTurn))
                            {
                                p++;
                            }
                            else if (gridMatrix[i, cc].Equals((int)Cell.Empty))
                            {
                                break;
                            }
                        }
                    }

                    if (p > 0)
                    {
                        possibleDirections.Add(2);  //UP_RIGHT
                    }
                }
            }
            if ((row >= 0 && row <= 7) && (column - 1 >= 0 && column - 1 <= 7))
            {
                if (gridMatrix[row, column - 1] != (int)Cell.Empty && gridMatrix[row, column - 1] != playerTurn)
                {
                    int p = 0;
                    for (int i = column - 1; i >= 0; i--)
                    {
                        if (gridMatrix[row, i].Equals(playerTurn))
                        {
                            p++;
                        }
                        else if (gridMatrix[row, i].Equals((int)Cell.Empty))
                        {
                            break;
                        }
                    }

                    if (p > 0)
                    {
                        possibleDirections.Add(3);  //LEFT
                    }
                }
            }
            if ((row >= 0 && row <= 7) && (column + 1 >= 0 && column + 1 <= 7))
            {
                if (gridMatrix[row, column + 1] != (int)Cell.Empty && gridMatrix[row, column + 1] != playerTurn)
                {
                    int p = 0;
                    for (int i = column + 1; i < 8; i++)
                    {
                        if (gridMatrix[row, i].Equals(playerTurn))
                        {
                            p++;
                        }
                        else if (gridMatrix[row, i].Equals((int)Cell.Empty))
                        {
                            break;
                        }
                    }

                    if (p > 0)
                    {
                        possibleDirections.Add(4);  //RIGHT
                    }
                }
            }
            if ((row + 1 >= 0 && row + 1 <= 7) && (column - 1 >= 0 && column - 1 <= 7))
            {
                if (gridMatrix[row + 1, column - 1] != (int)Cell.Empty && gridMatrix[row + 1, column - 1] != playerTurn)
                {
                    int p = 0;
                    int r = row;
                    int c = column - 1;
                    for (int i = r + 1; i < 8; i++)
                    {
                        int cc = c--;
                        if (i < 8 && c >= 0)
                        {
                            if (gridMatrix[i, cc].Equals(playerTurn))
                            {
                                p++;
                            }
                            else if (gridMatrix[i, cc].Equals((int)Cell.Empty))
                            {
                                break;
                            }
                        }
                    }

                    if (p > 0)
                    {
                        possibleDirections.Add(5);  //DOWN_LEFT
                    }
                }
            }
            if ((row + 1 >= 0 && row + 1 <= 7) && (column >= 0 && column <= 7))
            {
                if (gridMatrix[row + 1, column] != (int)Cell.Empty && gridMatrix[row + 1, column] != playerTurn)
                {
                    int p = 0;
                    for (int i = row + 1; i < 8; i++)
                    {
                        if (gridMatrix[i, column].Equals(playerTurn))
                        {
                            p++;
                        }
                        else if (gridMatrix[i, column].Equals((int)Cell.Empty))
                        {
                            break;
                        }
                    }

                    if (p > 0)
                    {
                        possibleDirections.Add(6);  //DOWN
                    }
                }
            }
            if ((row + 1 >= 0 && row + 1 <= 7) && (column + 1 >= 0 && column + 1 <= 7))
            {
                if (gridMatrix[row + 1, column + 1] != (int)Cell.Empty && gridMatrix[row + 1, column + 1] != playerTurn)
                {
                    int p = 0;
                    int r = row;
                    int c = column + 1;
                    for (int i = r + 1; i < 8; i++)
                    {
                        int cc = c++;
                        if (i < 8 && c < 8)
                        {
                            if (gridMatrix[i, cc].Equals(playerTurn))
                            {
                                p++;
                            }
                        }
                    }

                    if (p > 0)
                    {
                        possibleDirections.Add(7);  //DOWN_RIGHT
                    }
                }
            }

            return possibleDirections;
        }

        private bool IsValidMove(int row, int column)
        {
            flipCount = 0;
            bool valid = true;
            flipCells.Clear();
            //  cellsToFlip.Clear();

            if (gridMatrix[row, column].Equals((int)Cell.Empty))
            {
                List<int> possibleDirections = new List<int>();
                possibleDirections = GetPossibleDirections(row, column, playerTurn);

                if (possibleDirections.Count == 0)
                {
                    return false;
                }

                //0 1 2
                //3 X 4 -> X = Selected cell
                //5 6 7

                flipCount = 0;
                foreach (int item in possibleDirections)
                {
                    if (item == 0)  //UP_LEFT
                    {
                        int r = row;
                        int c = column-1;
                        for (int i = r - 1; i >= 0; i--)
                        {
                            int cc = c--;
                            if (cc >= 0)
                            {
                                if (gridMatrix[i, cc].Equals(playerTurn))
                                {                                    
                                    break;
                                }
                                else
                                {
                                    flipCount++;
                                    FCell fcell = new FCell(i, cc);
                                    if (!flipCells.Contains(fcell))
                                    {
                                        flipCells.Add(fcell);
                                    }
                                }
                            }                                    
                        }
                    }                                    
                    else if (item == 1)  //UP
                    {
                        for (int i = row - 1; i >= 0; i--)
                        {
                            if (gridMatrix[i, column].Equals(playerTurn))
                            {
                                break;
                            }
                            else
                            {
                                flipCount++;
                                FCell fcell = new FCell(i, column);
                                if (!flipCells.Contains(fcell))
                                {
                                    flipCells.Add(fcell);
                                }
                            }
                        }
                    }
                    else if (item == 2)  //UP_RIGHT
                    {
                        int r = row;
                        int c = column + 1;
                        for (int i = r - 1; i >= 0; i--)
                        {
                            int cc = c++;
                            if (cc < 8)
                            {
                                if (gridMatrix[i, cc].Equals(playerTurn))
                                {
                                    break;
                                }
                                else
                                {
                                    flipCount++;
                                    FCell fcell = new FCell(i, cc);
                                    if (!flipCells.Contains(fcell))
                                    {
                                        flipCells.Add(fcell);
                                    }
                                }
                            }
                        }
                    }
                    else if (item == 3)  //LEFT
                    {
                        for (int i = column - 1; i >= 0; i--)
                        {
                            if (gridMatrix[row, i].Equals(playerTurn))
                            {
                                break;
                            }
                            else
                            {
                                flipCount++;
                                FCell fcell = new FCell(row, i);
                                if (!flipCells.Contains(fcell))
                                {
                                    flipCells.Add(fcell);
                                }
                            }
                        }
                    }
                    else if (item == 4)  //RIGHT
                    {
                        for (int i = column + 1; i < 7; i++)
                        {
                            if (gridMatrix[row, i].Equals(playerTurn))
                            {
                                break;
                            }
                            else
                            {
                                flipCount++;
                                FCell fcell = new FCell(row, i);
                                if (!flipCells.Contains(fcell))
                                {
                                    flipCells.Add(fcell);
                                }
                            }
                        }
                    }
                    else if (item == 5)  //DOWN_LEFT
                    {
                        int r = row;
                        int c = column - 1;
                        for (int i = r + 1; i < 8; i++)
                        {
                            int cc = c--;
                            if (cc >= 0)
                            {
                                if (gridMatrix[i, cc].Equals(playerTurn))
                                {
                                    break;
                                }
                                else
                                {
                                    flipCount++;
                                    FCell fcell = new FCell(i, cc);
                                    if (!flipCells.Contains(fcell))
                                    {
                                        flipCells.Add(fcell);
                                    }
                                }
                            }
                        }
                    }
                    else if (item == 6)  //DOWN
                    {
                        for (int i = row + 1; i < 8; i++)
                        {
                            if (gridMatrix[i, column].Equals(playerTurn))
                            {
                                break;
                            }
                            else
                            {
                                flipCount++;
                                FCell fcell = new FCell(i, column);
                                if (!flipCells.Contains(fcell))
                                {
                                    flipCells.Add(fcell);
                                }
                            }
                        }
                    }
                    else if (item == 7)  //DOWN_RIGHT
                    {
                        int r = row;
                        int c = column + 1;
                        for (int i = r + 1; i < 8; i++)
                        {
                            int cc = c++;
                            if (cc < 8)
                            {
                                if (gridMatrix[i, cc].Equals(playerTurn))
                                {
                                    break;
                                }
                                else
                                {
                                    flipCount++;
                                    FCell fcell = new FCell(i, cc);
                                    if (!flipCells.Contains(fcell))
                                    {
                                        flipCells.Add(fcell);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (flipCount > 0)
            {                
                valid = true;
            }
            else
            {
                valid = false;
            }

            return valid;
        }

        private List<FCell> GetAIPossibleCells()
        {
            List<FCell> possibleCells = new List<FCell>();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (gridMatrix[i,j].Equals((int)Cell.Empty))
                    {
                        if (IsValidMove(i, j))
                        {
                            possibleCells.Add(new FCell(i, j, flipCount));
                        }
                    }                    
                }
            }
            return possibleCells;
        }

        private FCell GetBestCell(List<FCell> cells)
        {
            int max = 0;
            FCell f = new FCell();

            foreach (FCell cell in cells)
            {
                if (cell.FlipCount >= max)
                {
                    max = cell.FlipCount;
                }
            }

            foreach (FCell c in cells)
            {
                if (max == c.FlipCount)
                {
                    f.Row = c.Row;
                    f.Column = c.Column;
                    f.FlipCount = c.FlipCount;
                }
            }
            return f;
        }

        //***************** EVENTS *****************
        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            int r = Grid.GetRow(sender as Canvas);
            int c = Grid.GetColumn(sender as Canvas);
            Canvas canvas = (sender as Canvas);

            if (gridMatrix[r, c].Equals((int)Cell.Empty))
            {
                Circle newCircle = new Circle(25);
                if (playerTurn == 2)    //2 -> WHITE's    1-> BLACK's(AI)
                {
                    if (IsValidMove(r, c))
                    {
                        gridMatrix[r, c] = (int)Cell.White;
                        newCircle.Draw(canvas, Brushes.White);
                        Flip(flipCells, flipCount);
                        playerTurn = 1;

                        if (ai && playerTurn == 1)
                        {
                            List<FCell> posCells = GetAIPossibleCells();
                            FCell best = GetBestCell(posCells);
                            int flipC = best.FlipCount;

                            if (IsValidMove(best.Row, best.Column))
                            {
                                gridMatrix[best.Row, best.Column] = (int)Cell.Black;
                                Canvas canv = new Canvas();
                                Grid.SetRow(canv, best.Row);
                                Grid.SetColumn(canv, best.Column);
                                myGrid.Children.Add(canv);
                                Circle newCirc = new Circle(25);
                                newCirc.Draw(canv, Brushes.Black);
                                Flip(flipCells, flipC);
                            }
                            playerTurn = 2;
                        }
                    }
                }
                else
                {
                    if (!ai)
                    {                    
                        if (IsValidMove(r,c))
                        {
                            gridMatrix[r, c] = (int)Cell.Black;
                            newCircle.Draw(canvas, Brushes.Black);
                            Flip(flipCells, flipCount);
                            playerTurn = 2;
                        }
                    }
                }
                RefreshUI();
            }
        }

        //Highlighter
        private void Canvas_MouseEnter(object sender, MouseEventArgs e)
        {
            int r = Grid.GetRow(sender as Canvas);
            int c = Grid.GetColumn(sender as Canvas);
            Canvas canvas = (sender as Canvas);

            if (IsValidMove(r, c))
            {
                if (canvas.Children.Count < 1)
                {
                    Rectangle rect = new Rectangle();
                    rect.Width = canvas.ActualWidth;
                    rect.Height = canvas.ActualHeight;
                    rect.Stroke = Brushes.White;
                    canvas.Children.Add(rect);
                }
            }
            else
            {
                if (canvas.Children.Count < 1)
                {
                    Rectangle rect = new Rectangle();
                    rect.Width = canvas.ActualWidth;
                    rect.Height = canvas.ActualHeight;
                    rect.Stroke = Brushes.Red;
                    canvas.Children.Add(rect);
                }
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