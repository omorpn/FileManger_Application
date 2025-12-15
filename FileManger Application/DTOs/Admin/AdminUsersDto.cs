namespace FileManger_Application.DTOs.Admin
{

    public class AdminUsersDto
    {
        public int TotalUsers { get; set; }
        public List<AdminUserRowDto> Users { get; set; } = new();
    }

    public class AdminUserRowDto
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}


