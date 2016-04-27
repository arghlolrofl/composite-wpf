using NtErp.Shared.DataAccess;
using NtErp.Shared.Entities.MasterFileData;
using NtErp.Shared.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;

namespace NtErp.Shared.Repositories {
    public class ProductRepository : IProductRepository {
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
        ///   Private EF context instance
        /// </summary>
        private NtErpContext _context;

        /// <summary>
        ///   Constructor
        /// </summary>
        /// <param name="context"></param>
        public ProductRepository(NtErpContext context) {
            _context = context;
        }

        public Product New(int version = 1) {
            return new Product() { Id = 0, Version = version };
        }

        /// <summary>
        ///   Fetches all <see cref="Product"/>s including detailed information
        /// </summary>
        /// <returns>List of Products</returns>
        public IEnumerable<Product> GetAll() {
            return _context.Products.ToList();
        }

        /// <summary>
        ///   Fetches detailed information for the <see cref="Product"/> with the given id
        /// </summary>
        /// <param name="id">Primary Key</param>
        /// <returns>Single <see cref="Product"/></returns>
        public Product GetSingle(long id) {
            var product = _context.Products.Include(p => p.Components).Single(p => p.Id == id);
            return product;
        }

        /// <summary>
        ///   @TODO
        /// </summary>
        /// <returns>List of <see cref="Product"/>s</returns>
        public IEnumerable<Product> FindAll() {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   @TODO
        /// </summary>
        /// <param name="key">Primary Key</param>
        /// <returns>List of <see cref="Product"/>s</returns>
        public IEnumerable<Product> Find(long key) {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Saves an existing <see cref="Product"/> or creates a new one
        /// </summary>
        /// <param name="entity"><see cref="Product"/></param>
        public void Save(Product entity) {
            if (entity.Id > 0) {
                _context.Products.Attach(entity);
                _context.Entry(entity).State = EntityState.Modified;
            } else {
                _context.Entry(entity).State = EntityState.Added;
            }

            _context.SaveChanges();
        }

        /// <summary>
        ///   Deletes an existing <see cref="Product"/>
        /// </summary>
        /// <param name="entity"><see cref="Product"/></param>
        public void Delete(Product entity) {
            _context.Entry(entity).State = EntityState.Deleted;

            _context.SaveChanges();
        }

        public Product AddComponent(Product kit, Product component) {
            _context.Products.Attach(kit);
            _context.Products.Attach(component);

            _context.Entry(kit).Entity.Components.Add(
                new ProductComponent() { Product = kit, Component = component, Amount = 1 });

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

        #region IDisposable

        private bool disposed = false;

        protected virtual void Dispose(bool disposing) {
            if (!this.disposed) {
                if (disposing) {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
