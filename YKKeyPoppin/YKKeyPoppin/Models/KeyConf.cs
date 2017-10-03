namespace YKKeyPoppin.Models
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Timers;

    /// <summary>
    /// キー設定を表します。
    /// </summary>
    internal class KeyConf
    {
        /// <summary>
        /// 現在のインスタンスを取得します。
        /// </summary>
        public static KeyConf Current { get; private set; }

        /// <summary>
        /// 静的なコンストラクタ
        /// </summary>
        static KeyConf()
        {
            Current = new KeyConf();
        }

        /// <summary>
        /// プライベートはコンストラクタを定義することで外部からのインスタンス生成を抑止します。
        /// </summary>
        private KeyConf()
        {
            this._updateTimer.Elapsed += OnUpdateTimerElapsed;
        }

        /// <summary>
        /// キーに対する文字列を紐付けるディクショナリを更新します。
        /// </summary>
        /// <param name="info">キー情報を指定します。</param>
        /// <param name="str">キー情報に対する文字列を指定します。</param>
        public void UpdateDictionary(KeyInfo info, string str)
        {
            if (!this._keyDictionary.ContainsKey(info)) this._keyDictionary.Add(info, str);
            else this._keyDictionary[info] = str;

            this._updateTimer.Stop();
            this._updateTimer.Start();
        }

        /// <summary>
        /// キーに対する文字列を紐付けるディクショナリを自動保存するためのタイマーイベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行元</param>
        /// <param name="e">イベント引数</param>
        private void OnUpdateTimerElapsed(object sender, ElapsedEventArgs e)
        {
            this._updateTimer.Stop();
            SaveConf();
        }

        /// <summary>
        /// 既に登録されたキー情報かどうかを取得します。
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool ContainsKey(KeyInfo info)
        {
            return this._keyDictionary.ContainsKey(info);
        }

        /// <summary>
        /// 登録されたキー情報に紐付けされた文字列を取得します。
        /// </summary>
        /// <param name="info">登録されたキー情報を指定します。</param>
        /// <returns>登録情報に基づいた文字列を返します。</returns>
        public string this[KeyInfo info]
        {
            get
            {
                return this._keyDictionary[info];
            }
        }

        /// <summary>
        /// キー設定をファイルに保存します。
        /// </summary>
        public void SaveConf()
        {
            System.Diagnostics.Debug.WriteLine("SaveConf");
            FileStream stream = null;
            try
            {
                stream = new FileStream(KeyCountFilePath, FileMode.Create, FileAccess.Write);
                var serializer = new DataContractSerializer(typeof(Dictionary<KeyInfo, string>));
                serializer.WriteObject(stream, this._keyDictionary);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
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
        /// ファイルに保存されたキー設定を読み込みます。
        /// </summary>
        public void LoadConf()
        {
            System.Diagnostics.Debug.WriteLine("LoadConf");
            if (!File.Exists(KeyCountFilePath)) return;
            FileStream stream = null;
            try
            {
                stream = new FileStream(KeyCountFilePath, FileMode.Open, FileAccess.Read);
                var serializer = new DataContractSerializer(typeof(Dictionary<KeyInfo, string>));
                this._keyDictionary = serializer.ReadObject(stream) as Dictionary<KeyInfo, string>;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
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
        /// キーに対する文字列を紐付けるディクショナリ
        /// </summary>
        private Dictionary<KeyInfo, string> _keyDictionary = new Dictionary<KeyInfo, string>();

        /// <summary>
        /// キーに対する文字列を紐付けるディクショナリを自動保存するためのタイマー
        /// </summary>
        private Timer _updateTimer = new Timer(1000);

        /// <summary>
        /// キー設定を保存/読込するときのファイル名
        /// </summary>
        public const string KeyCountFilePath = "key.conf";
    }
}
