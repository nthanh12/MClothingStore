using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistances;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserCustomerRepository : IUserCustomerRepository
    {
        private readonly ApplicationDbContext _context;
        public UserCustomerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(UserCustomer userCustomer)
        {
            await _context.UserCustomers.AddAsync(userCustomer); 
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int userId, int customerId)
        {
            var userCustomer = await _context.UserCustomers.FirstOrDefaultAsync(uc => uc.UserID == userId && uc.CustomerID == customerId); 
            if (userCustomer != null) 
            { 
                _context.UserCustomers.Remove(userCustomer); 
                await _context.SaveChangesAsync(); 
            }
        }

        public async Task<IEnumerable<UserCustomer>> GetAllAsync()
        {
            return await _context.UserCustomers.ToListAsync();
        }

        public async Task<UserCustomer?> GetByIdsAsync(int userId, int customerId)
        {
            return await _context.UserCustomers.FirstOrDefaultAsync(uc => uc.UserID == userId && uc.CustomerID == customerId);
        }
    }
}
