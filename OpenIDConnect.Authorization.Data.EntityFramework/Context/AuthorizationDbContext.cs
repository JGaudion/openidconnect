using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenIDConnect.Authorization.Data.EntityFramework.Context
{
    using Microsoft.Data.Entity;
    using Microsoft.Data.Entity.Metadata.Builders;
    using Microsoft.Data.Entity.Metadata.Internal;

    using OpenIDConnect.Authorization.Data.EntityFramework.Dtos;
    using OpenIDConnect.Authorization.Domain.Models;

    public class AuthorizationDbContext : DbContext
    {
        public DbSet<ClientDto> Clients { get; set; }

        public DbSet<GroupDto> Groups { get; set; }

        public AuthorizationDbContext()
        {                        
            this.Database.EnsureCreated();
            this.Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClientDto>(
                client =>
                    {
                        client.ForSqlServerToTable("Client");
                        client.HasKey(c => c.Id);
                        client.Property(c => c.Id).IsRequired().HasMaxLength(200);
                        client.Property(c => c.Name).IsRequired().HasMaxLength(200);
                        client.Property(c => c.Enabled).IsRequired();
                        client.Property(c => c.ClaimsUri).IsRequired(false).HasMaxLength(200);
                        client.HasMany(c => c.Groups).WithOne(g => g.Client).HasForeignKey(g => g.ClientId);
                    });

            modelBuilder.Entity<GroupDto>(
                group =>
                    {
                        group.ForSqlServerToTable("Group");
                        group.HasKey(g => g.Id);
                        group.Property(g => g.Id).IsRequired().UseSqlServerIdentityColumn();
                        group.Property(g => g.Name).IsRequired().HasMaxLength(200);
                        group.Property(g => g.ClientId).IsRequired().HasMaxLength(200);
                        group.HasOne(g => g.Client).WithMany(c => c.Groups).HasForeignKey(g => g.ClientId);
                    });
        }        
    }    
}