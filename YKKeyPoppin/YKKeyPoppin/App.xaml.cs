namespace YKKeyPoppin
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Timers;
    using System.Windows;
    using System.Windows.Input;
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

            SetNotifyIcon();
            LoadFile();

            Enumerable.Range(0, 50).Select(x => new KeyView()).ToList().ForEach(x => this._views.Add(x));
            this._timer.Elapsed += (_, __) =>
            {
                this.Dispatcher.BeginInvoke((Action)(() =>
                {
                    Enumerable.Range(0, Math.Min(20, 100 - this._views.Count)).Select(x => new KeyView()).ToList().ForEach(x => this._views.Add(x));
                    if (this._views.Count >= 100) this._timer.Stop();
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

        private void SetNotifyIcon()
        {
            var uri = new Uri("icon.ico", UriKind.Relative);
            NotifyIcon.Icon = new System.Drawing.Icon(Application.GetResourceStream(uri).Stream);

            // 設定
            var confMenu = new Forms.ToolStripMenuItem("設定");
            confMenu.MouseDown += (_, e) =>
            {
                if (e.Button == Forms.MouseButtons.Left)
                {
                    var dlg = new ConfView();
                    var result = dlg.ShowDialog();
                    if (result.Value)
                    {
                        var dictionary = dlg.Tag as Dictionary<KeyInfo, string>;
                        if (dictionary != null)
                        {
                            KeyView.KeysDictionary = dictionary;
                        }
                    }
                }
            };
            NotifyIcon.ContextMenuStrip.Items.Add(confMenu);

            // 集計情報
            var infoMenu = new Forms.ToolStripMenuItem("集計");
            infoMenu.MouseDown += (_, e) =>
            {
                if (e.Button == Forms.MouseButtons.Left)
                {
                    var dlg = new InfoView();
                    dlg.ShowDialog();
                }
            };
            NotifyIcon.ContextMenuStrip.Items.Add(infoMenu);

            // 統計情報
            var graphMenu = new Forms.ToolStripMenuItem("統計");
            graphMenu.MouseDown += (_, e) =>
            {
                if (e.Button == Forms.MouseButtons.Left)
                {
                    var dlg = new GraphView();
                    dlg.ShowDialog();
                }
            };
            NotifyIcon.ContextMenuStrip.Items.Add(graphMenu);

            // セパレータ
            NotifyIcon.ContextMenuStrip.Items.Add(new Forms.ToolStripSeparator());

            // 終了
            var exitMenu = new Forms.ToolStripMenuItem("終了");
            exitMenu.MouseDown += (_, e) =>
            {
                if (e.Button == Forms.MouseButtons.Left) this.Shutdown();
            };
            NotifyIcon.ContextMenuStrip.Items.Add(exitMenu);

            NotifyIcon.Visible = true;
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            SaveFile();
            NotifyIcon.Dispose();

            this._keyHook.KeyDown -= OnKeyDown;
            this._keyHook.KeyUp -= OnKeyUp;
            this._keyHook.UnHook();
            this._keyHook.Dispose();
        }

        private static Forms.NotifyIcon _notifyIcon;
        private static Forms.NotifyIcon NotifyIcon
        {
            get
            {
                return _notifyIcon ?? (_notifyIcon = new Forms.NotifyIcon()
                {
                    ContextMenuStrip = new Forms.ContextMenuStrip(),
                });
            }
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
                var modifierKeys = ModifierKeys.None;
                if (this._downKeys.Contains(User32.VKs.VK_SHIFT)) modifierKeys |= ModifierKeys.Shift;
                if (this._downKeys.Contains(User32.VKs.VK_LSHIFT)) modifierKeys |= ModifierKeys.Shift;
                if (this._downKeys.Contains(User32.VKs.VK_RSHIFT)) modifierKeys |= ModifierKeys.Shift;
                if (this._downKeys.Contains(User32.VKs.VK_CONTROL)) modifierKeys |= ModifierKeys.Control;
                if (this._downKeys.Contains(User32.VKs.VK_LCONTROL)) modifierKeys |= ModifierKeys.Control;
                if (this._downKeys.Contains(User32.VKs.VK_RCONTROL)) modifierKeys |= ModifierKeys.Control;
                if (this._downKeys.Contains(User32.VKs.VK_MENU)) modifierKeys |= ModifierKeys.Alt;
                if (this._downKeys.Contains(User32.VKs.VK_LMENU)) modifierKeys |= ModifierKeys.Alt;
                if (this._downKeys.Contains(User32.VKs.VK_RMENU)) modifierKeys |= ModifierKeys.Alt;
                w.SetString(key, modifierKeys);

                this._downKeys.Add(key);
                var info = new KeyInfo() { Key = key, ModifierKeys = modifierKeys };
                this.LastDownKey = info;
                if (!this._keyHitCount.ContainsKey(info)) this._keyHitCount.Add(info, 0);
                this._keyHitCount[info]++;
            }
        }

        private void LoadFile()
        {
            FileStream stream = null;
            try
            {
                stream = new FileStream(App.KeyConfFilePath, FileMode.Open, FileAccess.Read);
                var serializer = new DataContractSerializer(typeof(Dictionary<KeyInfo, string>));
                KeyView.KeysDictionary = serializer.ReadObject(stream) as Dictionary<KeyInfo, string>;
            }
            catch
            {
            }
            finally
            {
                if (stream != null)
                {
                    stream.Dispose();
                    stream = null;
                }
            }
        }

        private void SaveFile()
        {
            FileStream stream = null;
            try
            {
                stream = new FileStream(DateTime.Now.ToString("yyyyMMddHHmmss") + App.KeyHitCountFilePath, FileMode.Create, FileAccess.Write);
                var serializer = new DataContractSerializer(typeof(Dictionary<KeyInfo, int>));
                serializer.WriteObject(stream, this.KeyHitCount);
            }
            catch
            {
            }
            finally
            {
                if (stream != null)
                {
                    stream.Dispose();
                    stream = null;
                }
            }
        }

        private KeyboardHook _keyHook = new KeyboardHook();
        private List<KeyView> _views = new List<KeyView>(100);
        private KeyView _comboView;
        private Timer _timer = new Timer(600);
        private Timer _comboTimer = new Timer(400);
        private List<User32.VKs> _downKeys = new List<User32.VKs>();
        private Dictionary<KeyInfo, int> _keyHitCount = new Dictionary<KeyInfo, int>();
        internal Dictionary<KeyInfo, int> KeyHitCount
        {
            get { return this._keyHitCount; }
        }

        public const string KeyConfFilePath = "conf.keys";
        public const string KeyHitCountFilePath = ".hitkeys";

        public Rect Bounds { get; private set; }

        public int ComboCount { get; private set; }

        internal KeyInfo LastDownKey { get; private set; }
    }
}
