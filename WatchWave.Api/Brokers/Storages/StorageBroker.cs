//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================

using Microsoft.EntityFrameworkCore;
using STX.EFxceptions.SqlServer;

namespace WatchWave.Api.Brokers.Storages
{
    internal partial class StorageBroker : EFxceptionsContext, IStorageBroker
    {
        private readonly IConfiguration configuration;

        public StorageBroker(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.Database.Migrate();
        }

        public async ValueTask<T> InsertAsync<T>(T @object)
        {
            using var broker = new StorageBroker(this.configuration);
            broker.Entry(@object).State = EntityState.Added;
            await broker.SaveChangesAsync();

            return @object;
        }

        public IQueryable<T> SelectAll<T>() where T : class
        {
            using var broker = new StorageBroker(this.configuration);

            return broker.Set<T>();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString =
                configuration.GetConnectionString(name: "DefaultConnection");

            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            optionsBuilder.UseSqlServer(connectionString);
        }

        public override void Dispose()
        { }
    }
}
