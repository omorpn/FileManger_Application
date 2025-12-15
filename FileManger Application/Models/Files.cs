using System.ComponentModel.DataAnnotations;

namespace FileManger_Application.Model
{
    public class Files
    {
        public Guid Id { get; set; }
        [Required, StringLength(50)]
        public string? FileName { get; set; }
        [Required, StringLength(50)]
        public string? Path { get; set; }
        [Required, StringLength(50)]

        public long Size { get; set; }
        [Required, StringLength(50)]
        public string? Type { get; set; }
        [Required, StringLength(50)]
        public Guid FolderId { get; set; }
        [Required, StringLength(50)]
        public Guid OwnerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }


    }
}
