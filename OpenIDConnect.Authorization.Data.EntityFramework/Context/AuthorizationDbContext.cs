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

        public AuthorizationDbContext()
        {
            this.Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClientDto>(
                client =>
                    {
                        client.ForSqlServerToTable("Client");
                        client.Property(c => c.Id).IsRequired().HasMaxLength(200);
                        client.Property(c => c.Name).IsRequired().HasMaxLength(200);
                        client.Property(c => c.Enabled).IsRequired();
                        client.Property(c => c.ClaimsUri).IsRequired(false).HasMaxLength(200);
                    });

        }        
    }    
}