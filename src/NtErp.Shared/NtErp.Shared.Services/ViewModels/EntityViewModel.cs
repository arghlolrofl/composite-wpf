using Autofac;
using Microsoft.Practices.Prism.Commands;
using NtErp.Shared.Entities.Base;
using Prism.Events;
using Prism.Regions;
using System.Windows.Input;

namespace NtErp.Shared.Services.ViewModels {
    public abstract class EntityViewModel : CommonViewModel {
        #region Commands

        private ICommand _refreshCommand;
        private ICommand _createCommand;
        private ICommand _saveCommand;
        private ICommand _deleteCommand;
        private ICommand _openSearchCommand;

        /// <summary>
        /// Reloads the RootEntity from the database.
        /// </summary>
        public ICommand RefreshCommand {
            get { return _refreshCommand ?? (_refreshCommand = new DelegateCommand(RefreshCommand_OnExecute)); }
        }

        /// <summary>
        /// Creates a new entity.
        /// </summary>
        public ICommand CreateCommand {
            get { return _createCommand ?? (_createCommand = new DelegateCommand(CreateCommand_OnExecute)); }
        }

        /// <summary>
        /// Saves or updates the entity.
        /// </summary>
        public ICommand SaveCommand {
            get { return _saveCommand ?? (_saveCommand = new DelegateCommand(SaveCommand_OnExecute)); }
        }

        /// <summary>
        /// Deletes the entity.
        /// </summary>
        public ICommand DeleteCommand {
            get { return _deleteCommand ?? (_deleteCommand = new DelegateCommand(DeleteCommand_OnExecute)); }
        }

        /// <summary>
        /// Opens an entity search window.
        /// </summary>
        public ICommand OpenSearchCommand {
            get { return _openSearchCommand ?? (_openSearchCommand = new DelegateCommand(OpenSearchCommand_OnExecute)); }
        }

        #endregion


        private EntityBase _rootEntity;
        protected string _nextView;

        /// <summary>
        /// The main entity of every view model.
        /// </summary>
        public EntityBase RootEntity {
            get { return _rootEntity; }
            set {
                _rootEntity = value;
                RaisePropertyChanged();

                RaisePropertyChanged(nameof(HasRootEntity));
                RaisePropertyChanged(nameof(CanCreateNew));
                RaisePropertyChanged(nameof(CanRefresh));
                RaisePropertyChanged(nameof(CanSave));
                RaisePropertyChanged(nameof(CanDelete));

                RefreshEnabledBindings();

                if (RootEntity != null) {
                    RootEntity.PropertyChanged += (sender, args) => {
                        if (args.PropertyName == nameof(RootEntity.HasChanges))
                            RaisePropertyChanged(nameof(CanSave));
                    };
                }
            }
        }

        /// <summary>
        /// Flag if a root entity exists or not.
        /// </summary>
        public virtual bool HasRootEntity {
            get { return RootEntity != null; }
        }

        /// <summary>
        /// Flag, if the user can create a new entity.
        /// </summary>
        public virtual bool CanCreateNew {
            get { return !HasRootEntity || (HasRootEntity && RootEntity.Exists); }
        }

        /// <summary>
        /// Flag, if the user can reload the current entity.
        /// </summary>
        public virtual bool CanRefresh {
            get { return HasRootEntity && RootEntity.Exists; }
        }

        /// <summary>
        /// Flag, if the user can save or update the entity.
        /// </summary>
        public virtual bool CanSave {
            get { return HasRootEntity && RootEntity.HasChanges; }
        }

        /// <summary>
        /// Flag, if the user can delete the current entity.
        /// </summary>
        public virtual bool CanDelete {
            get { return HasRootEntity && RootEntity.Exists; }
        }


        public EntityViewModel(ILifetimeScope scope, IEventAggregator eventAggregator, IRegionManager regionManager)
            : base(scope, eventAggregator, regionManager) {

        }


        protected abstract void RefreshCommand_OnExecute();

        protected abstract void CreateCommand_OnExecute();

        protected abstract void SaveCommand_OnExecute();

        protected abstract void DeleteCommand_OnExecute();

        protected abstract void OpenSearchCommand_OnExecute();

        protected abstract void RefreshEnabledBindings();
    }
}
