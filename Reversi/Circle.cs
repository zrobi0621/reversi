using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Reversi
{
    public class Circle
    {
        public int Radius { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }

        public Circle(int radius)
        {
            this.Radius = radius;
        }

        public Circle(int radius,int row, int column)
        {
            this.Radius = radius;
            this.Row = row;
            this.Column = Column;
        }

        public void Draw(Canvas canvas, SolidColorBrush fillColor)
        {
            var circle = new Ellipse();
            circle.Width = Radius * 2;
            circle.Height = Radius * 2;
            circle.Stroke = Brushes.Black;
            circle.Fill = fillColor;

            canvas.Children.Clear();
            canvas.Children.Add(circle);
        }

        public override string ToString()
        {
            return $"Circle - {Radius}";
        }
    }
}
