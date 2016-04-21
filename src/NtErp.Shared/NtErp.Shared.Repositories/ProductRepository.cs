using NtErp.Shared.DataAccess;
using NtErp.Shared.Entities.MasterFileData;
using NtErp.Shared.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NtErp.Shared.Repositories {
    public class ProductRepository : IProductRepository {
        public static bool Prepare() {
            try {
                using (var context = new NtErpContext()) {
                    //context.Database.CreateIfNotExists();

                    //if (!context.Database.CompatibleWithModel(true))
                    //    Debug.WriteLine("Database model is not compatible with class structure!");

                    Debug.WriteLine("PRODUCT REPOSITORY");
                    Debug.WriteLine("    [Entity] Products: " + context.Products.Count());
                }
            } catch (Exception ex) {
                Debug.WriteLine(ex.GetType().Name + ": " + ex.Message);
                throw;
            }

            return true;
        }

        public IEnumerable<Product> FindAll() {
            throw new NotImplementedException();
        }

        public Product Get(long id) {
            throw new NotImplementedException();
        }

        public void Save(Product entity) {
            throw new NotImplementedException();
        }

        public void Delete(Product entity) {
            throw new NotImplementedException();
        }
    }
}
