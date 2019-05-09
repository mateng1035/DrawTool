using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfApplication11
{
    public class TranslateEvent
    {
        bool isMove = false;
        int moveStep = 0;
        Point pm1 = new Point();

        public virtual void MouseLeftButtonDown(object sender, Point point)
        {
            if (isMove)
            {
                pm1 = point;
                moveStep = 1;
            }
        }

        public virtual void MouseMove(object sender, Point point, MyCapture cap, double scale)
        {
            if (isMove && moveStep == 1)
            {
                Point pm2 = point;
                Canvas cmain = sender as Canvas;

                //Console.WriteLine(string.Format("p1(x:{0},y:{1})", pm1.X, pm1.Y));
                //Console.WriteLine(string.Format("p2(x:{0},y:{1})", pm2.X, pm2.Y));
                double mx = (pm2.X - pm1.X) / scale;
                double my = (pm2.Y - pm1.Y) / scale;

                TranslateTransform tempTranslate = cap.SetTranslate(mx, my);
                //Console.WriteLine(string.Format("t(x:{0},y:{1})", tempTranslate.X, tempTranslate.Y));
                //btext.Content = string.Format("{0},{1}", tempTranslate.X, tempTranslate.Y);

                foreach (var item in cmain.Children)
                {
                    if (item is TextBlock)
                    {
                        continue;
                    }
                    Path path = item as Path;

                    TransformGroup tfGroup = path.RenderTransform as TransformGroup;
                    if (tfGroup == null)
                    {
                        TranslateTransform pathTranslate = new TranslateTransform();
                        pathTranslate.X = tempTranslate.X;
                        pathTranslate.Y = tempTranslate.Y;
                        tfGroup = new TransformGroup();
                        tfGroup.Children.Add(pathTranslate);
                        path.RenderTransform = tfGroup;
                    }
                    else
                    {
                        TranslateTransform pathTranslate = null;
                        foreach (var tran in tfGroup.Children)
                        {
                            pathTranslate = tran as TranslateTransform;
                            if (pathTranslate != null)
                            {
                                pathTranslate.X = tempTranslate.X;
                                pathTranslate.Y = tempTranslate.Y;
                                break;
                            }
                        }
                        if (pathTranslate == null)
                        {
                            pathTranslate = new TranslateTransform();
                            pathTranslate.X = tempTranslate.X;
                            pathTranslate.Y = tempTranslate.Y;
                            tfGroup.Children.Add(pathTranslate);
                        }
                        //TranslateTransform pathTranslate = tfGroup.Children[0] as TranslateTransform;
                    }
                }
                pm1 = pm2;
            }
        }


        public virtual void MouseLeftButtonUp(object sender, Point point)
        {
            if (isMove)
            {
                moveStep = 2;
            }
        }
        public virtual void Start()
        {
            isMove = true;
        }
        public virtual void Reset()
        {
            isMove = false;
            moveStep = 0;
        }


    }
}
