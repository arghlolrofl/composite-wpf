namespace NtErp.Shared.Contracts.Entity.MasterFileData {
    public interface ITaxRate : IEntityBase {
        string Category { get; set; }
        string Description { get; set; }
        decimal Value { get; set; }
    }
}