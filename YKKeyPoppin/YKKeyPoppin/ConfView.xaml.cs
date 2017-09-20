namespace YKKeyPoppin
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization;
    using System.Windows.Interop;
    using YKToolkit.Controls;
    using Forms = System.Windows.Forms;

    /// <summary>
    /// ConfView.xaml の相互作用ロジック
    /// </summary>
    public partial class ConfView : Window
    {
        public ConfView()
        {
            InitializeComponent();

            this._keyDictionary = KeyView.KeysDictionary != null ? KeyView.KeysDictionary : new Dictionary<KeyInfo, string>();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            var handle = (new WindowInteropHelper(this)).Handle;
            // メッセージ処理をフック
            var hwndSource = HwndSource.FromHwnd(handle);
            hwndSource.AddHook(WndProc);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == (int)User32.WMs.WM_CHAR)
            {
                var input = (byte)wParam.ToInt32();
                var str = System.Text.Encoding.ASCII.GetString(new byte[] { input });
                this.textblock.Text = str;
                if (this._keyDictionary.ContainsKey(App.Instance.LastDownKey))
                {
                    this._keyDictionary[App.Instance.LastDownKey] = str;
                }
                else
                {
                    this._keyDictionary.Add(App.Instance.LastDownKey, str);
                }
            }

            return IntPtr.Zero;
        }

        private void ClickOkButton(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Tag = this._keyDictionary;
            SaveFile();
            this.DialogResult = true;
        }

        private void ClickCancelButton(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Tag = null;
            this.DialogResult = false;
        }

        private void SaveFile()
        {
            FileStream stream = null;
            try
            {
                stream = new FileStream(App.KeyConfFilePath, FileMode.Create, FileAccess.Write);
                var serializer = new DataContractSerializer(this._keyDictionary.GetType());
                serializer.WriteObject(stream, this._keyDictionary);
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

        private Dictionary<KeyInfo, string> _keyDictionary;
    }
}
