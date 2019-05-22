using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication11
{
    public class SVGBase
    {
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

    }
}
