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
            CheckHasChanges(propertyName);

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion


        private bool _hasChanges;
        private long _id;
        private IList<string> _propertiesToTrack = new List<string>();
        protected HashSet<TrackedScalarProperty> _trackedProperties = new HashSet<TrackedScalarProperty>();


        #region Properties

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id {
            get { return _id; }
            set {
                _id = value;
                RaisePropertyChanged();
            }
        }

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


        public EntityBase() {
            RegisterPropertiesToTrack();
        }


        protected abstract void RegisterPropertiesToTrack();

        protected void TrackProperties(params string[] propertyNames) {
            foreach (string propertyName in propertyNames)
                _propertiesToTrack.Add(propertyName);
        }

        private void CheckHasChanges(string propertyName) {
            if (!_propertiesToTrack.Contains(propertyName))
                return;

            object newValue = GetType().GetProperty(propertyName).GetValue(this);

            if (!_trackedProperties.Any(tp => tp.Name == propertyName)) {
                _trackedProperties.Add(new TrackedScalarProperty(propertyName, newValue));
                return;
            }

            TrackedScalarProperty trackedProperty = _trackedProperties.Single(tp => tp.Name == propertyName);

            // We have to use different comparison method for decimals:
            //      e.g.: decimal value '19.00' equals '19.0'
            if (trackedProperty.Value.GetType() == typeof(decimal)) {
                string oldDecimalValueString = trackedProperty.Value.ToString();
                string newDecimalValueString = newValue.ToString();

                trackedProperty.HasChanged = !oldDecimalValueString.Equals(newDecimalValueString);
            } else
                trackedProperty.HasChanged = !trackedProperty.Value.Equals(newValue);

            bool hasChanges = _trackedProperties.Any(tp => tp.HasChanged);
            if (HasChanges != hasChanges)
                HasChanges = hasChanges;
        }

        public void ResetChangedProperties() {
            _trackedProperties.Clear();

            foreach (string propertyName in _propertiesToTrack) {
                object currentValue = GetType().GetProperty(propertyName).GetValue(this);
                _trackedProperties.Add(new TrackedScalarProperty(propertyName, currentValue));
            }

            HasChanges = false;
        }
    }
}