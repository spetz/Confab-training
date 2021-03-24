using System;
using Confab.Shared.Tests;
using Microsoft.EntityFrameworkCore;

namespace Confab.Tests.EndToEnd.Common
{
    public class TestDbContext : IDisposable
    {
        public Context DbContext { get; }

        public TestDbContext()
        {
            DbContext = new Context(DbHelper.Options<Context>());
        }

        public void Dispose()
        {
            DbContext?.Database?.EnsureDeleted();
            DbContext?.Dispose();
        }

        public class Context : DbContext
        {
            public Context(DbContextOptions<Context> options) : base(options)
            {
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
            }
        }
    }
}