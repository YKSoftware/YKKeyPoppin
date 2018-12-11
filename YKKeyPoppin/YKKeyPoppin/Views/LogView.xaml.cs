namespace YKKeyPoppin.Views
{
    using System;
    using System.ComponentModel;
    using YKToolkit.Controls;

    /// <summary>
    /// LogView.xaml の相互作用ロジック
    /// </summary>
    public partial class LogView : Window
    {
        public LogView()
        {
            InitializeComponent();

            this.Loaded += OnLoaded;
            this.IsVisibleChanged += OnIsVisibleChanged;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void OnLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            this._location = new System.Windows.Point(this.Left, this.Top);
            this._isLoaded = true;
        }

        private void OnIsVisibleChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (this._isLoaded && (this.Visibility == System.Windows.Visibility.Visible))
            {
                this.Dispatcher.BeginInvoke((Action)(() =>
                {
                    // このメソッドを抜けてから WindowState を変更しないと変な挙動になる
                    this.WindowState = System.Windows.WindowState.Normal;
                }), null);
                this.Left = this._location.X;
                this.Top = this._location.Y;
            }
        }

        private bool _isLoaded;
        private System.Windows.Point _location;
    }
}
