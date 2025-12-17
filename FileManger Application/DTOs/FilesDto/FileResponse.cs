using FileManger_Application.Model;
using System.ComponentModel.DataAnnotations;

namespace FileManger_Application.DTOs.FilesDto
{
    public class FileResponse
    {
        public Guid Id { get; set; }
        [Required, StringLength(50)]
        public string? FileName { get; set; }
        [Required, StringLength(50)]
        public string? Path { get; set; }
        [Required, StringLength(50)]
        public Decimal? Size { get; set; }
        [Required, StringLength(50)]
        public string? Type { get; set; }
        [Required, StringLength(50)]
        public Guid FolderId { get; set; }
        public bool IsDeleted { get; set; }
        [Required, StringLength(50)]
        public Guid OwnerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
    public static class ExtenstionFile
    {
        public static FileResponse ToFileResponse(this Files file)
        {
            return new()
            {
                Id = file.Id,
                FileName = file.FileName,
                Path = file.Path,
                Size = file.Size,
                Type = file.Type,
                OwnerId = file.OwnerId,
                FolderId = file.FolderId,
                IsDeleted = false,
                CreatedAt = file.CreatedAt,
                UpdatedAt = file.UpdatedAt,

            };
        }
    }
}
