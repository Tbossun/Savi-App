using Microsoft.AspNetCore.Http;
using SavingsApp.Data.Entities.Domains;

namespace SavingsApp.Core.Services.Interfaces
{
    public interface IDocumentUploadService
    {
        Task<DocumentUploadResult> UploadFileAsync(string documentContent);
        Task<DocumentUploadResult> UploadImageAsync(IFormFile image);
    }
}
