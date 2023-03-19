using poc_shared_context.domain;
using poc_shared_context.logic;

namespace poc_shared_context.console;
class Program
{
    static void Main(string[] args)
    {
        var writeContext = new ProductDbContext();
        var readOnlyContext = new ReadOnlyProductDbContext();
        // Start with a clean database
        writeContext.Database.EnsureCreated();
        writeContext.Add(new Product() { UPC = $"{System.Guid.NewGuid()}", Name = "Sample Product", Description = "A sample product" });
        writeContext.SaveChanges();

        foreach (var item in writeContext.Products.ToList())
        {
            Console.WriteLine(item);
        }

        try
        {
            readOnlyContext.Database.EnsureCreated();
            readOnlyContext.Add(new Product() { UPC = $"{System.Guid.NewGuid()}", Name = "Sample Product", Description = "A sample product" });
            readOnlyContext.SaveChanges();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Trying to save to a readonly context");
            Console.WriteLine(ex.Message);
        }

        foreach (var item in readOnlyContext.Products.ToList())
        {
            Console.WriteLine(item);
        }

        Console.WriteLine("Finished");
        Console.WriteLine(DateTime.Now);
    }
}

