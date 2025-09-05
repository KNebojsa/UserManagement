using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Application.Interfaces;
using UserManagement.Domain.Entities;
using UserManagement.Infrastructure.Data;

namespace UserManagement.Infrastructure.Repository
{
    public class ClientRepository(AppDataContext _context) : IClientRepository
    {
        public async Task<Guid> CreateApiKeyAsync(Client client)
        {
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return client.Id;
        }

        public async Task<string?> GetApiKeyAsync(Guid userId)
        {
            var apiKey = await _context
                .Clients
                .Where(c => c.UserId == userId)
                .Select(c => c.ApiKey)
                .FirstOrDefaultAsync();

            return apiKey;
        }
    }
}
