using System.ComponentModel.DataAnnotations;
using Data.DataStore.LLM_Platform.Common;

namespace Data.DataStore.LLM_Platform.Users
{
    public class User : ICreatedAt, IUpdatedAt
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Account { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string Password { get; set; } = null!;

        [Required]
        public DateTimeOffset CreatedAt { get; set; }

        [Required]
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
