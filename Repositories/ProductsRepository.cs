using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Repositories;

public class ProductsRepository : IProductsRepository
{
    OurStoreContext _OurStoreContext;
    public ProductsRepository(OurStoreContext ourStoreContext)
    {
        _OurStoreContext = ourStoreContext;
    }

    public async Task<List<Product>> GetProducts(string? desc, int? minPrice, int? maxPrice, int?[] categoryIds)
    {
       var query = _OurStoreContext.Products.Where(product =>
            (desc == null ? (true) : (product.Description.Contains(desc)))
            && ((minPrice == null) ? (true) : (product.Price >= minPrice))
            && ((maxPrice == null) ? (true) : (product.Price <= maxPrice))
            && ((categoryIds.Length == 0) ? (true) : (categoryIds.Contains(product.CategoryId))))
           .OrderBy(product => product.Price).Include(c => c.Category);
        Console.WriteLine(query.ToQueryString());
        List<Product> products = await query.ToListAsync();// return await...
        return products;
        //return await _OurStoreContext.Products.Include(c => c.Category).ToListAsync();
    }
}
