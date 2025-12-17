using System.ComponentModel.DataAnnotations;

namespace FileManger_Application.DTOs.FilesDto
{
    public class AddFileRequest
    {


        [Required, StringLength(50)]
        public IFormFileCollection File { get; set; } = null!;



        public Guid FolderId { get; set; }

        public Guid OwnerId { get; set; }

    }
}
