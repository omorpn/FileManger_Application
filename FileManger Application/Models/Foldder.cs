using System.ComponentModel.DataAnnotations;

namespace FileManger_Application.Model
{
    public class Foldder
    {
        public Guid Id { get; set; }
        [Required, StringLength(50)]

        public string Name { get; set; } = null!;

        public string? parentFolderId { get; set; }
        [Required, StringLength(50)]

        public Guid OwnerId { get; set; }


        public DateTime CreatedAt { get; set; }
    }
}
