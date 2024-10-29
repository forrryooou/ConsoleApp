using ConsoleApp1.Servises;
using ConsoleApp1.Servises.Interfaces;
using System;

namespace ConsoleApp
{
    class Program
    {
        static void Main()
        {
            string sessionsFile = "../../sessions.json";
            string usersFile = "../../users.json";

            IFileService fileService = new FileService();

            if (!fileService.Exists(usersFile))
            {
                Console.WriteLine($"Файл с пользователями '{usersFile}' не найден.");
                return;
            }

            ISessionManager sessionManager = new SessionManager(sessionsFile, fileService);
            IAuthenticationService authenticationService = new AuthenticationService(usersFile, fileService);



            while (true)
            {
                Console.Write("Введите идентификатор сессии: ");
                string sessionId = Console.ReadLine();

                if (sessionId.StartsWith("delete "))
                {
                    sessionManager.DeleteSession(sessionId.Split(' ')[1]);
                    continue;
                }

                if (sessionManager.IsSessionValid(sessionId))
                {
                    Console.WriteLine("Вы уже вошли в систему.");
                    continue;
                }

                Console.Write("Введите логин: ");
                string login = Console.ReadLine();
                Console.Write("Введите пароль: ");
                string password = Console.ReadLine();

                if (authenticationService.Authenticate(login, password))
                {
                    sessionId = sessionManager.CreateSession(login);
                    Console.WriteLine($"Идентификатор сессии: {sessionId}");
                }
                else
                {
                    Console.WriteLine("Неверный логин или пароль.");
                }
            }
        }
    }
}