namespace YKKeyPoppin.ViewModels
{
    using System.Timers;
    using System.Linq;
    using YKKeyPoppin.Models;
    using YKToolkit.Bindings;
    using YKToolkit.Controls;
    using YKToolkit.Helpers;
    using System.Collections.Generic;

    internal class ComboViewModel : NotificationObject
    {
        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        public ComboViewModel()
        {
            KeyHook.Current.KeyUp += OnKeyUp;
            this._comboTimer.Elapsed += OnComboTimerElapsed;
        }

        /// <summary>
        /// キーが離されたときのイベントハンドラ
        /// </summary>
        /// <param name="info">キー情報</param>
        private void OnKeyUp(KeyInfo info)
        {
            this.Combo++;
            if (info.Key == User32.VKs.VK_RETURN)
            {
                this._isChain = true;
            }
            else if (this._isChain)
            {
                // 連続でエンターキーが押されてもチェインしないようにあえて else if で判定している。
                this.Chain++;
                this._isChain = false;
            }
            this._comboTimer.Restart();
            RaisePropertyChanged("Top5Keys");
        }

        /// <summary>
        /// コンボ継続タイマーのイベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行元</param>
        /// <param name="e">イベント引数</param>
        private void OnComboTimerElapsed(object sender, ElapsedEventArgs e)
        {
            this._comboTimer.Stop();
            this.Combo = 0;
            this.Chain = 0;
        }

        /// <summary>
        /// タイトルを取得します。
        /// </summary>
        public string Title { get { return this._productInfo.Title; } }

        /// <summary>
        /// コンボ成立かどうかを取得します。
        /// </summary>
        public bool IsCombo { get { return this.Combo > 1; } }

        /// <summary>
        /// コンボレベルを取得します。
        /// </summary>
        public int ComboLevel
        {
            get
            {
                if (this.Combo < 10) return 0;
                if (this.Combo < 20) return 1;
                if (this.Combo < 50) return 2;
                if (this.Combo < 100) return 3;
                if (this.Combo < 200) return 4;
                return 100;
            }
        }

        private int _combo;
        /// <summary>
        /// コンボ数を取得します。
        /// </summary>
        public int Combo
        {
            get { return this._combo; }
            private set
            {
                if (SetProperty(ref this._combo, value))
                {
                    RaisePropertyChanged("IsCombo");
                    RaisePropertyChanged("ComboLevel");
                }
            }
        }

        private int _chain;
        /// <summary>
        /// チェイン数を取得します。
        /// </summary>
        public int Chain
        {
            get { return this._chain; }
            private set { SetProperty(ref this._chain, value); }
        }

        private ProductInfo _productInfo = new ProductInfo(true);

        /// <summary>
        /// メッセージ表示消失タイマー
        /// </summary>
        private Timer _messageTimer = new Timer(4000);

        /// <summary>
        /// コンボ継続タイマー
        /// </summary>
        private Timer _comboTimer = new Timer(600);

        /// <summary>
        /// チェイントリガ記憶
        /// </summary>
        private bool _isChain;

        /// <summary>
        /// キー集計
        /// </summary>
        private KeyCollector _collector = new KeyCollector();

        /// <summary>
        /// 押された回数上位 5 個のキーをインデックス付きで取得します。
        /// </summary>
        public IEnumerable<KeyValuePair<int, KeyValuePair<KeyInfo, int>>> Top5Keys
        {
            get
            {
                return this._collector.KeyCollection.OrderByDescending(x => x.Value).Where(x => !x.Key.Key.IsModifierKey()).Take(5).Select((x, i) => new KeyValuePair<int, KeyValuePair<KeyInfo, int>>(i + 1, x));
            }
        }

        private DelegateCommand _collectCommand;
        /// <summary>
        /// 集計コマンドを取得します。
        /// </summary>
        public DelegateCommand CollectCommand
        {
            get
            {
                return this._collectCommand ?? (this._collectCommand = new DelegateCommand(_ =>
                {
                    // 別ウィンドウで詳細情報を表示する
                }));
            }
        }

        private DelegateCommand _confCommand;
        /// <summary>
        /// 設定コマンドを取得します。
        /// </summary>
        public DelegateCommand ConfCommand
        {
            get
            {
                return this._confCommand ?? (this._confCommand = new DelegateCommand(_ =>
                {
                    // 別ウィンドウで設定を表示する
                }));
            }
        }
    }
}
