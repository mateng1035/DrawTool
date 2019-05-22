using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
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

        /// <summary>
        /// 深拷贝
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T DeepCopy<T>(T obj)
        {
            //如果是字符串或值类型则直接返回
            if (obj is string || obj.GetType().IsValueType) return obj;

            object retval = Activator.CreateInstance(obj.GetType());
            FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            foreach (FieldInfo field in fields)
            {
                try
                {
                    field.SetValue(retval, DeepCopy(field.GetValue(obj)));
                }
                catch
                {
                }
            }
            return (T)retval;
        }

        public static T Clone<T>(T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }

        /// <summary>
        /// 设置文件关联
        /// </summary>
        /// <param name="p_Filename">程序的名称</param>
        /// <param name="p_FileTypeName">扩展名 .VRD </param>
        public static void SaveReg(string p_Filename, string p_FileTypeName)
        {
            RegistryKey _RegKey = Registry.ClassesRoot.OpenSubKey("", true); //打开注册表

            RegistryKey _VRPkey = _RegKey.OpenSubKey(p_FileTypeName);
            if (_VRPkey != null) _RegKey.DeleteSubKey(p_FileTypeName, true);
            _RegKey.CreateSubKey(p_FileTypeName);
            _VRPkey = _RegKey.OpenSubKey(p_FileTypeName, true);
            _VRPkey.SetValue("", "Exec");

            _VRPkey = _RegKey.OpenSubKey("Exec", true);
            if (_VRPkey != null) _RegKey.DeleteSubKeyTree("Exec"); //如果等于空 就删除注册表DSKJIVR

            _RegKey.CreateSubKey("Exec");
            _VRPkey = _RegKey.OpenSubKey("Exec", true);
            _VRPkey.CreateSubKey("shell");
            _VRPkey = _VRPkey.OpenSubKey("shell", true); //写入必须路径
            _VRPkey.CreateSubKey("open");
            _VRPkey = _VRPkey.OpenSubKey("open", true);
            _VRPkey.CreateSubKey("command");
            _VRPkey = _VRPkey.OpenSubKey("command", true);
            string _PathString = "\"" + p_Filename + "\" \"%1\"";
            _VRPkey.SetValue("", _PathString); //写入数据
        }
    }
}
