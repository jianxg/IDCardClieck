using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IDCardClieck.Common
{
    public class LogHelper
    {
        static string path = Application.StartupPath + "\\Log\\log" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";

        public static bool SaveTestLog = true;
        public static string SaveFilePath
        {
            get
            {
                return path;
            }
            set
            {
                path = value;
            }
        }
        public DateTime Date;
        public string Remark = string.Empty;
        public string Message = string.Empty;
        public string StackTrace = string.Empty;
        /// <summary>
        /// 日志文件类
        /// </summary>
        /// <param name="exc">错误提示</param>
        public LogHelper(Exception exc)
        {
            Date = DateTime.Now;
            Remark = string.Empty;
            if (exc != null)
            {
                if (!string.IsNullOrEmpty(exc.Message))
                {
                    Message = exc.Message;
                }
                if (!string.IsNullOrEmpty(exc.StackTrace))
                {
                    StackTrace = exc.StackTrace;
                }
            }
        }

        public LogHelper()
        {
            Date = DateTime.Now;

        }
        /// <summary>
        /// 写日志文件
        /// </summary>
        /// <param name="strMessage">保存的信息</param>
        /// <param name="strFilepath">包括文件路径</param>
        public static void WriteLogA(LogHelper LogMessage)
        {
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(SaveFilePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(SaveFilePath));
                }
                if (!System.IO.File.Exists(SaveFilePath))
                {
                    FileStream fsNew = new FileStream(SaveFilePath, FileMode.Create);
                    fsNew.Close();
                }
                FileStream fs = new FileStream(SaveFilePath, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine("Date: " + LogMessage.Date.ToString());
                if (!string.IsNullOrEmpty(LogMessage.Remark))
                {
                    sw.WriteLine("Remark: " + LogMessage.Remark);
                }
                if (!string.IsNullOrEmpty(LogMessage.Message))
                {
                    sw.WriteLine("Message: " + LogMessage.Message);
                }
                if (!string.IsNullOrEmpty(LogMessage.StackTrace))
                {
                    sw.WriteLine("StackTrace: " + LogMessage.StackTrace);
                }
                sw.WriteLine("");
                sw.Close();
                fs.Close();
            }
            catch
            {

            }
        }

        /// <summary>
        /// 写日志文件
        /// </summary>
        /// <param name="strMessage">保存的信息</param>
        /// <param name="strFilepath">包括文件路径</param>
        public static void WriteLine(string Message)
        {
            try
            {
                if (!SaveTestLog)
                {
                    return;
                }
                string path = System.Windows.Forms.Application.StartupPath + "\\Log\\Log" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
                if (!Directory.Exists(Path.GetDirectoryName(path)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                }
                if (!System.IO.File.Exists(path))
                {
                    FileStream fsNew = new FileStream(SaveFilePath, FileMode.Create);
                    fsNew.Close();
                }
                FileStream fs = new FileStream(path, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(Message + "  " + DateTime.Now.ToString());

                sw.Close();
                fs.Close();
            }
            catch (Exception exc)
            {
                WriteLogA(exc);
            }
        }

        /// <summary>
        /// 写日志文件
        /// </summary>
        /// <param name="Exception">保存的信息</param>
        public static void WriteLine(Exception exc)
        {
            try
            {

                string path = System.Windows.Forms.Application.StartupPath + "\\Log\\Log" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
                if (!Directory.Exists(Path.GetDirectoryName(path)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                }
                if (!System.IO.File.Exists(path))
                {
                    FileStream fsNew = new FileStream(SaveFilePath, FileMode.Create);
                    fsNew.Close();
                }
                FileStream fs = new FileStream(path, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine("Exception Message: " + exc.Message);
                sw.WriteLine("StackTrace: " + exc.StackTrace);
                sw.WriteLine("Date: " + DateTime.Now.ToString());
                sw.WriteLine("");
                sw.Close();
                fs.Close();
            }
            catch (Exception e)
            {
                WriteLogA(e);
            }
        }

        /// <summary>
        /// 写日志文件
        /// </summary>
        /// <param name="strMessage">保存的信息</param>
        /// <param name="strFilepath">包括文件路径</param>
        public static void WriteLogA(Exception exc)
        {
            try
            {
                if (!System.IO.File.Exists(SaveFilePath))
                {
                    FileStream fsNew = new FileStream(SaveFilePath, FileMode.Create);
                    fsNew.Close();
                }
                FileStream fs = new FileStream(SaveFilePath, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);

                sw.WriteLine("Exception Message: " + exc.Message);
                sw.WriteLine("StackTrace: " + exc.StackTrace);
                sw.WriteLine("Date: " + DateTime.Now.ToString());
                sw.WriteLine("");
                sw.Close();
                fs.Close();
            }
            catch
            {

            }
        }

        public static void WriteErrorTagA(string value)
        {
            try
            {
                if (!System.IO.File.Exists(SaveFilePath))
                {
                    FileStream fsNew = new FileStream(SaveFilePath, FileMode.Create);
                    fsNew.Close();
                }
                FileStream fs = new FileStream(SaveFilePath, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(value);
                sw.WriteLine("");
                sw.Close();
                fs.Close();
            }
            catch (Exception exc)
            {
                WriteLogA(exc);
            }
        }

        public static void WriteLine(string value, string path)
        {
            try
            {
                if (!SaveTestLog)
                {
                    return;
                }
                string path1 = System.Windows.Forms.Application.StartupPath + "\\Log\\" + path + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
                if (!Directory.Exists(Path.GetDirectoryName(path1)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path1));
                }
                if (!System.IO.File.Exists(path1))
                {
                    FileStream fsNew = new FileStream(path1, FileMode.Create);
                    fsNew.Close();
                }
                FileStream fs = new FileStream(path1, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(value + " " + "  " + DateTime.Now.ToString());
                sw.Close();
                fs.Close();
            }
            catch (Exception exc)
            {
                WriteLogA(exc);
            }
        }
        public static void WriteLine(string value, string path, bool isError)
        {
            try
            {
                if (!SaveTestLog && !isError)
                {
                    return;
                }
                string path1 = System.Windows.Forms.Application.StartupPath + "\\Log\\" + path + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
                if (!Directory.Exists(Path.GetDirectoryName(path1)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path1));
                }
                if (!System.IO.File.Exists(path1))
                {
                    FileStream fsNew = new FileStream(path1, FileMode.Create);
                    fsNew.Close();
                }
                FileStream fs = new FileStream(path1, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(value + " " + "  " + DateTime.Now.ToString());
                sw.Close();
                fs.Close();
            }
            catch (Exception exc)
            {
                WriteLogA(exc);
            }
        }

        public static void WriteLogA(string value, string path)
        {
            try
            {
                if (!System.IO.File.Exists(path))
                {
                    FileStream fsNew = new FileStream(path, FileMode.Create);
                    fsNew.Close();
                }
                FileStream fs = new FileStream(path, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine("Date: " + DateTime.Now.ToString() + "  ");
                sw.WriteLine(value);
                sw.Close();
                fs.Close();
            }
            catch
            {

            }
        }
    }
}
