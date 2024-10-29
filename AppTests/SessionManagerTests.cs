using ConsoleApp1.Servises;
using ConsoleApp1.Servises.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Text.Json;

namespace AppTests
{
    [TestClass]
    public class SessionManagerTests
    {
        private const string _sessionsFilePath = "sessions.json";
        private readonly Mock<IFileService> _fileServiceMock = new Mock<IFileService>();

        [TestMethod]
        public void CreateSession_ShouldCreateNewSessionAndSaveToFile()
        {
            // Arrange
            string username = "testuser";
            string json = null;
            _fileServiceMock.Setup(f => f.Exists(_sessionsFilePath)).Returns(false);
            _fileServiceMock.Setup(f => f.WriteAllText(_sessionsFilePath, It.IsAny<string>())).
                Callback<string, string>((path, content) =>
            {
                json = content;
            });
            _fileServiceMock.Setup(f => f.ReadAllText(_sessionsFilePath)).Returns("{}");
            SessionManager manager = new SessionManager(_sessionsFilePath, _fileServiceMock.Object);

            // Act
            string sessionId = manager.CreateSession(username);

            // Assert
            Assert.IsNotNull(sessionId);

            Dictionary<string, string> sessions = JsonSerializer.Deserialize<Dictionary<string, string>>(json);

            Assert.AreEqual(1, sessions.Count);
            Assert.AreEqual(username, sessions[sessionId]);
        }

        [TestMethod]
        public void DeleteSession_ShouldRemoveSessionFromFile()
        {
            // Arrange
            string sessionId = "test_session_id";
            string username = "testuser";
            string json = null;
            _fileServiceMock.Setup(f => f.Exists(_sessionsFilePath)).Returns(true);
            _fileServiceMock.Setup(f => f.ReadAllText(_sessionsFilePath))
                .Returns(JsonSerializer.Serialize(new Dictionary<string, string> { { sessionId, username } }));
            _fileServiceMock.Setup(f => f.WriteAllText(_sessionsFilePath, It.IsAny<string>())).
                Callback<string, string>((path, content) =>
                {
                    json = content;
                });

            SessionManager manager = new SessionManager(_sessionsFilePath, _fileServiceMock.Object);

            // Act
            manager.DeleteSession(sessionId);

            // Assert

            Dictionary<string, string> sessions = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            Assert.IsFalse(sessions.ContainsKey(sessionId));
        }

        [TestMethod]
        public void IsSessionValid_ShouldReturnTrueForValidSessionId()
        {
            // Arrange
            string sessionId = "test_session_id";
            string username = "testuser";
            _fileServiceMock.Setup(f => f.Exists(_sessionsFilePath)).Returns(true);
            _fileServiceMock.Setup(f => f.ReadAllText(_sessionsFilePath))
                .Returns(JsonSerializer.Serialize(new Dictionary<string, string> { { sessionId, username } }));

            SessionManager manager = new SessionManager(_sessionsFilePath, _fileServiceMock.Object);

            // Act
            bool isValid = manager.IsSessionValid(sessionId);

            // Assert
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void IsSessionValid_ShouldReturnFalseForInvalidSessionId()
        {
            // Arrange
            string sessionId = "invalid_session_id";
            string username = "testuser";
            _fileServiceMock.Setup(f => f.Exists(_sessionsFilePath)).Returns(true);
            _fileServiceMock.Setup(f => f.ReadAllText(_sessionsFilePath))
                .Returns(JsonSerializer.Serialize(new Dictionary<string, string> { { "test_session_id", username } }));

            SessionManager manager = new SessionManager(_sessionsFilePath, _fileServiceMock.Object);

            // Act
            bool isValid = manager.IsSessionValid(sessionId);

            // Assert
            Assert.IsFalse(isValid);
        }
    }
}
