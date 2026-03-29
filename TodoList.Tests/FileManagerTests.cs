using System;
using Moq;
using TodoList;
using ToDoList;
using Xunit;

namespace TodoList.Tests
{
	public class FileManagerTests
	{
		[Fact]
		public void SaveProfile_WhenCalled_CallsWriteAllTextWithCorrectData()
		{
			var fsMock = new Mock<IFileSystem>();
			var manager = new FileManager(fsMock.Object);
			var profile = new Profile("Илья", "Тестов", 2000);
			string path = "profile.txt";

			manager.SaveProfile(profile, path);

			fsMock.Verify(fs => fs.WriteAllText(path, It.Is<string>(s => s.Contains("Илья") && s.Contains("2000"))), Times.Once);
		}

		[Fact]
		public void LoadProfile_FileDoesNotExist_ReturnsNull()
		{
			var fsMock = new Mock<IFileSystem>();
			fsMock.Setup(fs => fs.Exists(It.IsAny<string>())).Returns(false);
			var manager = new FileManager(fsMock.Object);

			var result = manager.LoadProfile("nonexistent.txt");

			Assert.Null(result);
		}

		[Fact]
		public void EnsureDataDirectory_DirectoryDoesNotExist_CreatesDirectory()
		{
			var fsMock = new Mock<IFileSystem>();
			fsMock.Setup(fs => fs.DirectoryExists(It.IsAny<string>())).Returns(false);
			var manager = new FileManager(fsMock.Object);
			string path = "data_folder";

			manager.EnsureDataDirectory(path);

			fsMock.Verify(fs => fs.CreateDirectory(path), Times.Once);
		}
	}
}