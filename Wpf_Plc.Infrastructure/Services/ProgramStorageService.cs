using System;
using System.IO;
using System.Threading.Tasks;
using Wpf_Plc.Domain.Entities;
using Wpf_Plc.Domain.Interfaces.Services;

namespace Wpf_Plc.Infrastructure.Services
{
    public class ProgramStorageService : IFileStorageService<PLCProgram>
    {
        private readonly string _storagePath;

        public ProgramStorageService(string storagePath)
        {
            _storagePath = storagePath 
                ?? throw new ArgumentNullException(nameof(storagePath));
            EnsureStorageDirectoryExists();
        }

        public void EnsureStorageDirectoryExists()
        {
            if (!Directory.Exists(_storagePath))
                Directory.CreateDirectory(_storagePath);
        }

        public async Task<PLCProgram> SaveFileAsync(
            Stream fileStream,
            string fileName,
            string fileExtension,
            PLCModel plcModel)
        {
            if (fileStream == null) 
                throw new ArgumentNullException(nameof(fileStream));
            if (!fileStream.CanRead) 
                throw new ArgumentException("Stream is not readable", nameof(fileStream));
            if (string.IsNullOrWhiteSpace(fileName)) 
                throw new ArgumentException("File name cannot be empty", nameof(fileName));
            if (plcModel == null) 
                throw new ArgumentNullException(nameof(plcModel));

            // Создаём новую программу
            var program = new PLCProgram
            {
                Id               = Guid.NewGuid(),
                Name             = fileName,
                FileExtension    = SanitizeExtension(fileExtension),
                OriginalFileName = $"{Guid.NewGuid()}{GetExtensionWithDot(fileExtension)}",
                PLCModel       = plcModel
            };

            var filePath = Path.Combine(_storagePath, program.OriginalFileName);

            // Сбрасываем позицию, если надо
            if (fileStream.CanSeek && fileStream.Position != 0)
                fileStream.Seek(0, SeekOrigin.Begin);

            // Сохраняем файл на диск
            await using (var outFs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                await fileStream.CopyToAsync(outFs).ConfigureAwait(false);
            }

            // Заполняем размер
            program.SizeBytes = new FileInfo(filePath).Length;

            return program;
        }

        public Stream GetFileStream(PLCProgram entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var filePath = Path.Combine(_storagePath, entity.OriginalFileName);
            if (!File.Exists(filePath))
                throw new FileNotFoundException("PLC program file not found", filePath);

            return File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        public void DeleteFile(PLCProgram entity)
        {
            if (entity == null) 
                throw new ArgumentNullException(nameof(entity));

            var filePath = Path.Combine(_storagePath, entity.OriginalFileName);
            if (File.Exists(filePath))
                File.Delete(filePath);
        }

        public string GetStoragePath()
        {
            return _storagePath;
        }

        private string SanitizeExtension(string ext) =>
            string.IsNullOrWhiteSpace(ext)
                ? "bin"
                : ext.Trim().TrimStart('.').ToLowerInvariant();

        private string GetExtensionWithDot(string ext)
        {
            var s = SanitizeExtension(ext);
            return s.Length > 0 ? "." + s : string.Empty;
        }
    }
}
