using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml;

namespace WpfApplication11
{
    public class SVGBase
    {
        private string _xmlInnerText;
        /// <summary>
        /// xml原内容
        /// </summary>
        public string XmlInnerText
        {
            get { return _xmlInnerText; }
            set { _xmlInnerText = value; }
        }

        private string _style;
        /// <summary>
        /// 样式
        /// </summary>
        public string Style
        {
            get { return _style; }
            set { _style = value; }
        }

        private Dictionary<string, string> _attrs;
        /// <summary>
        /// 附加属性集合-特殊 
        /// </summary>
        public Dictionary<string, string> Attrs
        {
            get { return _attrs; }
            set { _attrs = value; }
        }
        /// <summary>
        /// 获取svg字符串内容
        /// </summary>
        /// <returns></returns>
        public abstract string GetSVGToString();
        /// <summary>
        /// 获取svg xml内容
        /// </summary>
        /// <returns></returns>
        public abstract XmlNode GetSVGToXml();
        /// <summary>
        /// 获取Geometry
        /// </summary>
        /// <returns></returns>
        public abstract Geometry GetGeometry(XmlNode node);
    }
}
