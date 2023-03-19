using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using poc_shared_context.domain;

namespace poc_other_project_ms;


public class ProductsDepenencyProjection 
{

    private readonly ReadOnlyProductDbContext _readOnlyProductDbContext;
    private readonly OtherMSDBContext _otherMSDBContext;

    public ProductsDepenencyProjection()
    {
        _readOnlyProductDbContext = new ReadOnlyProductDbContext();
        _otherMSDBContext = new OtherMSDBContext();
    }

    public async Task UpdateProductDependecy()
    {
        var lastSync = _otherMSDBContext.DependencyProjectionSync.FirstOrDefault(s => s.DepenendencyName == nameof(ProductsDepenencyProjection));
        if (lastSync == null)
        {
            // Reload all items from products to our ms
            LoadAllProducts();

            _otherMSDBContext.DependencyProjectionSync.Add(new DependencyProjectionSync()
            {
                DepenendencyName = nameof(ProductsDepenencyProjection),
                LastSyncTime = DateTime.UtcNow
            });
        }
        else
        {
            LoadProductSince(lastSync);
            lastSync.LastSyncTime = DateTime.UtcNow;
        }

        _otherMSDBContext.SaveChanges();
    }

    private void LoadProductSince(DependencyProjectionSync lastSync)
    {
        var changedProducts = _readOnlyProductDbContext.Products.Where(p => p.UpdatedOn >= lastSync.LastSyncTime).ToList();
        foreach (var item in changedProducts)
        {
            var stores = GetStoreNameFromData(item.Data);

            foreach (var jStore in stores)
            {
                var store = jStore.ToObject<StoreInfo>();
                var dbCheck = _otherMSDBContext.ProductStores.FirstOrDefault(c => c.UPC == item.UPC && c.StoreName == store.StoreName);
                if (dbCheck == null)
                {
                    var pstore = new ProductStore()
                    {
                        UPC = item.UPC,
                        StoreName = store?.StoreName ?? "--"
                    };
                    _otherMSDBContext.ProductStores.Add(pstore);
                }
                else if (!item.IsDeleted)
                {
                    dbCheck.StoreName = store?.StoreName ?? "--";
                }
                else if (item.IsDeleted && dbCheck != null)
                {
                    _otherMSDBContext.ProductStores.Remove(dbCheck);
                }
            }
        }

        _otherMSDBContext.SaveChanges();
    }

    private void LoadAllProducts()
    {
        foreach (var item in _readOnlyProductDbContext.Products.ToList())
        {
            var stores = GetStoreNameFromData(item.Data);

            foreach (var jStore in stores)
            {
                var store = jStore.ToObject<StoreInfo>();
                var pstore = new ProductStore()
                {
                    UPC = item.UPC,
                    StoreName = store?.StoreName ?? "--"
                };
                _otherMSDBContext.ProductStores.Add(pstore);
            }
        }
    }

    private JArray GetStoreNameFromData(string data)
    {
        var store = JObject.Parse(data);
        return store["StoreInfo"]?.ToObject<JArray>() ?? new JArray();
    }
}