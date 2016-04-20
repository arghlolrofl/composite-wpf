﻿using NtErp.Shared.Entities.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NtErp.Shared.Entities.MasterFileData {
	/// <summary>
	/// 	Class representing a <see cref="Product" /> or a part of a <see cref="Kit" />.
	/// </summary>
    public class Product : EntityBase {
        private int _Version;
        private string _Number;
        private string _Name;
        private string _Description;
        private ICollection<ProductComponent> _components = new HashSet<ProductComponent>();
		
		/// <summary>
		/// 	Natural number representing the version of this Kit
		/// </summary>
        public int Version {
            get { return _Version; }
            set { _Version = value; RaisePropertyChanged(); }
        }

		/// <summary>
		/// 	Identifier string, e.g.: Code, Serial Number or EAN
		/// </summary>
        public string Number {
            get { return _Number; }
            set { _Number = value; RaisePropertyChanged(); }
        }

		/// <summary>
		/// 	Kit name
		/// </summary>
        public string Name {
            get { return _Name; }
            set { _Name = value; RaisePropertyChanged(); }
        }

		/// <summary>
		/// 	Kit description
		/// </summary>
        public string Description {
            get { return _Description; }
            set { _Description = value; RaisePropertyChanged(); }
        }
		
		/// <summary>
		/// 	Reference to a junction table (with metadata/additional attributes)
		///		representing the components of this kit.
		/// </summary>
        public ICollection<ProductComponent> Components {
            get { return _components; }
            set { _components = value; RaisePropertyChanged(); }
        }
    }
}
