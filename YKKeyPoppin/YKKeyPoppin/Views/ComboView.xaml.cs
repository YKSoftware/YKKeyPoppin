namespace YKKeyPoppin.Views
{
    using System.Windows;
    using YKKeyPoppin.Models;

    /// <summary>
    /// ComboView.xaml の相互作用ロジック
    /// </summary>
    public partial class ComboView : Window
    {
        public ComboView()
        {
            InitializeComponent();

            this.Left = Settings.Current.Bounds.Right - 410;
            this.Top = Settings.Current.Bounds.Top + 70;
        }
    }
}
