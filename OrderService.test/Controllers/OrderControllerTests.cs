using AutoMapper;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using OrderService.Controllers;
using OrderService.Dto;
using OrderService.Interfaces;
using OrderService.Models;

namespace OrderService.test.Controllers
{
    [TestClass]
    public sealed class OrderControllerTests
    {
        private readonly IOrderRepository _orderRepository = A.Fake<IOrderRepository>();
        private readonly IProductOrderRepository _ProductOrderRepository = A.Fake<IProductOrderRepository>();
        private readonly IMapper _mapper = A.Fake<IMapper>();

        [TestMethod]
        public async Task Test_CreateOrder_SimpelOrderNoProduct()
        {
            var order = A.Fake<Order>();
            var orderCreate = A.Fake<OrderDto>();
            orderCreate.Products = [];
            A.CallTo(() => _mapper.Map<Order>(orderCreate)).Returns(order);
            A.CallTo(() => _orderRepository.CreateOrder(order)).Returns(true);
            var controller = new OrderController(_orderRepository, _ProductOrderRepository, _mapper);

            var result = controller.CreateOrder(orderCreate);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType<NoContentResult>(result);    
            A.CallTo(() => _orderRepository.CreateOrder(order)).MustHaveHappenedOnceExactly();
        }

        [TestMethod]
        public async Task Test_CreateOrder_SimpelOrderWithAProduct()
        {
            var order = A.Fake<Order>();
            var orderCreate = A.Fake<OrderDto>();
            var productOrder = A.Fake<ProductOrder>();
            var productOrderCreate = A.Fake<ProductOrderDto>();
            orderCreate.Products = [productOrderCreate];
            A.CallTo(() => _mapper.Map<Order>(orderCreate)).Returns(order);
            A.CallTo(() => _mapper.Map<ProductOrder>(productOrderCreate)).Returns(productOrder);
            A.CallTo(() => _orderRepository.CreateOrder(order)).Returns(true);
            A.CallTo(() => _ProductOrderRepository.CreateProductOrder(productOrder)).Returns(true);
            var controller = new OrderController(_orderRepository, _ProductOrderRepository, _mapper);

            var result = controller.CreateOrder(orderCreate);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType<NoContentResult>(result);    
            A.CallTo(() => _orderRepository.CreateOrder(order)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _ProductOrderRepository.CreateProductOrder(productOrder)).MustHaveHappenedOnceExactly();
        }

        [TestMethod]
        public void Test_CreateOrder_NullInput_ReturnsBadRequest()
        {
            var controller = new OrderController(_orderRepository, _ProductOrderRepository, _mapper);
            var result = controller.CreateOrder(null);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void Test_CreateOrder_OrderAlreadyExists_Returns422()
        {
            var orderCreate = new OrderDto { OrderID = 0 };
            var existingOrder = new Order { OrderID = 0 };
            A.CallTo(() => _orderRepository.GetOrders()).Returns([existingOrder]);
            var controller = new OrderController(_orderRepository, _ProductOrderRepository, _mapper);
            var result = controller.CreateOrder(orderCreate);
            var statusResult = result as ObjectResult;
            Assert.IsNotNull(statusResult);
            Assert.AreEqual(422, statusResult.StatusCode);
        }
    }
}