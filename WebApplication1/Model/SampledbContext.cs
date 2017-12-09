using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebApplication1.Model
{
    public partial class SampledbContext : DbContext
    {
        public virtual DbSet<NewTaipeiWifiSpot> NewTaipeiWifiSpot { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer(@"Data Source=.\SqlExpress;Initial Catalog=SampleDB;Integrated Security=True");
        //    }
        //}

        public SampledbContext(DbContextOptions<SampledbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NewTaipeiWifiSpot>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });
        }
    }
}
