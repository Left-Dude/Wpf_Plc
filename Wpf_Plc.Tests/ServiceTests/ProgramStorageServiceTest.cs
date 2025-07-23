using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Wpf_Plc.Domain.Entities;
using Wpf_Plc.Infrastructure.Services;
using Xunit;

namespace Wpf_Plc.Tests.ServiceTests
{
    public class ProgramStorageServiceTests : IDisposable
    {
        private readonly string _testStoragePath;
        private readonly string _testFilesDirectory;
        private readonly ProgramStorageService _service;

        public ProgramStorageServiceTests()
        {
            // Директория для размещения исходных тестовых файлов
            _testFilesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "TestFiles");
            Directory.CreateDirectory(_testFilesDirectory);

            // Создаем небольшой тестовый файл
            var sampleFilePath = Path.Combine(_testFilesDirectory, "test_program.cxp");
            File.WriteAllText(sampleFilePath, "Sample CXP content");

            // Директория для хранилища
            _testStoragePath = Path.Combine(Path.GetTempPath(), "PLCStorageTests", Guid.NewGuid().ToString());
            Directory.CreateDirectory(_testStoragePath);

            _service = new ProgramStorageService(_testStoragePath);
        }
        
        [Fact]
        public async Task SaveFileAsync_WithRealCxpFile_WorksCorrectly()
        {
            // Arrange
            var plcModel = new PLCModel { Id = Guid.NewGuid(), Manufacturer = "Test", Model = "TestPLC" };
            var sourcePath = Path.Combine(_testFilesDirectory, "test_program.cxp");
            var fileName = "test_program";
            var fileExtension = "cxp";
            var originalSize = new FileInfo(sourcePath).Length;
            var originalHash = ComputeFileHash(sourcePath);

            await using var fs = File.OpenRead(sourcePath);

            // Act
            var result = await _service.SaveFileAsync(fs, fileName, fileExtension, plcModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(fileName, result.Name);
            Assert.Equal(fileExtension, result.FileExtension);
            Assert.Equal(originalSize, result.SizeBytes);

            var savedPath = Path.Combine(_testStoragePath, result.OriginalFileName);
            Assert.True(File.Exists(savedPath), "Файл должен сохраниться в хранилище");

            var savedHash = ComputeFileHash(savedPath);
            Assert.Equal(originalHash, savedHash);
        }
        
        [Fact]
        public void GetFileStream_ReturnsCorrectContent()
        {
            // Arrange
            var content = "HELLO-CXP";
            var fileName = $"{Guid.NewGuid()}.cxp";
            var filePath = Path.Combine(_testStoragePath, fileName);
            File.WriteAllText(filePath, content);

            var entity = new PLCProgram
            {
                OriginalFileName = fileName,
                FileExtension    = "cxp",
                Name             = "Test",
                SizeBytes        = content.Length
            };

            // Act
            using var stream = _service.GetFileStream(entity);
            using var reader = new StreamReader(stream);
            var read = reader.ReadToEnd();

            // Assert
            Assert.Equal(content, read);
        }

        [Fact]
        public void DeleteFile_RemovesFile()
        {
            // Arrange
            var content = "DELETE-ME";
            var fileName = $"{Guid.NewGuid()}.cxp";
            var filePath = Path.Combine(_testStoragePath, fileName);
            File.WriteAllText(filePath, content);

            var entity = new PLCProgram
            {
                OriginalFileName = fileName,
                FileExtension    = "cxp",
                Name             = "ToDelete",
                SizeBytes        = content.Length
            };

            // Act
            _service.DeleteFile(entity);

            // Assert
            Assert.False(File.Exists(filePath), "Файл должен быть удалён");
        }

        private string ComputeFileHash(string path)
        {
            using var stream = File.OpenRead(path);
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(stream);
            return BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant();
        }

        public void Dispose()
        {
            // Убираем временные директории
            if (Directory.Exists(_testStoragePath))
                Directory.Delete(_testStoragePath, true);
            if (Directory.Exists(_testFilesDirectory))
                Directory.Delete(_testFilesDirectory, true);
        }
    }
}
