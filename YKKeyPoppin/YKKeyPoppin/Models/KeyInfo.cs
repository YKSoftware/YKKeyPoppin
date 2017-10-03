namespace YKKeyPoppin
{
    using System.Runtime.Serialization;
    using System.Windows.Input;
    using YKToolkit.Controls;

    /// <summary>
    /// キー情報を表します。
    /// </summary>
    [DataContract(Namespace = "YKKeyPoppin")]
    internal struct KeyInfo
    {
        /// <summary>
        /// キーを取得または設定します。
        /// </summary>
        [DataMember]
        public User32.VKs Key { get; set; }

        /// <summary>
        /// 修飾キーを取得または設定します。
        /// </summary>
        [DataMember]
        public ModifierKeys ModifierKeys { get; set; }

        /// <summary>
        /// 文字列に変換します。
        /// </summary>
        /// <returns>変換された文字列を返します。</returns>
        public override string ToString()
        {
            return this.GetString();
        }
    }
}
