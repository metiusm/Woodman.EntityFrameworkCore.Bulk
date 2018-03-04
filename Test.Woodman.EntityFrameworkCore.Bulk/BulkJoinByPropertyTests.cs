using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Woodman.EntityFrameworkCore.DbScaffoldRunner.Generated.Sql;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Test.Woodman.EntityFrameworkCore.DbScaffoldRunner.Generated.NpgSql;
using System;
using System.Linq.Expressions;

namespace Test.Woodman.EntityFrameworkCore.Bulk
{
    public class BulkJoinByPropertyTests : BulkTestBase
    {
        public override string Name => nameof(BulkJoinByPropertyTests);

        public override int InMemId => 41;

        [Fact(DisplayName = "In Mem")]
        public async Task JoinsByPropertyInMem()
        {
            var expectedIds = InMemIds.Take(5).ToList();

            List<int> actualIds;

            using (var db = new woodmanContext(InMemDbOpts))
            {
                actualIds = await db.EfCoreTest
                    .Join(PropertyFilter<EfCoreTest>.As(e => e.Id, expectedIds))
                    .Select(e => e.Id)
                    .ToListAsync();
            }

            Assert.Equal(expectedIds.Count, actualIds.Count);

            foreach (var expected in expectedIds)
            {
                Assert.Contains(expected, actualIds);
            }
        }

        [Fact(DisplayName = "Sql")]
        public async Task JoinsByPropertySql()
        {
            List<EfCoreTest> expectedEntities;

            using(var db = new woodmanContext())
            {
                expectedEntities = await db.EfCoreTest
                    .Join(SqlIds.Take(5).ToList())
                    .ToListAsync();
            }

            List<EfCoreTest> actualEntities;

            var idQuery = expectedEntities.Select(e => (object)e.Id);
            var nameQuery = expectedEntities.Select(e => (object)e.Name);

            using (var db = new woodmanContext())
            {
                actualEntities = await db.EfCoreTest
                    .Join(
                        PropertyFilter<EfCoreTest>.As(e => e.Id, idQuery),
                        PropertyFilter<EfCoreTest>.As(e => e.Name, nameQuery))
                    .ToListAsync();
            }

            Assert.Equal(expectedEntities.Count, actualEntities.Count);

            foreach (var expected in expectedEntities)
            {
                var actual = actualEntities.FirstOrDefault(a => a.Id == expected.Id);

                Assert.NotNull(actual);
            }
        }

        [Fact(DisplayName = "NpgSql")]
        public async Task JoinsByPropertyNpgSql()
        {
            var expectedIds = NpgSqlIds.Take(5).ToList();

            List<int> actualIds;

            using (var db = new postgresContext())
            {
                actualIds = await db.Efcoretest
                    .Join(PropertyFilter<Efcoretest>.As(e => e.Id, expectedIds))
                    .Select(e => e.Id)
                    .ToListAsync();
            }

            Assert.Equal(expectedIds.Count, actualIds.Count);

            foreach (var expected in expectedIds)
            {
                Assert.Contains(expected, actualIds);
            }
        }
    }
}
