namespace YKKeyPoppin
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
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

            this._keyHook.KeyDown += OnKeyDown;
            this._keyHook.Hook();
        }

        private void OnKeyDown(User32.VKs key)
        {
            var w = this._views.FirstOrDefault();
            if (w == null) w = new KeyView();
            this._views.Remove(w);
            w.Show();
            w.SetString(key);
        }

        private KeyboardHook _keyHook = new KeyboardHook();
        private List<KeyView> _views = new List<KeyView>(50);

        public Rect Bounds { get; private set; }
    }
}
