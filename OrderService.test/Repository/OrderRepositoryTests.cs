using System.Runtime;
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
                    OrderID = i,
                    UserId = 123,
                    Date = DateTime.Today.AddDays(i-5),
                    Price = 147.15,
                    ProductOrders =
                    [
                        new ProductOrder() {OrderID = i, ProductID = i, Amount = i}
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
            OrderID = 11,
            Date = DateTime.Parse("27/10/2025"),
            Price = 123.45,
            ProductOrders = [],
            UserId = 1
        });

        Assert.IsTrue(result);
        Assert.IsTrue(amount+1 == dbContext.Orders.Count());

        Order order = dbContext.Orders.First(o => o.OrderID == 11);

        Assert.AreEqual(123.45, order.Price);
        Assert.AreEqual(1, order.UserId);
        Assert.AreEqual(DateTime.Parse("27/10/2025"), order.Date);
        Assert.IsFalse(order.ProductOrders.Count != 0);
    }

    [TestMethod]
    public async Task Test_CreateOrder_SimpelOrderAProduct()
    {
        var dbContext = await GetDatabaseContext();
        IOrderRepository orderRepository = new OrderRepository(dbContext);

        var result = orderRepository.CreateOrder(new Order() {
            OrderID = 11,
            Date = DateTime.Parse("27/10/2025"),
            Price = 123123.45,
            ProductOrders = [new ProductOrder() {OrderID = 11, ProductID = 4, Amount = 780}],
            UserId = 1
        });

        Assert.IsTrue(result);
        Assert.IsTrue(dbContext.Orders.Any());

        Order order = dbContext.Orders.First(o => o.OrderID == 11);

        Assert.AreEqual(1, order.ProductOrders.Count);
        Assert.AreEqual(4, order.ProductOrders.First().ProductID);
        Assert.AreEqual(780, order.ProductOrders.First().Amount);
    }

    [TestMethod]
    public async Task Test_GetOrders()
    {
        var dbContext = await GetDatabaseContext();
        IOrderRepository orderRepository = new OrderRepository(dbContext);

        var orders = orderRepository.GetOrders();

        Assert.IsNotNull(orders);
        Assert.AreEqual(10, orders.Count);
        Assert.IsTrue(orders.All(o => o.UserId == 123));
    }

    [TestMethod]
    public async Task Test_GetCurrentOrders()
    {
        //public ICollection<Order> GetCurrentOrders()
    }

    [TestMethod]
    public async Task Test_GetOrder_ByOrderID()
    {
        //public Order GetOrder(int orderID)
    }

    [TestMethod]
    public async Task Test_GetOrders_ByUserID()
    {
        // public ICollection<Order> GetOrders(int userId)
    }

    [TestMethod]
    public async Task Test_GetOrdersBetween()
    {
        // public ICollection<Order> GetOrdersBetween(DateTime startDate, DateTime endDate)
    }
}
