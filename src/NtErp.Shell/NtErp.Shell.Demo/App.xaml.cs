using System.Windows;

namespace NtErp.Shell.Demo {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);

            Bootstrapper bootstrap = new Bootstrapper();
            bootstrap.Run();
        }
    }
}
