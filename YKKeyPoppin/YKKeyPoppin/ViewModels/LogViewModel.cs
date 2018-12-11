namespace YKKeyPoppin.ViewModels
{
    using System.Collections.Generic;
    using YKKeyPoppin.Models;
    using YKToolkit.Bindings;
    using YKToolkit.Helpers;

    internal class LogViewModel : NotificationObject
    {
        /// <summary>
        /// タイトルを取得します。
        /// </summary>
        public string Title { get { return this._productInfo.Title + " Ver." + this._productInfo.VersionString; } }

        /// <summary>
        /// 全ログデータを取得します。
        /// </summary>
        public IEnumerable<KeyLogData> AllLog { get { return KeyCollector.Current.AllCollections; } }

        /// <summary>
        /// プロダクト情報
        /// </summary>
        private ProductInfo _productInfo = new ProductInfo(true);
    }
}
