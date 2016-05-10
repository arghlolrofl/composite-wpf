namespace NtErp.Shared.Services.ViewModels {
    public abstract class EntityViewModelBase : ViewModelBase {
        protected string _nextView;

        public virtual bool CanCreateNew {
            get { return !HasRootEntity && (HasRootEntity && RootEntity.Id > 0); }
        }

        public virtual bool CanRefresh {
            get { return HasRootEntity && RootEntity.Id > 0; }
        }

        public override bool CanSave {
            get { return HasRootEntity && RootEntity.HasChanges; }
        }

        public virtual bool CanDelete {
            get { return HasRootEntity && RootEntity.Id > 0; }
        }


    }
}
