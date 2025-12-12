using System.ComponentModel.DataAnnotations;

namespace FileManger_Application.DTOs.FolderDto
{
    public class FolderUpdate
    {
        public string Id { get; set; }
        [Required, StringLength(50)]

        public string? Name { get; set; }

    }
}
