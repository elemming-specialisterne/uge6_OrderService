using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using OrderService.Data;
using OrderService.Models;

namespace OrderService.test.Repository;

[TestClass]
public sealed class Test1
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
        // if (!await databaseContext.Orders.AnyAsync())
        // {
        //     for(int i = 1; i <= 10; i++)
        //     {
        //         databaseContext.Orders.Add(new Order()
        //         {
        //             UserId = 123,
        //             Date = DateTime.Now,
        //             OrderID = i,
        //             Price = 147.15,
        //             ProductOrders = new List<ProductOrder>()
        //             {
        //                 new ProductOrder() {OrderID = i, ProductID = i, Amount = 2,
        //                 Product = new Product() {Name = "testP", Description = "", Price = 12.5, ProductID = i}}
        //             }
        //         });
        //     }
        // }

        return databaseContext;
    }

    [TestMethod]
    public void TestMethod1()
    {
        Assert.AreEqual(true, true);
    }
}
