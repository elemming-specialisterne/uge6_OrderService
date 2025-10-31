using System.Runtime;
using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Interfaces;
using OrderService.Models;
using OrderService.Repository;

namespace OrderService.test.Repository;

[TestClass]
public sealed class OrderRepositoryTests
{
    private async Task<OrderContext> GetDatabaseContext()
    {
        //Create test database + Context
        DbContextOptions<OrderContext> options =
            new DbContextOptionsBuilder<OrderContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        OrderContext databaseContext = new(options);
        databaseContext.Database.EnsureCreated();

        //seed test database
        if (!await databaseContext.Orders.AnyAsync())
        {
            for (int i = 1; i <= 10; i++)
            {
                databaseContext.Orders.Add(new Order()
                {
                    Orderid = i,
                    Userid = (i % 3)+1,
                    CreatedAt = DateTime.Today.AddDays(i-5),
                    Total = 147.15m,
                    OrderItems =
                    [
                        new OrderItem() {Orderid = i, Productid = i, Qty = i}
                    ]
                });
            }
        }
        databaseContext.SaveChanges();

        return databaseContext;
    }

    [TestMethod]
    public async Task Test_CreateOrder_SimpelOrderNoProduct()
    {
        var dbContext = await GetDatabaseContext();
        IOrderRepository orderRepository = new OrderRepository(dbContext);

        
        int amount = dbContext.Orders.Count();

        var result = orderRepository.CreateOrder(new Order() {
            Orderid = 11,
            CreatedAt = DateTime.Today,
            Total = 123.45m,
            OrderItems = [],
            Userid = 1
        });

        Assert.IsTrue(result);
        Assert.IsTrue(amount+1 == dbContext.Orders.Count());

        Order order = dbContext.Orders.First(o => o.Orderid == 11);

        Assert.AreEqual(123.45m, order.Total);
        Assert.AreEqual(1, order.Userid);
        Assert.AreEqual(DateTime.Parse("27/10/2025"), order.CreatedAt);
        Assert.IsFalse(order.OrderItems.Count != 0);
    }

    [TestMethod]
    public async Task Test_CreateOrder_SimpelOrderAProduct()
    {
        var dbContext = await GetDatabaseContext();
        IOrderRepository orderRepository = new OrderRepository(dbContext);

        var result = orderRepository.CreateOrder(new Order() {
            Orderid = 11,
            CreatedAt = DateTime.Today,
            Total = 123123.45m,
            OrderItems = [new OrderItem() {Orderid = 11, Productid = 4, Qty = 780}],
            Userid = 1
        });

        Assert.IsTrue(result);
        Assert.IsTrue(dbContext.Orders.Any());

        Order order = dbContext.Orders.First(o => o.Orderid == 11);

        Assert.AreEqual(1, order.OrderItems.Count);
        Assert.AreEqual(4, order.OrderItems.First().Productid);
        Assert.AreEqual(780, order.OrderItems.First().Qty);
    }

    [TestMethod]
    public async Task Test_GetOrders()
    {
        var dbContext = await GetDatabaseContext();
        IOrderRepository orderRepository = new OrderRepository(dbContext);

        var orders = orderRepository.GetOrders();

        Assert.IsNotNull(orders);
        Assert.AreEqual(10, orders.Count);
    }

    [TestMethod]
    public async Task Test_GetOrder_ByOrderID()
    {
        var dbContext = await GetDatabaseContext();
        IOrderRepository orderRepository = new OrderRepository(dbContext);

        var order = orderRepository.GetOrder(3);

        Assert.IsNotNull(order);
        Assert.AreEqual(3, order.Orderid);
        Assert.AreEqual(1, order.Userid);
        Assert.AreEqual(DateTime.Today.AddDays(-2), order.CreatedAt);
        Assert.AreEqual(1, order.OrderItems.Count);
        Assert.AreEqual(3, order.OrderItems.First().Productid);
        Assert.AreEqual(3, order.OrderItems.First().Qty);
    }

    [TestMethod]
    public async Task Test_GetOrders_ByUserID()
    {
        var dbContext = await GetDatabaseContext();
        IOrderRepository orderRepository = new OrderRepository(dbContext);

        var orders = orderRepository.GetOrders(2);

        Assert.IsNotNull(orders);
        Assert.AreEqual(4, orders.Count);
        foreach (var order in orders)
        {
            Assert.AreEqual(2, order.Userid);
        }
    }

    [TestMethod]
    public async Task Test_GetOrdersBetween()
    {
        var dbContext = await GetDatabaseContext();
        IOrderRepository orderRepository = new OrderRepository(dbContext);

        var orders = orderRepository.GetOrdersBetween(DateTime.Today.AddDays(-2), DateTime.Today.AddDays(1));

        Assert.IsNotNull(orders);
        Assert.AreEqual(4, orders.Count);
    }
}
