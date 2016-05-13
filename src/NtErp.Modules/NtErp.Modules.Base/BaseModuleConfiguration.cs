using Autofac;
using System.Diagnostics;

namespace NtErp.Modules.Base {
    public class BaseModuleConfiguration : Module {
        protected override void Load(ContainerBuilder builder) {
            base.Load(builder);

            builder.RegisterType<BaseModule>();

            builder.RegisterType<ViewModels.ProductViewModel>();
            builder.RegisterType<Views.ProductView>();

            builder.RegisterType<ViewModels.TaxRateViewModel>();
            builder.RegisterType<Views.TaxRateView>();

            Debug.WriteLine(" > MODULE LOADED: " + nameof(BaseModule));
        }
    }
}
