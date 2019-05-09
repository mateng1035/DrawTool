using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfApplication11
{
    public class DrawEllipse : DrawBase
    {

        public DrawEllipse(MyCapture cap)
            : base(cap)
        {
            MyCapture = cap;
        }
        public override void MouseLeftButtonDown(object sender, Point point)
        {
            if (Step == 0)
            {
                EllipseGeometry ellipse = new EllipseGeometry();
                //Point p = e.GetPosition(CMain);
                ellipse.Center = point;
                Path path = CreatePath();
                path.Data = ellipse;
                path.Tag = point;
                CMain.Children.Add(path);
                MyCapture.Points.Add(point);
                Step = 1;
            }
            else if (Step == 1)
            {
                Next(point);
                Step = 0;
            }
        }

        public void Next(Point p2)
        {
            Path path = CMain.Children[CMain.Children.Count - 1] as Path;
            EllipseGeometry ellipse = path.Data as EllipseGeometry;


            Point p1 = (Point)path.Tag;

            double line = Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));

            ellipse.RadiusX = line;
            ellipse.RadiusY = line;
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
