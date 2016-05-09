using NtErp.Shared.Services.Base;
using System.Collections.ObjectModel;

namespace NtErp.Shared.Entities.MasterFileData {
    /// <summary>
    /// 	Class representing a <see cref="Product" /> or a part of a <see cref="Kit" />.
    /// </summary>
    public class Product : EntityBase {
        private int _version;
        private string _number;
        private string _name;
        private string _description;
        private ObservableCollection<ProductComponent> _components = new ObservableCollection<ProductComponent>();

        /// <summary>
        /// 	Natural number representing the version of this Kit
        /// </summary>
        public int Version {
            get { return _version; }
            set { _version = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 	Identifier string, e.g.: Code, Serial Number or EAN
        /// </summary>
        public string Number {
            get { return _number; }
            set { _number = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 	Kit name
        /// </summary>
        public string Name {
            get { return _name; }
            set { _name = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 	Kit description
        /// </summary>
        public string Description {
            get { return _description; }
            set { _description = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 	Reference to a junction table (with metadata/additional attributes)
        ///		representing the components of this kit.
        /// </summary>
        public virtual ObservableCollection<ProductComponent> Components {
            get { return _components; }
            set { _components = value; RaisePropertyChanged(); }
        }


        protected override void RegisterPropertiesToTrack() {
            TrackProperties(nameof(Version), nameof(Number), nameof(Name), nameof(Description));
        }
    }
}
