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
    /// <summary>
    /// PaintWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PaintWindow : Window
    {
        public PaintWindow()
        {
            InitializeComponent();
        }

        DrawBase _drawBase = null;
        MyCapture _cap = new MyCapture();
        TranslateEvent _slate = new TranslateEvent();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AddCapture();
            drawText(0, 0, "", Colors.White);
            //LineGeometry line = new LineGeometry();
        }
        //捕捉工具
        RectangleGeometry pRect = null;
        Path pPath = null;
        private void AddCapture()
        {
            pRect = new RectangleGeometry();
            pPath = new Path();
            _cap.CreateCapture(pRect, pPath);


            cmain.Children.Add(pPath);
        }
        //通用方法
        /// <summary>
        /// 回归原始坐标点
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public Point GetPoint(MouseEventArgs e)
        {
            TranslateTransform temp = _cap.GetTranslate();
            Point p = e.GetPosition(this.cmain);

            //Console.WriteLine((p.X - cx) / scaleLevel);
            p.X = (p.X - cx) / scaleLevel + cx;

            p.Y = (p.Y - cy) / scaleLevel + cy;

            p.X = p.X - temp.X;
            p.Y = p.Y - temp.Y;

            return p;
        }

        public void DefaultCursor()
        {
            this.cmain.Cursor = Cursors.Cross;

            UnSelected();

            _slate.Reset();
        }

        private void UnSelected()
        {
            for (int i = 2; i < this.cmain.Children.Count - 1; i++)
            {
                var temp = this.cmain.Children[i];
                Path path = temp as Path;
                if (path != null)
                {
                    path.Stroke = Brushes.White;
                }
            }
        }
        private void cmain_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_drawBase != null)
            {
                Point p = GetPoint(e); //e.GetPosition(this.cmain);
                p = _cap.Check(p);
                _drawBase.MouseLeftButtonDown(sender, p);
            }
            else
            {
                Point p = GetPoint(e); //e.GetPosition(this.cmain);
                _slate.MouseLeftButtonDown(sender, e.GetPosition(this.cmain));
            }
        }

        private void cmain_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            if (_drawBase != null)
            {
                Point p = GetPoint(e); //e.GetPosition(this.cmain);
                _drawBase.MouseLeftButtonUp(sender, p);
            }
            else
            {
                Point p = GetPoint(e); //e.GetPosition(this.cmain);
                _slate.MouseLeftButtonUp(sender, e.GetPosition(this.cmain));
            }
        }

        private void cmain_MouseMove(object sender, MouseEventArgs e)
        {
            if (_drawBase != null)
            {
                Point p = GetPoint(e); //e.GetPosition(this.cmain);

                bool isshow = false;
                p = _cap.Check(p, out isshow);
                if (!isshow)
                {
                    pPath.Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    double length = 10;// 10 / scaleLevel;
                    pRect.Rect = new Rect(new Point(p.X - length, p.Y - length), new Point(p.X + length, p.Y + length));
                    pPath.Visibility = System.Windows.Visibility.Visible;
                }

                _drawBase.MouseMove(sender, p);
            }
            else
            {
                //Point p = GetPoint(e);
                //Console.WriteLine(string.Format("p(x:{0},y:{1})", p.X, p.Y));
                //Console.WriteLine(string.Format("e(x:{0},y:{1})", e.GetPosition(this.cmain).X, e.GetPosition(this.cmain).Y));
                _slate.MouseMove(sender, e.GetPosition(this.cmain), _cap, scaleLevel);
            }
            Point rp = GetPoint(e);
            this.bpoint.Content = string.Format("(x:{0},y:{1})", (long)rp.X, (long)rp.Y);
            drawText(e.GetPosition(this.cmain).X + 20, e.GetPosition(this.cmain).Y + 20, string.Format("(x:{0},y:{1})", (long)rp.X, (long)rp.Y), Colors.White);

            //this.btran.Content = string.Format("(x:{0},y:{1})", (long)_cap.GetTranslate().X, (long)_cap.GetTranslate().Y);
        }

        private void Reset_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            this.bstatus.Content = "当前工具: 无";
            if (_drawBase != null)
            {
                if (_drawBase.Step != 0 && this.cmain.Children.Count > 1)
                {
                    this.cmain.Children.RemoveAt(this.cmain.Children.Count - 1);
                }
                _drawBase = null;
            }
            DefaultCursor();
        }
        private void Delete_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (_drawBase != null)
            {
                if (_drawBase.Step != 0)
                {
                    _drawBase.Step = 0;
                }
            }
            while (cmain.Children.Count > 2)
            {
                //永久保留默认的捕捉点和textblock
                cmain.Children.RemoveAt(2);
            }
            _cap.Points.Clear();
        }


        private void Line_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            _drawBase = new DrawLine(_cap);
            _drawBase.CMain = this.cmain;
            DefaultCursor();
            this.bstatus.Content = "当前工具: 直线";
        }

        private void Rectangle_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            _drawBase = new DrawRectangle(_cap);
            _drawBase.CMain = this.cmain;
            DefaultCursor();
            this.bstatus.Content = "当前工具: 矩形";
        }

        private void Ellipse_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            _drawBase = new DrawEllipse(_cap);
            _drawBase.CMain = this.cmain;
            DefaultCursor();
            this.bstatus.Content = "当前工具: 圆形";
        }

        private void Drag_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            _drawBase = null;
            this.cmain.Cursor = Cursors.Hand;
            UnSelected();
            _slate.Start();
        }


        double scaleLevel = 1;
        double cx = 0;
        double cy = 0;
        private void cmain_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Point scaleCenter = GetPoint(e);// e.GetPosition(this.cmain);
            double level = scaleLevel;
            if (e.Delta > 0)
            {
                level *= 1.08;
            }
            else
            {
                level /= 1.08;
            }
            if (level < 0.3)
            {
                return;
            }
            scaleLevel = level;
            bscale.Content = string.Format("放大倍数 {0}倍", scaleLevel);

            ScaleTransform totalScale = new ScaleTransform();
            totalScale.ScaleX = scaleLevel;
            totalScale.ScaleY = scaleLevel;
            totalScale.CenterX = scaleCenter.X;
            totalScale.CenterY = scaleCenter.Y;

            _cap.SetScale(totalScale);

            cx = scaleCenter.X;
            cy = scaleCenter.Y;

            for (int i = 0; i < cmain.Children.Count; i++)
            {
                var item = cmain.Children[i];
                if (item is TextBlock)
                {
                    continue;
                }
                Path path = item as Path;

                TransformGroup tfGroup = path.RenderTransform as TransformGroup;
                if (tfGroup == null)
                {
                    tfGroup = new TransformGroup();
                    tfGroup.Children.Add(totalScale);
                    path.RenderTransform = tfGroup;
                }
                else
                {
                    ScaleTransform scaleTemp = null;
                    foreach (var tran in tfGroup.Children)
                    {
                        scaleTemp = tran as ScaleTransform;
                        if (scaleTemp != null)
                        {
                            scaleTemp.ScaleX = scaleLevel;
                            scaleTemp.ScaleY = scaleLevel;
                            scaleTemp.CenterX = scaleCenter.X;
                            scaleTemp.CenterY = scaleCenter.Y;
                            break;
                        }
                    }
                    if (scaleTemp == null)
                    {
                        tfGroup.Children.Add(totalScale);
                    }
                    //TranslateTransform pathTranslate = tfGroup.Children[0] as TranslateTransform;
                    //pathTranslate.X = tempTranslate.X;
                    //pathTranslate.Y = tempTranslate.Y;
                }
            }
        }
        TextBlock textBlock = null;
        private void drawText(double x, double y, string text, Color color)
        {
            if (textBlock == null)
            {
                textBlock = new TextBlock();
                cmain.Children.Add(textBlock);
            }

            textBlock.Text = text;

            textBlock.Foreground = new SolidColorBrush(color);

            Canvas.SetLeft(textBlock, x);

            Canvas.SetTop(textBlock, y);
        }

        private void cmain_MouseLeave(object sender, MouseEventArgs e)
        {
            Point p = GetPoint(e); //e.GetPosition(this.cmain);
            _slate.MouseLeftButtonUp(sender, e.GetPosition(this.cmain));
        }

        private void Move_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            _drawBase = new DrawSelected(_cap);
            _drawBase.CMain = this.cmain;
            DefaultCursor();
            this.bstatus.Content = "当前工具: 移动";
        }

        private void cmain_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_drawBase != null)
            {
                _drawBase.MouseRightButtonUp(sender, e);
                UnSelected();
            }
        }
    }
}
