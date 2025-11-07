using System;
using System.IO;
using ToDoList.Commands;

namespace TodoList
{
    class Program
    {
        static Profile userProfile;
        static TodoList todoList = new TodoList();

        static string dataDir = Path.Combine(Directory.GetCurrentDirectory(), "data");
        static string profileFilePath = Path.Combine(dataDir, "profile.txt");
        static string todoFilePath = Path.Combine(dataDir, "todo.csv");

        static void Main(string[] args)
        {
            Console.WriteLine("Работу выполнили Фучаджи и Клюев");

            FileManager.EnsureDataDirectory(dataDir);
            LoadData();

            CreateUser();

            Console.WriteLine("Введите help для вывода доступных команд");


            while (true)
            {

                Console.Write("Введите команду:");
                string CommandUser = Console.ReadLine();

                if (CommandUser == null || CommandUser.ToLower() == "exit")
                    break;

                ICommand command = CommandParser.Parse(CommandUser, todoList, userProfile);
                if (command != null)
                    command.Execute();

                SaveData();
            }
        }

        static void CreateUser()
        {
            Console.Write("Введите ваше имя: ");
            string firstName = Console.ReadLine();
            if (string.IsNullOrEmpty(firstName))
            {
                Console.WriteLine("Имя не может быть пустым");
                return;
            }
            Console.Write("Введите вашу Фамилию: ");
            string lastName = Console.ReadLine();

            Console.Write("Введите вашу дату рождения: ");
            string birthYearInput = Console.ReadLine();
            if (!int.TryParse(birthYearInput, out int birthYear) || birthYear <= 0)
            {
                Console.WriteLine("Введите реальный возраст");
                return;
            }
            userProfile = new Profile(firstName, lastName, birthYear);
            Console.WriteLine($"Добавлен пользователь {userProfile.GetInfo()}");
        }

        static void SaveData()
        {
            if (userProfile != null)
                FileManager.SaveProfile(userProfile, profileFilePath);
            FileManager.SaveTodos(todoList, todoFilePath);
        }

        static void LoadData()
        {
            Console.WriteLine($"Создаем папку: {dataDir}");
            FileManager.EnsureDataDirectory(dataDir);

            if (!File.Exists(profileFilePath))
                File.WriteAllText(profileFilePath, "Default User");
            if (!File.Exists(todoFilePath))
                File.WriteAllText(todoFilePath, "");

            userProfile = FileManager.LoadProfile(profileFilePath);
            if (userProfile != null)
            {
                Console.WriteLine($"Загружен профиль: {userProfile.GetInfo()}");
            }
            var loadedTodos = FileManager.LoadTodos(todoFilePath);
            if (loadedTodos.Count > 0)
            {
                todoList = loadedTodos;
                Console.WriteLine($"Загружено задач: {todoList.Count}");
            }
        }
    }
}
