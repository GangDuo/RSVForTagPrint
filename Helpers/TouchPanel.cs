using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSVForTagPrint.Helpers
{
    class TouchPanel
    {
        private static readonly string TabTip = @"C:\Program Files\Common Files\microsoft shared\ink\TabTip.exe";

        public static void StartOnScreenKeyboard()
        {
            System.Diagnostics.Process.Start(TabTip);
        }

        public static bool CanStartOnScreenKeyboard()
        {
            return System.IO.File.Exists(TabTip);
        }
    }
}
