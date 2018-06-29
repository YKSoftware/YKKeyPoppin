namespace YKKeyPoppin.Models
{
    using System;
    using System.IO;
    using System.Runtime.Serialization;
    using YKToolkit.Controls;

    /// <summary>
    /// マウス情報収集をおこなうクラスを表します。
    /// </summary>
    [DataContract(Namespace = "YKKeyPoppin.Models")]
    internal class MouseCollector
    {
        public static MouseCollector Current { get; private set; }

        /// <summary>
        /// 静的なコンストラクタを表します。
        /// </summary>
        static MouseCollector()
        {
            Current = new MouseCollector();
        }

        /// <summary>
        /// プライベートなコンストラクタを定義することで外部からの生成を抑止します。
        /// </summary>
        private MouseCollector()
        {
            this._mouseHook.MouseMove += OnMouseMove;
            this._mouseHook.MouseLeftButtonUp += OnMouseLeftButtonUp;
            this._mouseHook.MouseRightButtonUp += OnMouseRightButtonUp;
            this._mouseHook.DoubleClick += OnDoubleClick;
            this._mouseHook.MouseMiddleButtonUp += OnMouseMiddleButtonUp;
            this._mouseHook.MouseWheel += OnMouseWheel;
            this._mouseHook.Hook();
        }

        /// <summary>
        /// MouseMove イベントハンドラ
        /// </summary>
        /// <param name="e">イベント引数</param>
        private void OnMouseMove(MouseHook.MSLLHOOKSTRUCT e)
        {
            if (this._point != null)
            {
                var distance = e.Point.CalcDistance(this._point.Value);
                if (distance > App.Instance.MinimumDistance)
                {
                    this.TotalDistance += distance;
                    this._point = e.Point;
                }
                RaiseValueChanged();
            }
            else
            {
                this._point = e.Point;
            }
        }

        /// <summary>
        /// MouseLeftButtonUp イベントハンドラ
        /// </summary>
        /// <param name="e">イベント引数</param>
        private void OnMouseLeftButtonUp(MouseHook.MSLLHOOKSTRUCT e)
        {
            this.LeftButtonCount++;
            RaiseValueChanged();
        }

        /// <summary>
        /// MouseRightButtonUp イベントハンドラ
        /// </summary>
        /// <param name="e">イベント引数</param>
        private void OnMouseRightButtonUp(MouseHook.MSLLHOOKSTRUCT e)
        {
            this.RightButtonCount++;
            RaiseValueChanged();
        }

        /// <summary>
        /// DoubleClick イベントハンドラ
        /// </summary>
        /// <param name="e">イベント引数</param>
        private void OnDoubleClick(MouseHook.MSLLHOOKSTRUCT e)
        {
            this.DoubleClickCount++;
            RaiseValueChanged();
        }

        /// <summary>
        /// MouseMiddleButtonUp イベントハンドラ
        /// </summary>
        /// <param name="e">イベント引数</param>
        private void OnMouseMiddleButtonUp(MouseHook.MSLLHOOKSTRUCT e)
        {
            this.MiddleButtonCount++;
            RaiseValueChanged();
        }

        /// <summary>
        /// MouseWheel イベントハンドラ
        /// </summary>
        /// <param name="e">イベント引数</param>
        private void OnMouseWheel(MouseHook.MSLLHOOKSTRUCT e)
        {
            var wheelDelta = (short)((e.MouseData >> 16) & 0x00ffff);
            if (wheelDelta > 0) this.WheelUpCount++;
            else this.WheelDownCount++;
            RaiseValueChanged();
        }

        #region ValueChanged イベント

        /// <summary>
        /// ValueChanged イベントハンドラのデリゲートを表します。
        /// </summary>
        public delegate void OnValueChanged();

        /// <summary>
        /// 各種情報が更新されたときに発生します。
        /// </summary>
        public event OnValueChanged ValueChagned;

        /// <summary>
        /// ValueChanged イベントを発行します。
        /// </summary>
        private void RaiseValueChanged()
        {
            var h = this.ValueChagned;
            if (h != null) h();
        }

        #endregion ValueChanged イベント

        /// <summary>
        /// 収集データを保存します。
        /// </summary>
        public void Save()
        {
            FileStream stream = null;
            try
            {
                if (!Directory.Exists(MouseInfoFileDirectory)) Directory.CreateDirectory(MouseInfoFileDirectory);
                stream = new FileStream(MouseInfoFileDirectory + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + MouseInfoFileExtension, FileMode.Create, FileAccess.Write);
                var serializer = new DataContractSerializer(typeof(MouseCollector));
                serializer.WriteObject(stream, this);
            }
            catch (Exception ex)
            {
                if (stream != null)
                {
                    stream.Dispose();
                }
                stream = new FileStream("log.txt", FileMode.Create, FileAccess.Write);
                using (var writer = new StreamWriter(stream))
                {
                    stream = null;
                    writer.Write(ex);
                }
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

        /// <summary>
        /// これまでの収集データを読み込みます。
        /// </summary>
        //private void Load()
        //{
        //    var files = Directory.GetFiles(".", "*" + MouseInfoFilePath);
        //    if (files.Any())
        //    {
        //        this.AllCollections = files.Select(x =>
        //        {
        //            FileStream stream = null;
        //            try
        //            {
        //                stream = new FileStream(x, FileMode.Open, FileAccess.Read);
        //                var serializer = new DataContractSerializer(typeof(Dictionary<KeyInfo, int>));
        //                return serializer.ReadObject(stream) as Dictionary<KeyInfo, int>;
        //            }
        //            catch (Exception ex)
        //            {
        //                System.Diagnostics.Debug.WriteLine(ex);
        //                return null;
        //            }
        //            finally
        //            {
        //                if (stream != null)
        //                {
        //                    stream.Dispose();
        //                    stream = null;
        //                }
        //            }
        //        }).ToArray();
        //    }
        //}

        /// <summary>
        /// マウス走破距離
        /// </summary>
        [DataMember]
        public double TotalDistance { get; private set; }

        /// <summary>
        /// 左ボタンクリック回数を取得します。
        /// </summary>
        [DataMember]
        public int LeftButtonCount { get; private set; }

        /// <summary>
        /// 右ボタンクリック回数を取得します。
        /// </summary>
        [DataMember]
        public int RightButtonCount { get; private set; }

        /// <summary>
        /// ホイールボタンクリック回数を取得します。
        /// </summary>
        [DataMember]
        public int MiddleButtonCount { get; private set; }

        /// <summary>
        /// ダブルクリック回数を取得します。
        /// </summary>
        [DataMember]
        public int DoubleClickCount { get; private set; }

        /// <summary>
        /// マウスホイールを上に上げた回数を取得します。
        /// </summary>
        [DataMember]
        public int WheelUpCount { get; private set; }

        /// <summary>
        /// マウスホイールを下に下げた回数を取得します。
        /// </summary>
        [DataMember]
        public int WheelDownCount { get; private set; }

        /// <summary>
        /// マウスフック
        /// </summary>
        private MouseHook _mouseHook = new MouseHook();

        /// <summary>
        /// 直前のマウス座標
        /// </summary>
        private User32.POINT? _point;

        /// <summary>
        /// マウス収集データ保存先フォルダ名
        /// </summary>
        public const string MouseInfoFileDirectory = "mouse";

        /// <summary>
        /// マウス収集データのファイル拡張子
        /// </summary>
        public const string MouseInfoFileExtension = ".mouse";
    }
}
