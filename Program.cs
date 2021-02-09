using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace JasonTestImage
{
    class Program
    {
        static void Main(string[] args)
        {
            var bmp = LoadImage("FakePath");

            var scaleHeight = bmp.Height - 20;
            var scaleDistanceFromTop = 10;
            var scaleDistanceFromLeft = bmp.Width - 80;

            var lines = new List<Line>()
            {
                // Vertical Main Line
                new VerticalLine(new Coordinate(scaleDistanceFromLeft, scaleDistanceFromTop), scaleHeight) {Colour = Color.Black},
                
                // Top Horizontal
                new HorizontalLine(new Coordinate(scaleDistanceFromLeft, scaleDistanceFromTop), 30) {Colour = Color.Black},
                
                // Bottom Horizontal
                new HorizontalLine(new Coordinate(scaleDistanceFromLeft, scaleDistanceFromTop + scaleHeight), 30) {Colour = Color.Black},
            };
            
            var totalPoints = 10;

            var distanceBetweenPoints = scaleHeight / totalPoints;
            
            for (int i = 0; i <= totalPoints; i++)
            {
                if (i != 0 && i != totalPoints)
                {
                    var line = new HorizontalLine(new Coordinate(scaleDistanceFromLeft, scaleDistanceFromTop + distanceBetweenPoints * i), 20) {Colour = Color.Black};
                    line.Draw(bmp);
                }

                bmp = AddText(bmp, new Coordinate(scaleDistanceFromLeft + 35, scaleDistanceFromTop + distanceBetweenPoints * i - 10), $"{i}°");
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

        private static Bitmap AddText(Bitmap bmp, Coordinate topLeftStart, string text)
        {
            RectangleF rectf = new RectangleF(topLeftStart.X, topLeftStart.Y, 30, 20);

            Graphics g = Graphics.FromImage(bmp);

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.DrawString(text, new Font("Tahoma", 10), Brushes.Black, rectf);
            g.Flush();

            return bmp;
        }
    }

    public class Line
    {
        public Color Colour = Color.Black;

        protected Coordinate StartPoint { get; set; }
        protected Coordinate EndPoint { get; set; }

        public virtual void Draw(Bitmap bmp)
        {
            using (var graphics = Graphics.FromImage(bmp))
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                graphics.DrawLine(new Pen(Colour, 1), StartPoint.X, StartPoint.Y, EndPoint.X, EndPoint.Y);
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
