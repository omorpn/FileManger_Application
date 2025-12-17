using FileManger_Application.DTOs.FilesDto;

namespace FileManger_Application.Views.ViewsDTOs
{
    public class UserDashboardViewModel
    {

        public string UserName { get; set; } = "";
        public int TotalFiles { get; set; }
        public long StorageUsed { get; set; }
        public List<FileResponse> RecentFiles { get; set; } = new();
    }

}

