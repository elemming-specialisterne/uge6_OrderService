using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OrderService.Dto;
using OrderService.Interfaces;
using OrderService.Models;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(IOrderRepository orderRepository, IProductOrderRepository productOrderRepository, IMapper mapper) : Controller
    {
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly IProductOrderRepository _productOrderRepository = productOrderRepository;
        private readonly IMapper _mapper = mapper;

        [HttpPost("[action]")]
        [ProducesResponseType(204)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult CreateOrder([FromBody] OrderDto orderCreate)
        {
            if (orderCreate == null)
                return BadRequest(ModelState);

            var order = _orderRepository.GetOrders()
                .Where(o => o.OrderID == orderCreate.OrderID)
                .FirstOrDefault();
            if (order is not null)
            {
                ModelState.AddModelError("", "Order Already Exists");
                return StatusCode(422, ModelState);
            }
            if (orderCreate.OrderID != 0)
            {
                ModelState.AddModelError("", "OrderId should be 0 to auto-update");
                return StatusCode(422, ModelState);
            }
            // if (!"Call to User"(orderCreate.UserId).exists) TODO If User not exist
            // {
            //     ModelState.AddModelError("", "User does not exist");
            //     return StatusCode(422, ModelState);
            // }
            // foreach (var productOrder in orderCreate.Products) TODO If product nor exist
            // {
            //     if (!"Call to Product API".Contains(product.ProductID))
            //     {
            //         ModelState.AddModelError("", $"ProductID {product.ProductID} does not exist");
            //         return StatusCode(422, ModelState);
            //     }
            // }
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var orderMap = _mapper.Map<Order>(orderCreate);

            foreach (var productOrderCreate in orderCreate.Products)
            {
                if (!_productOrderRepository.CreateProductOrder(_mapper.Map<ProductOrder>(productOrderCreate)))
                {
                    ModelState.AddModelError("", "Something went wrong while saving");
                    return StatusCode(500, ModelState);
                }
            }

            if (!_orderRepository.CreateOrder(orderMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}