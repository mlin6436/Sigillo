using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sigillo.Entity.Entities;

namespace Sigillo.Entity
{
    public class SigilloEntity : DbContext
    {
        public DbSet<Consent> Consent { get; set; }

        public SigilloEntity(string nameOrConnectionString)
            : base(nameOrConnectionString)
        { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingEntitySetNameConvention>();
        }

        public void DropAndCreate(string server, string database, string userId, string password)
        {
            var connectionString = String.Format("Data Source={0};Integrated Security=SSPI;Initial Catalog={1};User Id={2};Password={3};MultipleActiveResultSets=True", database, server, userId, password);
            var entity = new SigilloEntity(connectionString);

            try
            {
                if (entity.Database.Exists())
                {
                    entity.Database.ExecuteSqlCommand(String.Format("ALTER DATABASE {0} SET SINGLE_USER", database));
                    entity.Database.Delete();
                }
                entity.Database.Create();
                Initialisation(entity);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                entity.Database.ExecuteSqlCommand(String.Format("ALTER DATABASE {0} SET MULTI_USER", database));
                entity.Dispose();
            }
        }

        private void Initialisation(SigilloEntity entity)
        { }
    }
}
