using System.ComponentModel.DataAnnotations;

namespace FileManger_Application.DTOs.FilesDto
{
    public class UpdateFile
    {
        [Required, StringLength(50)]

        public string Id { get; set; }
        [Required, StringLength(50)]
        public string? FileName { get; set; }
        [Required, StringLength(50)]
        public Guid FolderId { get; set; }
        [Required, StringLength(50)]
        public Guid OwnerId { get; set; }

    }
}
