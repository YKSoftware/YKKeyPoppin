namespace YKKeyPoppin
{
    using System;
using System.Collections.Generic;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

    /// <summary>
    /// KeyView.xaml の相互作用ロジック
    /// </summary>
    public partial class KeyView : Window
    {
        public KeyView()
        {
            InitializeComponent();
        }

        public KeyView(bool isSpecial)
            : this()
        {
            this.textblock.Foreground = defaultForeground;
        }

        public KeyView(int count)
            : this()
        {
            UpdateComboMessage(count);
            this.textblock.FontStyle = FontStyles.Italic;

            this.Left = App.Instance.Bounds.Right - 400;
            this.Top = App.Instance.Bounds.Top + 80;
            this._comboTimer = new Timer(4000);
            this._comboTimer.Elapsed += (_, __) =>
            {
                this.Dispatcher.BeginInvoke((Action)(() =>
                {
                    this.Visibility = Visibility.Hidden;
                }), DispatcherPriority.SystemIdle, null);

                this._comboTimer.Stop();
            };
        }

        internal static Dictionary<KeyInfo, string> KeysDictionary { get; set; }

        private static SolidColorBrush defaultForeground = new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0x7f, 0x50));
        private static SolidColorBrush excellentForeground = new SolidColorBrush(Color.FromArgb(0xff, 0x26, 0x83, 0xc6));
        private static SolidColorBrush superForeground = new SolidColorBrush(Color.FromArgb(0xff, 0xe3, 0x80, 0x25));
        private static SolidColorBrush amazingForeground = new SolidColorBrush(Color.FromArgb(0xff, 0x5b, 0xe3, 0x2d));
        private static SolidColorBrush godForeground = new SolidColorBrush(Color.FromArgb(0xff, 0xdb, 0xe3, 0x2d));

        public void SetCount(int count)
        {
            if (count < 2)
            {
                this.Visibility = Visibility.Hidden;
            }
            else
            {
                this._comboTimer.Stop();
                this._comboTimer.Start();
                this.Visibility = Visibility.Visible;
                UpdateComboMessage(count);

                if (count > 20)
                {
                    var x = (0.6 * rand.NextDouble() + 0.2) * this.ActualWidth;
                    var y = (0.6 * rand.NextDouble() + 0.2) * this.ActualHeight;
                    PoppinStars.LetsPoppin(this.grid, new Point(x, y));
                }
            }
        }

        private Timer _comboTimer;

        private void UpdateComboMessage(int count)
        {
            var str = string.Format(count.ToString() + " Combo");
            if (count <= 0)
            {
                str = "";
                this.textblock.Foreground = defaultForeground;
            }
            else if (count > 200)
            {
                str += "\r\nOH MY GOD!!";
                this.textblock.Foreground = godForeground;
            }
            else if (count > 100)
            {
                str += "\r\nAmazing!";
                this.textblock.Foreground = amazingForeground;
            }
            else if (count > 50)
            {
                str += "\r\nSuper!";
                this.textblock.Foreground = superForeground;
            }
            else if (count > 20)
            {
                str += "\r\nExcellent!";
                this.textblock.Foreground = excellentForeground;
            }
            else if (count > 10)
            {
                str += "\r\nGreat!";
                this.textblock.Foreground = defaultForeground;
            }
            else
            {
                this.textblock.Foreground = defaultForeground;
            }
            this.textblock.Text = str;
        }

        private static readonly Random rand = new Random();

        public void SetString(User32.VKs key, ModifierKeys modifierkeys)
        {
            this.textblock.Text = KeysDictionary != null ? KeysDictionary.GetString(new KeyInfo() { Key = key, ModifierKeys = modifierkeys }) : key.ChangeString();
            this.Left = key.GetPosition();
            this.Top = App.Instance.Bounds.Bottom;
            this.Opacity = 0;

            var distance = 500;
            var appearTime = 500;
            var disappearTime = 200;

            var storyboard = new Storyboard();

            var shiftAnimation = new DoubleAnimation();
            shiftAnimation.By = 5.0 * (KeyView.rand.NextDouble() - 0.5);
            shiftAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(appearTime + disappearTime / 2.0));
            Storyboard.SetTarget(shiftAnimation, this);
            Storyboard.SetTargetProperty(shiftAnimation, new PropertyPath("Left"));
            storyboard.Children.Add(shiftAnimation);

            var moveAnimation = new DoubleAnimation();
            moveAnimation.From = App.Instance.Bounds.Bottom;
            moveAnimation.To = App.Instance.Bounds.Bottom - distance;
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

            storyboard.Completed += (s, e) =>
            {
                this.Dispatcher.BeginInvoke((Action)(() =>
                {
                    this.Close();
                }), DispatcherPriority.SystemIdle, null);
            };
            storyboard.Begin(this);
        }
    }
}
