using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace IDCardClieck.Common
{
    public class ConfigHelper
    {
        /* ---http://www.ibeifeng.com/tech.php?id=55401
        ---一步一步教你玩转.NET Framework的配置文件app.config*/
        #region 初始化及加密实现
        private static string pwd = "www.gs-softwares.com";

        private static string configname = "param.gsc";

        /// <summary>
        /// 将配置文件sourceFile加密转换成param.gsc文件内容
        /// </summary>
        static ConfigHelper()
        {
            //sourceFile="D:\\Solution1118\\Solution1\\ServiceApplication\\bin\\Debug\\ServiceApplication.vshost.exe.Config"
            //destFile="D:\\Solution1118\\Solution1\\ServiceApplication\\bin\\Debug\\param.gsc";
            string sourceFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            string destFile = Path.Combine(Application.StartupPath, configname);
            if (File.Exists(sourceFile) && File.Exists(destFile))
            {
                FileInfo sf = new FileInfo(sourceFile);
                FileInfo df = new FileInfo(destFile);

                //获取上次写入文档或目录的时间
                if (sf.LastWriteTime > df.LastWriteTime)
                {
                    string c = File.ReadAllText(sourceFile);
                    string jmwFCf = Encode(c, pwd);
                    File.Delete(destFile);
                    StreamWriter sw = File.CreateText(destFile);
                    sw.Write(jmwFCf);
                    sw.Close();
                    sw.Dispose();
                }
            }
            else if (File.Exists(sourceFile) && !File.Exists(destFile))
            {
                string c = File.ReadAllText(sourceFile);
                string jmwFCf = Encode(c, pwd);
                StreamWriter sw = File.CreateText(destFile);
                sw.Write(jmwFCf);
                sw.Close();
                sw.Dispose();
            }

        }
        /// <summary>
        /// 将param.gsc文件进行解码，加载成xml文件
        /// </summary>
        /// <returns></returns>
        private static XmlDocument GetDocument()
        {
            string path = System.Windows.Forms.Application.StartupPath;//"D:\\Solution1118\\Solution1\\ServiceApplication\\bin\\Debug"
            //configname="param.gsc";
            string textstring = File.ReadAllText(Path.Combine(System.Windows.Forms.Application.StartupPath, configname), System.Text.Encoding.UTF8);
            string xmlstring = Decode(textstring, pwd);
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(xmlstring);
            return xml;
        }

        /// <summary>
        /// 删除配置文件sourceFile
        /// </summary>
        public static void DelConfig()
        {
            string sourceFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            string destFile = Path.Combine(Application.StartupPath, configname);
            if (File.Exists(sourceFile) && !File.Exists(destFile))
            {
                string c = File.ReadAllText(sourceFile);
                string jmwFCf = Encode(c, pwd);
                StreamWriter sw = File.CreateText(destFile);
                sw.Write(jmwFCf);
                sw.Close();
                sw.Dispose();
            }

            File.Delete(sourceFile);
        }


        /// <summary>
        /// 读取系统配置文件param.gsc中params组信息
        /// </summary>
        /// <param name="sectionname">段名</param>
        /// <param name="keyname">key名</param>
        /// <returns></returns>
        public static string getConfigValue(string sectionname, string keyname)
        {
            try
            {
                XmlDocument xDoc = GetDocument();
                XmlNode xNode;
                XmlElement xElem;
                xNode = xDoc.SelectSingleNode("//params/" + sectionname);
                xElem = (XmlElement)xNode.SelectSingleNode("//add[@key='" + keyname + "']");
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
        #endregion

        #region 加密解密
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Encode(string str, string key)
        {
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            provider.Key = Encoding.ASCII.GetBytes(key.Substring(0, 8));
            provider.IV = Encoding.ASCII.GetBytes(key.Substring(0, 8));
            byte[] bytes = Encoding.GetEncoding("GB2312").GetBytes(str);
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, provider.CreateEncryptor(), CryptoStreamMode.Write);
            stream2.Write(bytes, 0, bytes.Length);
            stream2.FlushFinalBlock();
            StringBuilder builder = new StringBuilder();
            foreach (byte num in stream.ToArray())
            {
                builder.AppendFormat("{0:X2}", num);
            }
            stream.Close();
            return builder.ToString();
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Decode(string str, string key)
        {
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            provider.Key = Encoding.ASCII.GetBytes(key.Substring(0, 8));
            provider.IV = Encoding.ASCII.GetBytes(key.Substring(0, 8));
            byte[] buffer = new byte[str.Length / 2];
            for (int i = 0; i < (str.Length / 2); i++)
            {
                int num2 = Convert.ToInt32(str.Substring(i * 2, 2), 0x10);
                buffer[i] = (byte)num2;
            }
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(), CryptoStreamMode.Write);
            stream2.Write(buffer, 0, buffer.Length);
            stream2.FlushFinalBlock();
            stream.Close();
            return Encoding.GetEncoding("GB2312").GetString(stream.ToArray());
        }
        #endregion

        #region 判断系统文件hosts中是否存在222.91.161.253 www.shaanxicertprint.com

        public static void SetSystemHosts(string addString)
        {
            string hasAdress = Environment.SystemDirectory + "\\drivers\\etc\\hosts";
            //string addString = "222.91.161.253 www.shaanxicertprint.com";
            if (!File.Exists(hasAdress)) //判断该文件是否存在于系统文件中
            {
                StreamWriter sw = new StreamWriter(hasAdress, false, Encoding.UTF8);
                sw.WriteLine();
                sw.WriteLine(addString);
                sw.Close();
                sw.Dispose();
            }
            System.IO.StreamReader sr = new System.IO.StreamReader(hasAdress);
            bool isExist = false;
            while (!sr.EndOfStream) //判断是否需要向文件中添加内容
            {
                string ss = sr.ReadLine();
                if (ss.IndexOf("#") == 0) continue;
                if (ss == addString) //说明该文件中存在要添加的内容
                {
                    isExist = true;
                    break;
                }
            }
            sr.Close();
            sr.Dispose();
            if (!isExist)// 该文件中不存在要添加的内容
            {
                System.IO.StreamWriter sw = new System.IO.StreamWriter(hasAdress, true);
                sw.WriteLine();
                sw.WriteLine(addString);
                sw.Close();
                sw.Dispose();
            }
        }
        #endregion
    }
}
