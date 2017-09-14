namespace YKKeyPoppin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Timers;
    using System.Windows;
    using System.Windows.Threading;
    using Forms = System.Windows.Forms;

    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        public static App Instance { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Instance = this;

            this.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            this.Bounds = new Rect(new Size(Forms.Screen.PrimaryScreen.Bounds.Width, Forms.Screen.PrimaryScreen.Bounds.Height));


            Enumerable.Range(0, 50).Select(x => new KeyView()).ToList().ForEach(x => this._views.Add(x));
            this._timer.Elapsed += (_, __) =>
            {
                this.Dispatcher.BeginInvoke((Action)(() =>
                {
                    Enumerable.Range(0, Math.Min(20, 100 - this._views.Count)).Select(x => new KeyView()).ToList().ForEach(x => this._views.Add(x));
                }), DispatcherPriority.SystemIdle, null);
            };
            this._timer.Start();

            this._comboView = new KeyView(0);
            this._comboView.Show();
            this._comboTimer.Elapsed += (_, __) =>
            {
                this.ComboCount = 0;
            };

            this._keyHook.KeyDown += OnKeyDown;
            this._keyHook.KeyUp += OnKeyUp;
            this._keyHook.Hook();
        }

        private void OnKeyUp(User32.VKs key)
        {
            this._downKeys.Remove(key);
        }

        private void OnKeyDown(User32.VKs key)
        {
            if (!this._downKeys.Contains(key))
            {
                this._timer.Stop();
                this._timer.Start();

                this._comboTimer.Stop();
                this._comboTimer.Start();
                this.ComboCount++;
                this._comboView.SetCount(this.ComboCount);

                var w = this._views.FirstOrDefault();
                if (w == null) w = new KeyView(true);
                this._views.Remove(w);
                w.Show();
                w.SetString(key);

                this._downKeys.Add(key);
            }
        }

        private KeyboardHook _keyHook = new KeyboardHook();
        private List<KeyView> _views = new List<KeyView>(100);
        private KeyView _comboView;
        private Timer _timer = new Timer(600);
        private Timer _comboTimer = new Timer(400);
        private List<User32.VKs> _downKeys = new List<User32.VKs>();

        public Rect Bounds { get; private set; }

        public int ComboCount { get; private set; }
    }
}
