﻿namespace YKKeyPoppin.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using YKKeyPoppin.Models;
    using YKToolkit.Bindings;

    internal class Top5KeysViewModel : NotificationObject, IMenuContentViewModel
    {
        public Top5KeysViewModel()
        {
            KeyCollector.Current.KeyUp += _ => RaisePropertyChanged("Top5Keys");
        }

        /// <summary>
        /// 押された回数上位 5 個のキーをインデックス付きで取得します。
        /// </summary>
        public IEnumerable<KeyValuePair<int, KeyValuePair<KeyInfo, int>>> Top5Keys
        {
            get
            {
                return KeyCollector.Current.KeyCollection.OrderByDescending(x => x.Value).Take(5).Select((x, i) => new KeyValuePair<int, KeyValuePair<KeyInfo, int>>(i + 1, x));
            }
        }

        private bool _isPoppinEnabled = true;
        /// <summary>
        /// Let's Poppin' するかどうかを取得または設定します。
        /// </summary>
        public bool IsPoppinEnabled
        {
            get { return this._isPoppinEnabled; }
            set
            {
                if (SetProperty(ref this._isPoppinEnabled, value))
                {
                    (App.Current as App).IsPoppinEnabled = this._isPoppinEnabled;
                }
            }
        }

        public void Loaded()
        {
        }

        public void Unloaded()
        {
        }
    }
}
