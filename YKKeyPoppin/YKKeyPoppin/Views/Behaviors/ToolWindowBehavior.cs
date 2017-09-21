namespace YKKeyPoppin.Views.Behaviors
{
    using System;
    using System.Windows;
    using System.Windows.Interop;
    using YKToolkit.Controls;

    internal class ToolWindowBehavior
    {
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(ToolWindowBehavior), new PropertyMetadata(false, OnIsEnabledPropertyChanged));

        public static bool GetIsEnabled(DependencyObject target)
        {
            return (bool)target.GetValue(IsEnabledProperty);
        }

        public static void SetIsEnabled(DependencyObject target, bool value)
        {
            target.SetValue(IsEnabledProperty, value);
        }

        private static void OnIsEnabledPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var window = sender as System.Windows.Window;
            if (window == null) return;

            window.ShowInTaskbar = false;

            window.SourceInitialized += (obj, args) =>
            {
                var w = obj as System.Windows.Window;
                if (w == null) return;

                var handle = (new WindowInteropHelper(w)).Handle;
                var original = User32.GetWindowLongPtr(handle, (int)User32.GWLs.GWL_EXSTYLE).ToInt32();
                var current = original | (int)User32.WSs.WS_EX_TOOLWINDOW;
                User32.SetWindowLongPtr(handle, (int)User32.GWLs.GWL_EXSTYLE, new IntPtr(current));
            };
        }
    }
}
