using ConsoleApp1.Servises.Interfaces;
using System.Collections.Generic;
using System.Text.Json;

namespace ConsoleApp1.Servises
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly Dictionary<string, string> _users;
        private readonly IFileService _fileService;

        public AuthenticationService(string filename, IFileService fileService)
        {
            _fileService = fileService;
            _users = LoadUsers(filename);
        }

        public bool Authenticate(string username, string password)
        {
            return _users.ContainsKey(username) && _users[username] == password;
        }

        private Dictionary<string, string> LoadUsers(string filename)
        {
            string jsonString = _fileService.ReadAllText(filename);
            return JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString);
        }
    }
}