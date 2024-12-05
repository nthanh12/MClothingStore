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
    public class PaymentMethodRepository : IPaymentMethodRepository
    {
        private readonly ApplicationDbContext _context;

        public PaymentMethodRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(PaymentMethod entity)
        {
            await _context.PaymentMethods.AddAsync(entity); 
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var paymentMethod = await _context.PaymentMethods.FindAsync(id); 
            if (paymentMethod != null) 
            { 
                _context.PaymentMethods.Remove(paymentMethod); 
                await _context.SaveChangesAsync(); 
            }
        }

        public async Task<IEnumerable<PaymentMethod>> GetAllAsync()
        {
            return await _context.PaymentMethods.ToListAsync();
        }

        public async Task<PaymentMethod> GetByIdAsync(int id)
        {
            return await _context.PaymentMethods.FindAsync(id);
        }

        public async Task UpdateAsync(PaymentMethod entity)
        {
            _context.PaymentMethods.Update(entity); 
            await _context.SaveChangesAsync();
        }
        public async Task<PaymentMethod> GetByNameAsync(string name)
        {
            return await _context.PaymentMethods
                .FirstOrDefaultAsync(c => c.Name == name);
        }
    }
}
