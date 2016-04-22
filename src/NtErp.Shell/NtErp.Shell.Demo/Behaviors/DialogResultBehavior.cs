using System.Windows;

namespace NtErp.Shell.Demo.Behaviors {
    public class DialogResultBehavior {
        public static readonly DependencyProperty DialogResultProperty = DependencyProperty.RegisterAttached(
            "DialogResult", typeof(bool?), typeof(DialogResultBehavior),
            new PropertyMetadata(DialogResultPropertyChanged)
        );

        private static void DialogResultPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e) {
            var childWindow = dependencyObject as Window;
            if (childWindow != null)
                childWindow.DialogResult = e.NewValue as bool?;
        }

        public static bool? GetDialogResult(Window childWindow) {
            return childWindow.GetValue(DialogResultProperty) as bool?;
        }

        public static void SetDialogResult(Window childWindow, bool? value) {
            childWindow.SetValue(DialogResultProperty, value);
        }
    }
}