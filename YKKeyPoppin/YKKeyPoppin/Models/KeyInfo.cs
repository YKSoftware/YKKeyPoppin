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
            return this.GetString();
        }
    }
}
