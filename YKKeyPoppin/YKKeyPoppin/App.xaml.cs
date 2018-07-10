namespace YKKeyPoppin
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Timers;
    using System.Windows;
    using System.Windows.Threading;
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

        private const int MaxViewCount = 255;

        /// <summary>
        /// 起動時のイベントハンドラ
        /// </summary>
        /// <param name="e">イベント引数</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Instance = this;

            this.DispatcherUnhandledException += OnDispatcherUnhandledException;

            this.SessionEnding += OnSessionEnding;
            this.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            this.IsPoppinEnabled = true;

            // KeyView のインスタンスを倉庫に詰める
            Enumerable.Range(0, MaxViewCount).ToList().ForEach(x =>
            {
                this._keyViews.Add(new KeyView());
            });

            // 出庫した分のインスタンスを倉庫に詰め直す
            //this._keyViewTimer.Elapsed += (_, __) =>
            //{
            //    if (this._keyViews.Count < MaxViewCount)
            //    {
            //        this.Dispatcher.BeginInvoke((Action)(() =>
            //        {
            //            Enumerable.Range(0, MaxViewCount - this._keyViews.Count).ToList().ForEach(x =>
            //            {
            //                this._keyViews.Add(new KeyView());
            //            });
            //        }), DispatcherPriority.SystemIdle, null);
            //    }
            //};
            //this._keyViewTimer.Start();

            KeyConf.Current.LoadConf();
            KeyCollector.Current.KeyUp += OnKeyUp;

            var w = new ComboView();
            w.DataContext = new ComboViewModel();
            w.Show();

            // DPI 取得
            var dpiXProperty = typeof(SystemParameters).GetProperty("DpiX", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            var dpiYProperty = typeof(SystemParameters).GetProperty("Dpi", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            this.DpiX = (int)dpiXProperty.GetValue(null, null);
            this.DpiY = (int)dpiYProperty.GetValue(null, null);
            this.MinimumDistance = new User32.POINT(1, 1).CalcDistance(new User32.POINT());
        }

        /// <summary>
        /// DispatcherUnhandledException イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行元</param>
        /// <param name="e">イベント引数</param>
        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var str = string.Concat(new string[]
            {
                "[DispatcherUnhandledException : " + DateTime.Now.ToString() + Environment.NewLine,
                e.Exception.ToString() + Environment.NewLine,
            });
            File.AppendAllText("log.txt", str);
            e.Handled = true;
            End();
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
            if (this.IsPoppinEnabled)
            {
                var w = this._keyViews.FirstOrDefault(x => x.IsBusy == false);
                var isExtra = w == null;
                if (isExtra) w = new KeyView();
                w.Poppin(info, isExtra);
            }
        }

        /// <summary>
        /// KeyView のインスタンスの倉庫
        /// </summary>
        private List<KeyView> _keyViews = new List<KeyView>(MaxViewCount);

        /// <summary>
        /// KeyView インスタンス補充タイマー
        /// </summary>
        private Timer _keyViewTimer = new Timer(400);

        /// <summary>
        /// Windows ログオフやシャットダウンなどのセッション終了イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行元</param>
        /// <param name="e">イベント引数</param>
        private void OnSessionEnding(object sender, SessionEndingCancelEventArgs e)
        {
            End();
        }

        /// <summary>
        /// アプリケーションを終了します。
        /// </summary>
        public void End()
        {
            KeyCollector.Current.SaveCollection();
            MouseCollector.Current.Save();

            WpfNotifyIcon.Dispose();
            this.Shutdown();
        }

        public int DpiX { get; private set; }
        public int DpiY { get; private set; }

        public double MinimumDistance { get; private set; }

        public bool IsPoppinEnabled { get; set; }
    }
}
