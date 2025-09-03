    using Microsoft.EntityFrameworkCore;
using UserManagement.Domain.Entities;
using UserManagement.Domain.Interfaces;

namespace UserManagement.Infrastructure.Data
    {
        public class AppDataContext : DbContext
        {
            public AppDataContext(DbContextOptions<AppDataContext> options) : base(options)
            {

            }
            public virtual DbSet<User> Users { get; set; }
            public virtual DbSet<Client> Clients { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDataContext).Assembly);

                modelBuilder.Entity<User>(user =>
                {
                    user.HasIndex(u => u.Email).IsUnique();
                    user.HasIndex(u => u.UserName).IsUnique();
                });

                foreach (var entityType in modelBuilder.Model.GetEntityTypes()
                    .Where(t => typeof(IDomainEntity).IsAssignableFrom(t.ClrType)))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .Property(nameof(IDomainEntity.Id))
                        .HasDefaultValueSql("NEWID()")
                        .ValueGeneratedOnAdd();

                    modelBuilder.Entity(entityType.ClrType)
                        .Property(nameof(IDomainEntity.DateCreated))
                        .HasDefaultValueSql("GETUTCDATE()")
                        .ValueGeneratedOnAdd();
                }
            }
            public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            {
                var modifiedUsers = ChangeTracker.Entries<User>()
                    .Where(u => u.State == EntityState.Modified);

                foreach (var entry in modifiedUsers)
                {
                    entry.Entity.DateModified = DateTime.UtcNow;
                }

                return await base.SaveChangesAsync(cancellationToken);
            }
        }
    }
