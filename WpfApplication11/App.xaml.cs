using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace WpfApplication11
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        //public App()
        //{
        //    this.Startup += App_Startup;
        //}

        //void App_Startup(object sender, StartupEventArgs e)
        //{
        //    //string path = AppDomain.CurrentDomain.BaseDirectory + "WpfApplication11.exe";
        //    //CommonFun.SaveReg(path, ".msvg");
        //}


        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            string file = string.Empty;
            if (args.Length > 0)
            {
                file = args[0];
            }
            WpfApplication11.App app = new WpfApplication11.App();
            AddSource(app, "Resource/MyButton.xaml");

            PaintWindow windows = new PaintWindow(file);
            app.Run(windows);
        }
        private static void AddSource(WpfApplication11.App app, string key)
        {
            ResourceDictionary languageResDic = new ResourceDictionary();
            languageResDic.Source = new Uri(key, UriKind.RelativeOrAbsolute);
            app.Resources.MergedDictionaries.Add(languageResDic);
        }
    }
}
