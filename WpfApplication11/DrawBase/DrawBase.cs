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

namespace WpfApplication11
{
    public class DrawBase
    {
        public DrawBase(MyCapture cap)
        {
        }

        public MyCapture MyCapture { get; set; }
        public Path CreatePath()
        {
            Path path = new Path();
            path.StrokeThickness = 1;
            path.Stroke = Brushes.White;
            SetTranslate(path);
            path.MouseLeftButtonDown += path_MouseLeftButtonDown;
            return path;
        }

        protected virtual void path_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        public List<Point> Points = new List<Point>();

        public Canvas CMain { get; set; }
        public int Step { get; set; }

        public virtual void MouseLeftButtonDown(object sender, Point point)
        {
        }

        public virtual void MouseMove(object sender, Point point)
        {

        }
        public virtual void MouseLeftButtonUp(object sender, Point point)
        {
        }
        public virtual void MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        public virtual void SetTranslate(Path path)
        {
            TranslateTransform pathTranslate = MyCapture.GetTranslate();
            ScaleTransform pathScale = MyCapture.GetScale();
            TransformGroup tfGroup = path.RenderTransform as TransformGroup;
            if (tfGroup == null)
            {
                tfGroup = new TransformGroup();
                tfGroup.Children.Add(pathTranslate);
                tfGroup.Children.Add(pathScale);
                path.RenderTransform = tfGroup;
            }
            else
            {
                TranslateTransform tempTranslate = null;
                foreach (var item in tfGroup.Children)
                {
                    tempTranslate = item as TranslateTransform;
                    if (tempTranslate != null)
                    {
                        tempTranslate.X = pathTranslate.X;
                        tempTranslate.Y = pathTranslate.Y;
                        break;
                    }
                    ScaleTransform tempScale = item as ScaleTransform;
                    if (tempScale != null)
                    {
                        tempScale.CenterX = pathScale.CenterX;
                        tempScale.CenterY = pathScale.CenterY;
                        tempScale.ScaleX = pathScale.ScaleX;
                        tempScale.ScaleY = pathScale.ScaleY;
                    }
                }
            }
        }

    }
}
