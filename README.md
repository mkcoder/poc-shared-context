# shared efcore dbcontext
 A shared readonly db context example using efcore and sqlite

So we have 3 projects in this repo:
* poc-shared-context.logic
- this is an internal project that has it's own dbcontext with write permissions
* poc-shared-context.domain
- this is the project we expose to the outside world, either via nuget or just a direct dll link
* poc-other-project-ms
- this another microservice that we have that will be reading from our readonly context and building out a projection of it own.
- we can have this projection be on a cron job to run every 20seconds or 1min
- but notice we aren't going to the write product context, we are only using the read product context as that is the only thing exposing product db
in the domain project we have the domain object for the repo:
```
public class Product
{
    [Key]
    public virtual string UPC { get; set; }
    [Required]
    public virtual string Name { get; set; }
    [Required]
    public virtual string Description { get; set; }

    public override string? ToString()
    {
        return $"UPC: {UPC} | Name: {Name} | Description: {Description} |";
    }
    // Other fields


}

```

we use this in both DbContext so even the readonly context will have all the properties, we can also override this class and create our own context we need and just keep the properties we want.


# How are we doing the readonly context:
```
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
```

you can see the readonly context (here)[https://github.com/mkcoder/poc-shared-context/blob/dec9c919810ccdb97a1615bb16f9d8a47dc14097/poc-shared-context.domain/Domain.cs#L28] it's bascially we inherit the dbcontext class and set all modification to throw an exception. 
Notice also that we also have a readonly connection string in the ReadOnlyDBContext.


Here is the output of running the code:
```
$ ./poc-shared-context.console

| UPC                                  | Name           | Description      | Data                                                                                                       | UpdatedOn            | CreatedOn            | DeletedOn            | IsDeleted |
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
| 17b75c50-cd33-4b83-abd9-ea8843c9d6d0 | Sample Product | A sample product | {"StoreInfo":[{"StoreName":"Test 123 -- 1463785933","StoreLocation":"1234 Main St.","Random":567451262}]}  | 3/19/2023 6:32:44 PM | 3/19/2023 6:32:44 PM | 1/1/0001 12:00:00 AM | False     |
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
| 044678b8-6e06-4574-bbad-003e7a22beac | Sample Product | A sample product | {"StoreInfo":[{"StoreName":"Test 123 -- 992839768","StoreLocation":"1234 Main St.","Random":15881879}]}    | 3/19/2023 6:33:14 PM | 3/19/2023 6:33:14 PM | 1/1/0001 12:00:00 AM | False     |
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
| 6ccb9dfe-d4b8-43e3-b2cb-55e2086c262a | Sample Product | A sample product | {"StoreInfo":[{"StoreName":"Test 123 -- 333454173","StoreLocation":"1234 Main St.","Random":1023576362}]}  | 3/19/2023 6:33:26 PM | 3/19/2023 6:33:26 PM | 1/1/0001 12:00:00 AM | False     |
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
| 91988a3d-14ff-47a9-bb1c-cd5e7aa733d9 | Sample Product | A sample product | {"StoreInfo":[{"StoreName":"Test 123 -- 1637856560","StoreLocation":"1234 Main St.","Random":1182517905}]} | 3/19/2023 6:34:07 PM | 3/19/2023 6:34:07 PM | 1/1/0001 12:00:00 AM | False     |
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
| 6873d5ef-8b71-49dc-81dd-c8a486f1bd5d | Sample Product | A sample product | {"StoreInfo":[{"StoreName":"Test 123 -- 2123872959","StoreLocation":"1234 Main St.","Random":1807685064}]} | 3/19/2023 6:34:34 PM | 3/19/2023 6:34:34 PM | 1/1/0001 12:00:00 AM | False     |
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
Trying to save to a readonly context
This context is read-only.
| UPC: 17b75c50-cd33-4b83-abd9-ea8843c9d6d0 | Name: Sample Product | Description: A sample product |
| UPC: 044678b8-6e06-4574-bbad-003e7a22beac | Name: Sample Product | Description: A sample product |
| UPC: 6ccb9dfe-d4b8-43e3-b2cb-55e2086c262a | Name: Sample Product | Description: A sample product |
| UPC: 91988a3d-14ff-47a9-bb1c-cd5e7aa733d9 | Name: Sample Product | Description: A sample product |
| UPC: 6873d5ef-8b71-49dc-81dd-c8a486f1bd5d | Name: Sample Product | Description: A sample product |
Last Projection Sync Table:
-----------------------------------------------------------
| Id | DepenendencyName            | LastSyncTime         |
-----------------------------------------------------------
| 1  | ProductsDepenencyProjection | 3/19/2023 6:32:45 PM |
-----------------------------------------------------------
Projection in another MS:
----------------------------------------------------------------------
| Id | UPC                                  | StoreName              |
----------------------------------------------------------------------
| 1  | 17b75c50-cd33-4b83-abd9-ea8843c9d6d0 | Test 123 -- 1463785933 |
----------------------------------------------------------------------
| 2  | 044678b8-6e06-4574-bbad-003e7a22beac | Test 123 -- 992839768  |
----------------------------------------------------------------------
| 3  | 6ccb9dfe-d4b8-43e3-b2cb-55e2086c262a | Test 123 -- 333454173  |
----------------------------------------------------------------------
| 4  | 91988a3d-14ff-47a9-bb1c-cd5e7aa733d9 | Test 123 -- 1637856560 |
----------------------------------------------------------------------
| 5  | 6873d5ef-8b71-49dc-81dd-c8a486f1bd5d | Test 123 -- 2123872959 |
----------------------------------------------------------------------
```
