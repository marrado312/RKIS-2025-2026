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
        static bool[] statuses = new bool[2];
        static DateTime[] dates = new DateTime[2];
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
                    addCommand(userCommand);
                }
                else if (userCommand == "view")
                {
                    viewCommand();
                }
                else if (userCommand.StartsWith("done "))
                {
                    doneCommand(userCommand);
                }
                else if (userCommand.StartsWith("delete "))
                {
                    deleteCommand(userCommand);
                }
                else if (userCommand.StartsWith("update "))
                {
                    updateCommand(userCommand);
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
                string[] parts = UserCommand.Split(' ');

                if (parts.Length >= 2)
                {
                    string task = parts[1];

                    if (taskCount >= todos.Length)
                    {
                        string[] newTodos = new string[todos.Length * 2];
                        bool[] newStatuses = new bool[statuses.Length * 2];
                        DateTime[] newDates = new DateTime[dates.Length * 2];

                        for (int i = 0; i < todos.Length; i++)
                        {
                            newTodos[i] = todos[i];
                            newStatuses[i] = statuses[i];
                            newDates[i] = dates[i];
                        }

                        todos = newTodos;
                        statuses = newStatuses;
                        dates = newDates;
                    }

                    todos[taskCount] = task;
                    statuses[taskCount - 1] = false;
                    dates[taskCount - 1] = DateTime.Now;
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
                        string statusText = statuses[i] ? "сделано" : "не сделано";
                        string dateText = dates[i].ToString("dd.MM.yyyy");
                        Console.WriteLine($"{i + 1} {todos[i]} {statusText} {dateText}");
                    }
                }
            }

            static void doneCommand(string userCommand)
            {
                string[] parts = userCommand.Split(' ');
                if (parts.Length >= 2 && int.TryParse(parts[1], out int index))
                {
                    index--;
                    if (index >= 0 && index < taskCount)
                    {
                        statuses[taskCount] = false;
                        dates[taskCount] = DateTime.Now;
                        Console.WriteLine($"Задача {index + 1} отмечена как выполненная");
                    }
                    else
                    {
                        Console.WriteLine("Ошибка: неверный индекс задачи");
                    }
                }
                else
                {
                    Console.WriteLine("Ошибка: используйте формат done <номер_задачи>");
                }
            }

            static void deleteCommand(string userCommand)
            {
                string[] parts = userCommand.Split(' ');
                if (parts.Length >= 2 && int.TryParse(parts[1], out int index))
                {
                    index--;
                    if (index >= 0 && index < taskCount)
                    {
                        for (int i = index; i < taskCount - 1; i++)
                        {
                            todos[i] = todos[i + 1];
                            statuses[i] = statuses[i + 1];
                            dates[i] = dates[i + 1];
                        }
                        taskCount--;
                        Console.WriteLine($"Задача {index + 1} удалена");
                    }
                    else
                    {
                        Console.WriteLine("Ошибка: неверный индекс задачи");
                    }
                }
                else
                {
                    Console.WriteLine("Ошибка: используйте формат delete *номер_задачи*");
                }
            }

            static void updateCommand(string userCommand)
            {
                int firstQuote = userCommand.IndexOf('"');
                int secondQuote = userCommand.LastIndexOf('"');
                if (firstQuote != -1 && secondQuote != -1 && firstQuote != secondQuote)
                {
                    string indexPart = userCommand.Substring(7, firstQuote - 7).Trim();
                    if (int.TryParse(indexPart, out int index))
                    {
                        index--;
                        if (index >= 0 && index < taskCount)
                        {
                            string newText = userCommand.Substring(firstQuote + 1, secondQuote - firstQuote - 1);
                            todos[index] = newText;
                            dates[index] = DateTime.Now;
                            Console.WriteLine($"Задача {index + 1} обновлена: {newText}");
                        }
                        else
                        {
                            Console.WriteLine("Ошибка: неверный индекс задачи");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Ошибка: используйте формат update <номер_задачи> *новый текст*");
                    }
                }
                else
                {
                    Console.WriteLine("Ошибка: используйте формат update <номер_задачи> *новый текст*");
                }
            }
        }
    }
}
