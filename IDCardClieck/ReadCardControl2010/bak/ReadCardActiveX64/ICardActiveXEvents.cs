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
    [Guid("7C60EC87-93CD-49a0-9465-CA1520E87E15")]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IOCardActiveXEvents
    {
        [DispId(21)]
        void OnStateChanged(string state);
        [DispId(22)]
        void OnDataBind(string Name, string Gender, string Folk,
            string BirthDay, string Code, string Address,
            string Agency, string ExpireStart, string ExpireEnd, string ImageBase64String);
    }

}
