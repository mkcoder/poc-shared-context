using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json.Linq;

namespace poc_shared_context.domain;

public class StoreInfo
{
    public string StoreName { get; set; }
    public string StoreLocation { get; set; }
    public int Random { get; set; }
}

public class Product : IAuditable
{
    [Key]
    public virtual string UPC { get; set; }
    [Required]
    public virtual string Name { get; set; }
    [Required]
    public virtual string Description { get; set; }
    [Required]
    [DataType("json")]
    public virtual string Data { get; set; }

    // Audit Properties
    public DateTime UpdatedOn { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime DeletedOn { get; set; }
    public bool IsDeleted     { get; set; }

    // Other fields
    public override string? ToString()
    {
        return $"| UPC: {UPC} | Name: {Name} | Description: {Description} |";
    }
}



public class ReadOnlyProductDbContext : ReadOnlyDbContext
{
    public IQueryable<Product> Products => Set<Product>().AsNoTracking();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Filename=./Products.sqlite;Mode=ReadOnly");
        // base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Product>(c => {

            c.ToTable("Products");
        });
    }
}