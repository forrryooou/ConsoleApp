using ConsoleApp1.Servises;
using ConsoleApp1.Servises.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AppTests
{
    [TestClass]
    public class AuthenticationServiceTests
    {
        private const string UsersFilePath = "users.txt";
        private readonly Mock<IFileService> _fileServiceMock = new Mock<IFileService>();

        [TestMethod]
        public void Authenticate_ShouldReturnTrueForValidCredentials()
        {
            // Arrange
            _fileServiceMock.Setup(f => f.ReadAllText(UsersFilePath)).Returns("{\"user1\":\"password1\",\"user2\":\"password2\"}");
            AuthenticationService service = new AuthenticationService(UsersFilePath, _fileServiceMock.Object);

            // Act
            bool isAuthenticated = service.Authenticate("user1", "password1");

            // Assert
            Assert.IsTrue(isAuthenticated);
        }

        [TestMethod]
        public void Authenticate_ShouldReturnFalseForNotValidCredentials()
        {
            // Arrange
            _fileServiceMock.Setup(f => f.ReadAllText(UsersFilePath)).Returns("{\"user2\":\"password2\",\"user3\":\"password3\"}");
            AuthenticationService service = new AuthenticationService(UsersFilePath, _fileServiceMock.Object);

            // Act
            bool isAuthenticated = service.Authenticate("testuser", "testpassword");

            // Assert
            Assert.IsFalse(isAuthenticated);
        }
    }
}
