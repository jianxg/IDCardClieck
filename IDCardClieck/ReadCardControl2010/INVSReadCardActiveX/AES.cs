using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace GSFramework
{
    public class EncryptClass
    {
        //http://blog.csdn.net/hhay7758/article/details/8164836

        //private byte[] key = new byte[]{119,46,103,115,45,115,111,102,116,119,97,114,101,115,46,99};

        [DllImport("GSDll.dll", CharSet = CharSet.Ansi)]
        private static extern void getData(StringBuilder returnData, string code, string id, string name, string ncode, string scode, string ecode);

        [DllImport("GSDll.dll", CharSet = CharSet.Ansi)]
        private static extern void getCode(StringBuilder returnCode, string code, string id, string name, string ncode, string scode, string ecode);

        /// <summary>
        /// 加密 
        /// </summary>
        /// <param name="toEncrypt">加密字符</param>
        /// <param name="key">加密的密码</param>
        /// <returns></returns>
        public string Encrypt(string code,string id, string name, string ncode, string scode, string ecode,out string errString)
        {
            int rGotoCount = 0;
            Rgoto:
            errString = string.Empty;
            try
            {
                if (code.Trim().Length != 7)
                {
                    errString = "请通过系统正常调用！";
                    return "";
                }
                //int sum = test1(1, 2, 3);
                //sum = test2(1, 2);
                StringBuilder sbData = new StringBuilder();
                getData(sbData, code, id, name, ncode, scode, ecode);
                string sb = sbData.ToString();
                
                if (sb.Length > 16)
                {
                    sb = sb.Substring(0, 16);
                }
                if (sb.Length < 16)
                {
                    errString = "获取加密信息失败，无法加密！";
                    return "";
                }
                
                byte[] keyArray = UTF8Encoding.UTF8.GetBytes(sb.Trim());
                //byte[] keyArray = UTF8Encoding.UTF8.GetBytes(Encoding.ASCII.GetString(key));

                StringBuilder sbCode = new StringBuilder();
                getCode(sbCode,code, id, name, ncode, scode, ecode);
                string sCode = sbCode.ToString();
                if (sCode.Length > 18)
                {
                    sCode = sCode.Substring(0, 18);
                }
                if (sCode.Length < 18)
                {
                    errString = "获取加密信息失败，无法加密！！";
                    return "";
                }
                byte[] EncryptArray = UTF8Encoding.UTF8.GetBytes(sCode.Trim());

                RijndaelManaged rDel = new RijndaelManaged();
                rDel.Key = keyArray;
                rDel.Mode = CipherMode.ECB;
                rDel.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = rDel.CreateEncryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(EncryptArray, 0, EncryptArray.Length);

                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
            catch(Exception exc)
            {
                if (exc.Message == "尝试读取或写入受保护的内存。这通常指示其他内存已损坏。" && rGotoCount==0)
                {
                    rGotoCount++;
                    goto Rgoto;
                }
                errString = exc.Message;
                return "";
            }
        }

        ///// <summary>
        ///// 解密
        ///// </summary>
        ///// <param name="text"></param>
        ///// <param name="password"></param>
        ///// <param name="iv"></param>
        ///// <returns></returns>
        //public string Decrypt(string toDecrypt)
        //{
        //    byte[] keyArray = UTF8Encoding.UTF8.GetBytes(retstring(id, name, ncode, scode, ecode));
        //    byte[] keyArray = UTF8Encoding.UTF8.GetBytes(Encoding.ASCII.GetString(key));
        //    byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);

        //    RijndaelManaged rDel = new RijndaelManaged();
        //    rDel.Key = keyArray;
        //    rDel.Mode = CipherMode.ECB;
        //    rDel.Padding = PaddingMode.PKCS7;

        //    ICryptoTransform cTransform = rDel.CreateDecryptor();
        //    byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

        //    return UTF8Encoding.UTF8.GetString(resultArray);
        //}

    }
}
