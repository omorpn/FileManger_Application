namespace FileManger_Application.DTOs.ApplicationSettingDto
{
    public class UpdateApplicationSetting
    {
        public string Id { get; set; } = null!;
        public string? Name { get; set; }
        public string? Value { get; set; }
    }
}
