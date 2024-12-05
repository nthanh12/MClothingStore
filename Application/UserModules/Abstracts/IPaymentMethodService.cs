using Application.UserModules.DTOs.PaymentMethod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserModules.Abstracts
{
    public interface IPaymentMethodService
    {
        Task<IEnumerable<PaymentMethodDto>> GetAllAsync();
        Task<PaymentMethodDto> GetByIdAsync(int pmId);
        Task AddAsync(AddPaymentMethodDto pmDto);
        Task UpdateAsync(UpdatePaymentMethodDto pmDto);
        Task DeleteAsync(int pmId);
    }
}
