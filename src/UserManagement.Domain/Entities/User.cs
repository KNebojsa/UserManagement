using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using UserManagement.Domain.Interfaces;

namespace UserManagement.Domain.Entities
{
    public class User : IDomainEntity
    {
        [Key]
        public Guid Id { get; set; }

        [MinLength(3), MaxLength(30)]
        public required string UserName { get; set; }

        [MinLength(2), MaxLength(512)]
        public required string PasswordHash { get; set; }

        [MinLength(2), MaxLength(30)]
        public required string FirstName { get; set; }

        [MinLength(2), MaxLength(30)]
        public required string LastName { get; set; }

        [MinLength(2), MaxLength(50)]
        public required string Email { get; set; }

        [MinLength(2), MaxLength(30)]
        public required string MobileNumber { get; set; }

        [MinLength(2), MaxLength(10)]
        public string? Language { get; set; }

        [MinLength(2), MaxLength(10)]
        public string? Culture { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateModified { get; set; }

        public required IEnumerable<Client> Clients { get; set; }
    }
}
