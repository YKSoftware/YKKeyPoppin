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
            };

            var yValues = this.AllLog.Select(x => (double)x.TotalHits);
            min = ((long)yValues.Min() / 100) * 100;
            max = ((long)yValues.Max() / 100) * 100 + 100;
            this.YAxisSettings = new AxisSettings()
            {
                Minimum = min,
                Maximum = max,
                MajorStep = (max - min) / 10.0,
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
