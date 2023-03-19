using ConsoleTableExt;
using Newtonsoft.Json;
using poc_other_project_ms;
using poc_shared_context.domain;
using poc_shared_context.logic;

namespace poc_shared_context.console;
class Program
{
    static async Task Main(string[] args)
    {
        var writeContext = new ProductDbContext();
        var readOnlyContext = new ReadOnlyProductDbContext();
        var otherMsDbContext = new OtherMSDBContext();
        var projectionHandler = new ProductsDepenencyProjection();

        // Start with a clean database
        writeContext.Database.EnsureCreated();
        otherMsDbContext.Database.EnsureCreated();



        writeContext.Add(new Product() { UPC = $"{System.Guid.NewGuid()}", Name = "Sample Product", Description = "A sample product", Data = GetProductJson() });
        writeContext.SaveChanges();

        ConsoleTableBuilder
                .From(writeContext.Products.ToList())
                .ExportAndWriteLine();

        /*
         
         Example of us trying to write to a readonly context
         
         */
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


        /*
         
            Example of us building a projection based on last updated time.
         
         */
        ConsoleTableBuilder
                .From(otherMsDbContext.DependencyProjectionSync.ToList())
                .ExportAndWriteLine();
        await projectionHandler.UpdateProductDependecy();
        ConsoleTableBuilder
                .From(otherMsDbContext.ProductStores.ToList())
                .ExportAndWriteLine();

        Console.WriteLine("Finished");
        Console.WriteLine(DateTime.Now);
    }

    private static string GetProductJson()
    {
        var json = new
        {
            StoreInfo = new[]
            {
                new
                {
                    StoreName = $"Test 123 -- {new Random().Next()}",
                    StoreLocation = "1234 Main St.",
                    Random = new Random().Next()
                }
            }
        };

        return JsonConvert.SerializeObject(json);
    }
}

