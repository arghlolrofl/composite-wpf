using NtErp.Shared.Repositories;
using System.Windows;

namespace NtErp.Shell.Demo {
    public partial class App : Application {
        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);

            ProductRepository.Prepare();

            Bootstrapper bootstrap = new Bootstrapper();
            bootstrap.Run();
        }
    }
}
