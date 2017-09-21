namespace YKKeyPoppin.Views
{
    using System;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Markup;
    using System.Windows.Media;
    using YKToolkit.Controls;
    using Drawing = System.Drawing;
    using Forms = System.Windows.Forms;

    /// <summary>
    /// タスクトレイに表示するアイコンを制御する WPF 用 NotifyIcon を表します。
    /// </summary>
    [ContentProperty("Content")]
    internal class WpfNotifyIcon : FrameworkElement
    {
        /// <summary>
        /// デザイン実行かどうかを取得します。
        /// </summary>
        private bool IsInDesignMode
        {
            get { return System.ComponentModel.DesignerProperties.GetIsInDesignMode(this); }
        }

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        public WpfNotifyIcon()
        {
            if (this.IsInDesignMode) return;

            this._notify.MouseUp += OnNotifyIconMouseUp;

            this._mouseHook = new MouseHook();
            this._mouseHook.MouseLeftButtonUp += OnMouseUp;
            this._mouseHook.MouseRightButtonUp += OnMouseUp;
            this._mouseHook.MouseMiddleButtonUp += OnMouseUp;
            this._mouseHook.Hook();

            this._menuWindow = new System.Windows.Window()
            {
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStyle = WindowStyle.None,
                ResizeMode = ResizeMode.NoResize,
                ShowInTaskbar = false,
                AllowsTransparency = true,
                Background = Brushes.Transparent,
                Topmost = true,
            };
            this._menuWindow.PreviewMouseDown += OnMenuWindowPreviewMouseDown;
            this.DataContextChanged += OnDataContextChanged;
        }

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="iconPath">タスクトレイに表示するアイコン画像のパスを指定します。</param>
        /// <param name="toolTipText">アイコンのツールチップに表示するテキストを指定します。</param>
        /// <param name="element">アイコンを右クリックしたときに表示するコンテンツを指定します。</param>
        public WpfNotifyIcon(string iconPath, string toolTipText, FrameworkElement element)
            : this()
        {
            this.IconPath = iconPath;
            this.Text = toolTipText;
            this.Content = element;
        }

        /// <summary>
        /// リソースを破棄します。
        /// </summary>
        internal void Dispose()
        {
            if (this._mouseHook != null)
            {
                this._mouseHook.MouseLeftButtonUp -= OnMouseUp;
                this._mouseHook.MouseRightButtonUp -= OnMouseUp;
                this._mouseHook.MouseMiddleButtonUp -= OnMouseUp;
                this._mouseHook = null;
            }
            this._notify.Dispose();
            this._menuWindow.Close();
        }

        /// <summary>
        /// DataContext プロパティ変更イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行元</param>
        /// <param name="e">イベント引数</param>
        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            this._menuWindow.DataContext = this.DataContext;
        }

        /// <summary>
        /// タスクトレイのアイコンでマウスボタンを離したときのイベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行元</param>
        /// <param name="e">イベント引数</param>
        private void OnNotifyIconMouseUp(object sender, Forms.MouseEventArgs e)
        {
            if (e.Button == Forms.MouseButtons.Right)
            {
                // アイコンを右クリックされたとき
                if (this._menuWindow != null)
                {
                    this._menuWindow.Content = this.Content;
                    this._menuWindow.Show();
                    this._menuWindow.Left = this._mouseReleasePoint.X - this._menuWindow.ActualWidth;
                    this._menuWindow.Top = this._mouseReleasePoint.Y - this._menuWindow.ActualHeight;
                }
            }
        }

        /// <summary>
        /// MenuWindow 上でマウスボタンを押したときのイベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行元</param>
        /// <param name="e">イベント引数</param>
        private void OnMenuWindowPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            this._canClose = false;
        }

        /// <summary>
        /// 任意の位置でマウスボタンを離したときのイベントハンドラ
        /// </summary>
        /// <param name="e">イベント引数</param>
        private void OnMouseUp(MouseHook.MSLLHOOKSTRUCT e)
        {
            this._mouseReleasePoint = new Point(e.Point.X, e.Point.Y);

            if (this._canClose)
            {
                this._menuWindow.Hide();
                //this._mouseHook.UnHook();
            }

            this._canClose = true;
        }

        /// <summary>
        /// IconPath 依存関係プロパティの定義
        /// </summary>
        public static readonly DependencyProperty IconPathProperty = DependencyProperty.Register("IconPath", typeof(string), typeof(WpfNotifyIcon), new UIPropertyMetadata(null, (s, e) => (s as WpfNotifyIcon).OnIconPathPropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// タスクトレイに表示するアイコン画像のパスを取得または設定します。
        /// </summary>
        public string IconPath
        {
            get { return (string)GetValue(IconPathProperty); }
            set { SetValue(IconPathProperty, value); }
        }

        /// <summary>
        /// IconPath プロパティ変更イベントハンドラ
        /// </summary>
        /// <param name="oldValue">変更前の値</param>
        /// <param name="newValue">変更後の値</param>
        private void OnIconPathPropertyChanged(string oldValue, string newValue)
        {
            if (this.IsInDesignMode) return;
            if (!string.IsNullOrWhiteSpace(newValue))
            {
                var uri = new Uri(newValue);
                this._notify.Icon = new Drawing.Icon(Application.GetResourceStream(uri).Stream);
            }
        }

        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content", typeof(object), typeof(WpfNotifyIcon), new UIPropertyMetadata(null));
        /// <summary>
        /// アイコンを右クリックしたときに表示するコンテンツを取得または設定します。
        /// </summary>
        public object Content
        {
            get { return GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        /// <summary>
        /// Text 依存関係プロパティの定義
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(WpfNotifyIcon), new UIPropertyMetadata(null, (s, e) => (s as WpfNotifyIcon).OnTextPropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// ツールヒントテキストを取得または設定します。
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        /// <summary>
        /// Text プロパティ変更イベントハンドラ
        /// </summary>
        /// <param name="oldValue">変更前の値</param>
        /// <param name="newValue">変更後の値</param>
        private void OnTextPropertyChanged(string oldValue, string newValue)
        {
            this._notify.Text = newValue;
        }

        /// <summary>
        /// Visibility 依存関係プロパティの定義
        /// </summary>
        public static readonly new DependencyProperty VisibilityProperty = DependencyProperty.Register("Visibility", typeof(Visibility), typeof(WpfNotifyIcon), new UIPropertyMetadata(Visibility.Hidden, (s, e) => (s as WpfNotifyIcon).OnVisibilityPropertyChanged((Visibility)e.OldValue, (Visibility)e.NewValue)));

        /// <summary>
        /// タスクトレイアイコンの表示状態を取得または設定します。
        /// </summary>
        public new Visibility Visibility
        {
            get { return (Visibility)GetValue(VisibilityProperty); }
            set { SetValue(VisibilityProperty, value); }
        }

        /// <summary>
        /// Visibility プロパティ変更イベントハンドラ
        /// </summary>
        /// <param name="oldValue">変更前の値</param>
        /// <param name="newValue">変更後の値</param>
        private void OnVisibilityPropertyChanged(System.Windows.Visibility oldValue, System.Windows.Visibility newValue)
        {
            this._notify.Visible = newValue == System.Windows.Visibility.Visible ? true : false;
        }

        /// <summary>
        /// System.Windows.Forms.NotifyIcon
        /// </summary>
        private Forms.NotifyIcon _notify = new Forms.NotifyIcon();

        /// <summary>
        /// マウスフック
        /// </summary>
        private MouseHook _mouseHook;

        /// <summary>
        /// マウスフックで閉じていいかどうか判定する
        /// </summary>
        private bool _canClose;

        /// <summary>
        /// 右クリックメニューを表示するためのウィンドウ
        /// </summary>
        private System.Windows.Window _menuWindow;

        /// <summary>
        /// マウスを離したときのスクリーン座標
        /// </summary>
        private Point _mouseReleasePoint;
    }
}
