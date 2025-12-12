using System.ComponentModel.DataAnnotations;

namespace FileManger_Application.DTOs.FilesDto
{
    public class AddFileRequest
    {


        [Required, StringLength(50)]
        public IFormFile File { get; set; } = null!;


        [Required, StringLength(50)]
        public Guid FolderId { get; set; }
        [Required, StringLength(50)]
        public Guid OwnerId { get; set; }

    }
}
