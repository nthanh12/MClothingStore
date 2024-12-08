using Application.UserModules.Abstracts;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserModules.Implements
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IOrderDetailRepository _orderDetailRepository; 
        private readonly ILogger<OrderDetailService> _logger;

        public OrderDetailService(IOrderDetailRepository orderDetailRepository, ILogger<OrderDetailService> logger)
        {
            _orderDetailRepository = orderDetailRepository;
            _logger = logger;
        }

        public async Task AddAsync(OrderDetail orderDetail)
        {
            try 
            { 
                await _orderDetailRepository.AddAsync(orderDetail); 
            } 
            catch (Exception ex) 
            { 
                _logger.LogError(ex, "Error adding order detail"); 
                throw; 
            }
        }

        public async Task DeleteAsync(int id) 
        { 
            try 
            { 
                await _orderDetailRepository.DeleteAsync(id); 
            } catch (Exception ex) 
            { 
                _logger.LogError(ex, $"Error deleting order detail with ID {id}"); throw; 
            } 
        }

        public async Task<IEnumerable<OrderDetail>> GetAllByOrderIdAsync(int orderId)
        {
            try 
            { 
                return await _orderDetailRepository.GetAllByOrderIdAsync(orderId); 
            } 
            catch (Exception ex) 
            { 
                _logger.LogError(ex, $"Error retrieving order details for Order ID {orderId}"); 
                throw; 
            }
        }

        public async Task<OrderDetail?> GetByIdAsync(int id) 
        { 
            try 
            { 
                return await _orderDetailRepository.GetByIdAsync(id); 
            } 
            catch (Exception ex) 
            { 
                _logger.LogError(ex, $"Error retrieving order detail with ID {id}"); 
                throw; 
            } 
        }
        public async Task UpdateAsync(OrderDetail orderDetail) 
        { 
            try 
            { 
                await _orderDetailRepository.UpdateAsync(orderDetail); 
            } 
            catch (Exception ex) 
            { 
                _logger.LogError(ex, "Error updating order detail"); throw; 
            } 
        }
    }
}
