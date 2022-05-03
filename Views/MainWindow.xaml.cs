using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RSVForTagPrint.Views
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region "最大化・最小化・閉じるボタンの非表示設定"

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, long dwNewLong);

        const int GWL_STYLE = -16;
        // 最大化ボタン
        const long WS_MAXIMIZEBOX = 0x00010000L;
        // 最小化ボタン
        const long WS_MINIMIZEBOX = 0x00020000L;

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            IntPtr handle = new WindowInteropHelper(this).Handle;
            long style = GetWindowLong(handle, GWL_STYLE);
            style &= ((WS_MAXIMIZEBOX | WS_MINIMIZEBOX) ^ 0xFFFFFFFFL);
            SetWindowLong(handle, GWL_STYLE, style);
        }

        #endregion
    }
}
