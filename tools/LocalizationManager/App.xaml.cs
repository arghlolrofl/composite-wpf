﻿using LocalizationManager.Views;
using System.Windows;

namespace LocalizationManager {
    public partial class App : Application {
        private void Application_Startup(object sender, StartupEventArgs e) {
            var window = new MainWindow();
            window.Show();
        }
    }
}
