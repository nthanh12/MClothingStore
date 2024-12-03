using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Exceptions
{
    public class UserFriendlyException : Exception
    {
        public int ErrorCode { get; set; }
        public UserFriendlyException(int errorCode) : base(Consts.Exceptions.ErrorCode.GetMessage(errorCode))
        {
        }
    }
}
