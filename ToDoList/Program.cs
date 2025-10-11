using System.Data;

namespace TodoList
{
    class Program
    {
        static string userName;
        static string userLastName;
        static int userAge;
        static string[] todos = new string[2];
        static int taskCount = 0;
        static void Main(string[] args)
        {
            Console.WriteLine("Работу выполнили Фучаджи и Клюев");

            userCreate();

            Console.WriteLine("Введите help для вывода доступных команд");


            while (true)
            {

                Console.Write("Введите команду:");
                string userCommand = Console.ReadLine();

                if (userCommand == null || userCommand.ToLower() == "exit") break;

                if (userCommand == "help")
                {
                    helpCommand();
                }
                else if (userCommand == "profile")
                {
                    profileCommand();
                }
                else if (userCommand == "add")
                {
                    addCommand(UserCommand);
                }
                else if (userCommand == "view")
                {
                    viewCommand();
                }
            }
        }
        static void userCreate()
        {
            Console.Write("Введите ваше имя: ");
            userName = Console.ReadLine();
            Console.Write("Введите вашу Фамилию: ");
            userLastName = Console.ReadLine();

            Console.Write("Введите вашу дату рождения: ");
            string birthYearInput = Console.ReadLine();
            int birthYear = int.Parse(birthYearInput);
            if (birthYear <= 0)
            {
                Console.WriteLine("Введите реальный возраст");
                return;
            }
            int currentYear = DateTime.Now.Year;
            userAge = currentYear - birthYear;

            Console.WriteLine($"Добавлен пользователь {userName} {userLastName}, Возвраст - {userAge}");
        } 
        static void helpCommand()
        {
            Console.WriteLine("Доступные команды:");
            Console.WriteLine("help — выводит список всех доступных команд с кратким описанием.");
            Console.WriteLine("profile - выводит данные пользователя в формате: <Имя> <Фамилия>, <Год рождения>.");
            Console.WriteLine("add — добавляет новую задачу. Формат ввода: add *Задача*");
            Console.WriteLine("view — выводит все задачи из массива (только непустые элементы).");
            Console.WriteLine("exit — завершает цикл и останавливает выполнение программы.");
        }
        static void profileCommand()
        {
            Console.WriteLine($" Пользователь:{userName} {userLastName}, Год рождения: {userAge}");
        }
        static void addCommand(string UserCommand)
        {
            string[] parts = userCommand.Split(' ');

            if (parts.Length >= 2)
            {
                string task = parts[1];

                if (taskCount >= todos.Length)
                {
                    string[] newTodos = new string[todos.Length * 2];

                    for (int i = 0; i < todos.Length; i++)
                    {
                        newTodos[i] = todos[i];
                    }

                    todos = newTodos;
                }

                todos[taskCount] = task;
                taskCount++;
                Console.WriteLine($"Задача добавлена: {task}");
            }
            else
            {
                Console.WriteLine("Ошибка: используйте формат add *текст задачи*");
            }
        }
        static void viewCommand()
        {
            if (taskCount == 0)
            {
                Console.WriteLine("Задач нет");
            }
            else
            {
                Console.WriteLine("Ваши задачи: ");
                for (int i = 0; i < taskCount; i++)

                {
                    Console.WriteLine($"{i + 1}. {todos[i]}");
                }
            }
            

