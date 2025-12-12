using FileManger_Application.Exception;
using FileManger_Application.ServiceContract;

namespace FileManger_Application.Services
{
    public class StorageService : IStorageContract
    {
        private readonly IApplicationSettingContract _applicationSevice;
        private readonly IWebHostEnvironment _env;
        private string _contentRootPath { set; get; }
        private string _webRootPath { set; get; }
        public StorageService(IApplicationSettingContract applicationSevice,
            IWebHostEnvironment env
            )
        {
            var publicPaths = new[]
            {
                Path.Combine(env.WebRootPath,"uploads","images"),
                Path.Combine(env.WebRootPath,"uploads","document"),
                Path.Combine(env.WebRootPath,"uploads","temp"),

            };
            var privatePaths = new[]
            {
                Path.Combine(env.ContentRootPath,"storage","app","logs"),
                Path.Combine(env.ContentRootPath,"storage","app","temp"),
                Path.Combine(env.ContentRootPath,"storage","private","user-files"),
                Path.Combine(env.ContentRootPath,"storage","upload","raw"),
            };
            foreach (var publicPath in publicPaths.Concat(privatePaths))
            {
                if (!Directory.Exists(publicPath))
                {
                    Directory.CreateDirectory(publicPath);
                }
            }
            _applicationSevice = applicationSevice;

            _env = env;
            _contentRootPath = _env.ContentRootPath;
            _webRootPath = _env.WebRootPath;

        }
        public async Task<Result<string>> SavePublicFileAsync(IFormFile file)
        {
            if (file == null)
            {
                return Result<string>.Fail("file cant be empty", ErrorType.Validation);
            }
            string? filePath = null;

            if (file.ContentType.StartsWith("image"))
            {
                var result = await FileSaver(file, "image", _webRootPath);
                return result;
            }

            var result2 = await FileSaver(file, "document", _webRootPath);
            return result2;
        }

        public async Task<Result<string>> SavePrivateFileAsync(IFormFile file)
        {
            if (file == null)
            {
                return Result<string>.Fail("file cant be empty", ErrorType.Validation);
            }
            string? filePath = null;
            filePath = Path.Combine("storage", "private", "user-files");
            var result = await FileSaver(file, filePath, _contentRootPath);
            return result;
        }
        private string FileNameGenerator()
        {
            var newFileName = Guid.NewGuid().ToString("N");

            return newFileName;
        }
        private async Task<Result<string>> FileSaver(IFormFile file, string targetDirectory, string evn)
        {

            var maxSettings = await _applicationSevice.GetApplicationSettingsByName("MAXFILESIZE");
            long? maxFileSize = null;

            //convert max file to long 
            if (long.TryParse(maxSettings?.Data?.Value, out long parsedLimit))
            {
                maxFileSize = parsedLimit;
            }
            var fullDirectoryPath = Path.Combine(evn, targetDirectory.TrimStart('/', '\\'));
            if (!Directory.Exists(fullDirectoryPath))
            {
                Directory.CreateDirectory(targetDirectory);
            }
            string? fileName = null;

            // check either file size limit is set 
            if (maxFileSize.HasValue && file.Length > maxFileSize.Value)
            {
                return Result<string>.Fail("file size execced max file size", ErrorType.Validation);
            }
            var extension = Path.GetExtension(file.FileName);
            fileName = $"{FileNameGenerator()}{extension}";
            var fullFilePath = Path.Combine(fullDirectoryPath, fileName);
            using (var stream = new FileStream(fullFilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            var relativePath = fullFilePath.Replace(evn, "").Replace("\\", "/");
            return Result<string>.Ok(relativePath);
        }

        public async Task<Result<bool>> DeleteFileByPath(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
                return Result<bool>.Ok(true);

            }
            return Result<bool>.Ok(false);
        }

        public async Task<string> GetPublicFile()
        {
            return _webRootPath.ToString();
        }

        public async Task<string> GetPrivateFile()
        {
            return _contentRootPath.ToString();
        }
    }
}
