namespace YKKeyPoppin.Models
{
    using System.Collections.Generic;

    internal class KeyCollector
    {
        public KeyCollector()
        {
            KeyHook.Current.KeyUp += OnKeyUp;
        }

        private void OnKeyUp(KeyInfo info)
        {
            if (!this._keyCollection.ContainsKey(info)) this._keyCollection.Add(info, 0);
            this._keyCollection[info]++;
        }

        private Dictionary<KeyInfo, int> _keyCollection = new Dictionary<KeyInfo, int>();
        public Dictionary<KeyInfo, int> KeyCollection
        {
            get { return this._keyCollection; }
        }
    }
}
