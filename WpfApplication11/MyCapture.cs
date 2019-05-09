using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfApplication11
{
    public class MyCapture
    {
        public MyCapture()
        {
            Points = new List<Point>();
            totalTranslate.X = 0;
            totalTranslate.Y = 0;
            totalScale.CenterX = 0;
            totalScale.CenterY = 0; 
            totalScale.ScaleX = 1;
            totalScale.ScaleY = 1;
        }
        TranslateTransform totalTranslate = new TranslateTransform();
        ScaleTransform totalScale = new ScaleTransform();
        public TranslateTransform SetTranslate(double x, double y)
        {
            totalTranslate.X = totalTranslate.X + x;
            totalTranslate.Y = totalTranslate.Y + y;
            return totalTranslate;
        }
        public TranslateTransform GetTranslate()
        {
            return totalTranslate;
        }

        public void SetScale(ScaleTransform totalScale)
        {
            this.totalScale = totalScale;
        }
        public ScaleTransform GetScale()
        {
            return this.totalScale;
        }

        public void CreateCapture(RectangleGeometry pRect, Path pPath)
        {
            pRect.Rect = new Rect(new Point(0, 0), new Point(0, 0));
            pPath.Stroke = Brushes.Yellow;
            pPath.StrokeThickness = 1;
            pPath.Data = pRect;
            pPath.Visibility = System.Windows.Visibility.Collapsed;

            ScaleTransform sc = new ScaleTransform();
            sc.ScaleX = 1;
            sc.ScaleY = 1;
            sc.CenterX = 0;
            sc.CenterY = 0;
            TranslateTransform ts = new TranslateTransform();
            ts.X = 0;
            ts.Y = 0;
            TransformGroup tg = new TransformGroup();
            tg.Children.Add(ts);
            tg.Children.Add(sc);

            pPath.RenderTransform = tg;
        }

        public List<Point> Points { get; set; }

        public Point Check(Point p2, out bool isShow)
        {
            //Point p2 = new Point(point.X - totalTranslate.X, point.Y - totalTranslate.Y);
            isShow = false;
            foreach (Point p1 in Points)
            {
                double line = Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
                if (line <= 10)
                {
                    isShow = true;
                    return p1;
                }
            }
            return p2;
        }
        public Point Check(Point p2)
        {
            //Point p2 = new Point(point.X - totalTranslate.X, point.Y - totalTranslate.Y);
            foreach (Point p1 in Points)
            {
                double line = Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
                if (line <= 10)
                {
                    return p1;
                }
            }
            return p2;
        }
    }
}
