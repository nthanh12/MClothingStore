using Application.UserModules.Abstracts;
using Application.UserModules.DTOs.PaymentMethod;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using Shared.Consts.Exceptions;
using Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserModules.Implements
{
    public class PaymentMethodService : IPaymentMethodService
    {
        private readonly IPaymentMethodRepository _paymentMethodRepository;
        private readonly ILogger<PaymentMethodService> _logger;
        private readonly IMapper _mapper;

        public PaymentMethodService(IPaymentMethodRepository paymentMethodRepository, ILogger<PaymentMethodService> logger, IMapper mapper)
        {
            _paymentMethodRepository = paymentMethodRepository;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task AddAsync(AddPaymentMethodDto pmDto)
        {
            try
            {
                var pm = _mapper.Map<PaymentMethod>(pmDto);

                var existingPM = await _paymentMethodRepository.GetByNameAsync(pm.Name);
                if (existingPM != null)
                {
                    _logger.LogWarning($"Payment method with name '{pm.Name}' already exists.");
                    throw new InvalidOperationException($"Payment method with name '{pm.Name}' already exists.");
                }

                await _paymentMethodRepository.AddAsync(pm);
                _logger.LogInformation($"Payment method '{pm.Name}' added successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding payment method '{pmDto.Name}'");
                throw;
            }
        }

        public async Task DeleteAsync(int pmId)
        {
            try
            {
                await _paymentMethodRepository.DeleteAsync(pmId);
                _logger.LogInformation($"Payment method with ID {pmId} deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting payment method with ID {pmId}");
                throw;
            }
        }

        public async Task<IEnumerable<PaymentMethodDto>> GetAllAsync()
        {
            try
            {
                var result = await _paymentMethodRepository.GetAllAsync();
                if (result == null || !result.Any())
                {
                    return new List<PaymentMethodDto>();
                }
                var pmDto = _mapper.Map<IEnumerable<PaymentMethodDto>>(result);
                return pmDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all payment methods");
                throw;
            }
        }

        public async Task<PaymentMethodDto> GetByIdAsync(int pmId)
        {
            try
            {
                var pm = await _paymentMethodRepository.GetByIdAsync(pmId);
                if (pm == null)
                {
                    _logger.LogWarning($"Payment method with ID {pmId} not found.");
                    throw new UserFriendlyException(ErrorCode.PaymentMethodNotFound);
                }
                _logger.LogInformation($"Retrieved payment method with ID {pmId} successfully.");
                var pmDto = _mapper.Map<PaymentMethodDto>(pm);

                return pmDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving payment method with ID {pmId}");
                throw;
            }
        }

        public async Task UpdateAsync(UpdatePaymentMethodDto pmDto)
        {
            try
            {
                var existingPm = await _paymentMethodRepository.GetByIdAsync(pmDto.Id);
                if (existingPm == null)
                {
                    _logger.LogWarning($"Payment method with ID {pmDto.Id} not found. Cannot update.");
                    throw new KeyNotFoundException($"Payment method with ID {pmDto.Id} not found.");
                }

                // Cập nhật các thuộc tính cần thay đổi
                _mapper.Map(pmDto, existingPm);

                await _paymentMethodRepository.UpdateAsync(existingPm);
                _logger.LogInformation($"Payment method with ID {existingPm.Id} updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating payment method with ID {pmDto.Id}");
                throw;
            }
        }
    }
}
