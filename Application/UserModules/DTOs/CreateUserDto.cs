using Shared.ApplicationBase.Common.Validations;
using Shared.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserModules.DTOs
{
    public class CreateUserDto
    {
        private string _username = null!;

        [Required]
        public string Username
        {
            get => _username;
            set => _username = value.Trim();
        }
        private string _password = null!;
        [Required]
        public string Password
        {
            get => _password;
            set => _password = value.Trim();
        }
        [IntegerRange(AllowableValues = new int[] { UserTypes.ADMIN, UserTypes.CUSTOMER })]
        public int UserType { get; set; }
    }
}
