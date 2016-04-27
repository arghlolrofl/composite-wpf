namespace NtErp.Shared.Services.Events {
    public class EntitySearchResultEvent {
        public long EntityId { get; set; }
        public bool? DialogResult { get; set; }

        public EntitySearchResultEvent(long entityId, bool? dialogResult) {
            EntityId = entityId;
            DialogResult = dialogResult;
        }
    }
}