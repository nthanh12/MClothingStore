using Application.UserModules.DTOs.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserModules.Abstracts
{
    public interface ICustomerService
    {
        Task RegisterCustomerAsync(UpdateCustomerDto customerDto, int userId);
        Task UpdateCustomerAsync(UpdateCustomerDto customerDto);

    }


}
