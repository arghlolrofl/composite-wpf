using NtErp.Shared.Services.Events;
using Prism.Events;
using Prism.Logging;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Linq;

namespace NtErp.Shared.Services.Modules {
    public abstract class ModuleBase : IModule {
        protected IEventAggregator _eventAggregator;
        protected ILoggerFacade _logger;
        protected IRegionManager _regionManager;


        public ModuleBase(ILoggerFacade logger, IEventAggregator eventAggregator, IRegionManager regionManager) {
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;
            _logger = logger;
        }

        /// <summary>
        /// Initializes and loads a Prism module.
        /// </summary>
        public virtual void Initialize() {
            Log(" ==> Loading " + GetType().Name);

            Log("   > Updating IoC container");
            RequestContainerUpdate();

            Log("   > Registering views");
            RegisterViews();

            Log("   > " + GetType().Name + " loaded successfully");
        }

        /// <summary>
        /// Updates the IoC-Container at runtime, when a module is being loaded,
        /// with the module registrations.
        /// </summary>
        protected virtual void RequestContainerUpdate() {
            Type[] assemblyTypes = GetType().Assembly.GetTypes();
            Type type = assemblyTypes.Single(t => t.IsSubclassOf(typeof(Autofac.Module)));

            Autofac.Module module = (Autofac.Module)type.GetConstructor(new Type[0]).Invoke(null);

            _eventAggregator.GetEvent<PubSubEvent<ModuleLoadEvent>>()
                            .Publish(new ModuleLoadEvent(module));
        }

        /// <summary>
        /// Override in inherited classes to register all views of the module with
        /// Prism's RegionManager.
        /// </summary>
        protected abstract void RegisterViews();

        /// <summary>
        /// Prints module messages to debug output.
        /// </summary>
        /// <param name="message"></param>
        protected void Log(string message) {
            _logger.Log("MODULE: " + message, Category.Debug, Priority.Low);
        }
    }
}
