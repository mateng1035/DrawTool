﻿using DevExpress.Xpf.Bars;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        List<SVGBase> _svgList;
        public PaintWindow(string filename, List<SVGBase> svgList)
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(filename))
            {
                this.Title = filename;
                _filename = filename;
                _svgList = svgList;
            }
        }

        DrawBase _drawBase = null;
        MyCapture _cap = new MyCapture();
        TranslateEvent _slate = new TranslateEvent();
        string _filename;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AddCapture();
            DrawText(0, 0, "", Colors.White);
            DrawEditBox(0, 0, "", false);
            SetControls();

            if (_svgList != null && _svgList.Count > 0)
            {
                foreach (var item in _svgList)
                {
                    Path path = new Path();
                    path.StrokeThickness = 1;
                    path.Stroke = Brushes.White;
                    if (item is SVGRectangle)
                    {
                        SVGRectangle rect = item as SVGRectangle;
                        RectangleGeometry r = new RectangleGeometry();
                        r.Rect = new Rect(rect.Point1, rect.Point2);
                        path.Data = r;
                    }
                    else if (item is SVGLine)
                    {
                        SVGLine line = item as SVGLine;
                        LineGeometry l = new LineGeometry();
                        l.StartPoint = line.StartPoint;
                        l.EndPoint = line.EndPoint;
                        path.Data = l;
                    }
                    else if (item is SVGEllipse)
                    {
                        SVGEllipse ellipse = item as SVGEllipse;
                        EllipseGeometry el = new EllipseGeometry();
                        el.Center = ellipse.Center;
                        el.RadiusX = ellipse.RadiusX;
                        el.RadiusY = ellipse.RadiusY;
                        path.Data = el;
                    }
                    cmain.Children.Add(path);
                }
            }
        }

        private void SetControls()
        {
            bbz.IsChecked = true;
            bzj.IsChecked = false;
            CommonParam._isBuzhuo = true;
            CommonParam._isZhengjiao = false;
            this.bbz.Content = "捕捉：开";
            this.bzj.Content = "正交：关";
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
                if (_drawBase is DrawFont)
                {
                    DrawEditBox(p.X, p.Y, "", true);
                }
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
                if (CommonParam._isBuzhuo)
                {
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
            DrawText(e.GetPosition(this.cmain).X + 20, e.GetPosition(this.cmain).Y + 20, string.Format("(x:{0},y:{1})", (long)rp.X, (long)rp.Y), Colors.White);

            //this.btran.Content = string.Format("(x:{0},y:{1})", (long)_cap.GetTranslate().X, (long)_cap.GetTranslate().Y);
        }

        private void Reset_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            BarButtonItem bbi = sender as BarButtonItem;
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
        private void Clear_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (_drawBase != null)
            {
                if (_drawBase.Step != 0)
                {
                    _drawBase.Step = 0;
                }
            }
            while (cmain.Children.Count > CommonParam._defaultCount)
            {
                //永久保留默认的捕捉点和textblock、textbox
                cmain.Children.RemoveAt(CommonParam._defaultCount);
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
                    TextBlock tb = item as TextBlock;
                    if (tb.Tag != null && tb.Tag.ToString() == "sp_bz")
                    {
                        continue;
                    }
                    //continue;
                }
                if (item is UCTextBox)
                {
                    continue;
                }
                UIElement path = item as UIElement;

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
                            //if (path is TextBlock)
                            //{
                            //    scaleTemp.CenterX = e.GetPosition(this.cmain).X;
                            //    scaleTemp.CenterY = e.GetPosition(this.cmain).Y;
                            //}
                            //else
                            //{
                            scaleTemp.CenterX = scaleCenter.X;
                            scaleTemp.CenterY = scaleCenter.Y;
                            //}
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
        private void DrawText(double x, double y, string text, Color color)
        {
            if (textBlock == null)
            {
                textBlock = new TextBlock();
                textBlock.Tag = "sp_bz";
                cmain.Children.Add(textBlock);
            }
            textBlock.Text = text;

            textBlock.Foreground = new SolidColorBrush(color);

            Canvas.SetLeft(textBlock, x);

            Canvas.SetTop(textBlock, y);
        }

        UCTextBox textBox = null;
        private void DrawEditBox(double x, double y, string text, bool isShow)
        {
            if (textBox == null)
            {
                textBox = new UCTextBox();
                cmain.Children.Add(textBox);
                textBox.Edited = UCTextBox_Edited;
            }
            if (isShow)
            {
                textBox.Visibility = System.Windows.Visibility.Visible;
                textBox.Edit.Focus();
            }
            else
            {
                textBox.Visibility = System.Windows.Visibility.Collapsed;
            }
            textBox.Edit.Text = text;
            textBox.X = x;
            textBox.Y = y;

            Canvas.SetLeft(textBox, x + 10);

            Canvas.SetTop(textBox, y + 10);
        }

        public void UCTextBox_Edited(object sender, EventArgs e)
        {
            UCTextBox tcEdit = sender as UCTextBox;
            if (_drawBase is DrawFont)
            {
                DrawFont df = _drawBase as DrawFont;
                df.AddFont(tcEdit);
            }
            //TextBlock tbAdd = new TextBlock();
            //tbAdd.Text = tcEdit.Edit.Text;
            //tbAdd.Foreground = new SolidColorBrush(Colors.White);
            //Canvas.SetLeft(tbAdd, tcEdit.X);
            //Canvas.SetTop(tbAdd, tcEdit.Y);

            //tcEdit.Visibility = System.Windows.Visibility.Collapsed;
            //this.cmain.Children.Add(tbAdd);
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

        private void bbz_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (bbz.IsChecked == true)
            {
                this.bbz.Content = "捕捉：开";
                CommonParam._isBuzhuo = true;
            }
            else
            {
                this.bbz.Content = "捕捉：关";
                CommonParam._isBuzhuo = false;
            }
        }

        private void bzj_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (bzj.IsChecked == true)
            {
                this.bzj.Content = "正交：开";
                CommonParam._isZhengjiao = true;
            }
            else
            {
                this.bzj.Content = "正交：关";
                CommonParam._isZhengjiao = false;
            }
        }

        private void Copy_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            _drawBase = new DrawCopy(_cap);
            _drawBase.CMain = this.cmain;
            DefaultCursor();
            this.bstatus.Content = "当前工具: 拷贝";
        }

        private void Font_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            _drawBase = new DrawFont(_cap);
            _drawBase.CMain = this.cmain;
            DefaultCursor();
            this.bstatus.Content = "当前工具: 文字";
        }

        private void Group_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            _drawBase = new DrawGroup(_cap);
            _drawBase.CMain = this.cmain;
            DefaultCursor();
            this.bstatus.Content = "当前工具: 组合";
        }

        private void TestVideo_ItemClick(object sender, ItemClickEventArgs e)
        {
            string t = "{\"title\":\"测试数据\",\"url\":\"http://vod.htysx.com.cn/efdafe4389764a35ad19308aa6dff68c/0842c0709db341a8a661dd5700132628-7beceed664e50ff43206b1fdf6a73fbd-od-S00000001-200000.mp4\",\"link\":\"http://www.htyfw.com.cn/demand/toVodInfo?courseId=177\",\"type\":\"D\"}";
            string app = AppDomain.CurrentDomain.BaseDirectory + "video\\ZHCWVideo.exe";
            //System.Diagnostics.Process.Start(app, t);

            string argument1 = "\"" + t + "\"";
            Process process = new Process();
            process.StartInfo.FileName = app;
            process.StartInfo.Arguments = "测试数据 http://vod.htysx.com.cn/efdafe4389764a35ad19308aa6dff68c/0842c0709db341a8a661dd5700132628-7beceed664e50ff43206b1fdf6a73fbd-od-S00000001-200000.mp4 http://www.htyfw.com.cn/demand/toVodInfo?courseId=177 D";
            process.StartInfo.UseShellExecute = true;
            //启动  
            process.Start();
        }

        private void Open_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Title = "请选择svg文件";

            ofd.Filter = "svg文件(*.msvg)|*.msvg|svg文件(*.svg)|*.svg|所有文件(*.*)|*.*";
            if (ofd.ShowDialog() == true)
            {
                string file = ofd.FileName;
                this.Title = file;
                _filename = file;
            }
        }

        private void Save_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (string.IsNullOrEmpty(_filename))
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Title = " 保存";
                sfd.Filter = "svg文件(*.msvg,*.svg)|*.msvg;*.svg";
                if (sfd.ShowDialog() == true)
                {

                }
            }
        }

        private void New_ItemClick(object sender, ItemClickEventArgs e)
        {
            _filename = "";
        }
    }
}
