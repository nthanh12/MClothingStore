using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ApplicationDbContext _context;
        
        public PaymentRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Payment entity)
        {
            await _context.Payments.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Payment>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Payment> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Payment entity)
        {
            throw new NotImplementedException();
        }
    }
}
