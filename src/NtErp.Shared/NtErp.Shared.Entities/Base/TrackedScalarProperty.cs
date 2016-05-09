namespace NtErp.Shared.Entities.Base {
    public class TrackedScalarProperty {
        public string Name { get; set; }
        public object Value { get; set; }
        public bool HasChanged { get; set; }

        public TrackedScalarProperty(string name, object value) {
            Name = name;
            Value = value;
        }
    }
}
