using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LessonsAPI.Models;
using LessonsAPI.Models.Converters;

namespace LessonsAPI.Data
{
    public class LessonsAPIContext : DbContext
    {
        protected readonly IConfiguration Configuration;
        public DbSet<LessonsAPI.Models.Lesson> Lessons { get; set; } = default!;

        public DbSet<Author> Authors { get; set; } = default!;

        public DbSet<Models.Task> Tasks { get; set; } = default!;

        public LessonsAPIContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to postgres with connection string from app settings
            options.UseNpgsql(Configuration.GetConnectionString("LessonsAPI"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Lesson>()
                .HasOne(p => p.Author)
                .WithMany(t => t.Lesons)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Models.Task>().HasOne(p => p.Lesson).WithMany(t => t.Tasks).OnDelete(DeleteBehavior.Cascade);
        }


        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<List<string>>().HaveConversion<StringListConverter>();
        }
    }
}
