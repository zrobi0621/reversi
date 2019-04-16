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
        

        public MainWindow()
        {
            InitializeComponent();
            CreateCanvas();
        }

        private void CreateCanvas()
        {
            Grid myGrid = GameGrid;
            //Rectangle rect = new Rectangle();
           // rect.Fill = Brushes.Red;
            int r = 0;
            int c = 0;
            

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Rectangle rect = new Rectangle();
                    rect.MouseEnter += Rect_MouseEnter;
                    rect.MouseLeave += Rect_MouseLeave;
                    rect.Fill = Brushes.AntiqueWhite;
                    rect.Stroke = Brushes.Black;
                    Grid.SetRow(rect, i);
                    Grid.SetColumn(rect, j);
                    myGrid.Children.Add(rect);
                }
            }
        }

       
        private void Rect_MouseEnter(object sender, MouseEventArgs e)
        {
            (sender as Rectangle).Fill = Brushes.Red;
        }

        private void Rect_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as Rectangle).Fill = Brushes.Azure;
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
