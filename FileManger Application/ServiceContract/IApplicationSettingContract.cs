using FileManger_Application.DTOs.ApplicationSettingDto;
using FileManger_Application.Exception;

namespace FileManger_Application.ServiceContract
{
    public interface IApplicationSettingContract
    {
        /// <summary>
        /// Adds a new application setting asynchronously.
        /// </summary>
        /// <param name="settingRequest">The request containing the details of the application setting to add. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a value indicating whether the
        /// setting was added successfully.</returns>
        Task<Result<bool>> AddSettings(AddApplicationSettingRequest settingRequest);
        /// <summary>
        /// 
        /// Deletes the settings associated with the specified identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the settings to delete. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous delete operation. The task result contains a value indicating
        /// whether the settings were successfully deleted.</returns>
        Task<Result<bool>> DeleteSettings(string id);
        /// <summary>
        /// Updates the application settings using the specified configuration request.
        /// </summary>
        /// <param name="settingRequest">An object containing the new application settings to apply. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a value indicating whether the
        /// update was successful.</returns>
        Task<Result<bool>> UpdateSettings(UpdateApplicationSetting settingRequest);
        /// <summary>
        /// Asynchronously retrieves the application settings associated with the specified name.
        /// </summary>
        /// <param name="name">The name of the application settings to retrieve. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="Result{T}"/>
        /// wrapping an <see cref="ApplicationSettingResponse"/> if the settings are found; otherwise, an error result.</returns>
        Task<Result<ApplicationSettingResponse>> GetApplicationSettingsByName(string name);
        /// <summary>
        /// Retrieves all application settings asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of application settings.
        /// The list will be empty if no settings are found.</returns>
        Task<Result<List<ApplicationSettingResponse>>> GetAllApplicationSettings();

    }
}
