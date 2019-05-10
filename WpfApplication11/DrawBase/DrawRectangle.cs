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
    public class DrawRectangle : DrawBase
    {
        public DrawRectangle(MyCapture cap)
            : base(cap)
        {
            MyCapture = cap;
        }
        //Point p1 = new Point();
        public override void MouseLeftButtonDown(object sender, Point point)
        {
            if (Step == 0)
            {
                //Point p = e.GetPosition(CMain);
                RectangleGeometry rect = new RectangleGeometry();

                rect.Rect = new Rect(point, point);
                //p1 = p;

                Path path = CreatePath();
                path.Tag = point;
                path.Data = rect;
                CMain.Children.Add(path);
                Step = 1;
            }
            else if (Step == 1)
            {
                Rect rect = Next(point);

                MyCapture.Points.Add(rect.BottomLeft);
                MyCapture.Points.Add(rect.BottomRight);
                MyCapture.Points.Add(rect.TopLeft);
                MyCapture.Points.Add(rect.TopRight);
                Step = 0;
            }
        }

        private Rect Next(Point p2)
        {
            Path path = CMain.Children[CMain.Children.Count - 1] as Path;
            RectangleGeometry rect = path.Data as RectangleGeometry;

            Point p1 = (Point)path.Tag;

            //Point p2 = e.GetPosition(CMain);

            Point topleft = new Point();
            Point bottomright = new Point();

            if (p1.X < p2.X)
            {
                topleft.X = p1.X;
                bottomright.X = p2.X;
            }
            else
            {
                topleft.X = p2.X;
                bottomright.X = p1.X;
            }

            if (p1.Y < p2.Y)
            {
                topleft.Y = p1.Y;
                bottomright.Y = p2.Y;
            }
            else
            {
                topleft.Y = p2.Y;
                bottomright.Y = p1.Y;
            }

            rect.Rect = new Rect(topleft, bottomright);

            return rect.Rect;
        }

        public override void MouseMove(object sender, Point point)
        {
            if (Step == 1)
            {
                Next(point);
            }
        }
    }
}
