namespace YKKeyPoppin
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Input;
    using YKKeyPoppin.Models;
    using YKKeyPoppin.ViewModels;
    using YKKeyPoppin.Views;
    using YKToolkit.Controls;
    using Forms = System.Windows.Forms;

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

            this.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            ThemeManager.Instance.Initialize();

            KeyHook.Current.KeyUp += OnKeyUp;

            var w = new ComboView();
            w.DataContext = new ComboViewModel();
            w.Show();
        }

        /// <summary>
        /// キーボードグローバルフックでキーを離したときのイベントハンドラ
        /// </summary>
        /// <param name="key">離されたキー</param>
        /// <param name="modifierKeys">変換キー押下状態</param>
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
    }
}
