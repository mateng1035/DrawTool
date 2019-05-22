using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml;

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
            //args = new string[1] { AppDomain.CurrentDomain.BaseDirectory + "m.msvg" };
            string file = string.Empty;
            List<SVGBase> svgList = new List<SVGBase>();
            if (args.Length > 0)
            {
                file = args[0];

                //string str = File.ReadAllText(file);

                XmlDocument doc = new XmlDocument();
                doc.Load(file);
                foreach (XmlNode item in doc.ChildNodes)
                {
                    if (item.Name.ToLower() == "svg")
                    {
                        foreach (XmlNode svg in item.ChildNodes)
                        {
                            switch (svg.Name)
                            {
                                case "rect":
                                    SVGRectangle rect = new SVGRectangle();
                                    rect.XmlInnerText = svg.InnerXml;
                                    Point p1 = new Point();
                                    if (svg.Attributes["x"] != null)
                                    {
                                        p1.X = double.Parse(svg.Attributes["x"].InnerText);
                                    }
                                    else
                                    {
                                        p1.X = 0;
                                    }
                                    if (svg.Attributes["y"] != null)
                                    {
                                        p1.Y = double.Parse(svg.Attributes["y"].InnerText);
                                    }
                                    else
                                    {
                                        p1.Y = 0;
                                    }
                                    Point p2 = new Point();
                                    if (svg.Attributes["width"] != null)
                                    {
                                        p2.X = p1.X + double.Parse(svg.Attributes["width"].InnerText);
                                    }
                                    else
                                    {
                                        p2.X = p1.X;
                                    }
                                    if (svg.Attributes["height"] != null)
                                    {
                                        p2.Y = p1.Y + double.Parse(svg.Attributes["height"].InnerText);
                                    }
                                    else
                                    {
                                        p2.Y = p1.Y;
                                    }
                                    rect.Point1 = p1;
                                    rect.Point2 = p2;
                                    svgList.Add(rect);
                                    break;
                                case "line":
                                    Point sp = new Point();
                                    sp.X = double.Parse(svg.Attributes["x1"].InnerText);
                                    sp.Y = double.Parse(svg.Attributes["y1"].InnerText);
                                    Point ep = new Point();
                                    ep.X = double.Parse(svg.Attributes["x2"].InnerText);
                                    ep.Y = double.Parse(svg.Attributes["y2"].InnerText);
                                    SVGLine line = new SVGLine();
                                    line.XmlInnerText = svg.InnerXml;
                                    line.StartPoint = sp;
                                    line.EndPoint = ep;
                                    svgList.Add(line);
                                    break;
                                case "circle":
                                    Point cp = new Point();
                                    cp.X = double.Parse(svg.Attributes["cx"].InnerText);
                                    cp.Y = double.Parse(svg.Attributes["cy"].InnerText);
                                    SVGEllipse ellipse = new SVGEllipse();
                                    ellipse.XmlInnerText = svg.InnerXml;
                                    ellipse.Center = cp;
                                    ellipse.RadiusX = double.Parse(svg.Attributes["r"].InnerText);
                                    ellipse.RadiusY = double.Parse(svg.Attributes["r"].InnerText);
                                    svgList.Add(ellipse);
                                    break;
                                case "ellipse":
                                    Point cp1 = new Point();
                                    cp1.X = double.Parse(svg.Attributes["cx"].InnerText);
                                    cp1.Y = double.Parse(svg.Attributes["cy"].InnerText);
                                    SVGEllipse ellipse1 = new SVGEllipse();
                                    ellipse1.XmlInnerText = svg.InnerXml;
                                    ellipse1.Center = cp1;
                                    ellipse1.RadiusX = double.Parse(svg.Attributes["rx"].InnerText);
                                    ellipse1.RadiusY = double.Parse(svg.Attributes["ry"].InnerText);
                                    svgList.Add(ellipse1);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
            WpfApplication11.App app = new WpfApplication11.App();
            AddSource(app, "Resource/MyButton.xaml");

            PaintWindow windows = new PaintWindow(file, svgList);
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
