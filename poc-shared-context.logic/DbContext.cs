using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using poc_shared_context.domain;

namespace poc_shared_context.logic;
public class Class1
{

}

public class ProductDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Filename=./Products.sqlite");
        // base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>();
    }
}
