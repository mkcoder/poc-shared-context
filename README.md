# shared efcore dbcontext
 A shared readonly db context example using efcore and sqlite

So we have 2 projects in this repo:
* poc-shared-context.logic
- this is an internal project that has it's own dbcontext with write permissions
* poc-shared-context.domain
- this is the project we expose to the outside world, either via nuget or just a direct dll link

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
UPC: ea225549-72c0-4c8e-b930-b15af96f59ee | Name: Sample Product | Description: A sample product |
UPC: 1d269d38-092c-4784-8c79-3a8712417b04 | Name: Sample Product | Description: A sample product |
UPC: e29fbfe1-ff34-4ed0-a5ae-208318fedd22 | Name: Sample Product | Description: A sample product |
UPC: 9a094a2c-80a2-4607-acba-c8d65dc49925 | Name: Sample Product | Description: A sample product |
UPC: 9559de79-aafc-40fa-92a9-de246e3b57f0 | Name: Sample Product | Description: A sample product |
UPC: 5570ab2a-2411-4831-835d-bd5455fb03a7 | Name: Sample Product | Description: A sample product |
UPC: 6f722fe6-7c49-48c5-a723-1a61777c239a | Name: Sample Product | Description: A sample product |
UPC: 1da7196e-3b48-469a-b4fc-4026711e649e | Name: Sample Product | Description: A sample product |
UPC: ae1c61af-9358-4176-b3c3-a3eb2924e256 | Name: Sample Product | Description: A sample product |
UPC: 32dad38a-4642-4f5a-ad1d-7465020d9151 | Name: Sample Product | Description: A sample product |
UPC: 61712e30-cca3-495c-877d-c18706f8a8bf | Name: Sample Product | Description: A sample product |
UPC: 779180b6-8ee6-4261-81eb-78f10aed11c3 | Name: Sample Product | Description: A sample product |
UPC: f0139528-6446-43c6-b981-1ceae86194be | Name: Sample Product | Description: A sample product |
UPC: 75513771-9365-4ea0-a8be-199adaaecee4 | Name: Sample Product | Description: A sample product |
UPC: 6bcd7220-473a-4194-819a-b47411814cd5 | Name: Sample Product | Description: A sample product |
UPC: ac37f5f4-4358-4108-a3ff-346fb78b6901 | Name: Sample Product | Description: A sample product |
UPC: 786c7c31-791f-4bcd-b002-ce2d17090e14 | Name: Sample Product | Description: A sample product |
UPC: bb95971e-acc4-4dd2-9ab0-d2bd3a599896 | Name: Sample Product | Description: A sample product |
UPC: 69d712c2-3f48-48ef-97d9-e9d093fc237e | Name: Sample Product | Description: A sample product |
UPC: e67fcf44-6de4-4c18-8863-b8999dada14b | Name: Sample Product | Description: A sample product |
UPC: 22196e90-200d-427f-8ef8-c58d5fad5bf3 | Name: Sample Product | Description: A sample product |
UPC: a4c668ff-b573-4408-9bf7-a278312ca1c5 | Name: Sample Product | Description: A sample product |
UPC: cc53b3a6-c3e9-456e-98d3-f15a57bdfe6a | Name: Sample Product | Description: A sample product |
UPC: 081524c1-33ec-407d-b4d2-04d5330a5609 | Name: Sample Product | Description: A sample product |
UPC: db35b751-01fd-49de-85f5-077b9eafaba0 | Name: Sample Product | Description: A sample product |
UPC: 90c7e00d-e57d-4a74-8f96-a04c9afe016d | Name: Sample Product | Description: A sample product |
UPC: 06969064-ddd5-4c31-ace9-b24c10a20279 | Name: Sample Product | Description: A sample product |
UPC: b6c08cb2-627a-47eb-9fb5-659b756b79bc | Name: Sample Product | Description: A sample product |
UPC: 2d236b6b-a8bc-4a6d-8346-4ef9420d73d7 | Name: Sample Product | Description: A sample product |
UPC: 7e03ea74-adc7-44eb-82de-75afe5b2c505 | Name: Sample Product | Description: A sample product |
UPC: 0d53326f-2c73-486b-bbbe-975071de7edb | Name: Sample Product | Description: A sample product |
UPC: 80e52f11-7891-4be3-9ede-23a1b8f0f1ee | Name: Sample Product | Description: A sample product |
UPC: debdc53d-1b48-4e2d-a60f-c575f56c77df | Name: Sample Product | Description: A sample product |
UPC: 2b5aaf75-2c32-4671-bf06-501e0914643f | Name: Sample Product | Description: A sample product |
UPC: d8c31fff-f1a8-4650-9512-9a28d6c977ca | Name: Sample Product | Description: A sample product |
UPC: a8ea1a0e-6d89-4785-adc5-64b57ad5d44e | Name: Sample Product | Description: A sample product |
UPC: d88d448f-b57d-48dc-a30f-ceaaac755bb6 | Name: Sample Product | Description: A sample product |
UPC: bc0adfe8-08d6-4f25-a8b1-9f2af6140015 | Name: Sample Product | Description: A sample product |
UPC: c4451580-efa9-47c5-bd71-522bb2699850 | Name: Sample Product | Description: A sample product |
UPC: 7975008d-431e-42f8-b0f4-8c89cb31276f | Name: Sample Product | Description: A sample product |
UPC: 34b1e0f9-ab8a-4912-a59c-78b8a8ccf0bc | Name: Sample Product | Description: A sample product |
UPC: d21b12db-c01b-4fb4-b25e-b291da11d97c | Name: Sample Product | Description: A sample product |
UPC: f03ee2f0-e44b-4c67-9395-ef4a65baffce | Name: Sample Product | Description: A sample product |
UPC: bcd4f5bd-b0cf-4fc8-84ed-8d79aad0ac7f | Name: Sample Product | Description: A sample product |
Trying to save to a readonly context
This context is read-only.
UPC: ea225549-72c0-4c8e-b930-b15af96f59ee | Name: Sample Product | Description: A sample product |
UPC: 1d269d38-092c-4784-8c79-3a8712417b04 | Name: Sample Product | Description: A sample product |
UPC: e29fbfe1-ff34-4ed0-a5ae-208318fedd22 | Name: Sample Product | Description: A sample product |
UPC: 9a094a2c-80a2-4607-acba-c8d65dc49925 | Name: Sample Product | Description: A sample product |
UPC: 9559de79-aafc-40fa-92a9-de246e3b57f0 | Name: Sample Product | Description: A sample product |
UPC: 5570ab2a-2411-4831-835d-bd5455fb03a7 | Name: Sample Product | Description: A sample product |
UPC: 6f722fe6-7c49-48c5-a723-1a61777c239a | Name: Sample Product | Description: A sample product |
UPC: 1da7196e-3b48-469a-b4fc-4026711e649e | Name: Sample Product | Description: A sample product |
UPC: ae1c61af-9358-4176-b3c3-a3eb2924e256 | Name: Sample Product | Description: A sample product |
UPC: 32dad38a-4642-4f5a-ad1d-7465020d9151 | Name: Sample Product | Description: A sample product |
UPC: 61712e30-cca3-495c-877d-c18706f8a8bf | Name: Sample Product | Description: A sample product |
UPC: 779180b6-8ee6-4261-81eb-78f10aed11c3 | Name: Sample Product | Description: A sample product |
UPC: f0139528-6446-43c6-b981-1ceae86194be | Name: Sample Product | Description: A sample product |
UPC: 75513771-9365-4ea0-a8be-199adaaecee4 | Name: Sample Product | Description: A sample product |
UPC: 6bcd7220-473a-4194-819a-b47411814cd5 | Name: Sample Product | Description: A sample product |
UPC: ac37f5f4-4358-4108-a3ff-346fb78b6901 | Name: Sample Product | Description: A sample product |
UPC: 786c7c31-791f-4bcd-b002-ce2d17090e14 | Name: Sample Product | Description: A sample product |
UPC: bb95971e-acc4-4dd2-9ab0-d2bd3a599896 | Name: Sample Product | Description: A sample product |
UPC: 69d712c2-3f48-48ef-97d9-e9d093fc237e | Name: Sample Product | Description: A sample product |
UPC: e67fcf44-6de4-4c18-8863-b8999dada14b | Name: Sample Product | Description: A sample product |
UPC: 22196e90-200d-427f-8ef8-c58d5fad5bf3 | Name: Sample Product | Description: A sample product |
UPC: a4c668ff-b573-4408-9bf7-a278312ca1c5 | Name: Sample Product | Description: A sample product |
UPC: cc53b3a6-c3e9-456e-98d3-f15a57bdfe6a | Name: Sample Product | Description: A sample product |
UPC: 081524c1-33ec-407d-b4d2-04d5330a5609 | Name: Sample Product | Description: A sample product |
UPC: db35b751-01fd-49de-85f5-077b9eafaba0 | Name: Sample Product | Description: A sample product |
UPC: 90c7e00d-e57d-4a74-8f96-a04c9afe016d | Name: Sample Product | Description: A sample product |
UPC: 06969064-ddd5-4c31-ace9-b24c10a20279 | Name: Sample Product | Description: A sample product |
UPC: b6c08cb2-627a-47eb-9fb5-659b756b79bc | Name: Sample Product | Description: A sample product |
UPC: 2d236b6b-a8bc-4a6d-8346-4ef9420d73d7 | Name: Sample Product | Description: A sample product |
UPC: 7e03ea74-adc7-44eb-82de-75afe5b2c505 | Name: Sample Product | Description: A sample product |
UPC: 0d53326f-2c73-486b-bbbe-975071de7edb | Name: Sample Product | Description: A sample product |
UPC: 80e52f11-7891-4be3-9ede-23a1b8f0f1ee | Name: Sample Product | Description: A sample product |
UPC: debdc53d-1b48-4e2d-a60f-c575f56c77df | Name: Sample Product | Description: A sample product |
UPC: 2b5aaf75-2c32-4671-bf06-501e0914643f | Name: Sample Product | Description: A sample product |
UPC: d8c31fff-f1a8-4650-9512-9a28d6c977ca | Name: Sample Product | Description: A sample product |
UPC: a8ea1a0e-6d89-4785-adc5-64b57ad5d44e | Name: Sample Product | Description: A sample product |
UPC: d88d448f-b57d-48dc-a30f-ceaaac755bb6 | Name: Sample Product | Description: A sample product |
UPC: bc0adfe8-08d6-4f25-a8b1-9f2af6140015 | Name: Sample Product | Description: A sample product |
UPC: c4451580-efa9-47c5-bd71-522bb2699850 | Name: Sample Product | Description: A sample product |
UPC: 7975008d-431e-42f8-b0f4-8c89cb31276f | Name: Sample Product | Description: A sample product |
UPC: 34b1e0f9-ab8a-4912-a59c-78b8a8ccf0bc | Name: Sample Product | Description: A sample product |
UPC: d21b12db-c01b-4fb4-b25e-b291da11d97c | Name: Sample Product | Description: A sample product |
UPC: f03ee2f0-e44b-4c67-9395-ef4a65baffce | Name: Sample Product | Description: A sample product |
UPC: bcd4f5bd-b0cf-4fc8-84ed-8d79aad0ac7f | Name: Sample Product | Description: A sample product |
Finished
3/18/2023 10:56:41 PM
```
