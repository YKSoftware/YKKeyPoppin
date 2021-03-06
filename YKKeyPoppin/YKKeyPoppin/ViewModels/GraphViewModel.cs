﻿namespace YKKeyPoppin.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;
    using System.Windows.Threading;
    using YKKeyPoppin.Models;
    using YKToolkit.Bindings;
    using YKToolkit.Controls;

    internal class GraphViewModel : NotificationObject, IMenuContentViewModel
    {
        public GraphViewModel()
        {
            KeyCollector.Current.KeyUp += OnKeyUp;
        }

        private void OnKeyUp(KeyInfo info)
        {
            RaisePropertyChanged("TotalHits");
            RaisePropertyChanged("AggregatedKeys");
        }

        public int TotalHits { get { return KeyCollector.Current.KeyCollection.Sum(x => x.Value); } }

        public IEnumerable<object> AggregatedKeys
        {
            get
            {
                if (!this._isLoaded) return Enumerable.Empty<object>();
                var maxHit = KeyCollector.Current.KeyCollection.Any() ? KeyCollector.Current.KeyCollection.Max(x => x.Value) : 0;
                var keys = KeyCollector.Current.KeyCollection.Select(x => new { Key = x.Key, Value = x.Value, Ratio = (double)x.Value * 100.0 / (double)maxHit });
                if (this.IsExcept) keys = keys.Where(x => x.Key.IsTypingChar());
                return this.IsOrder ? keys.OrderByDescending(x => x.Value) : keys.OrderBy(x => x.Key.Key);
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

        private bool _isExcept;
        public bool IsExcept
        {
            get { return this._isExcept; }
            set
            {
                if (SetProperty(ref this._isExcept, value))
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

        public void Unloaded()
        {
            if (!this._isLoaded) return;

            this._isLoaded = false;
            RaisePropertyChanged("AggregatedKeys");
        }
    }
}
