using FileManger_Application.Model;

namespace FileManger_Application.DTOs.ApplicationSettingDto
{
    public class ApplicationSettingResponse
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Value { get; set; }
    }
    public static class ExtensionSettings
    {
        public static ApplicationSettingResponse ToApplicatonSettingResponse(this ApplicationSetting applicationSetting)
        {
            return new()
            {
                Id = applicationSetting.Id,
                Name = applicationSetting.Name,
                Value = applicationSetting.Value,
            };
        }
    }
}
