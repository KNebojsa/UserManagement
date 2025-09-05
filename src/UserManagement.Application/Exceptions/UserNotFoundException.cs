using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Application.Exceptions
{
    public class UserNotFoundException: Exception
    {
        public UserNotFoundException(Guid id)
          : base($"User with id '{id}' doesn't exist.")
        {
        }
    }
}
