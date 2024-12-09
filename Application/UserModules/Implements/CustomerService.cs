using Application.UserModules.Abstracts;
using Application.UserModules.DTOs.Customer;
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
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUserCustomerService _userCustomerService;
        private readonly ILogger<CustomerService> _logger;
        private readonly IMapper _mapper;

        public CustomerService(ICustomerRepository customerRepository, IUserCustomerService userCustomerService, ILogger<CustomerService> logger, IMapper mapper)
        {
            _userCustomerService = userCustomerService;
            _customerRepository = customerRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task RegisterCustomerAsync(RegisterCustomerDto customerDto, int userId)
        {
            try
            {
                _logger.LogInformation("Add customer: " + customerDto.LastName);
                var customer = _mapper.Map<Customer>(customerDto);
                await _customerRepository.AddAsync(customer);

                //
                var customerId = customer.Id;

                await _userCustomerService.RegisterCustomerAsync(customerId, userId);
                _logger.LogInformation("Added customer successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding customer: " + customerDto.LastName);
                throw;
            }
        }
    }
}
