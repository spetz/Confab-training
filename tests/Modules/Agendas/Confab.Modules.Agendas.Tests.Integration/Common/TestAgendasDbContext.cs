using System;
using Confab.Modules.Agendas.Infrastructure.EF;
using Confab.Shared.Tests;

namespace Confab.Modules.Agendas.Tests.Integration.Common
{
    public class TestAgendasDbContext : IDisposable
    {
        public AgendasDbContext DbContext { get; }

        public TestAgendasDbContext()
        {
            DbContext = new AgendasDbContext(DbHelper.Options<AgendasDbContext>());
        }

        public void Dispose()
        {
            DbContext?.Database?.EnsureDeleted();
            DbContext?.Dispose();
        }
    }
}