﻿using System.IO;
using System.Reflection;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using Hermes.Extensions;
using Hermes.Models;

namespace Hermes.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options = null)
            : base(options)
        { }

        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.AddEntityConfigurationsFromAssembly(GetType().GetTypeInfo().Assembly);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            var connectionString = configuration.GetConnectionString("Default");

            optionsBuilder.UseSqlite(connectionString);
        }
    }
}