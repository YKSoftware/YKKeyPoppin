namespace YKKeyPoppin.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using YKKeyPoppin.Models;
    using YKToolkit.Bindings;
    using YKToolkit.Controls;
    using YKToolkit.Helpers;

    internal class LogViewModel : NotificationObject
    {
        public LogViewModel()
        {
            var xValues = this.AllLog.Select(x => (double)x.DateTime.ToBinary());
            var min = xValues.Min();
            var max = xValues.Max();
            this.XAxisSettings = new AxisSettings()
            {
                Minimum = min,
                Maximum = max,
                MajorStep = (max - min) / 10.0,
                GridLabelVisibility = System.Windows.Visibility.Collapsed,
                Title = "",
            };

            var yValues = this.AllLog.Select(x => (double)x.TotalHits);
            min = ((long)yValues.Min() / 500) * 500;
            max = ((long)yValues.Max() / 500) * 500 + 500;
            this.YAxisSettings = new AxisSettings()
            {
                Minimum = min,
                Maximum = max,
                MajorStep = (max - min) / 10.0,
                StringFormat = "#,##0",
                Title = "",
            };
        }

        /// <summary>
        /// タイトルを取得します。
        /// </summary>
        public string Title { get { return this._productInfo.Title + " Ver." + this._productInfo.VersionString; } }

        /// <summary>
        /// 全ログデータを取得します。
        /// </summary>
        public IEnumerable<KeyLogData> AllLog { get { return KeyCollector.Current.AllCollections; } }

        public Dictionary<KeyInfo, int> AllLogByKeyInfo
        {
            get
            {
                return KeyCollector.Current.AllCollections.Select(x => x.Log).Aggregate(new Dictionary<KeyInfo, int>(), (x, y) =>
                {
                    foreach (var z in y)
                    {
                        if (x.ContainsKey(z.Key)) x[z.Key] += z.Value;
                        else x.Add(z.Key, z.Value);
                    }
                    return x;
                });
            }
        }

        public AxisSettings XAxisSettings { get; private set; }
        public AxisSettings YAxisSettings { get; private set; }
        public double[] XData { get { return this.AllLog.Select(x => (double)x.DateTime.ToBinary()).ToArray(); } }
        public double[] YData { get { return this.AllLog.Select(x => (double)x.TotalHits).ToArray(); } }

        /// <summary>
        /// プロダクト情報
        /// </summary>
        private ProductInfo _productInfo = new ProductInfo(true);
    }
}
