namespace YKKeyPoppin.ViewModels
{
    using System.Collections.Generic;
    using YKKeyPoppin.Models;
    using YKToolkit.Bindings;

    internal class KeyConfViewModel : NotificationObject, IMenuContentViewModel
    {
        private DelegateCommand _inputCommand;
        public DelegateCommand InputCommand
        {
            get
            {
                if (this._inputCommand == null)
                {
                    this._inputCommand = new DelegateCommand(
                    p =>
                    {
                        var pair = (KeyValuePair<KeyInfo, string>)p;
                        KeyConf.Current.UpdateDictionary(pair.Key, pair.Value);

                        this.InputKey = pair.Key.ChangeString();
                        this.InputText = pair.Value;
                    },
                    _ => this.IsEnabled);
                }
                return this._inputCommand;
            }
        }

        private string _inputKey;
        public string InputKey
        {
            get { return this._inputKey; }
            private set { SetProperty(ref this._inputKey, value); }
        }

        private string _inputText;
        public string InputText
        {
            get { return this._inputText; }
            private set { SetProperty(ref this._inputText, value); }
        }

        private bool _isEnabled;
        public bool IsEnabled
        {
            get { return this._isEnabled; }
            private set
            {
                if (SetProperty(ref this._isEnabled, value))
                {
                    this.InputCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public void Loaded()
        {
            this.IsEnabled = true;
        }

        public void Unloaded()
        {
            this.IsEnabled = false;
        }
    }
}
