namespace NtErp.Shared.Entities.Base {
    public class TrackedProperty {
        public string Name { get; set; }
        public object Value { get; set; }
        public bool HasChanged { get; set; }

        public TrackedProperty(string name, object value) {
            Name = name;
            Value = value;
        }
    }
}
