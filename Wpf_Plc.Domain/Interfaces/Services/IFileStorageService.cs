using Wpf_Plc.Domain.Entities;

namespace Wpf_Plc.Domain.Interfaces.Services;

public interface IFileStorageService<TFileEntity> where TFileEntity : BaseEntity
{
    void EnsureStorageDirectoryExists();
    Task<TFileEntity> SaveFileAsync(Stream fileStream, string fileName, string fileExtension, PLCModel plcModel);
    Stream GetFileStream(TFileEntity entity);
    void DeleteFile(TFileEntity entity);
    public string GetStoragePath();
}