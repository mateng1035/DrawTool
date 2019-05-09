using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.IO;
using System.Windows.Shapes;
using Newtonsoft.Json;
using System.Drawing;
using System.ComponentModel;

namespace WpfApplication11
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BackgroundWorker bkWorker = new BackgroundWorker();
            bkWorker.WorkerReportsProgress = true;
            bkWorker.WorkerSupportsCancellation = true;
            bkWorker.DoWork += new DoWorkEventHandler(DoWork);
            bkWorker.ProgressChanged += new ProgressChangedEventHandler(ProgessChanged);
            bkWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(CompleteWork);
            bkWorker.RunWorkerAsync();

        }
        public void ProgessChanged(object sender, ProgressChangedEventArgs e)
        {
            //    value = e.ProgressPercentage;
            this.lblinfo.Content = e.UserState;
        }

        public void DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                string temp = AccessToken.getAccessToken();
                tokenmodel tm = JsonConvert.DeserializeObject<tokenmodel>(temp);
                DirectoryInfo dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "in");
                foreach (FileInfo file in dir.GetFiles())
                {
                    temp = ImageQualityEnhance.imageQualityEnhance(file.FullName, tm.access_token);
                    imagemodel im = JsonConvert.DeserializeObject<imagemodel>(temp);
                    if (im != null && im.image != null)
                    {
                        string img = im.image;
                        Bitmap bmp = GetImageFromBase64(img);
                        bmp.Save(AppDomain.CurrentDomain.BaseDirectory + "out\\" + file.Name);

                        (sender as BackgroundWorker).ReportProgress(0, "转换成功，输出文件" + file.Name);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public void CompleteWork(object sender, RunWorkerCompletedEventArgs e)
        {
        }

        public Bitmap GetImageFromBase64(string base64string)
        {
            byte[] b = Convert.FromBase64String(base64string);
            MemoryStream ms = new MemoryStream(b);
            Bitmap bitmap = new Bitmap(ms);
            return bitmap;
        }
    }
}
