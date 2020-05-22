using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Timers;
using System.Threading;
using System.IO;

namespace GSFramework
{
    [ComVisible(true)]
    [Guid("E137671B-DCF3-4E05-ABD2-19882D617936")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IOCardActiveX
    {
        [DispId(1)]
        void Start();
        [DispId(2)]
        void Stop();
        [DispId(3)]
        void Clear();
        [DispId(4)]
        void SetMessage(string message);
        [DispId(5)]
        void SetFunc(object win, string databindfunc,string infofunc);
        [DispId(6)]
        string GetCurrentCardInfo();
        [DispId(7)]
        string GetStatus();

        [DispId(8)]
        void Start1(string code);
        [DispId(9)]
        string GetSAMID();

    }
}
