namespace YKKeyPoppin.Models
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization;

    /// <summary>
    /// キー収集をおこなうクラスを表します。
    /// </summary>
    [DataContract(Namespace = "YKKeyPoppin.Models")]
    internal class KeyCollector
    {
        /// <summary>
        /// 現在のインスタンスを取得します。
        /// </summary>
        public static KeyCollector Current { get; private set; }

        /// <summary>
        /// 静的なコンストラクタを表します。
        /// </summary>
        static KeyCollector()
        {
            Current = new KeyCollector();
        }

        /// <summary>
        /// プライベートなコンストラクタを定義することで外部からのインスタンス生成を抑止します。
        /// </summary>
        private KeyCollector()
        {
            LoadCollection();
            KeyHook.Current.KeyUp += OnKeyUp;
        }

        /// <summary>
        /// KeyHook.KeyUp イベントハンドラ
        /// </summary>
        /// <param name="info">離されたキーの情報を指定します。</param>
        private void OnKeyUp(KeyInfo info)
        {
            if (info.Key.IsModifierKey())
            {
                var modifierKey = info.Key.ToModifierKeys();
                info.ModifierKeys = info.ModifierKeys & ~modifierKey;
            }
            if (!this._keyCollection.ContainsKey(info)) this._keyCollection.Add(info, 0);
            this._keyCollection[info]++;

            RaiseKeyUp(info);
        }

        /// <summary>
        /// 過去に収集したすべてのデータを取得します。
        /// </summary>
        public IEnumerable<KeyLogData> AllCollections { get; private set; }

        private Dictionary<KeyInfo, int> _keyCollection = new Dictionary<KeyInfo, int>();
        /// <summary>
        /// 現在収集しているすべてのデータを取得します。
        /// </summary>
        [DataMember]
        public Dictionary<KeyInfo, int> KeyCollection
        {
            get { return this._keyCollection; }
        }

        /// <summary>
        /// KeyUp イベントハンドラのデリゲート
        /// </summary>
        /// <param name="info"></param>
        internal delegate void KeyUpHandler(KeyInfo info);

        /// <summary>
        /// キーを離したときに発生します。
        /// </summary>
        public event KeyUpHandler KeyUp;

        /// <summary>
        /// KeyUp イベントを発行します。
        /// </summary>
        /// <param name="info"></param>
        private void RaiseKeyUp(KeyInfo info)
        {
            var h = this.KeyUp;
            if (h != null) h(info);
        }

        /// <summary>
        /// 収集データを保存します。
        /// </summary>
        public void SaveCollection()
        {
            FileStream stream = null;
            try
            {
                if (!Directory.Exists(KeyHitCountFileDirectory)) Directory.CreateDirectory(KeyHitCountFileDirectory);
                stream = new FileStream(KeyHitCountFileDirectory + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + KeyHitCountFileExtension, FileMode.Create, FileAccess.Write);
                var serializer = new DataContractSerializer(typeof(Dictionary<KeyInfo, int>));
                serializer.WriteObject(stream, this.KeyCollection);
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
        private void LoadCollection()
        {
            var files = Directory.GetFiles(KeyHitCountFileDirectory, "*" + KeyHitCountFileExtension);
            if (files.Any())
            {

                this.AllCollections = files.Select(x =>
                {
                    var filename = Path.GetFileNameWithoutExtension(x);
                    var dateTimeString = string.Concat(new string[]
                    {
                        filename.Substring(0, 4),
                        "/",
                        filename.Substring(4, 2),
                        "/",
                        filename.Substring(6, 2),
                        //" ",
                        //filename.Substring(8, 2),     // 日付だけにして時刻は無視する
                        //":",
                        //filename.Substring(10, 2),
                        //":",
                        //filename.Substring(12, 2),
                    });
                    DateTime datetime;
                    DateTime.TryParse(dateTimeString, out datetime);
                    Dictionary<KeyInfo, int> log = null;

                    FileStream stream = null;
                    try
                    {
                        stream = new FileStream(x, FileMode.Open, FileAccess.Read);
                        var serializer = new DataContractSerializer(typeof(Dictionary<KeyInfo, int>));
                        log = serializer.ReadObject(stream) as Dictionary<KeyInfo, int>;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex);
                        return null;
                    }
                    finally
                    {
                        if (stream != null)
                        {
                            stream.Dispose();
                            stream = null;
                        }
                    }

                    return new KeyLogData(datetime, log);
                }).GroupBy(x => x.DateTime).Select(x => x.Aggregate((x1, x2) => x1 + x2))   // 同一日付のログデータを集約する
                .ToArray();
            }
        }

        /// <summary>
        /// 収集データ保存先フォルダ名
        /// </summary>
        public const string KeyHitCountFileDirectory = "hitkeys";

        /// <summary>
        /// 収集データのファイル拡張子
        /// </summary>
        public const string KeyHitCountFileExtension = ".hitkeys";
    }

    internal class KeyLogData
    {
        public KeyLogData(DateTime datetime, Dictionary<KeyInfo, int> log)
        {
            this.DateTime = datetime;
            this.Log = log;
        }

        public int TotalHits { get { return this.Log.Sum(x => x.Value); } }
        public DateTime DateTime { get; private set; }
        public Dictionary<KeyInfo, int> Log { get; private set; }

        public static KeyLogData operator +(KeyLogData x, KeyLogData y)
        {
            if (x.DateTime != y.DateTime)
                return null;

            var newLog = x.Log;
            foreach (var log in y.Log)
            {
                if (newLog.ContainsKey(log.Key))
                {
                    newLog[log.Key] += log.Value;
                }
                else
                {
                    newLog.Add(log.Key, log.Value);
                }
            }

            return new KeyLogData(x.DateTime, newLog);
        }
    }
}
