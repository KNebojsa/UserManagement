using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UserManagement.Domain.Interfaces;

namespace UserManagement.Domain.Entities
{
    public class Client : IDomainEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        public required string ApiKey { get; set; }

        public DateTime DateCreated { get; set; }

        [ForeignKey(nameof(UserId))]
        public required User User { get; set; }
    }
}