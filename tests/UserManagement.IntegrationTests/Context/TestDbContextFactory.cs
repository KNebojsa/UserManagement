using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Infrastructure.Data;

namespace UserManagement.IntegrationTests.Context
{
    public static class TestDbContextFactory
    {
        public static AppDataContext CreateInMemoryContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<AppDataContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new AppDataContext(options);
        }
    }
}
