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

            if (instance != null) throw new Exception("1 つのアプリで複数のインスタンスを持つことを考慮した実装になっていません。");
            instance = this;
        }

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="iconPath">タスクトレイに表示するアイコン画像のパスを指定します。</param>
        /// <param name="toolTipText">アイコンのツールチップに表示するテキストを指定します。</param>
        /// <param name="window">アイコンを右クリックしたときに表示するコンテンツを指定します。</param>
        public WpfNotifyIcon(string iconPath, string toolTipText, System.Windows.Window window)
            : this()
        {
            this.IconPath = iconPath;
            this.Text = toolTipText;
            this.Content = window;
        }

        /// <summary>
        /// インスタンスの保持
        /// WpfNotifyIcon のインスタンスはアプリ上に 1 個しかないことを前提としています。
        /// 複数のインスタンスを持つ場合は修正が必要です。
        /// </summary>
        private static WpfNotifyIcon instance;

        /// <summary>
        /// リソースを破棄します。
        /// </summary>
        public static void Dispose()
        {
            if (instance == null) return;

            if (instance._mouseHook != null)
            {
                instance._mouseHook.MouseLeftButtonUp -= instance.OnMouseUp;
                instance._mouseHook.MouseRightButtonUp -= instance.OnMouseUp;
                instance._mouseHook.MouseMiddleButtonUp -= instance.OnMouseUp;
                instance._mouseHook = null;
            }
            instance._notify.Dispose();
            if (instance.Content != null)
            {
                instance.Content.PreviewMouseDown -= instance.OnContentPreviewMouseDown;
                instance.Content.Close();
            }
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
                if (this.Content != null)
                {
                    this.Content.Show();
                    this.Content.Left = this._mouseReleasePoint.X - this.Content.ActualWidth;
                    this.Content.Top = this._mouseReleasePoint.Y - this.Content.ActualHeight;
                }
            }
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
                if (this.Content != null) this.Content.Hide();
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

        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content", typeof(System.Windows.Window), typeof(WpfNotifyIcon), new UIPropertyMetadata(null, (s, e) => (s as WpfNotifyIcon).OnContentPropertyChanged(e.OldValue as System.Windows.Window, e.NewValue as System.Windows.Window)));
        /// <summary>
        /// アイコンを右クリックしたときに表示するコンテンツを取得または設定します。
        /// </summary>
        public System.Windows.Window Content
        {
            get { return (System.Windows.Window)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        /// <summary>
        /// Content プロパティ変更イベントハンドラ
        /// </summary>
        /// <param name="oldValue">変更前の値</param>
        /// <param name="newValue">変更後の値</param>
        private void OnContentPropertyChanged(System.Windows.Window oldValue, System.Windows.Window newValue)
        {
            if (oldValue != null)
            {
                oldValue.PreviewMouseDown -= OnContentPreviewMouseDown;
            }
            if (newValue != null)
            {
                newValue.PreviewMouseDown += OnContentPreviewMouseDown;
            }
        }

        /// <summary>
        /// Content プロパティで指定されたウィンドウ上でマウスボタンを押したときのイベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行元</param>
        /// <param name="e">イベント引数</param>
        private void OnContentPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            this._canClose = false;
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
        /// マウスを離したときのスクリーン座標
        /// </summary>
        private Point _mouseReleasePoint;
    }
}
