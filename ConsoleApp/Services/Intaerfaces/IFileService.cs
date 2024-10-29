namespace ConsoleApp1.Servises.Interfaces
{
    public interface IFileService
    {
        bool Exists(string filePath);
        string ReadAllText(string filePath);
        void WriteAllText(string filePath, string content);
    }
}
