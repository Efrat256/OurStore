using Entities;
using Microsoft.Extensions.Logging;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services;

public class OrderService : IOrderService
{
    IOrderRepository orderRepository;
    IProductsRepository productsRepository;
    ILogger logger;
    public OrderService(IOrderRepository _orderRepository, IProductsRepository _productsRepository,ILogger<OrderService> _logger)
    {
        productsRepository = _productsRepository;
        orderRepository = _orderRepository;
        logger = _logger;
    }
    public async Task<Order> addOrder(Order order)
    {
        double orderSum = await getSumProduct(order);
        if (orderSum != order.OrderSum)
        {
            logger.LogCritical($"The orderSum is not equals to the original sum. user: {order.UserId} try to change the sum!!!");
            order.OrderSum = orderSum;
        }
        return await orderRepository.addOrder(order);
    }
    public async Task<Order> getById(int id)
    {
        return await orderRepository.getById(id);
    }
    private async Task<double> getSumProduct(Order order)
    {
        double sum = 0;
        foreach (var item in order.OrderItems)
        {
            Product product = await productsRepository.GetProductById(item.ProductId);
            sum += product.Price;
        }
        return sum;
    }
}
