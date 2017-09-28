namespace YKKeyPoppin
{
    using Microsoft.Win32;
    using System.Collections.Generic;
    using System.Linq;
    using System.Timers;
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

            // KeyView のインスタンスを倉庫に詰める
            Enumerable.Range(0, 100).ToList().ForEach(x =>
            {
                this._keyViews.Add(new KeyView());
            });
            // 出庫した分のインスタンスを倉庫に詰め直す
            this._keyViewTimer.Elapsed += (_, __) =>
            {
                if (this._keyViews.Count < 100)
                {
                    Enumerable.Range(0, 100 - this._keyViews.Count).ToList().ForEach(x =>
                    {
                        this._keyViews.Add(new KeyView());
                    });
                }
            };
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
            var w = this._keyViews.FirstOrDefault();
            this._keyViews.Remove(w);
            if (w == null) w = new KeyView();
            w.Poppin(info);
            w.Show();
        }

        /// <summary>
        /// KeyView のインスタンスの倉庫
        /// </summary>
        private List<KeyView> _keyViews = new List<KeyView>(100);

        /// <summary>
        /// KeyView インスタンス補充タイマー
        /// </summary>
        private Timer _keyViewTimer = new Timer(400);

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
