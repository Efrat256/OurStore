using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories;

public class OrderRepository : IOrderRepository
{
    OurStoreContext _OurStoreContext;
    public OrderRepository(OurStoreContext ourStoreContext)
    {
        _OurStoreContext = ourStoreContext;
    }
    public async Task<Order> getById(int id)
    {
        return await _OurStoreContext.Orders.Include(o=>o.User).Include(or=>or.OrderItems).FirstOrDefaultAsync(order => order.OrderId == id);
    }
    public async Task<Order> addOrder(Order order)
    {
        await _OurStoreContext.AddAsync(order);
        Console.WriteLine(order);
        await _OurStoreContext.SaveChangesAsync();
        return order;
    }

}
