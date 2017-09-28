namespace YKKeyPoppin.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;
    using System.Windows.Threading;
    using YKKeyPoppin.Models;
    using YKToolkit.Bindings;
    using YKToolkit.Controls;

    internal class GraphViewModel : NotificationObject
    {
        public GraphViewModel()
        {
            KeyCollector.Current.KeyUp += OnKeyUp;
        }

        private void OnKeyUp(KeyInfo info)
        {
            RaisePropertyChanged("AggregatedKeys");
        }

        public IEnumerable<object> AggregatedKeys
        {
            get
            {
                if (!this._isLoaded) return Enumerable.Empty<object>();
                var maxHit = KeyCollector.Current.KeyCollection.Any() ? KeyCollector.Current.KeyCollection.Max(x => x.Value) : 0;
                var keys = KeyCollector.Current.KeyCollection.Select(x => new { Key = x.Key, Value = x.Value, Ratio = (double)x.Value * 100.0 / (double)maxHit });;
                return this.IsOrder ? keys.OrderByDescending(x => x.Value) : keys;
            }
        }

        private bool _isOrder = true;
        public bool IsOrder
        {
            get { return this._isOrder; }
            set
            {
                if (SetProperty(ref this._isOrder, value))
                {
                    RaisePropertyChanged("AggregatedKeys");
                }
            }
        }

        private bool _isLoaded;

        public void Loaded()
        {
            this._isLoaded = true;
            RaisePropertyChanged("AggregatedKeys");
        }

        public void UnLoaded()
        {
            if (!this._isLoaded) return;

            this._isLoaded = false;
            RaisePropertyChanged("AggregatedKeys");
        }
    }
}
