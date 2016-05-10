using Autofac;
using System.Diagnostics;

namespace NtErp.Modules.Finances {
    public class FinancesModuleConfiguration : Module {
        protected override void Load(ContainerBuilder builder) {
            base.Load(builder);

            builder.RegisterType<FinancesModule>();

            builder.RegisterType<ViewModels.CashJournalViewModel>();
            builder.RegisterType<Views.CashJournalView>();

            builder.RegisterType<ViewModels.CashJournalEntryViewModel>();
            builder.RegisterType<Views.CashJournalEntryView>();

            builder.RegisterType<Shared.Services.ViewModels.MenuItemViewModel>()
                   .As<Shared.Contracts.Infrastructure.IMenuItemViewModel>();

            Debug.WriteLine(" > MODULE LOADED: " + nameof(FinancesModule));
        }
    }
}
