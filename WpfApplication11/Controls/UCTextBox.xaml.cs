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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApplication11
{
    /// <summary>
    /// UCTextBox.xaml 的交互逻辑
    /// </summary>
    public partial class UCTextBox : UserControl
    {
        public UCTextBox()
        {
            InitializeComponent();
        }

        public double X { get; set; }
        public double Y { get; set; }

        public EventHandler Edited;

        private void Edited_Click(object sender, RoutedEventArgs e)
        {
            Edited(this, e);
        }
    }

    //public class UCTextBoxEventArgs : RoutedEventArgs
    //{
    //    public double X { get; set; }
    //    public double Y { get; set; }
    //}
}
