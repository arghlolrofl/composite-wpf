using System;

namespace NtErp.Shell.JscriptSuite {
    static class Program {

        [STAThread]
        static void Main(string[] args) {
            WebBootstrapper bootstrapper = new WebBootstrapper();
            bootstrapper.Run();
        }
    }
}
