using NtErp.Shared.Contracts.Repository;
using NtErp.Shared.DataAccess;
using NtErp.Shared.Entities.MasterFileData;
using NtErp.Shared.Services.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;

namespace NtErp.Shared.Repositories {
    public class ProductRepository : RepositoryBase<Product>, IProductRepository {
        /// <summary>
        ///   Dev: Startup integrity check
        ///   
        ///   @TODO: Once basic entity relations and related stuff is fixed,
        ///          we need to find a secure way to verify the integrity/availability
        ///          of the data access layer.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        ///   Constructor
        /// </summary>
        /// <param name="context"></param>
        public ProductRepository(NtErpContext context) : base(context) {

        }

        public Product New(int version = 1) {
            return new Product() { Id = 0, Version = version };
        }

        /// <summary>
        ///   Fetches all <see cref="Product"/>s including detailed information
        /// </summary>
        /// <returns>List of Products</returns>
        public override IEnumerable<Product> Fetch(int maxResultCount = -1) {
            if (maxResultCount < 0)
                return _context.Products.ToList();
            else
                return _context.Products.Take(maxResultCount).ToList();
        }

        /// <summary>
        ///   Fetches detailed information for the <see cref="Product"/> with the given id
        /// </summary>
        /// <param name="id">Primary Key</param>
        /// <returns>Single <see cref="Product"/></returns>
        public override Product Find(long id) {
            var product = _context.Products.Include(p => p.Components).Single(p => p.Id == id);
            return product;
        }

        /// <summary>
        ///   Saves an existing <see cref="Product"/> or creates a new one
        /// </summary>
        /// <param name="entity"><see cref="Product"/></param>
        public override void Save(EntityBase entity) {
            Product p = null;

            if (entity.Exists && ((p = entity as Product) != null))
                _context.Products.Attach(p);

            base.Save(entity);
        }

        public Product AddComponent(Product kit, Product component) {
            _context.Products.Attach(kit);
            _context.Products.Attach(component);
            _context.Components.Add(new ProductComponent() { Product = kit, Component = component, Amount = 1 });

            _context.SaveChanges();

            return kit;
        }

        public Product RemoveComponent(ProductComponent componentToDelete) {
            var product = componentToDelete.Product;

            _context.Components.Attach(componentToDelete);
            _context.Entry(componentToDelete).State = EntityState.Deleted;
            _context.SaveChanges();

            return product;
        }
    }
}
