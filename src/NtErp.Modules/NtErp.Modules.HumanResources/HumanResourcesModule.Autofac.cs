using Autofac;

namespace NtErp.Modules.HumanResources {
    public class AutofacModule : Module {
        protected override void Load(ContainerBuilder builder) {
            base.Load(builder);

            builder.RegisterType<HumanResourcesModule>();

            //builder.RegisterType<ViewModels.ProductViewModel>().SingleInstance();
            //builder.RegisterType<Views.ProductView>();

            //builder.RegisterType<ViewModels.TaxRateViewModel>().SingleInstance();
            //builder.RegisterType<Views.TaxRateView>();
        }
    }
}
