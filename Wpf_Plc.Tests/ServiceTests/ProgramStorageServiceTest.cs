using System.Security.Cryptography;
using Moq;
using Wpf_Plc.Domain.Entities;
using Wpf_Plc.Domain.Interfaces.Repositories;
using Wpf_Plc.Infrastructure.Services;

namespace Wpf_Plc.Tests.ServiceTests
{
    public class ProgramStorageServiceTests : IDisposable
    {
        private readonly string _testStoragePath;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<PLCProgram>> _repositoryMock;
        private readonly ProgramStorageService _service;
        private readonly string _testFilesDirectory;

        public ProgramStorageServiceTests()
        {
            // Директория для тестовых файлов
            _testFilesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "TestFiles");
            Directory.CreateDirectory(_testFilesDirectory);
            
            // Директория для хранилища
            _testStoragePath = Path.Combine(Path.GetTempPath(), "PLCStorageTests", Guid.NewGuid().ToString());
            Directory.CreateDirectory(_testStoragePath);
            
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _repositoryMock = new Mock<IRepository<PLCProgram>>();
            
            // Настраиваем моки
            _unitOfWorkMock.Setup(u => u.GetRepository<PLCProgram>()).Returns(_repositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.CommitTransactionAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.RollbackAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.CommitAsync()).ReturnsAsync(1); // Возвращаем 1 сохраненную запись
            
            _service = new ProgramStorageService(_testStoragePath, _unitOfWorkMock.Object);
        }
        
        [Fact]
        public async Task SaveFileAsync_WithRealCxpFile_WorksCorrectly()
        {
            // Arrange
            var plcModel = new PLCModel { Id = Guid.NewGuid(), Manufacturer = "Test", Model = "TestPLC" };
    
            // Путь к реальному файлу в проекте
            var projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            var cxpFilePath = Path.Combine(projectDirectory, "TestFiles", "test_program.cxp");
    
            if (!File.Exists(cxpFilePath))
            {
                throw new FileNotFoundException("Test CXP file not found", cxpFilePath);
            }
    
            var fileName = "test_program";
            var fileExtension = "cxp";
            var originalFileSize = new FileInfo(cxpFilePath).Length;
    
            await using var fileStream = File.OpenRead(cxpFilePath);
            var originalFileHash = ComputeFileHash(cxpFilePath);

            // Act
            var result = await _service.SaveFileAsync(fileStream, fileName, fileExtension, plcModel);
    
            // Assert
            Assert.NotNull(result);
            Assert.Equal(fileName, result.Name);
            Assert.Equal("cxp", result.FileExtension);
            Assert.Equal(originalFileSize, result.SizeBytes);
    
            var savedFilePath = Path.Combine(_testStoragePath, result.OriginalFileName);
            var savedFileHash = ComputeFileHash(savedFilePath);
            Assert.Equal(originalFileHash, savedFileHash);
        }
        
        [Fact]
        public void GetFileStream_WithRealCxpFile_ReturnsCorrectContent()
        {
            // Arrange
            // Создаем тестовый файл в хранилище
            var fileName = $"{Guid.NewGuid()}.cxp";
            var filePath = Path.Combine(_testStoragePath, fileName);
            
            var cxpContent = "CX-Programmer file content";
            File.WriteAllText(filePath, cxpContent);
            
            var entity = new PLCProgram 
            { 
                OriginalFileName = fileName,
                FileExtension = "cxp",
                Name = "TestProgram",
                SizeBytes = cxpContent.Length
            };

            // Act
            using var stream = _service.GetFileStream(entity);
            using var reader = new StreamReader(stream);
            var content = reader.ReadToEnd();

            // Assert
            Assert.Equal(cxpContent, content);
        }

        [Fact]
        public void DeleteFile_WithRealCxpFile_DeletesCorrectly()
        {
            // Arrange
            var fileName = $"{Guid.NewGuid()}.cxp";
            var filePath = Path.Combine(_testStoragePath, fileName);
            
            var cxpContent = "CX-Programmer file content";
            File.WriteAllText(filePath, cxpContent);
            
            var entity = new PLCProgram 
            { 
                OriginalFileName = fileName,
                FileExtension = "cxp",
                Name = "TestProgram",
                SizeBytes = cxpContent.Length
            };

            // Act
            _service.DeleteFile(entity);

            // Assert
            Assert.False(File.Exists(filePath));
            _repositoryMock.Verify(r => r.DeleteEntity(entity), Times.Once);
            _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
        }

        private void CreateLargeFile(string path, long sizeInBytes)
        {
            using var fileStream = File.Create(path);
            fileStream.SetLength(sizeInBytes);
        }

        private string ComputeFileHash(string filePath)
        {
            using var stream = File.OpenRead(filePath);
            using var sha256 = SHA256.Create();
            var hashBytes = sha256.ComputeHash(stream);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
        }

        public void Dispose()
        {
            // Очистка тестовых директорий
            if (Directory.Exists(_testStoragePath))
            {
                Directory.Delete(_testStoragePath, true);
            }
            
            if (Directory.Exists(_testFilesDirectory))
            {
                Directory.Delete(_testFilesDirectory, true);
            }
        }
    }
}