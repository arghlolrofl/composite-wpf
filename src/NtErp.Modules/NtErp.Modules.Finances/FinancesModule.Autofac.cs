using Autofac;

namespace NtErp.Modules.Finances {
    public class AutofacModule : Module {
        protected override void Load(ContainerBuilder builder) {
            base.Load(builder);

            builder.RegisterType<FinancesModule>();

            builder.RegisterType<ViewModels.CashJournalViewModel>().SingleInstance();
            builder.RegisterType<Views.CashJournalView>();

            builder.RegisterType<ViewModels.CashJournalEntryViewModel>().SingleInstance();
            builder.RegisterType<Views.CashJournalEntryView>();
        }
    }
}
