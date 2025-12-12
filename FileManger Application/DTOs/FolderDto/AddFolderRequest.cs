using System.ComponentModel.DataAnnotations;

namespace FileManger_Application.DTOs.FolderDto
{
    public class AddFolderRequest
    {
        public string? Name { get; set; }

        public string? parentFolderId { get; set; }
        [Required, StringLength(50)]

        public string? OwnerId { get; set; }
    }
}
