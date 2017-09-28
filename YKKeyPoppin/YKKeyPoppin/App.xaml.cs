namespace YKKeyPoppin
{
    using Microsoft.Win32;
    using System.Windows;
    using YKKeyPoppin.Models;
    using YKKeyPoppin.ViewModels;
    using YKKeyPoppin.Views;
    using YKToolkit.Controls;

    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// 現在のインスタンスを取得します。
        /// </summary>
        public static App Instance { get; private set; }

        /// <summary>
        /// 起動時のイベントハンドラ
        /// </summary>
        /// <param name="e">イベント引数</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Instance = this;

            SystemEvents.SessionEnding += OnSessionEnding;
            this.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            ThemeManager.Instance.Initialize();

            KeyCollector.Current.KeyUp += OnKeyUp;

            var w = new ComboView();
            w.DataContext = new ComboViewModel();
            w.Show();
        }

        /// <summary>
        /// キーボードグローバルフックでキーを離したときのイベントハンドラ
        /// </summary>
        private void OnKeyUp(KeyInfo info)
        {
            LetsPoppin(info);
        }

        /// <summary>
        /// Let's Poppin' !!
        /// </summary>
        /// <param name="info">Poppin' するキー情報</param>
        private void LetsPoppin(KeyInfo info)
        {
            var w = new KeyView(info);
            w.Show();
        }

        /// <summary>
        /// Windows ログオフやシャットダウンなどのセッション終了イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行元</param>
        /// <param name="e">イベント引数</param>
        private void OnSessionEnding(object sender, SessionEndingEventArgs e)
        {
            End();
        }

        /// <summary>
        /// アプリケーションを終了します。
        /// </summary>
        public void End()
        {
            WpfNotifyIcon.Dispose();
            KeyCollector.Current.SaveCollection();
            this.Shutdown();
        }
    }
}
