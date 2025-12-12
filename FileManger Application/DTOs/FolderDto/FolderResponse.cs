using FileManger_Application.Model;
using System.ComponentModel.DataAnnotations;

namespace FileManger_Application.DTOs.FolderDto
{
    public class FolderResponse
    {

        public Guid Id { get; set; }
        [Required, StringLength(50)]

        public string? Name { get; set; }

        public string? parentFolderId { get; set; }
        [Required, StringLength(50)]

        public string? OwnerId { get; set; }


        public DateTime CreatedAt { get; set; }

    }
    public static class ExtensionFolder
    {
        public static FolderResponse ToFolderResponse(this Foldder foldder)
        {

            return new FolderResponse
            {
                Id = foldder.Id,
                Name = foldder.Name,
                OwnerId = foldder.OwnerId.ToString(),
                CreatedAt = foldder.CreatedAt,
                parentFolderId = foldder.parentFolderId
            };

        }
    }

}
