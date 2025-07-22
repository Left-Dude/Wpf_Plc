using System;
using System.IO;
using System.Threading.Tasks;
using Wpf_Plc.Domain.Entities;
using Wpf_Plc.Domain.Interfaces.Repositories;
using Wpf_Plc.Domain.Interfaces.Services;

namespace Wpf_Plc.Infrastructure.Services;

public class ProgramStorageService : IFileStorageService<PLCProgram>
{
    private readonly string _storagePath;
    private readonly IUnitOfWork _unitOfWork;

    public ProgramStorageService(string storagePath, IUnitOfWork unitOfWork)
    {
        _storagePath = storagePath ?? throw new ArgumentNullException(nameof(storagePath));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        EnsureStorageDirectoryExists();
    }

    public void EnsureStorageDirectoryExists()
    {
        if (!Directory.Exists(_storagePath))
        {
            Directory.CreateDirectory(_storagePath);
        }
    }

    public async Task<PLCProgram> SaveFileAsync(
        Stream fileStream, 
        string fileName, 
        string fileExtension,
        PLCModel plcModel)
    {
        if (fileStream == null) throw new ArgumentNullException(nameof(fileStream));
        if (!fileStream.CanRead) throw new ArgumentException("Stream is not readable", nameof(fileStream));
        if (string.IsNullOrWhiteSpace(fileName)) throw new ArgumentException("File name cannot be empty", nameof(fileName));
        if (plcModel == null) throw new ArgumentNullException(nameof(plcModel));

        var repository = _unitOfWork.GetRepository<PLCProgram>();
        var plcProgram = new PLCProgram
        {
            FileExtension = SanitizeExtension(fileExtension),
            Name = fileName,
            PLCModel = plcModel,
            OriginalFileName = $"{Guid.NewGuid()}{GetExtensionWithDot(fileExtension)}"
        };

        var filePath = Path.Combine(_storagePath, plcProgram.OriginalFileName);
        bool fileSaved = false;

        try
        {
            // Начинаем транзакцию
            await _unitOfWork.BeginTransactionAsync();

            await SaveFileToDiskAsync(fileStream, filePath);
            fileSaved = true;
            
            plcProgram.SizeBytes = new FileInfo(filePath).Length;
            
            // Добавляем сущность в репозиторий
            repository.AddEntity(plcProgram);
            
            // Сохраняем изменения - игнорируем возвращаемое количество строк
            await _unitOfWork.CommitAsync();
            
            // Фиксируем транзакцию
            await _unitOfWork.CommitTransactionAsync();

            return plcProgram;
        }
        catch
        {
            // Откатываем транзакцию при ошибке
            await _unitOfWork.RollbackAsync();
            
            if (fileSaved)
            {
                TryDeleteFile(filePath);
            }
            
            throw;
        }
    }

    private async Task SaveFileToDiskAsync(Stream sourceStream, string filePath)
    {
        if (sourceStream.CanSeek && sourceStream.Position != 0)
        {
            sourceStream.Seek(0, SeekOrigin.Begin);
        }

        await using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        await sourceStream.CopyToAsync(fileStream);
    }

    private void TryDeleteFile(string path)
    {
        try
        {
            if (File.Exists(path)) File.Delete(path);
        }
        catch
        {
            // Игнорируем ошибки удаления файла
        }
    }

    private string SanitizeExtension(string extension)
    {
        return string.IsNullOrWhiteSpace(extension) 
            ? "bin" 
            : extension.Trim().TrimStart('.').ToLowerInvariant();
    }

    private string GetExtensionWithDot(string extension)
    {
        var sanitized = SanitizeExtension(extension);
        return sanitized.Length > 0 ? $".{sanitized}" : "";
    }

    public Stream GetFileStream(PLCProgram entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        
        var filePath = Path.Combine(_storagePath, entity.OriginalFileName);
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("PLC program file not found", filePath);
        }

        return File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
    }

    public void DeleteFile(PLCProgram entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        
        var filePath = Path.Combine(_storagePath, entity.OriginalFileName);
        var repository = _unitOfWork.GetRepository<PLCProgram>();

        try
        {
            // Начинаем транзакцию синхронно
            _unitOfWork.BeginTransactionAsync().GetAwaiter().GetResult();
            
            repository.DeleteEntity(entity);
            
            // Сохраняем изменения - игнорируем возвращаемое количество строк
            _unitOfWork.CommitAsync().GetAwaiter().GetResult();
            
            // Фиксируем транзакцию
            _unitOfWork.CommitTransactionAsync().GetAwaiter().GetResult();
            
            // Удаляем файл только после успешного удаления из БД
            TryDeleteFile(filePath);
        }
        catch
        {
            // Откатываем транзакцию при ошибке
            _unitOfWork.RollbackAsync().GetAwaiter().GetResult();
            throw;
        }
    }

    public string GetStoragePath() => _storagePath;
}

public class FileStorageException : Exception
{
    public FileStorageException() { }
    public FileStorageException(string message) : base(message) { }
    public FileStorageException(string message, Exception inner) : base(message, inner) { }
}