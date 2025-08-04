using Microsoft.AspNetCore.Http;

namespace MotorbikeRental.Application.Interface.IExternalServices.Storage
{
    public interface IFileService
    {
        Task<string> SaveImage(IFormFile formFile, string folder, CancellationToken cancellationToken = default);
        Task<List<string>> SaveImages(IList<IFormFile> formFiles, string folder, CancellationToken cancellationToken = default);
        bool DeleteFile(string filePath);
        bool DeleteFiles(IList<string> filePaths);
        bool FileExists(string filePath);
        string GetFileUrl(string relativePath);
        bool IsValidImage(IFormFile formFile);
    }
}