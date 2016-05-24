using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Input;

namespace NtErp.Shell.Demo.ViewModels {
    public partial class ShellViewModel {
        public event EventHandler CloseWindowRequested;
        private void RaiseCloseWindowRequested() => CloseWindowRequested?.Invoke(this, EventArgs.Empty);


        static readonly IList<string> SupportedLanguages = new List<string>() {
            "en-US", "de-DE"
        };


        #region Commands

        private ICommand _applyCommand;
        private ICommand _cancelCommand;


        public ICommand ApplyCommand {
            get { return _applyCommand ?? (_applyCommand = new DelegateCommand(ApplyCommand_OnExecute)); }
        }

        public ICommand CancelCommand {
            get { return _cancelCommand ?? (_cancelCommand = new DelegateCommand(CancelCommand_OnExecute)); }
        }

        #endregion


        private ObservableCollection<CultureInfo> _availableCultures;
        private CultureInfo _selectedCulture;


        #region Properties

        public ObservableCollection<CultureInfo> AvailableCultures {
            get { return _availableCultures; }
            set { _availableCultures = value; RaisePropertyChanged(); }
        }

        public CultureInfo SelectedCulture {
            get { return _selectedCulture; }
            set { _selectedCulture = value; RaisePropertyChanged(); }
        }

        #endregion


        private void InitializeSettings() {
            AvailableCultures = new ObservableCollection<CultureInfo>(
                CultureInfo.GetCultures(CultureTypes.AllCultures).Where(
                    ci => SupportedLanguages.Contains(ci.Name)
                ).ToList()
            );

            SelectedCulture = AvailableCultures.Single(c => c.Name == App.CurrentCulture.Name);
        }


        private void ApplyCommand_OnExecute() {
            if (SelectedCulture.Name != App.CurrentCulture.Name)
                App.SaveCultureInfoToRegistry(SelectedCulture);

            RaiseCloseWindowRequested();
        }

        private void CancelCommand_OnExecute() {
            RaiseCloseWindowRequested();
        }
    }
}
