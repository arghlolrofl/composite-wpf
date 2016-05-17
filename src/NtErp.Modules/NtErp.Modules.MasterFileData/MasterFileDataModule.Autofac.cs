using Autofac;

namespace NtErp.Modules.MasterFileData {
    public class AutofacModule : Module {
        protected override void Load(ContainerBuilder builder) {
            base.Load(builder);

            builder.RegisterType<MasterFileDataModule>();

            builder.RegisterType<ViewModels.ProductViewModel>().SingleInstance();
            builder.RegisterType<Views.ProductView>();

            builder.RegisterType<ViewModels.TaxRateViewModel>().SingleInstance();
            builder.RegisterType<Views.TaxRateView>();
        }
    }
}
