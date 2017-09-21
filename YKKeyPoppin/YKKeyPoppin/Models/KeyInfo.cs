namespace YKKeyPoppin
{
    using System.Runtime.Serialization;
    using System.Windows.Input;
    using YKToolkit.Controls;

    [DataContract(Namespace = "YKKeyPoppin")]
    internal struct KeyInfo
    {
        [DataMember]
        public User32.VKs Key { get; set; }

        [DataMember]
        public ModifierKeys ModifierKeys { get; set; }

        public override string ToString()
        {
            if (this.Key.IsModifierKey()) return this.Key.ChangeString();

            var str = "";
            if (this.ModifierKeys.HasFlag(ModifierKeys.Control)) str += "Ctrl+";
            if (this.ModifierKeys.HasFlag(ModifierKeys.Alt)) str += "Alt+";
            if (this.ModifierKeys.HasFlag(ModifierKeys.Shift)) str += "Shift+";
            if (this.ModifierKeys.HasFlag(ModifierKeys.Windows)) str += "Win+";
            str += Key.ChangeString();
            return str;
        }
    }
}
