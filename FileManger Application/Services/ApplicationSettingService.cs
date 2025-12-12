using FileManger_Application.DTOs.ApplicationSettingDto;
using FileManger_Application.Exception;
using FileManger_Application.Model;
using FileManger_Application.ServiceContract;
using FileManger_Application.UnitOfWorks;
using Microsoft.EntityFrameworkCore;

namespace FileManger_Application.Services
{
    public class ApplicationSettingService : IApplicationSettingContract
    {
        private readonly IUnitOfWork _unitOfWork;
        public ApplicationSettingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> AddSettings(AddApplicationSettingRequest settingRequest)
        {
            if (settingRequest == null)
            {
                return Result<bool>.Fail("Setting request cannot be null", ErrorType.Validation);
            }
            // Implementation logic to add the setting goes here.
            ApplicationSetting settings = new()
            {
                Name = settingRequest.Name,
                Value = settingRequest.Value,
            };
            await _unitOfWork.ApplicationSetting.AddAsync(settings);
            var result = await _unitOfWork.CompleteAsync();
            return Result<bool>.Ok(result > 0, "Setting added successfully");
        }

        public async Task<Result<bool>> DeleteSettings(string id)
        {
            if (!Guid.TryParse(id, out Guid settingId))
            {
                return Result<bool>.Fail("Setting ID cannot be null or empty", ErrorType.Validation);
            }
            var setting = await _unitOfWork.ApplicationSetting.GetByIdAsync(settingId);
            if (setting == null)
            {
                return Result<bool>.Fail("Setting not found", ErrorType.Not_Found);
            }
            _unitOfWork.ApplicationSetting.RemoveAsync(setting);
            await _unitOfWork.CompleteAsync();
            return Result<bool>.Ok(true);
        }

        public async Task<Result<List<ApplicationSettingResponse>>> GetAllApplicationSettings()
        {
            var result = await _unitOfWork.ApplicationSetting.GetAllAsync();

            return Result<List<ApplicationSettingResponse>>.Ok(result.Select(x => x.ToApplicatonSettingResponse()).ToList());
        }

        public async Task<Result<ApplicationSettingResponse>> GetApplicationSettingsByName(string name)
        {
            if (string.IsNullOrEmpty(name.Trim()))
            {
                return Result<ApplicationSettingResponse>.Fail(" Application setting name ca't be null or empty", ErrorType.Validation);
            }
            var query = _unitOfWork.ApplicationSetting.Query();
            var setting = await query.FirstOrDefaultAsync(query => query.Name == name);
            if (setting == null)
            {
                return Result<ApplicationSettingResponse>.Fail(" Application setting not found", ErrorType.Validation);
            }
            return Result<ApplicationSettingResponse>.Ok(setting.ToApplicatonSettingResponse());

        }

        public async Task<Result<bool>> UpdateSettings(UpdateApplicationSetting settingRequest)
        {
            if (settingRequest == null || (!Guid.TryParse(settingRequest.Id, out Guid settingId)))
            {
                return Result<bool>.Fail(" Application setting ID can't be null. ", ErrorType.Validation);
            }
            var setting = await _unitOfWork.ApplicationSetting.GetByIdAsync(settingId);
            if (setting == null)
            {
                return Result<bool>.Fail(" Application setting not found. ", ErrorType.Not_Found);

            }
            //Update only if new value provided
            if (!string.IsNullOrWhiteSpace(settingRequest.Value))
            {
                setting.Value = settingRequest.Value;
            }
            //mark for update 
            _unitOfWork.ApplicationSetting.Update(setting);
            var result = await _unitOfWork.CompleteAsync();
            //Important: savechanges return the number of state change 
            return Result<bool>.Ok(result > 0);


        }
    }
}
