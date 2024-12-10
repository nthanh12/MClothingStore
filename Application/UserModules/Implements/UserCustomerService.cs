using Application.UserModules.Abstracts;
using Application.UserModules.DTOs.Customer;
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
    public class UserCustomerService : IUserCustomerService
    {
        private readonly IUserCustomerRepository _userCustomerRepository;
        private readonly ILogger<UserCustomerService> _logger;

        public UserCustomerService(IUserCustomerRepository userCustomerRepository, ILogger<UserCustomerService> logger)
        {
            _userCustomerRepository = userCustomerRepository;
            _logger = logger;
        }
        public async Task RegisterCustomerAsync(int customerId, int userId)
        {
            try
            {
                var userCustomer = new UserCustomer { UserID = userId, CustomerID = customerId };
                await _userCustomerRepository.AddAsync(userCustomer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding UserCustomer"); 
                throw;
            }
        }
        public async Task<int> GetCustomerIdByUserIdAsync(int userId) 
        { 
            try
            {
                _logger.LogInformation("Get customerId: " + userId);
                return await _userCustomerRepository.GetCustomerIdByUserIdAsync(userId);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error getting CustomerId");
                throw;
            }
        }
    }
}
