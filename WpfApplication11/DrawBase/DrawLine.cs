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
                Next(point);
                MyCapture.Points.Add(point);
                Step = 0;
            }
        }

        public void Next(Point p2)
        {
            Path path = CMain.Children[CMain.Children.Count - 1] as Path;
            LineGeometry l = path.Data as LineGeometry;
            l.EndPoint = p2;
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
