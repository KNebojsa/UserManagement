using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserManagement.Domain.Entities
{
    public class Client
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey(nameof(UserId))]
        public required User User { get; set; }

        public required Guid UserId { get; set; }

        public required string ApiKey { get; set; }

        public required DateTime CreatedAt { get; set; }
    }
}