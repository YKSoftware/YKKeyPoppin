namespace YKKeyPoppin
{
    using System;
    using System.Windows;
    using System.Windows.Media.Animation;

    /// <summary>
    /// KeyView.xaml の相互作用ロジック
    /// </summary>
    public partial class KeyView : Window
    {
        public KeyView()
        {
            InitializeComponent();
        }

        private static readonly Random rand = new Random();

        public void SetString(User32.VKs key)
        {
            this.textblock.Text = key.ChangeString();
            this.Left = key.GetPosition();
            this.Top = App.Instance.Bounds.Bottom;

            var appearTime = 300;
            var disappearTime = 200;

            var storyboard = new Storyboard();

            var shiftAnimation = new DoubleAnimation();
            shiftAnimation.By = 5.0 * (KeyView.rand.NextDouble() - 0.5);
            shiftAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(appearTime + disappearTime / 2.0));
            Storyboard.SetTarget(shiftAnimation, this);
            Storyboard.SetTargetProperty(shiftAnimation, new PropertyPath("Left"));
            storyboard.Children.Add(shiftAnimation);

            var moveAnimation = new DoubleAnimation();
            //moveAnimation.By = -20;// -App.Instance.Bounds.Height / 5.0;
            moveAnimation.From = App.Instance.Bounds.Bottom;
            moveAnimation.To = App.Instance.Bounds.Bottom - 200;
            moveAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(appearTime));
            moveAnimation.AccelerationRatio = 0.6;
            Storyboard.SetTarget(moveAnimation, this);
            Storyboard.SetTargetProperty(moveAnimation, new PropertyPath("Top"));
            storyboard.Children.Add(moveAnimation);

            var opacityAnimation = new DoubleAnimation(1.0, new Duration(TimeSpan.FromMilliseconds(appearTime/4)));
            Storyboard.SetTarget(opacityAnimation, this);
            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath("Opacity"));
            storyboard.Children.Add(opacityAnimation);

            var downAnimation = new DoubleAnimation();
            downAnimation.By = 20;
            downAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(disappearTime));
            downAnimation.BeginTime = TimeSpan.FromMilliseconds(appearTime);
            Storyboard.SetTarget(downAnimation, this);
            Storyboard.SetTargetProperty(downAnimation, new PropertyPath("Top"));
            storyboard.Children.Add(downAnimation);

            var closingOpacityAnimation = new DoubleAnimation(0.0, new Duration(TimeSpan.FromMilliseconds(disappearTime)));
            closingOpacityAnimation.BeginTime = TimeSpan.FromMilliseconds(appearTime);
            Storyboard.SetTarget(closingOpacityAnimation, this);
            Storyboard.SetTargetProperty(closingOpacityAnimation, new PropertyPath("Opacity"));
            storyboard.Children.Add(closingOpacityAnimation);

            storyboard.Completed += (s, e) => this.Close();
            storyboard.Begin(this);
        }
    }
}
