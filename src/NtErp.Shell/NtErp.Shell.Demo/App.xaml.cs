using NtErp.Shared.Repositories;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace NtErp.Shell.Demo {
    public partial class App : Application {
        protected override void OnStartup(StartupEventArgs e) {
            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(
                    System.Windows.Markup.XmlLanguage.GetLanguage(CultureInfo.CurrentUICulture.IetfLanguageTag)));

            EventManager.RegisterClassHandler(typeof(TextBox),
                TextBox.GotFocusEvent,
                new RoutedEventHandler(TextBox_GotFocus));

            base.OnStartup(e);

            ProductRepository.Prepare();

            Bootstrapper bootstrap = new Bootstrapper();
            bootstrap.Run();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e) {
            (sender as TextBox).SelectAll();
        }
    }
}
