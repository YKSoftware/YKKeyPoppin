namespace YKKeyPoppin.Views
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;

    /// <summary>
    /// PoppinStars.xaml の相互作用ロジック
    /// </summary>
    public partial class PoppinStars : UserControl
    {
        public PoppinStars()
        {
            InitializeComponent();
        }

        private static Random rand = new Random();

        #region 依存関係プロパティ

        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register("Stroke", typeof(Brush), typeof(PoppinStars), new PropertyMetadata(Brushes.Crimson));

        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        public static readonly DependencyProperty FillProperty = DependencyProperty.Register("Fill", typeof(Brush), typeof(PoppinStars), new PropertyMetadata(null));

        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register("StrokeThickness", typeof(double), typeof(PoppinStars), new PropertyMetadata(5.0));

        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        #endregion 依存関係プロパティ

        #region Count 添付プロパティ

        public static readonly DependencyProperty CountProperty = DependencyProperty.RegisterAttached("Count", typeof(int), typeof(PoppinStars), new PropertyMetadata(0, OnCountPropertyChanged));

        public static int GetCount(DependencyObject target)
        {
            return (int)target.GetValue(CountProperty);
        }

        public static void SetCount(DependencyObject target, int value)
        {
            target.SetValue(CountProperty, value);
        }

        private static void OnCountPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var element = sender as FrameworkElement;
            if (element == null) return;

            if (GetCount(element) >= 20)
            {
                var x = (0.6 * rand.NextDouble() + 0.2) * element.ActualWidth;
                var y = (0.6 * rand.NextDouble() + 0.2) * element.ActualHeight;
                PoppinStars.LetsPoppin(element, new Point(x, y));
            }
        }

        #endregion Count 添付プロパティ

        #region Size 添付プロパティ

        public static readonly DependencyProperty SizeProperty = DependencyProperty.RegisterAttached("Size", typeof(Size), typeof(PoppinStars), new PropertyMetadata(new Size(50.0, 50.0)));

        public static Size GetSize(DependencyObject target)
        {
            return (Size)target.GetValue(SizeProperty);
        }

        public static void SetSize(DependencyObject target, Size value)
        {
            target.SetValue(SizeProperty, value);
        }

        #endregion Size 添付プロパティ

        public static void LetsPoppin(UIElement element, Point point)
        {
            var adorner = new PoppinAdorner(element, point);
        }

        internal class PoppinAdorner : Adorner
        {
            public PoppinAdorner(UIElement adornedElement, Point pt)
                : base(adornedElement)
            {
                this._layer = AdornerLayer.GetAdornerLayer(adornedElement);
                var canvas = new Canvas();

                var mainStar = CreateMainStar(canvas, pt);
                canvas.Children.Add(mainStar);

                var rand = new Random();
                Enumerable.Range(0, 6).Select(x => CreateSubStar(canvas, pt, rand)).ToList().ForEach(star =>
                {
                    canvas.Children.Add(star);
                });

                this._adorner = canvas;
                AddVisualChild(this._adorner);
                this._layer.Add(this);
            }

            private AdornerLayer _layer;

            private FrameworkElement _adorner;

            protected override Size MeasureOverride(Size constraint)
            {
                this._adorner.Measure(constraint);
                return this._adorner.DesiredSize;
            }

            protected override Size ArrangeOverride(Size finalSize)
            {
                this._adorner.Arrange(new Rect(this._adorner.DesiredSize));
                return this._adorner.DesiredSize;
            }

            protected override int VisualChildrenCount
            {
                get
                {
                    return 1;
                }
            }

            protected override Visual GetVisualChild(int index)
            {
                return this._adorner;
            }

            /// <summary>
            /// メインの星を生成します。
            /// </summary>
            /// <param name="canvas">親パネルを指定します。</param>
            /// <param name="leftTop">描画する左上点を指定します。</param>
            /// <returns>生成した星を返します。</returns>
            private UIElement CreateMainStar(Canvas canvas, Point leftTop)
            {
                var scaling = 0.6;
                var time = 200;
                var duration = new Duration(TimeSpan.FromMilliseconds(time));
                var size = GetSize(this.AdornedElement);

                var scaleTransform = new ScaleTransform(scaling, scaling);
                var transforms = new TransformGroup();
                transforms.Children.Add(scaleTransform);
                var star = new PoppinStars()
                {
                    Width = size.Width,
                    Height = size.Height,
                    Opacity = 0.0,
                    RenderTransformOrigin = new Point(0.5, 0.5),
                    RenderTransform = transforms,
                    StrokeThickness = 2.0,
                };
                Canvas.SetLeft(star, leftTop.X);
                Canvas.SetTop(star, leftTop.Y);

                star.Loaded += (_, __) =>
                {
                    var translateTransform = new TranslateTransform(-star.ActualWidth / 2.0, -star.ActualHeight / 2.0);
                    transforms.Children.Add(translateTransform);

                    var storyboard = new Storyboard();
                    var opacityAnimation = new DoubleAnimation()
                    {
                        To = 1.0,
                        Duration = duration,
                    };
                    Storyboard.SetTarget(opacityAnimation, star);
                    Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(PoppinStars.OpacityProperty));
                    storyboard.Children.Add(opacityAnimation);

                    var endOpacityAnimation = new DoubleAnimation()
                    {
                        To = 0.0,
                        BeginTime = TimeSpan.FromMilliseconds(time),
                        Duration = duration,
                    };
                    Storyboard.SetTarget(endOpacityAnimation, star);
                    Storyboard.SetTargetProperty(endOpacityAnimation, new PropertyPath(PoppinStars.OpacityProperty));
                    storyboard.Children.Add(endOpacityAnimation);

                    var xScaleAnimation = new DoubleAnimation()
                    {
                        To = 1.0,
                        Duration = duration,
                    };
                    Storyboard.SetTarget(xScaleAnimation, star);
                    Storyboard.SetTargetProperty(xScaleAnimation, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleX)"));
                    storyboard.Children.Add(xScaleAnimation);

                    var yScaleAnimation = new DoubleAnimation()
                    {
                        To = 1.0,
                        Duration = duration,
                    };
                    Storyboard.SetTarget(yScaleAnimation, star);
                    Storyboard.SetTargetProperty(yScaleAnimation, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleY)"));
                    storyboard.Children.Add(yScaleAnimation);

                    storyboard.Completed += (___, ____) =>
                    {
                        canvas.Children.Remove(star);
                        star = null;
                        RemoveAdorner(canvas);
                    };
                    storyboard.Begin();
                };

                return star;
            }

            /// <summary>
            /// サブの星を生成します。
            /// </summary>
            /// <param name="canvas">親パネルを指定します。</param>
            /// <param name="leftTop">描画する左上点を指定します。</param>
            /// <param name="rand">描画するためのランダム要素を決定するための乱数ジェネレータを指定します。</param>
            /// <returns>生成した星を返します。</returns>
            private UIElement CreateSubStar(Canvas canvas, Point leftTop, Random rand)
            {
                var scaling = 0.25;
                var time = 200;
                var duration = new Duration(TimeSpan.FromMilliseconds(400));
                var size = GetSize(this.AdornedElement);

                var scaleTransform = new ScaleTransform(scaling, scaling);
                var transforms = new TransformGroup();
                transforms.Children.Add(scaleTransform);

                var brush = new SolidColorBrush(Color.FromRgb((byte)rand.Next(0x80, 0xff), (byte)rand.Next(0x80, 0xff), (byte)rand.Next(0x80, 0xff)));

                var star = new PoppinStars()
                {
                    Width = size.Width,
                    Height = size.Height,
                    RenderTransformOrigin = new Point(0.5, 0.5),
                    RenderTransform = transforms,
                    Fill = brush,
                    Stroke = brush,
                };
                Canvas.SetLeft(star, leftTop.X);
                Canvas.SetTop(star, leftTop.Y);

                star.Loaded += (_, __) =>
                {
                    var nominal = star.ActualWidth / 2.0;
                    var r = rand.Next(8, 13) / 10.0;
                    var angle = rand.Next(0, 360);
                    var x = r * nominal * Math.Cos(angle);
                    var y = r * nominal * Math.Sin(angle);

                    var rotateTransform = new RotateTransform(angle);
                    transforms.Children.Add(rotateTransform);

                    var translateTransform = new TranslateTransform(-star.ActualWidth / 2.0, -star.ActualHeight / 2.0);
                    transforms.Children.Add(translateTransform);

                    var storyboard = new Storyboard();
                    var xTranslateAnimation = new DoubleAnimation()
                    {
                        By = x,
                        Duration = duration,
                    };
                    Storyboard.SetTarget(xTranslateAnimation, star);
                    Storyboard.SetTargetProperty(xTranslateAnimation, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[2].(TranslateTransform.X)"));
                    storyboard.Children.Add(xTranslateAnimation);

                    var yTranslateAnimation = new DoubleAnimation()
                    {
                        By = y,
                        Duration = duration,
                    };
                    Storyboard.SetTarget(yTranslateAnimation, star);
                    Storyboard.SetTargetProperty(yTranslateAnimation, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[2].(TranslateTransform.Y)"));
                    storyboard.Children.Add(yTranslateAnimation);

                    if (rand.Next() % 3 == 0)
                    {
                        var rotateAnimation = new DoubleAnimation()
                        {
                            By = 300,
                            Duration = duration,
                        };
                        Storyboard.SetTarget(rotateAnimation, star);
                        Storyboard.SetTargetProperty(rotateAnimation, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[1].(RotateTransform.Angle)"));
                        storyboard.Children.Add(rotateAnimation);
                    }

                    var xScaleAnimation = new DoubleAnimation()
                    {
                        To = 0,
                        BeginTime = TimeSpan.FromMilliseconds(time),
                        Duration = duration,
                    };
                    Storyboard.SetTarget(xScaleAnimation, star);
                    Storyboard.SetTargetProperty(xScaleAnimation, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleX)"));
                    storyboard.Children.Add(xScaleAnimation);

                    var yScaleAnimation = new DoubleAnimation()
                    {
                        To = 0,
                        BeginTime = TimeSpan.FromMilliseconds(time),
                        Duration = duration,
                    };
                    Storyboard.SetTarget(yScaleAnimation, star);
                    Storyboard.SetTargetProperty(yScaleAnimation, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleY)"));
                    storyboard.Children.Add(yScaleAnimation);

                    storyboard.Completed += (___, ____) =>
                    {
                        canvas.Children.Remove(star);
                        star = null;
                        RemoveAdorner(canvas);
                    };
                    storyboard.Begin();
                };

                return star;
            }

            private void RemoveAdorner(Canvas canvas)
            {
                if (canvas.Children.Count == 0)
                {
                    this._layer.Remove(this);
                }
            }
        }
    }
}
