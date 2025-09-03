using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Domain.Interfaces
{
    public interface IDomainEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }
    }
}
