using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication11
{
    public class CommonFun
    {
        public static double CorrectDoubleValue(double Value)
        {
            return double.IsNaN(Value) ? 0 : Value;
        }
    }
}
