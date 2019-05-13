using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfApplication11
{
    public class DrawSelected : DrawBase
    {
        public DrawSelected(MyCapture cap)
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
                        else if (temp is TextBlock)
                        {
                            TextBlock edit = temp as TextBlock;
                            double x = Canvas.GetLeft(edit);
                            double y = Canvas.GetTop(edit);
                            bool p1 = CommonFun.IsPointIn(selected, new Point(x, y));
                            bool p2 = CommonFun.IsPointIn(selected, new Point(x + edit.ActualWidth, y + edit.ActualHeight));
                            if (p1 && p2)
                            {
                                edit.Foreground = Brushes.LightBlue;
                                selectedPathes.Add(edit);
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
        List<UIElement> selectedPathes = new List<UIElement>();
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

                foreach (UIElement UI in selectedPathes)
                {
                    if (UI is Path)
                    {
                        Path path = UI as Path;
                        if (path.Data is LineGeometry)
                        {
                            LineGeometry line = path.Data as LineGeometry;
                            MyCapture.Points.Remove(line.StartPoint);
                            MyCapture.Points.Remove(line.EndPoint);
                            line.StartPoint = new Point(line.StartPoint.X + mx, line.StartPoint.Y + my);
                            line.EndPoint = new Point(line.EndPoint.X + mx, line.EndPoint.Y + my);
                            MyCapture.Points.Add(line.StartPoint);
                            MyCapture.Points.Add(line.EndPoint);
                        }
                        else if (path.Data is RectangleGeometry)
                        {
                            RectangleGeometry rect = path.Data as RectangleGeometry;

                            MyCapture.Points.Remove(rect.Rect.BottomLeft);
                            MyCapture.Points.Remove(rect.Rect.BottomRight);
                            MyCapture.Points.Remove(rect.Rect.TopLeft);
                            MyCapture.Points.Remove(rect.Rect.TopRight);
                            rect.Rect = new Rect(rect.Rect.X + mx, rect.Rect.Y + my, rect.Rect.Width, rect.Rect.Height);
                            MyCapture.Points.Add(rect.Rect.BottomLeft);
                            MyCapture.Points.Add(rect.Rect.BottomRight);
                            MyCapture.Points.Add(rect.Rect.TopLeft);
                            MyCapture.Points.Add(rect.Rect.TopRight);
                        }
                        else if (path.Data is EllipseGeometry)
                        {
                            EllipseGeometry ellipse = path.Data as EllipseGeometry;
                            MyCapture.Points.Remove(ellipse.Center);
                            ellipse.Center = new Point(ellipse.Center.X + mx, ellipse.Center.Y + my);
                            MyCapture.Points.Add(ellipse.Center);
                        }
                    }
                    else if (UI is TextBlock)
                    {
                        TextBlock edit = UI as TextBlock;
                        
                        Canvas.SetLeft(edit, Canvas.GetLeft(edit) + mx);
                        Canvas.SetTop(edit, Canvas.GetTop(edit) + my);
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
            }
        }
        public override void MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Step == 2)
            {
                Step = 0;
                foreach (UIElement item in selectedPathes)
                {
                    if (item is Path)
                    {
                        ((Path)item).Stroke = Brushes.White;
                    }
                    if (item is TextBlock)
                    {
                        ((TextBlock)item).Foreground = Brushes.White;
                    }
                }
                selectedPathes.Clear();
            }
        }
    }
}
