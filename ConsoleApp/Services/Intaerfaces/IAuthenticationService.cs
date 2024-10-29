namespace ConsoleApp1.Servises.Interfaces
{
    public interface IAuthenticationService
    {
        bool Authenticate(string username, string password);
    }
}
