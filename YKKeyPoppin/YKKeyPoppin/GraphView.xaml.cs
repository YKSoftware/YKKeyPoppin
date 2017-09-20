namespace YKKeyPoppin
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization;
    using YKToolkit.Controls;

    /// <summary>
    /// GraphView.xaml の相互作用ロジック
    /// </summary>
    public partial class GraphView : Window
    {
        public GraphView()
        {
            InitializeComponent();

            LoadFiles();
            UpdateData();
        }

        private void LoadFiles()
        {
            var list = new List<KeyValuePair<DateTime, Dictionary<KeyInfo, int>>>();
            var files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*" + App.KeyHitCountFilePath);

            foreach (var file in files)
            {
                FileStream stream = null;
                try
                {
                    stream = new FileStream(file, FileMode.Open, FileAccess.Read);
                    var serializer = new DataContractSerializer(typeof(Dictionary<KeyInfo, int>));
                    var item = serializer.ReadObject(stream) as Dictionary<KeyInfo, int>;
                    if (item != null)
                    {
                        var str = Path.GetFileNameWithoutExtension(file);
                        var year = int.Parse(str.Substring(0, 4));
                        var month = int.Parse(str.Substring(4, 2));
                        var day = int.Parse(str.Substring(6, 2));
                        var hour = int.Parse(str.Substring(8, 2));
                        var min = int.Parse(str.Substring(10, 2));
                        var sec = int.Parse(str.Substring(12, 2));
                        list.Add(new KeyValuePair<DateTime, Dictionary<KeyInfo, int>>(new DateTime(year, month, day, hour, min, sec), item));
                    }
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

            this._allHistory = list;
        }

        private void UpdateData()
        {
            var orderedCollections = this._allHistory.Select(x => new { Date = x.Key, Contents = x.Value.OrderByDescending(y => y.Value) });

            var allData = orderedCollections.Select(x => new
            {
                Date = x.Date,
                InputString = string.Join("\r\n", x.Contents.Where(y => KeyView.KeysDictionary.GetString(y.Key).Length == 1)
                              .Select((y, i) => string.Join(", ", new string[]
                              {
                                  "No." + (i + 1).ToString(),
                                  "Key = " + KeyView.KeysDictionary.GetString(y.Key),
                                  "Hits = " + y.Value,
                              }))),
                ControlCommand = string.Join("\r\n", x.Contents.Where(y => KeyView.KeysDictionary.GetString(y.Key).Length > 1)
                                 .Select((y, i) => string.Join(", ", new string[]
                                 {
                                     "No." + (i + 1).ToString(),
                                     "Key = " + KeyView.KeysDictionary.GetString(y.Key),
                                     "Hits = " + y.Value,
                                 }))),
            });
            this.tabcontrol.ItemsSource = allData;
        }

        private List<KeyValuePair<DateTime, Dictionary<KeyInfo, int>>> _allHistory;

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
