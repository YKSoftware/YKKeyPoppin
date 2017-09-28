namespace YKKeyPoppin.Models
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization;

    [DataContract(Namespace = "YKKeyPoppin")]
    internal class KeyCollector
    {
        public static KeyCollector Current { get; private set; }

        static KeyCollector()
        {
            Current = new KeyCollector();
        }

        private KeyCollector()
        {
            LoadCollection();
            KeyHook.Current.KeyUp += OnKeyUp;
        }

        private void OnKeyUp(KeyInfo info)
        {
            if (!this._keyCollection.ContainsKey(info)) this._keyCollection.Add(info, 0);
            this._keyCollection[info]++;

            this.LatestKeys.Remove(this.LatestKeys.LastOrDefault());
            this.LatestKeys.Insert(0, info);

            RaiseKeyUp(info);
        }

        public IEnumerable<Dictionary<KeyInfo, int>> AllCollections { get; private set; }

        private Dictionary<KeyInfo, int> _keyCollection = new Dictionary<KeyInfo, int>();
        [DataMember]
        public Dictionary<KeyInfo, int> KeyCollection
        {
            get { return this._keyCollection; }
        }

        private List<KeyInfo> _latestKeys = new List<KeyInfo>(5);
        public List<KeyInfo> LatestKeys
        {
            get { return this._latestKeys; }
        }

        public IEnumerable<KeyValuePair<KeyInfo, int>> LatestKeyInfo
        {
            get { return this.LatestKeys.Count > 0 ? this.LatestKeys.Select(x => new KeyValuePair<KeyInfo, int>(x, this.KeyCollection[x])) : Enumerable.Empty<KeyValuePair<KeyInfo, int>>(); }
        }

        internal delegate void KeyUpHandler(KeyInfo info);

        public event KeyUpHandler KeyUp;

        private void RaiseKeyUp(KeyInfo info)
        {
            var h = this.KeyUp;
            if (h != null) h(info);
        }

        public void SaveCollection()
        {
            FileStream stream = null;
            try
            {
                stream = new FileStream(DateTime.Now.ToString("yyyyMMddHHmmss") + KeyHitCountFilePath, FileMode.Create, FileAccess.Write);
                var serializer = new DataContractSerializer(typeof(Dictionary<KeyInfo, int>));
                serializer.WriteObject(stream, this.KeyCollection);
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

        private void LoadCollection()
        {
            FileStream stream = null;
            try
            {
                var files = Directory.GetFiles(".", "*" + KeyHitCountFilePath);
                if (files.Any())
                {
                    this.AllCollections = files.Select(x =>
                    {
                        stream = new FileStream(x, FileMode.Open, FileAccess.Read);
                        var serializer = new DataContractSerializer(typeof(Dictionary<KeyInfo, int>));
                        return serializer.ReadObject(stream) as Dictionary<KeyInfo, int>;
                    }).ToArray();
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

        public const string KeyHitCountFilePath = ".hitkeys";
    }
}
