using Application.UserModules.Abstracts;
using Application.UserModules.DTOs.Order;
using Application.UserModules.DTOs.OrderDetail;
using Application.UserModules.DTOs.Product;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistances;
using Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserModules.Implements
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IOrderRepository orderRepository, IOrderDetailService orderDetailService, IMapper mapper, ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _orderDetailService = orderDetailService;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task AddOrderAsync(AddOrderWithDetailsDto orderDto)
        {
            try
            {
                decimal totalAmount = CalcTotal(orderDto.OrderDetails);

                totalAmount = ApplyDiscount(totalAmount, orderDto.DiscountRate);
                //cần thêm transaction
                var order = _mapper.Map<Order>(orderDto);
                order.TotalAmount = totalAmount;

                await _orderRepository.AddAsync(order);

                foreach (var detailDto in orderDto.OrderDetails)
                {
                    var orderDetail = _mapper.Map<OrderDetail>(detailDto);
                    orderDetail.OrderId = order.Id;

                    await _orderDetailService.AddAsync(orderDetail);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding order"); throw;
            }
        }

        public async Task DeleteOrderAsync(int id)
        {
            try
            {
                var orderDetails = await _orderDetailService.GetAllByOrderIdAsync(id);
                foreach (var detail in orderDetails)
                {
                    await _orderDetailService.DeleteAsync(detail.Id);
                }

                // Xóa đơn hàng
                await _orderRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting order with ID {id}");
                throw;
            }
        }

        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
            try
            {
                var result = await _orderRepository.GetAllAsync();
                if (result == null || !result.Any())
                {
                    return new List<OrderDto>();
                }
                var orderDtos = _mapper.Map<IEnumerable<OrderDto>>(result);
                return orderDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all order");
                throw;
            }
        }

        public async Task<OrderWithDetailsDto?> GetOrderByIdAsync(int id)
        {
            try
            {
                var order = await _orderRepository.GetByIdAsync(id);
                if (order == null)
                {
                    _logger.LogWarning($"Order with ID {id} not found.");
                    return null;
                }

                var orderDetails = await _orderDetailService.GetAllByOrderIdAsync(id) ?? Enumerable.Empty<OrderDetail>();
                var orderDto = _mapper.Map<OrderWithDetailsDto>(order);
                orderDto.OrderDetails = _mapper.Map<List<OrderDetailDto>>(orderDetails);
                return orderDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving order with ID {id}");
                throw;
            }
        }

        public async Task UpdateOrderAsync(UpdateOrderWithDetailsDto orderDto)
        {
            try
            {
                var existingOrder = await _orderRepository.GetByIdAsync(orderDto.Id);
                if (existingOrder == null)
                {
                    _logger.LogWarning($"Order with ID {orderDto.Id} not found.");
                    throw new Exception($"Order with ID {orderDto.Id} not found.");
                }
                _mapper.Map(orderDto, existingOrder);
                await _orderRepository.UpdateAsync(existingOrder);

                var existingOD = await _orderDetailService.GetAllByOrderIdAsync(orderDto.Id);

                var updatedOD = orderDto.OrderDetails.Select(dto => _mapper.Map<OrderDetail>(dto)).ToList();

                foreach (var existingDetail in existingOD)
                {
                    if (!updatedOD.Any(uod => uod.Id == existingDetail.Id))
                    {
                        await _orderDetailService.DeleteAsync(existingDetail.Id);
                    }
                }

                foreach (var updatedDetail in updatedOD)
                {
                    updatedDetail.OrderId = existingOrder.Id;
                    if (existingOD.Any(eod => eod.Id == updatedDetail.Id))
                    {
                        await _orderDetailService.UpdateAsync(updatedDetail);
                    }
                    else
                    {
                        await _orderDetailService.AddAsync(updatedDetail);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating order with ID {orderDto.Id}");
                throw;
            }
        }

        private decimal CalcTotal(List<AddOrderDetailDto> orderDetails)
        {
            decimal total = 0;
            foreach (var detail in orderDetails)
            {
                total += Calc1Type(detail.Quantity, detail.UnitPrice);
            }
            return total;
        }
        private decimal Calc1Type(int quantity, decimal unitPrice) { return quantity * unitPrice; }
        private decimal ApplyDiscount(decimal totalAmount, decimal discountRate)
        {
            return totalAmount * (1 - discountRate / 100);

        }
    }
}