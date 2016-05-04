using NtErp.Shared.Services.Base;

namespace NtErp.Shared.Entities.MasterFileData {
    /// <summary>
    /// 	Class representing the junction between <see cref="Product" /> 
    ///		and a Product Kit, that holds additional metadata.
    /// </summary>
    public class ProductComponent : EntityBase {
        private Product _product;
        private Product _component;
        private int _amount;

        /// <summary>
        /// 	Reference to the Kit that contains a component/product
        /// </summary>
        public virtual Product Product {
            get { return _product; }
            set { _product = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 	Reference to the product/component of the referenced kit
        /// </summary>
        public virtual Product Component {
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

        public ProductComponent() {

        }
    }
}