namespace YKKeyPoppin.ViewModels
{
    using YKKeyPoppin.Models;
    using YKToolkit.Bindings;

    internal class MouseInfoViewModel : NotificationObject, IMenuContentViewModel
    {
        public MouseInfoViewModel()
        {
            MouseCollector.Current.ValueChagned += OnValueChanged;
        }

        private void OnValueChanged()
        {
            RaiseAllPropertyChanged();
        }

        public void Loaded()
        {
        }

        public void Unloaded()
        {
        }

        /// <summary>
        /// マウス走破距離
        /// </summary>
        public double TotalDistance { get { return MouseCollector.Current.TotalDistance; } }

        /// <summary>
        /// 左ボタンクリック回数を取得します。
        /// </summary>
        public int LeftButtonCount { get { return MouseCollector.Current.LeftButtonCount; } }

        /// <summary>
        /// 右ボタンクリック回数を取得します。
        /// </summary>
        public int RightButtonCount { get { return MouseCollector.Current.RightButtonCount; } }

        /// <summary>
        /// ホイールボタンクリック回数を取得します。
        /// </summary>
        public int MiddleButtonCount { get { return MouseCollector.Current.MiddleButtonCount; } }

        /// <summary>
        /// ダブルクリック回数を取得します。
        /// </summary>
        public int DoubleClickCount { get { return MouseCollector.Current.DoubleClickCount; } }

        /// <summary>
        /// マウスホイールを上に上げた回数を取得します。
        /// </summary>
        public int WheelUpCount { get { return MouseCollector.Current.WheelUpCount; } }

        /// <summary>
        /// マウスホイールを下に下げた回数を取得します。
        /// </summary>
        public int WheelDownCount { get { return MouseCollector.Current.WheelDownCount; } }
    }
}
