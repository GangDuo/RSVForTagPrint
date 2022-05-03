using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RSVForTagPrint.Helpers
{
    class Process
    {
        public static void DestroyAll(string processName)
        {
            try
            {
                foreach (var proc in System.Diagnostics.Process.GetProcessesByName(processName))
                {
                    proc.Kill();
                    proc.WaitForExit();
                }
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                // The wait setting could not be accessed.
                Debug.WriteLine(ex.ToString());
            }
            //catch (System.NotSupportedException) 
            //{
            //    MessageBox.Show("NotSupportedException");
            //}
            //catch (System.InvalidOperationException) 
            //{
            //    MessageBox.Show("InvalidOperationException");
            //}
            //catch (System.NullReferenceException)
            //{
            //    MessageBox.Show("No instances of " + processName + " running.");
            //}
        }

        public static void OpenPrintPreviewWithIE(string code)
        {
            var args = String.Format(
                "/year:{0} /month:{1} /day:{2} /code:{3} /hostname:{4} /access-key-id:{5} /user-name:{6} /secret-access-key:{7} /password:{8}",
                DateTime.Today.Year,
                DateTime.Today.Month,
                DateTime.Today.Day,
                code,
                Environment.GetEnvironmentVariable("FMWW_HOST_NAME"),
                Environment.GetEnvironmentVariable("FMWW_ACCESS_KEY_ID"),
                Environment.GetEnvironmentVariable("FMWW_USER_NAME"),
                Environment.GetEnvironmentVariable("FMWW_SECRET_ACCESS_KEY"),
                Environment.GetEnvironmentVariable("FMWW_PASSWORD")
            );
            Console.WriteLine(String.Format("印刷ｺｰﾄﾞ = {0}", code));
            using (var hProcess = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
            {
                FileName = @"wscript",
                Arguments = String.Format(@"//B //Nologo fmww.jse {0}", args),
            }))
            {
                //hProcess.WaitForExit();
            }
        }
    }
}
