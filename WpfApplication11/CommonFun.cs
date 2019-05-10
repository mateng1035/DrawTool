using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApplication11
{
    public class CommonFun
    {
        public static double CorrectDoubleValue(double Value)
        {
            return double.IsNaN(Value) ? 0 : Value;
        }

        public static bool IsPointIn(Rect rect, Point pt)
        {
            if (pt.X >= rect.X && pt.Y >= rect.Y && pt.X <= rect.X + rect.Width && pt.Y <= rect.Y + rect.Height)
            {
                return true;
            }
            else return false;
        }
    }
}
