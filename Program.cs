using System.Collections.Generic;
using System.Drawing;

namespace JasonTestImage
{
    class Program
    {
        static void Main(string[] args)
        {
            var bmp = LoadImage("FakePath");

            var scaleHeight = bmp.Height - 20;
            var scaleDistanceFromTop = 10;
            var scaleDistanceFromLeft = 10;

            var lines = new List<Line>()
            {
                // Vertical Main Line
                new VerticalLine(new Coordinate(scaleDistanceFromLeft, scaleDistanceFromTop), scaleHeight),
                
                // Top Horizontal
                new HorizontalLine(new Coordinate(scaleDistanceFromLeft, scaleDistanceFromTop), 30),
                
                // Bottom Horizontal
                new HorizontalLine(new Coordinate(scaleDistanceFromLeft, scaleDistanceFromTop + scaleHeight), 30),
            };
            
            var totalPoints = 10;
            var distanceBetweenPoints = scaleHeight / totalPoints;
            
            for (int i = 0; i < 10; i++)
            {
                var line = new HorizontalLine(new Coordinate(scaleDistanceFromLeft, scaleDistanceFromTop + distanceBetweenPoints * i), 20);
                line.Draw(bmp);
            }

            lines.ForEach(l=>l.Draw(bmp));

            bmp.Save("c:\\temp\\Test.bmp");
        }

        private static Bitmap LoadImage(string path)
        {
            // For time being create a blank bitmap instead of generating one from file
            var bmp = new Bitmap(640, 480);
            using (Graphics gfx = Graphics.FromImage(bmp))
            using (SolidBrush brush = new SolidBrush(Color.Beige))
            {
                gfx.FillRectangle(brush, 0, 0, bmp.Width, bmp.Height);
            }

            return bmp;
        }

    }

    public class Line
    {
        protected Coordinate StartPoint { get; set; }
        protected Coordinate EndPoint { get; set; }

        public virtual void Draw(Bitmap bmp)
        {
            using (var graphics = Graphics.FromImage(bmp))
            {
                graphics.DrawLine(new Pen(Color.Red, 2), StartPoint.X, StartPoint.Y, EndPoint.X, EndPoint.Y);
            }
        }
    }

    public class VerticalLine : Line
    {
        public VerticalLine(Coordinate startPoint, int length)
        { 
            StartPoint = startPoint;
            EndPoint = new Coordinate()
            {
                X = startPoint.X,
                Y = startPoint.Y + length
            };
        }
    }

    public class HorizontalLine : Line
    {
        public HorizontalLine(Coordinate startPoint, int length)
        {
            StartPoint = startPoint;
            EndPoint = new Coordinate()
            {
                X = startPoint.X + length,
                Y = startPoint.Y
            };
        }
    }

    public class Coordinate
    {
        public Coordinate()
        { }

        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }
    }
}
