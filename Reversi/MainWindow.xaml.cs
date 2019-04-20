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
        };  // 0 -> EMPTY   1 -> BLACK  2 -> WHITE

        int player = 0; //0 -> WHITE    1-> BLACK
        int whitePoints = 0;
        int blackPoints = 0;


        public MainWindow()
        {
            InitializeComponent();
            CreateGrid();
            UpdateCounter();
        }

        private void CreateGrid()
        {
            myGrid = GameGrid;

            whitePoints = 0;
            blackPoints = 0;

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

        //Update Black and White Counters
        void UpdateCounter()
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

            BlackPointsCounter.Content = blackPoints.ToString();
            WhitePointsCounter.Content = whitePoints.ToString();
        }

        //EVENTS
        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {

            //e.getPosition TODO grid position

                Circle newCircle = new Circle(25);
                if (player == 0)
                {
                    newCircle.Draw(sender as Canvas, Brushes.White);
                    player = 1;
                    TurnLabel.Content = "BLACK's turn!";
                }
                else
                {
                    newCircle.Draw(sender as Canvas, Brushes.Black);
                    player = 0;
                    TurnLabel.Content = "WHITE's turn!";
                }
            UpdateCounter();
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

        //BUTTONS
        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            NewGameWindow newGameWindow = new NewGameWindow();
            
            newGameWindow.ShowDialog();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
