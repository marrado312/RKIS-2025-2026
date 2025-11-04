using System;

namespace TodoList
{
    class Program
    {
        static Profile userProfile;
        static TodoList todoList = new TodoList();

        static void Main(string[] args)
        {
            Console.WriteLine("Работу выполнили Фучаджи и Клюев");

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
    }
}