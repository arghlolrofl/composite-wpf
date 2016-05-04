using NtErp.Shared.Entities.Base;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;

namespace NtErp.Shared.Services.Base {
    public abstract class EntityBase : INotifyPropertyChanged {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([CallerMemberName]string propertyName = "") {
            updateHasChanges(propertyName);

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private bool _hasChanges;
        private IList<string> _propertiesToTrack = new List<string>();
        protected HashSet<TrackedProperty> _trackedProperties = new HashSet<TrackedProperty>();


        #region Properties

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [NotMapped]
        public bool HasChanges {
            get { return Id > 0 ? _hasChanges : true; }
            set { _hasChanges = value; RaisePropertyChanged(); }
        }

        [NotMapped]
        public bool Exists {
            get { return Id > 0; }
        }


        #endregion

        protected void trackProperties(params string[] propertyNames) {
            foreach (string propertyName in propertyNames)
                _propertiesToTrack.Add(propertyName);
        }

        private void updateHasChanges(string propertyName) {
            if (!_propertiesToTrack.Contains(propertyName))
                return;

            object newValue = GetType().GetProperty(propertyName).GetValue(this);

            if (!_trackedProperties.Any(tp => tp.Name == propertyName)) {
                _trackedProperties.Add(new TrackedProperty(propertyName, newValue));
                return;
            }

            TrackedProperty trackedProperty = _trackedProperties.Single(tp => tp.Name == propertyName);

            if (trackedProperty.Value.Equals(newValue))
                trackedProperty.HasChanged = false;
            else
                trackedProperty.HasChanged = true;

            bool hasChanges = _trackedProperties.Any(tp => tp.HasChanged);
            if (HasChanges != hasChanges)
                HasChanges = hasChanges;
        }

        public void UpdateTrackedProperties() {
            _trackedProperties.Clear();

            foreach (string propertyName in _propertiesToTrack) {
                object currentValue = GetType().GetProperty(propertyName).GetValue(this);
                _trackedProperties.Add(new TrackedProperty(propertyName, currentValue));
            }

            HasChanges = false;
        }
    }
}