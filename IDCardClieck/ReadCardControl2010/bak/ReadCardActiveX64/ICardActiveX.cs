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
    [Guid("58A384B1-5B04-4b24-B116-6140C9075599")]
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
        void SetFunc(object win, string func);
        [DispId(6)]
        string GetCurrentCardInfo();

    }
}
