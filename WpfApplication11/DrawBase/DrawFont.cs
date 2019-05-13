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
    public class DrawFont : DrawBase
    {
        public DrawFont(MyCapture cap)
            : base(cap)
        {
            MyCapture = cap;
        }
        public override void MouseLeftButtonDown(object sender, Point point)
        {
            Label block = new Label();

            SetTranslate(block);

            block.Foreground = new SolidColorBrush(Colors.White);

            Canvas.SetLeft(block, point.X);

            Canvas.SetTop(block, point.Y);

            CMain.Children.Add(block);
        }

        public void AddFont(UCTextBox tcEdit)
        {
            TextBlock tbAdd = new TextBlock();
            tbAdd.Text = tcEdit.Edit.Text;
            tbAdd.Foreground = new SolidColorBrush(Colors.White);
            Canvas.SetLeft(tbAdd, tcEdit.X);
            Canvas.SetTop(tbAdd, tcEdit.Y);

            tcEdit.Visibility = System.Windows.Visibility.Collapsed;

            SetTranslate(tbAdd);

            CMain.Children.Add(tbAdd);
        }
    }
}
