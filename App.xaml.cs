using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace RSVForTagPrint
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// アプリケーション開始時のイベントハンドラ
        /// </summary>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            this.DispatcherUnhandledException += Application_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        /// <summary>
        /// アプリケーション終了時のイベントハンドラ
        /// </summary>
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            this.DispatcherUnhandledException -= Application_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException -= CurrentDomain_UnhandledException;
        }

        /// <summary>
        /// WPF UIスレッドでの未処理例外スロー時のイベントハンドラ
        /// </summary>
        private void Application_DispatcherUnhandledException(
            object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            this.ReportUnhandledException(e.Exception);
        }

        /// <summary>
        /// UIスレッド以外の未処理例外スロー時のイベントハンドラ
        /// </summary>
        private void CurrentDomain_UnhandledException(
            object sender, UnhandledExceptionEventArgs e)
        {
            this.ReportUnhandledException(e.ExceptionObject as Exception);
        }

        /// <summary>
        /// 未処理例外をイベントログに出力します。
        /// </summary>
        private void ReportUnhandledException(Exception ex)
        {
            //EventLog.WriteEntry("RSVForTagPrint", ex.ToString(), EventLogEntryType.Error);
            MessageBox.Show(ex.ToString());
            this.Shutdown();
        }
    }
}
