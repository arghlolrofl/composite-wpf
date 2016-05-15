using Autofac.Core;

namespace NtErp.Shared.Services.Events {
  public class ModuleLoadEvent {
    public Autofac.Module Module { get; set; }
    public IComponentRegistry ComponentRegistry { get; set; }

    public ModuleLoadEvent(Autofac.Module module, IComponentRegistry reg) {
      Module = module;
      ComponentRegistry = reg;
    }
  }
}
