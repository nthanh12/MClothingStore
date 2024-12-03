using Shared.Consts.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ApplicationBase.Common.Validations
{
    public class IntegerRange : ValidationAttribute
    {
        public int[] AllowableValues { get; set; } = null!;
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || AllowableValues?.Contains((int)value) == true)
            {
                return ValidationResult.Success;
            }
            var msg = $"Vui lòng chọn 1 trong các giá trị sau: {string.Join(", ", AllowableValues?.Select(i => i.ToString()).ToArray() ?? new string[] { "Không có giá trị nào được phép" })}.";
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                msg = ErrorMessage;
            }
            return new ValidationResult(msg);
        }
    }
}
