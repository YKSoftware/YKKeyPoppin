namespace YKKeyPoppin.Models
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Timers;

    internal class KeyConf
    {
        public static KeyConf Current { get; private set; }

        static KeyConf()
        {
            Current = new KeyConf();
        }

        private KeyConf()
        {
            this._updateTimer.Elapsed += OnUpdateTimerElapsed;
        }

        private Dictionary<KeyInfo, string> _keyDictionary = new Dictionary<KeyInfo,string>();

        public void UpdateDictionary(KeyInfo info, string str)
        {
            if (!this._keyDictionary.ContainsKey(info)) this._keyDictionary.Add(info, str);
            else this._keyDictionary[info] = str;

            this._updateTimer.Stop();
            this._updateTimer.Start();
        }

        private void OnUpdateTimerElapsed(object sender, ElapsedEventArgs e)
        {
            this._updateTimer.Stop();
            SaveConf();
        }

        private Timer _updateTimer = new Timer(1000);

        public bool ContainsKey(KeyInfo info)
        {
            return this._keyDictionary.ContainsKey(info);
        }

        public string this[KeyInfo info]
        {
            get
            {
                return this._keyDictionary[info];
            }
        }

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

        public const string KeyCountFilePath = "key.conf";
    }
}
