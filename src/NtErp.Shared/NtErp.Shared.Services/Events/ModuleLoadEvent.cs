using Autofac;

namespace NtErp.Shared.Services.Events {
    public class ModuleLoadEvent {
        public Module Module { get; set; }

        public ModuleLoadEvent(Module module) {
            Module = module;
        }
    }
}
