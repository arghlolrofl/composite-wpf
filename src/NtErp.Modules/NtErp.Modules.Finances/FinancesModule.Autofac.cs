using Autofac;

namespace NtErp.Modules.Finances {
    /// <summary>
    /// Finances Module definition for IoC-registrations
    /// </summary>
    public class AutofacModule : Module {
        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder) {
            base.Load(builder);

            builder.RegisterType<FinancesModule>();

            builder.RegisterType<ViewModels.TaxRateViewModel>().SingleInstance();
            builder.RegisterType<Views.TaxRateView>();

            builder.RegisterType<ViewModels.CashJournalViewModel>().SingleInstance();
            builder.RegisterType<Views.CashJournalView>();

            builder.RegisterType<ViewModels.CashJournalEntryViewModel>().SingleInstance();
            builder.RegisterType<Views.CashJournalEntryView>();
        }
    }
}
