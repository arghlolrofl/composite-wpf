using Autofac;
using System.Diagnostics;

namespace NtErp.Modules.Base {
    public class BaseModuleConfiguration : Module {
        protected override void Load(ContainerBuilder builder) {
            base.Load(builder);

            builder.RegisterType<BaseModule>();
            builder.RegisterType<ViewModels.ProductViewModel>();
            builder.RegisterType<Views.ProductView>();
            builder.RegisterType<Shared.Services.ViewModels.MenuItemViewModel>()
                   .As<Shared.Contracts.Infrastructure.IMenuItemViewModel>();

            Debug.WriteLine(" > MODULE LOADED: " + nameof(BaseModule));
        }
    }
}
