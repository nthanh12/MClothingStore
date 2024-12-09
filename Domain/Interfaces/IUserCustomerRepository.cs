using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUserCustomerRepository
    {
        Task AddAsync(UserCustomer userCustomer);
        Task DeleteAsync(int userId, int customerId);
        Task<UserCustomer?> GetByIdsAsync(int userId, int customerId);
        Task<IEnumerable<UserCustomer>> GetAllAsync();
    }

}
