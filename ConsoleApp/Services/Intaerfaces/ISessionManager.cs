namespace ConsoleApp1.Servises.Interfaces
{
    public interface ISessionManager
    {
        string CreateSession(string username);
        bool IsSessionValid(string sessionId);
        void DeleteSession(string sessionId);
    }
}
