using NtErp.Shared.Entities.Base;

namespace NtErp.Shared.Entities.MasterFileData {
	/// <summary>
	/// 	Class representing the junction between <see cref="Product" /> 
	///		and a <see cref="Kit" />, that holds additional metadata.
	/// </summary>
    public class ProductComponent : EntityBase {
        private Kit _kit;
        private Product _component;
        private int _amount;

		/// <summary>
		/// 	Reference to the Kit that contains a component/product
		/// </summary>
        public Kit Kit {
            get { return _kit; }
            set { _kit = value; RaisePropertyChanged(); }
        }

		/// <summary>
		/// 	Reference to the product/component of the referenced kit
		/// </summary>
        public Product Component {
            get { return _component; }
            set { _component = value; RaisePropertyChanged(); }
        }
		
		/// <summary>
		/// 	The amount of that exact product/component as part(s)
		///		in the referenced kit
		/// </summary>
        public int Amount {
            get { return _amount; }
            set { _amount = value; RaisePropertyChanged(); }
        }

    }
}