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
    public class DrawCopy : DrawBase
    {
        public DrawCopy(MyCapture cap)
            : base(cap)
        {
            MyCapture = cap;
        }


        //step:
        //0 初始
        //1 按下左键开始拖动
        //2 完成选择的拖动,确认选择,再次按下左键开始拖动,若按下右键取消选择从0开始
        //3 拖动开始,光标改为箭头,释放左键完成拖动
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
                path.StrokeThickness = 0.8;
                path.StrokeDashArray = new DoubleCollection() { 5, 5 };
                CMain.Children.Add(path);
                Step = 1;
                selectedPathes.Clear();
            }
            else if (Step == 1)
            {
                if (CMain.Children.Count > CommonParam._defaultCount + 1)
                {
                    Rect selected = Next(point);

                    //MyCapture.Points.Add(rect.BottomLeft);
                    //MyCapture.Points.Add(rect.BottomRight);
                    //MyCapture.Points.Add(rect.TopLeft);
                    //MyCapture.Points.Add(rect.TopRight);
                    for (int i = CommonParam._defaultCount; i < CMain.Children.Count - 1; i++)
                    {
                        var temp = CMain.Children[i];
                        if (temp is Path)
                        {
                            Path path = temp as Path;
                            path.Stroke = Brushes.White;
                            if (path != null)
                            {
                                if (path.Data is LineGeometry)
                                {
                                    LineGeometry line = path.Data as LineGeometry;
                                    bool p1 = CommonFun.IsPointIn(selected, line.StartPoint);
                                    bool p2 = CommonFun.IsPointIn(selected, line.EndPoint);
                                    if (p1 && p2)
                                    {
                                        path.Stroke = Brushes.LightBlue;
                                        selectedPathes.Add(path);
                                    }
                                }
                                else if (path.Data is RectangleGeometry)
                                {
                                    RectangleGeometry rect = path.Data as RectangleGeometry;
                                    bool p1 = CommonFun.IsPointIn(selected, rect.Rect.BottomLeft);
                                    bool p2 = CommonFun.IsPointIn(selected, rect.Rect.BottomRight);
                                    bool p3 = CommonFun.IsPointIn(selected, rect.Rect.TopLeft);
                                    bool p4 = CommonFun.IsPointIn(selected, rect.Rect.TopRight);
                                    if (p1 && p2 && p3 && p4)
                                    {
                                        path.Stroke = Brushes.LightBlue;
                                        selectedPathes.Add(path);
                                    }
                                }
                                else if (path.Data is EllipseGeometry)
                                {
                                    EllipseGeometry ellipse = path.Data as EllipseGeometry;
                                    bool p1 = CommonFun.IsPointIn(selected, new Point(ellipse.Center.X - ellipse.RadiusX, ellipse.Center.Y));
                                    bool p2 = CommonFun.IsPointIn(selected, new Point(ellipse.Center.X + ellipse.RadiusX, ellipse.Center.Y));
                                    bool p3 = CommonFun.IsPointIn(selected, new Point(ellipse.Center.X, ellipse.Center.Y + ellipse.RadiusY));
                                    bool p4 = CommonFun.IsPointIn(selected, new Point(ellipse.Center.X, ellipse.Center.Y + ellipse.RadiusY));
                                    if (p1 && p2 && p3 && p4)
                                    {
                                        path.Stroke = Brushes.LightBlue;
                                        selectedPathes.Add(path);
                                    }
                                }
                            }
                        }
                    }
                }

                CMain.Children.RemoveAt(CMain.Children.Count - 1);
                if (selectedPathes.Count > 0)
                {
                    Step = 2;
                }
                else
                {
                    Step = 0;
                }
            }
            else if (Step == 2)
            {
                pm1 = point;
                CMain.Cursor = Cursors.Arrow;
                Step = 3;
            }
        }
        List<Path> selectedPathes = new List<Path>();
        List<Path> copyPathes = new List<Path>();
        Point pm1 = new Point();

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
            else if (Step == 3 && selectedPathes.Count > 0)
            {
                Point pm2 = point;

                double mx = (pm2.X - pm1.X);
                double my = (pm2.Y - pm1.Y);
                if (copyPathes.Count == 0)
                {
                    foreach (var path in selectedPathes)
                    {
                        if (path != null)
                        {
                            Path copyPath = CreatePath(); // CommonFun.Clone<Path>(path);
                            //Path t = CommonFun.DeepCopy<Path>(path);
                            if (path.Data is LineGeometry)
                            {
                                LineGeometry line = path.Data as LineGeometry;
                                LineGeometry copyLine = new LineGeometry();
                                copyLine.StartPoint = new Point(line.StartPoint.X, line.StartPoint.Y);
                                copyLine.EndPoint = new Point(line.EndPoint.X, line.EndPoint.Y);
                                copyPath.Data = copyLine;
                            }
                            else if (path.Data is RectangleGeometry)
                            {
                                RectangleGeometry rect = path.Data as RectangleGeometry;
                                RectangleGeometry rectCpoy = new RectangleGeometry();
                                rectCpoy.Rect = new Rect(rect.Rect.X, rect.Rect.Y, rect.Rect.Width, rect.Rect.Height);
                                copyPath.Data = rectCpoy;
                            }
                            else if (path.Data is EllipseGeometry)
                            {
                                EllipseGeometry ellipse = path.Data as EllipseGeometry;
                                EllipseGeometry ellipseCopy = new EllipseGeometry();
                                ellipseCopy.Center = new Point(ellipse.Center.X, ellipse.Center.Y);
                                ellipseCopy.RadiusX = ellipse.RadiusX;
                                ellipseCopy.RadiusY = ellipse.RadiusY;
                                copyPath.Data = ellipseCopy;
                            }
                            copyPathes.Add(copyPath);
                            CMain.Children.Add(copyPath);
                        }
                    }
                }
                foreach (Path path in copyPathes)
                {
                    if (path != null)
                    {
                        if (path.Data is LineGeometry)
                        {
                            LineGeometry line = path.Data as LineGeometry;
                            line.StartPoint = new Point(line.StartPoint.X + mx, line.StartPoint.Y + my);
                            line.EndPoint = new Point(line.EndPoint.X + mx, line.EndPoint.Y + my);
                        }
                        else if (path.Data is RectangleGeometry)
                        {
                            RectangleGeometry rect = path.Data as RectangleGeometry;
                            rect.Rect = new Rect(rect.Rect.X + mx, rect.Rect.Y + my, rect.Rect.Width, rect.Rect.Height);
                        }
                        else if (path.Data is EllipseGeometry)
                        {
                            EllipseGeometry ellipse = path.Data as EllipseGeometry;
                            ellipse.Center = new Point(ellipse.Center.X + mx, ellipse.Center.Y + my);
                        }
                    }
                }
                pm1 = pm2;
            }
        }

        public override void MouseLeftButtonUp(object sender, Point point)
        {
            if (Step == 3)
            {
                Step = 2;
                CMain.Cursor = Cursors.Cross;

                foreach (Path path in copyPathes)
                {
                    if (path != null)
                    {
                        if (path.Data is LineGeometry)
                        {
                            LineGeometry line = path.Data as LineGeometry;
                            MyCapture.Points.Add(line.StartPoint);
                            MyCapture.Points.Add(line.EndPoint);
                        }
                        else if (path.Data is RectangleGeometry)
                        {
                            RectangleGeometry rect = path.Data as RectangleGeometry;

                            MyCapture.Points.Add(rect.Rect.BottomLeft);
                            MyCapture.Points.Add(rect.Rect.BottomRight);
                            MyCapture.Points.Add(rect.Rect.TopLeft);
                            MyCapture.Points.Add(rect.Rect.TopRight);
                        }
                        else if (path.Data is EllipseGeometry)
                        {
                            EllipseGeometry ellipse = path.Data as EllipseGeometry;
                            MyCapture.Points.Add(ellipse.Center);
                        }
                    }
                }
                foreach (Path item in selectedPathes)
                {
                    item.Stroke = Brushes.White;
                }
                selectedPathes.Clear();
                copyPathes.Clear();
                Step = 0;
            }
        }
        public override void MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Step == 2)
            {
                Step = 0;
                foreach (Path item in selectedPathes)
                {
                    item.Stroke = Brushes.White;
                }
                selectedPathes.Clear();

                copyPathes.Clear();
            }
        }
    }
}
