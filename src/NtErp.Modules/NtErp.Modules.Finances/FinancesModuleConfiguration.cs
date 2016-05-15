using Autofac;
using System.Diagnostics;

namespace NtErp.Modules.Finances {
  public class FinancesModuleConfiguration : Module {
    protected override void Load(ContainerBuilder builder) {
      base.Load(builder);

      builder.RegisterType<FinancesModule>();

      builder.RegisterType<ViewModels.CashJournalViewModel>().SingleInstance();
      builder.RegisterType<Views.CashJournalView>();

      builder.RegisterType<ViewModels.CashJournalEntryViewModel>();
      builder.RegisterType<Views.CashJournalEntryView>();

      Debug.WriteLine(" > MODULE LOADED: " + nameof(FinancesModule));
    }
  }
}
