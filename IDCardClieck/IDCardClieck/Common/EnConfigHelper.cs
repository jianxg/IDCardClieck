using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace IDCardClieck.Common
{
    public class EnConfigHelper
    {
        private static string sourceFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;


        /// <summary>
        /// 读取系统配置文件param.gsc中params组信息
        /// </summary>
        /// <param name="sectionname">段名</param>
        /// <param name="keyname">key名</param>
        /// <returns></returns>
        public static string GetConfigValue(string sectionname, string keyname)
        {
            try
            {
                XmlDocument xDoc = GetDocument();
                XmlElement xElem;
                xElem = (XmlElement)xDoc.SelectSingleNode("//params/" + sectionname + "/add[@key='" + keyname + "']");
                if (xElem != null)
                    return xElem.GetAttribute("value");
                else
                    return "";
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>
        /// 读取系统配置文件param.gsc中params组信息
        /// </summary>
        /// <param name="sectionname">段名</param>
        /// <param name="keyname">key名</param>
        /// <returns></returns>
        public static string GetConfigValue(string sectionname, string keyname, string attributeName)
        {
            try
            {
                XmlDocument xDoc = GetDocument();
                XmlElement xElem;
                xElem = (XmlElement)xDoc.SelectSingleNode("//params/" + sectionname + "/add[@key='" + keyname + "']");
                if (xElem != null)
                    return xElem.GetAttribute(attributeName);
                else
                    return "";
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static bool SetConfigValue(string sectionname, string keyname, string newValue)
        {
            try
            {
                XmlDocument xDoc = GetDocument();
                XmlElement xElem;
                xElem = (XmlElement)xDoc.SelectSingleNode("//params/" + sectionname + "/add[@key='" + keyname + "']");
                if (xElem != null)
                {
                    xElem.SetAttribute("value", newValue);
                    xDoc.Save(sourceFile);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static bool SetConfigValue(string sectionname, string keyname, string attributeName, string newValue)
        {
            try
            {
                XmlDocument xDoc = GetDocument();
                XmlNode xNode;
                XmlElement xElem;
                //xNode = xDoc.SelectSingleNode();
                xElem = (XmlElement)xDoc.SelectSingleNode("//params/" + sectionname + "/add[@key='" + keyname + "']");
                if (xElem != null)
                {
                    xElem.SetAttribute(attributeName, newValue);
                    xDoc.Save(sourceFile);


                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static XmlDocument GetDocument()
        {
            string textstring = File.ReadAllText(sourceFile, System.Text.Encoding.UTF8);
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(textstring);
            return xml;
        }
    }
}
