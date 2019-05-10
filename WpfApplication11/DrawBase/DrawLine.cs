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
    public class DrawLine : DrawBase
    {
        public DrawLine(MyCapture cap)
            : base(cap)
        {
            MyCapture = cap;
        }
        public override void MouseLeftButtonDown(object sender, Point point)
        {
            if (Step == 0)
            {
                LineGeometry l = new LineGeometry();
                //Point p = e.GetPosition(CMain);
                l.StartPoint = point;
                l.EndPoint = point;
                Path path = CreatePath();
                path.Data = l;
                path.Tag = point;
                CMain.Children.Add(path);
                Step = 1;
                MyCapture.Points.Add(point);
            }
            else if (Step == 1)
            {
                Point p2 = Next(point);
                MyCapture.Points.Add(p2);
                Step = 0;
            }
        }

        public Point Next(Point p2)
        {
            Path path = CMain.Children[CMain.Children.Count - 1] as Path;
            LineGeometry l = path.Data as LineGeometry;
            if (CommonParam._isZhengjiao)
            {
                Point p1 = l.StartPoint;
                if (Math.Abs(p1.X - p2.X) < Math.Abs(p1.Y - p2.Y))
                {
                    p2 = new Point(p1.X, p2.Y);
                }
                else
                {
                    p2 = new Point(p2.X, p1.Y);
                }
            }
            l.EndPoint = p2;
            return p2;
            //l.EndPoint = e.GetPosition(CMain);
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
