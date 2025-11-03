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
        private readonly IOrderItemRepository _ProductOrderRepository = A.Fake<IOrderItemRepository>();
        private readonly IUserRepository _UserRepository = A.Fake<IUserRepository>();
        private readonly IMapper _mapper = A.Fake<IMapper>();

        [TestMethod]
        public async Task Test_CreateOrder_SimpelOrderNoProduct()
        {
            var order = A.Fake<Order>();
            var orderCreate = A.Fake<OrderDto>();
            orderCreate.OrderItems = [];
            A.CallTo(() => _mapper.Map<Order>(orderCreate)).Returns(order);
            A.CallTo(() => _orderRepository.CreateOrder(order)).Returns(true);
            var controller = new OrderController(_orderRepository, _ProductOrderRepository,_UserRepository, _mapper);

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
            var productOrder = A.Fake<OrderItem>();
            var productOrderCreate = A.Fake<OrderItemDto>();
            orderCreate.OrderItems = [productOrderCreate];
            A.CallTo(() => _mapper.Map<Order>(orderCreate)).Returns(order);
            A.CallTo(() => _mapper.Map<OrderItem>(productOrderCreate)).Returns(productOrder);
            A.CallTo(() => _orderRepository.CreateOrder(order)).Returns(true);
            A.CallTo(() => _ProductOrderRepository.CreateProductOrder(productOrder)).Returns(true);
            var controller = new OrderController(_orderRepository, _ProductOrderRepository,_UserRepository, _mapper);

            var result = controller.CreateOrder(orderCreate);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType<NoContentResult>(result);    
            A.CallTo(() => _orderRepository.CreateOrder(order)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _ProductOrderRepository.CreateProductOrder(productOrder)).MustHaveHappenedOnceExactly();
        }

        [TestMethod]
        public async Task Test_CreateOrder_NullInput_ReturnsBadRequest()
        {
            var controller = new OrderController(_orderRepository, _ProductOrderRepository,_UserRepository, _mapper);
            var result = controller.CreateOrder(null!);
            Assert.IsInstanceOfType<BadRequestObjectResult>(result);
        }

        [TestMethod]
        public async Task Test_CreateOrder_OrderAlreadyExists_Returns422()
        {
            var orderCreate = new OrderDto { Orderid = 0 };
            var existingOrder = new Order { Orderid = 0 };
            A.CallTo(() => _orderRepository.GetOrders()).Returns([existingOrder]);
            var controller = new OrderController(_orderRepository, _ProductOrderRepository,_UserRepository, _mapper);
            var result = await controller.CreateOrder(orderCreate);
            var statusResult = result as ObjectResult;
            Assert.IsNotNull(statusResult);
            Assert.AreEqual(422, statusResult.StatusCode);
        }

        [TestMethod]
        public async Task Test_ViewOrder_GetAllOrders_CallsRepositoryGetOrders()
        {
            // Arrange
            var controller = new OrderController(_orderRepository, _ProductOrderRepository,_UserRepository, _mapper);
            var orders = A.CollectionOfFake<Order>(10);
            var orderDtos = A.CollectionOfFake<OrderDto>(10);
            A.CallTo(() => _orderRepository.GetOrders()).Returns(orders);
            A.CallTo(() => _mapper.Map<List<OrderDto>>(orders)).Returns(orderDtos.ToList());

            // Act
            var result = controller.GetOrders();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(200, okResult!.StatusCode);

            Assert.IsInstanceOfType<List<OrderDto>>(okResult.Value);
            var returnedOrders = okResult.Value as List<OrderDto>;
            Assert.AreEqual(10, returnedOrders!.Count);

            A.CallTo(() => _orderRepository.GetOrders()).MustHaveHappenedOnceExactly();
            A.CallTo(() => _mapper.Map<List<OrderDto>>(orders)).MustHaveHappenedOnceExactly();

        }

        [TestMethod]
        public async Task Test_ViewOrder_GetAllUserOrders_CallsRepositoryGetOrders()
        {
            // Arrange
            var controller = new OrderController(_orderRepository, _ProductOrderRepository,_UserRepository, _mapper);
            var userId = 3;
            var orders = A.CollectionOfFake<Order>(5);
            var orderDtos = A.CollectionOfFake<OrderDto>(5);
            A.CallTo(() => _orderRepository.GetOrders(userId)).Returns(orders);
            A.CallTo(() => _mapper.Map<List<OrderDto>>(orders)).Returns(orderDtos.ToList());

            // Act
            var result = controller.GetOrders(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.AreEqual(200, okResult.StatusCode);

            Assert.IsInstanceOfType<List<OrderDto>>(okResult.Value);
            var returnedOrders = (List<OrderDto>)okResult.Value;
            Assert.AreEqual(5, returnedOrders.Count);

            A.CallTo(() => _orderRepository.GetOrders(userId)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _mapper.Map<List<OrderDto>>(orders)).MustHaveHappenedOnceExactly();
        }

        [TestMethod]
        public async Task Test_ViewOrder_GetOrderByID_CallsRepositoryGetOrder()
        {
            // Arrange
            var controller = new OrderController(_orderRepository, _ProductOrderRepository,_UserRepository, _mapper);
            var orderId = 7;
            var order = new Order { Orderid = orderId, Userid = 11, Total = 55.5m, CreatedAt = DateTime.Parse("2025-10-27") };
            var orderDto = new OrderDto { Orderid = orderId, Userid = 11, Total = 55.5m, CreatedAt = DateTime.Parse("2025-10-27") };
            A.CallTo(() => _orderRepository.GetOrder(orderId)).Returns(order);
            A.CallTo(() => _mapper.Map<OrderDto>(order)).Returns(orderDto);

            // Act
            var result = controller.GetOrder(orderId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsInstanceOfType<OrderDto>(okResult.Value);
            var returned = (OrderDto)okResult.Value;
            Assert.AreEqual(orderId, returned.Orderid);
            Assert.AreEqual(orderDto.Userid, returned.Userid);

            A.CallTo(() => _orderRepository.GetOrder(orderId)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _mapper.Map<OrderDto>(order)).MustHaveHappenedOnceExactly();
        }

        [TestMethod]
        public async Task Test_ViewOrder_GetOrdersByDate_CallsRepositoryGetOrdersBetween()
        {
            // Arrange
            var controller = new OrderController(_orderRepository, _ProductOrderRepository,_UserRepository, _mapper);
            var startDate = DateTime.Parse("2025-10-21");
            var endDate = DateTime.Parse("2025-10-25");

            var orders = new List<Order>();
            var orderDtos = new List<OrderDto>();
            for (int i = 0; i < 3; i++)
            {
                var d = startDate.AddDays(i);
                orders.Add(new Order { Orderid = i + 1, Userid = 1, Total = 10m + i, CreatedAt = d });
                orderDtos.Add(new OrderDto { Orderid = i + 1, Userid = 1, Total = 10 + i, CreatedAt = d });
            }

            A.CallTo(() => _orderRepository.GetOrdersBetween(startDate, endDate)).Returns(orders);
            A.CallTo(() => _mapper.Map<List<OrderDto>>(orders)).Returns(orderDtos);

            // Act
            var result = controller.GetOrdersBetween(startDate, endDate);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = (OkObjectResult)result;
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsInstanceOfType(okResult.Value, typeof(List<OrderDto>));
            var returned = (List<OrderDto>)okResult.Value;
            Assert.AreEqual(3, returned.Count);

            foreach (var dto in returned)
            {
                Assert.IsTrue(dto.CreatedAt >= startDate && dto.CreatedAt <= endDate, "Returned order date is outside requested range");
            }

            A.CallTo(() => _orderRepository.GetOrdersBetween(startDate, endDate)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _mapper.Map<List<OrderDto>>(orders)).MustHaveHappenedOnceExactly();
        }
    }
}