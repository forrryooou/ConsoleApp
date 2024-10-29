using ConsoleApp1.Servises.Interfaces;
using System.IO;

namespace ConsoleApp1.Servises
{
    public class FileService : IFileService
    {
        public bool Exists(string filePath)
        {
            return File.Exists(filePath);
        }

        public string ReadAllText(string filePath)
        {
            return File.ReadAllText(filePath);
        }

        public void WriteAllText(string filePath, string content)
        {
            File.WriteAllText(filePath, content);
        }
    }
}
