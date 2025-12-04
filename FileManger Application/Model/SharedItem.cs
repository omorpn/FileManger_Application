using System.ComponentModel.DataAnnotations;

namespace FileManger_Application.Model
{
    public class SharedItem
    {
        public Guid Id { get; set; }
        [Required, StringLength(50)]
        public string? ItemType { get; set; }
        [Required, StringLength(50)]
        public string? SharedWithUserId { get; set; }
        [Required, StringLength(50)]
        public List<string>? Permissions { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
