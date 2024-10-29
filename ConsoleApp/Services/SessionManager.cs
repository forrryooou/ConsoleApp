using ConsoleApp1.Servises.Interfaces;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace ConsoleApp1.Servises
{
    public class SessionManager : ISessionManager
    {
        private readonly string _sessionsFilePath;
        private readonly IFileService _fileService;
        private Dictionary<string, string> _sessions = new Dictionary<string, string>();

        public SessionManager(string sessionsFilePath, IFileService fileService)
        {
            _sessionsFilePath = sessionsFilePath;
            _fileService = fileService;
            LoadSessions();
        }

        public string CreateSession(string username)
        {
            string sessionId = Guid.NewGuid().ToString();
            _sessions[sessionId] = username;
            SaveSessions();
            return sessionId;
        }

        public bool IsSessionValid(string sessionId)
        {
            return _sessions.ContainsKey(sessionId);
        }

        public void DeleteSession(string sessionId)
        {
            if (_sessions.ContainsKey(sessionId))
            {
                _sessions.Remove(sessionId);
                SaveSessions();
            }
            else Console.WriteLine("Такой сессии не существует");
        }

        private void LoadSessions()
        {
            if (_fileService.Exists(_sessionsFilePath))
            {
                string json = _fileService.ReadAllText(_sessionsFilePath);
                _sessions = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            }
            else _sessions = new Dictionary<string, string>();
        }

        private void SaveSessions()
        {
            string json = JsonSerializer.Serialize(_sessions);
            _fileService.WriteAllText(_sessionsFilePath, json);
        }
    }
}
