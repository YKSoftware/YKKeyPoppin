namespace YKKeyPoppin.Views
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Interop;
    using YKKeyPoppin.Models;

    /// <summary>
    /// KeyConfView.xaml の相互作用ロジック
    /// </summary>
    public partial class KeyConfView : UserControl
    {
        public KeyConfView()
        {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this)) return;

            this.Loaded += (_, __) =>
            {
                if (this._hasHooked) return;
                var w = Window.GetWindow(this);
                var handle = (new WindowInteropHelper(w)).Handle;
                // メッセージ処理をフック
                var hwndSource = HwndSource.FromHwnd(handle);
                hwndSource.AddHook(WndProc);
                this._hasHooked = true;
            };
        }

        private bool _hasHooked;

        private YKToolkit.Controls.User32.VKs _key;
        private ModifierKeys _modifierKeys;

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch ((YKToolkit.Controls.User32.WMs)msg)
            {
                case YKToolkit.Controls.User32.WMs.WM_CHAR:
                    var input = (byte)wParam.ToInt32();
                    // ASCII で図形文字以外の場合は文字として受け付けない
                    if ((input < 0x21) || (0x7e < input)) break;

                    if (this.InputCommand != null)
                    {
                        var str = System.Text.Encoding.ASCII.GetString(new byte[] { input });
                        var info = new KeyInfo() { Key = this._key, ModifierKeys = this._modifierKeys };
                        var pair = new KeyValuePair<KeyInfo, string>(info, str);

                        if (this.InputCommand.CanExecute(pair))
                            this.InputCommand.Execute(pair);
                    }
                    break;

                case YKToolkit.Controls.User32.WMs.WM_KEYDOWN:
                case YKToolkit.Controls.User32.WMs.WM_SYSKEYDOWN:
                    this._key = (YKToolkit.Controls.User32.VKs)wParam.ToInt32();
                    if (this._key.IsModifierKey())
                    {
                        var modifierKey = this._key.ToModifierKeys();
                        this._modifierKeys |= modifierKey;
                    }
                    break;

                case YKToolkit.Controls.User32.WMs.WM_KEYUP:
                case YKToolkit.Controls.User32.WMs.WM_SYSKEYUP:
                    var upKey = (YKToolkit.Controls.User32.VKs)wParam.ToInt32();
                    if (upKey.IsModifierKey())
                    {
                        var modifierKey = upKey.ToModifierKeys();
                        this._modifierKeys &= ~modifierKey;
                    }
                    break;

                default:
                    break;
            }

            return IntPtr.Zero;
        }

        public static DependencyProperty InputCommandProperty = DependencyProperty.Register("InputCommand", typeof(ICommand), typeof(KeyConfView), new PropertyMetadata(null));

        public ICommand InputCommand
        {
            get { return (ICommand)GetValue(InputCommandProperty); }
            set { SetValue(InputCommandProperty, value); }
        }
    }
}
