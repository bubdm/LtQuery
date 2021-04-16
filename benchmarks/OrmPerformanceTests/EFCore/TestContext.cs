using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace OrmPerformanceTests.EFCore
{
    public class TestContext : DbContext
    {
        private readonly DbConnection _sqlConnection;
        public TestContext(DbConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_sqlConnection, o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
        }

        public DbSet<TestEntity> TestEntity { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TestEntity>(b =>
            {
                b.HasKey(e => e.Id);
                b.HasIndex(e => e.Code).IsUnique();
                b.HasIndex(e => e.Code2);
                b.Property(e => e.Name).HasMaxLength(30);
                b.HasIndex(e => e.Name).IsUnique();
            });
        }
    }
}
