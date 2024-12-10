using Application.UserModules.Abstracts;
using Application.UserModules.DTOs.Payment;
using AutoMapper;
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
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository; 
        private readonly IOrderRepository _orderRepository; 
        private readonly IPaymentMethodRepository _paymentMethodRepository; 
        private readonly ILogger<PaymentService> _logger;
        private readonly IMapper _mapper;

        public PaymentService(IPaymentRepository paymentRepository, IOrderRepository orderRepository, IPaymentMethodRepository paymentMethodRepository, ILogger<PaymentService> logger, IMapper mapper)
        {
            _paymentRepository = paymentRepository;
            _orderRepository = orderRepository;
            _paymentMethodRepository = paymentMethodRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<bool> AddPaymentAsync(AddPaymentDto paymentDto)
        {
            try
            {
                var payment = _mapper.Map<Payment>(paymentDto);

                // Kiểm tra tính hợp lệ của OrderID 
                var order = await _orderRepository.GetByIdAsync(payment.OrderID); 
                if (order == null) 
                { 
                    _logger.LogWarning($"Order with ID {payment.OrderID} not found."); 
                    return false; 
                }

                var paymentMethod = await _paymentMethodRepository.GetByIdAsync(payment.PaymentMethodID); 
                if (paymentMethod == null) 
                { 
                    _logger.LogWarning($"PaymentMethod with ID {payment.PaymentMethodID} not found."); 
                    return false; 
                }

                await _paymentRepository.AddAsync(payment); 
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding Payment"); 
                return false;
            }
        }
    }
}
