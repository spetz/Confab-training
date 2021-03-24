using System;
using Confab.Modules.Tickets.Core.DAL;
using Confab.Shared.Tests;

namespace Confab.Modules.Tickets.Tests.Integration.Common
{
    public sealed class TestTicketsDbContext : IDisposable
    {
        public TicketsDbContext DbContext { get; }

        public TestTicketsDbContext()
        {
            DbContext = new TicketsDbContext(DbHelper.Options<TicketsDbContext>());
        }

        public void Dispose()
        {
            DbContext?.Database?.EnsureDeleted();
            DbContext?.Dispose();
        }
    }
}