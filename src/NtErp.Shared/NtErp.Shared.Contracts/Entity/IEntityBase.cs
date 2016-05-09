using System.ComponentModel;

namespace NtErp.Shared.Contracts.Entity {
    public interface IEntityBase {
        event PropertyChangedEventHandler PropertyChanged;

        bool Exists { get; }
        bool HasChanges { get; set; }
        long Id { get; set; }

        void ResetScalarProperties();
    }
}