namespace YKKeyPoppin
{
    using System.Linq;
    using System.Windows.Input;
    using YKToolkit.Controls;

    /// <summary>
    /// InfoView.xaml の相互作用ロジック
    /// </summary>
    public partial class InfoView : Window
    {
        public InfoView()
        {
            InitializeComponent();

            this.KeyDown += (_, e) =>
            {
                if (e.Key == System.Windows.Input.Key.Escape) this.DialogResult = true;
            };
            SetInfo();
        }

        private void SetInfo()
        {
            var orderedCollection = App.Instance.KeyHitCount.OrderByDescending(x => x.Value);

            this.textblock1.Text = string.Join("\r\n", orderedCollection.Where(x => KeyView.KeysDictionary.GetString(x.Key).Length == 1)
                                   .Select((x, i) => string.Join(", ", new string[]
                                   {
                                       "No." + (i + 1).ToString(),
                                       "Key = " + KeyView.KeysDictionary.GetString(x.Key),
                                       "Hits = " + x.Value,
                                   })));
            this.textblock2.Text = string.Join("\r\n", orderedCollection.Where(x => KeyView.KeysDictionary.GetString(x.Key).Length > 1)
                                   .Select((x, i) => string.Join(", ", new string[]
                                   {
                                       "No." + (i + 1).ToString(),
                                       "Key = " + KeyView.KeysDictionary.GetString(x.Key),
                                       "Hits = " + x.Value,
                                   })));
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
