using Microsoft.Win32;
using NtErp.Shared.Repositories;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using WPFLocalizeExtension.Engine;
using WPFLocalizeExtension.Extensions;

namespace NtErp.Shell.Demo {
    public partial class App : Application {
        const string AppRegistryKey = "SOFTWARE\\NT\\NT ERP";
        const string AppRegistryValueName_CultureInfo = "Culture";

        public static CultureInfo CurrentCulture {
            get { return CultureInfo.CurrentCulture; }
        }

        protected override void OnStartup(StartupEventArgs e) {
            CultureInfo appCulture = GetCultureInfoFromRegistry();
            SetCulture(appCulture);

            EventManager.RegisterClassHandler(typeof(TextBox),
                TextBox.GotFocusEvent,
                new RoutedEventHandler(TextBox_GotFocus));

            base.OnStartup(e);

            ProductRepository.Prepare();

            Bootstrapper bootstrap = new Bootstrapper();
            bootstrap.Run();
        }

        private static void SetCulture(CultureInfo cultureInfo) {
            FrameworkElement.LanguageProperty.OverrideMetadata(
                    typeof(FrameworkElement),
                    new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(cultureInfo.IetfLanguageTag)));

            CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo;

            LocalizeDictionary.Instance.SetCurrentThreadCulture = true;
            LocalizeDictionary.Instance.Culture = cultureInfo;

            Debug.WriteLine("[LOC] Application culture set to: " + cultureInfo.IetfLanguageTag);
        }

        private CultureInfo GetCultureInfoFromRegistry() {
            // Create a Subkey
            RegistryKey newKey = Registry.CurrentUser.CreateSubKey(AppRegistryKey, false);

            object regValue = newKey.GetValue(AppRegistryValueName_CultureInfo);
            if (regValue == null)
                return CultureInfo.CurrentCulture;

            return CultureInfo.GetCultures(CultureTypes.AllCultures).Single(c => c.Name == regValue.ToString());
        }

        internal static void SaveCultureInfoToRegistry(CultureInfo selectedCulture) {
            CultureInfo currentCulture = LocalizeDictionary.Instance.Culture;

            LocalizeDictionary.Instance.Culture = selectedCulture;

            RegistryKey newKey = Registry.CurrentUser.CreateSubKey(AppRegistryKey, true);
            newKey.SetValue(AppRegistryValueName_CultureInfo, selectedCulture.Name);

            MessageBox.Show(GetLocalizedString("Text_RestartAfterSettingsChanged"));

            LocalizeDictionary.Instance.Culture = currentCulture;
        }

        public static string GetLocalizedString(string key) {
            string uiString;
            LocTextExtension locExtension = new LocTextExtension(key);
            locExtension.ResolveLocalizedValue(out uiString);
            return uiString;
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e) {
            (sender as TextBox).SelectAll();
        }
    }
}
