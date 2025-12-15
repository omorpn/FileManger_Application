using FileManger_Application.DTOs.UserDto;

namespace FileManger_Application.DTOs.Admin
{

    public class AdminDashboardDto
    {
        // KPI cards
        public int TotalUsers { get; set; }
        public int TotalFiles { get; set; }
        public int ActiveSessions { get; set; }
        public decimal TotalStorageUsedInMB { get; set; }
        public double StorageUsedPercentage { get; set; }

        // Recent users table
        public List<UserResponse> RecentUsers { get; set; } = new();
    }



}


