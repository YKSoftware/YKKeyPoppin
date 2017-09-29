namespace YKKeyPoppin.Models
{
    using System.Windows;
    using Forms = System.Windows.Forms;

    /// <summary>
    /// システム設定を集約したシングルトンクラスです。
    /// </summary>
    internal class Settings
    {
        #region シングルトンクラス

        /// <summary>
        /// 現在のインスタンスを取得します。
        /// </summary>
        public static Settings Current { get; private set; }

        /// <summary>
        /// 静的なコンストラクタ
        /// </summary>
        static Settings()
        {
            Current = new Settings();
        }

        /// <summary>
        /// プライベートなコンストラクタ
        /// </summary>
        private Settings()
        {
            var bound = Forms.Screen.PrimaryScreen.Bounds;
            this.Bounds = new Rect(new Point(bound.Left, bound.Top), new Size(bound.Width, bound.Height));
            this.Jump = 300;
        }

        #endregion シングルトンクラス

        /// <summary>
        /// プライマリスクリーンのスクリーン座標およびサイズを取得します。
        /// </summary>
        public Rect Bounds { get; private set; }

        /// <summary>
        /// キーストリングのジャンプ力を取得または設定します。
        /// </summary>
        public double Jump { get; set; }
    }
}
