using HZH_Controls;
using IDCardClieck.Common;
using IDCardClieck.Forms;
using IDCardClieck.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IDCardClieck
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            Application.EnableVisualStyles();

            Application.SetCompatibleTextRenderingDefault(false);

            RegisterFrm registerFrm = null;
            SimpleLoading loadingfrm = new SimpleLoading(registerFrm);
            //将Loaing窗口，注入到 SplashScreenManager 来管理
            SplashScreenManager loading = new SplashScreenManager(loadingfrm);

            //判断是否已经有实例在运行
            Process instance = RunningInstance();
            if (instance == null) //没有实例在运行
            {
                loading.ShowLoading();
                //try catch 包起来，防止出错
                try
                {
                    CheckoutModel checkoutModel = new CheckoutModel();
                    string a = string.Empty, b = string.Empty;
                    checkoutModel.res = RegeditTime.InitRegedit(ref a, ref b, checkoutModel.path, checkoutModel.registerCodeName);

                    checkoutModel.sericalNumber = a;
                    checkoutModel.registerCode = b;
                    
                    LogHelper.WriteLine("程序主入口: 注册结果:" + checkoutModel.res + ",激活码:" + checkoutModel.sericalNumber + ",注册码:" +
                        "" + checkoutModel.registerCode + "");

                    loading.CloseWaitForm();

                    registerFrm = new RegisterFrm(checkoutModel);
                    registerFrm.ShowDialog();//显示注册激活窗体

                    if (registerFrm.DialogResult == DialogResult.OK)
                    {
                        Application.Run(new HomeForm(checkoutModel, registerFrm.json));
                    }
                }
                catch (Exception e)
                {
                    loading.CloseWaitForm();
                    /*可选处理异常*/
                    LogHelper.WriteLine("程序主入口:" + e.Message.ToString());
                }
            }
            else //已经有一个实例在运行
            {
                //显示窗口
                ShowWindowAsync(instance.MainWindowHandle, 1);
                //窗口显示在最前端
                SetForegroundWindow(instance.MainWindowHandle);
            }
        }

        /// <summary>
        /// 在进程中查找是否已经有实例在运行
        /// </summary>
        /// <returns></returns>
        private static Process RunningInstance()
        {
            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(current.ProcessName);
            //遍历与当前进程名称相同的进程列表
            foreach (Process process in processes)
            {
                //如果实例已经存在则忽略当前进程
                if (process.Id != current.Id)
                {
                    //保证要打开的进程同已经存在的进程来自同一文件路径
                    if (Assembly.GetExecutingAssembly().Location.Replace("/", @"\") == current.MainModule.FileName)
                    {
                        //返回已经存在的进程
                        return process;
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// 调用API函数，正常显示窗口
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="cmdShow"></param>
        /// <returns></returns>
        [DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);
        /// <summary>
        /// 将窗口放置最前端
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.ExceptionObject.ToString());
        }

    }
}
