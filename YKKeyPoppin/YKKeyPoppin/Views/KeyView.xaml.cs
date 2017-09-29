namespace YKKeyPoppin.Views
{
    using System;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using YKKeyPoppin.Models;

    /// <summary>
    /// KeyView.xaml の相互作用ロジック
    /// </summary>
    public partial class KeyView : Window
    {
        public KeyView()
        {
            InitializeComponent();

            this.textblock.Foreground = Brushes.Aqua;
        }

        internal KeyView(bool isExtra)
            :this()
        {
            if (isExtra) this.textblock.Foreground = Brushes.Chocolate;
        }

        internal void Poppin(KeyInfo info)
        {
            this._keyInfo = info;
            this.textblock.FontSize = 44;
            this.textblock.Text = info.ToString();
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= OnLoaded;
            CreateAndStartAnimation();
        }

        private void CreateAndStartAnimation()
        {
            var unit = Settings.Current.Bounds.Width / 17.0;
            this.Left = Settings.Current.Bounds.Left + this._keyInfo.Key.GetPosition() * unit;
            this.Top = Settings.Current.Bounds.Bottom;

            this._storyboard = new Storyboard();
            this._storyboard.Completed += OnCompleted;

            var xMoveAnimation = new DoubleAnimation()
            {
                From = this.Left,
                To = this.Left + 2.0 * unit * (random.NextDouble() - 0.5),
                Duration = new Duration(TimeSpan.FromMilliseconds(800)),
                EasingFunction = new SineEase()
                {
                    EasingMode = EasingMode.EaseInOut,
                },
            };
            Storyboard.SetTarget(xMoveAnimation, this);
            Storyboard.SetTargetProperty(xMoveAnimation, new PropertyPath("Left"));

            var yMoveAnimation = new DoubleAnimation()
            {
                From = this.Top,
                To = this.Top - Settings.Current.Jump,
                Duration = new Duration(TimeSpan.FromMilliseconds(400)),
                //EasingFunction = new ElasticEase()
                //{
                //    EasingMode = EasingMode.EaseOut,
                //    Oscillations = 1,
                //    Springiness = 4,
                //},
                EasingFunction = new SineEase()
                {
                    EasingMode = EasingMode.EaseInOut,
                },
                AutoReverse = true,
            };
            Storyboard.SetTarget(yMoveAnimation, this);
            Storyboard.SetTargetProperty(yMoveAnimation, new PropertyPath("Top"));

            // Opacity プロパティのアニメーションは動作がもっさりするのでマシンパワーが強いときだけ使ったほうがよい
            //var opacityAnimation = new DoubleAnimation()
            //{
            //    From = 1,
            //    To = 0,
            //    Duration = new Duration(TimeSpan.FromMilliseconds(300)),
            //    BeginTime = TimeSpan.FromMilliseconds(400),
            //};
            //Storyboard.SetTarget(opacityAnimation, this);
            //Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath("Opacity"));
            //this._storyboard.Children.Add(opacityAnimation);

            this._storyboard.Children = new TimelineCollection()
            {
                xMoveAnimation,
                yMoveAnimation,
            };
            this._storyboard.Begin(this);
        }

        private void OnCompleted(object sender, EventArgs e)
        {
            this.Dispatcher.BeginInvoke((Action)(() =>
            {
                this._storyboard.Completed -= OnCompleted;
                this._storyboard = null;
                this.Close();
            }));
        }

        private KeyInfo _keyInfo;

        private static Random random = new Random();

        private Storyboard _storyboard;
    }
}
