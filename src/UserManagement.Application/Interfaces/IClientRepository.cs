using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Domain.Entities;

namespace UserManagement.Application.Interfaces
{
    public interface IClientRepository
    {
        Task<string?> GetApiKeyAsync(Guid userId);
        Task<Guid> CreateApiKeyAsync(Client client);
    }
}
