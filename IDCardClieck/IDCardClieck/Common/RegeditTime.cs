using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace IDCardClieck.Common
{
    /// <summary>
    /// 注册表绑定类
    /// </summary>
    public class RegeditTime
    {


        private static string pwd = "tongruan1688";
        public static int InitRegedit(ref string sericalNumber, ref string registerCode, string path, string key)
        {
            /*检查注册表*/
            string RegisterCode = ReadSetting(path, "registerCode", "-1");    // 读取注册表， 检查是否注册 -1为未注册
            registerCode = RegisterCode;
            if (registerCode == "-1")
            {
                return 1;
            }

            RegisterCode = Encryption.DisEncryPW(RegisterCode, RegeditTime.pwd);

            //激活码
            string SericalNumber = RegeditTime.GetSoftEndDateAllCpuId(2, RegisterCode);
            sericalNumber = SericalNumber;

            /* 比较CPUid */
            string CpuId = GetSoftEndDateAllCpuId(1, RegisterCode);   //从注册表读取CPUid
            string CpuIdThis = GetCpuId();//获取本机CPUId         
            if (CpuId != CpuIdThis)
            {
                return 2;
            }

            /* 比较时间 */
            //string NowDate = RegeditTime.GetNowDate();
            //string EndDate = RegeditTime.GetSoftEndDateAllCpuId(0, RegisterCode);
            //if (Convert.ToInt32(EndDate) - Convert.ToInt32(NowDate) < 0)
            //{
            //    return 3;
            //}
            return 0;
        }


        /*CPUid*/
        public static string GetCpuId()
        {
            ManagementClass mc = new ManagementClass("Win32_Processor");
            ManagementObjectCollection moc = mc.GetInstances();

            string strCpuID = null;
            foreach (ManagementObject mo in moc)
            {
                strCpuID = mo.Properties["ProcessorId"].Value.ToString();
                break;
            }
            return strCpuID;
        }

        /*当前时间*/
        public static string GetNowDate()
        {
            string NowDate = DateTime.Now.ToString("yyyyMMddhhmmss"); //.Year + DateTime.Now.Month + DateTime.Now.Day).ToString();

            //     DateTime date = Convert.ToDateTime(NowDate, "yyyy/MM/dd");
            return NowDate;
        }

        /// <summary>
        /// 生成序列号
        /// </summary>
        /// <returns></returns>
        public static string CreatSerialNumber(string code, string date)
        {
            string SerialNumber = GetCpuId() + "-" + code + "-" + date;

            SerialNumber = Encryption.EncryPW(SerialNumber, RegeditTime.pwd);

            return SerialNumber;
        }

        /* 
         * i=1 得到 CUP 的id 
         * i=0 得到上次或者 开始时间 
         */
        public static string GetSoftEndDateAllCpuId(int i, string SerialNumber)
        {
            if (i == 1)
            {
                string cupId = SerialNumber.Split('-')[0]; // .LastIndexOf("-"));

                return cupId;
            }
            else if (i == 0)
            {
                string dateTime = SerialNumber.Split('-')[2];
                //  dateTime = dateTime.Insert(4, "/").Insert(7, "/");
                //  DateTime date = Convert.ToDateTime(dateTime);

                return dateTime;
            }
            else if (i == 2)
            {
                string cdKey = SerialNumber.Split('-')[1];
                return cdKey;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 写入注册表
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="Key"></param>
        /// <param name="Setting"></param>
        public static void WriteSetting(string Section, string Key, string Setting)  // name = key  value=setting  Section= path
        {
            //RegistryKey key1 = Registry.CurrentUser.CreateSubKey("Software\\My_ChildPlat\\ChildPlat"); // .LocalMachine.CreateSubKey("Software\\mytest");
            RegistryKey key1 = Registry.LocalMachine.CreateSubKey(Section);
            if (key1 == null)
            {
                return;
            }
            try
            {
                key1.SetValue(Key, Setting);
            }
            catch (Exception exception1)
            {
                return;
            }
            finally
            {
                key1.Close();
            }

        }

        /// <summary>
        /// 读取注册表
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="Key"></param>
        /// <param name="Default"></param>
        /// <returns></returns>
        public static string ReadSetting(string Section, string Key, string Default)
        {
            if (Default == null)
            {
                Default = "-1";
            }
            string text2 = Section;
            RegistryKey key1 = Registry.LocalMachine.OpenSubKey(Section);
            if (key1 != null)
            {
                object obj1 = key1.GetValue(Key, Default);
                key1.Close();
                if (obj1 != null)
                {
                    if (!(obj1 is string))
                    {
                        return "-1";
                    }
                    return obj1.ToString(); ;
                }
                return "-1";
            }


            return Default;
        }
    }
}
