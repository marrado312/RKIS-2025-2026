using System.Data;

namespace TodoList
{
    class Program
    {
        static string UserName;
        static string UserLastName;
        static int UserAge;
        static string[] todos = new string[2];
        static int taskCount = 0;
        static bool[] statuses = new bool[2];
        static DateTime[] dates = new DateTime[2];
        static void Main(string[] args)
        {
            Console.WriteLine("Работу выполнили Фучаджи и Клюев");

            CreateUser();

            Console.WriteLine("Введите help для вывода доступных команд");


            while (true)
            {

                Console.Write("Введите команду:");
                string CommandUser = Console.ReadLine();

                if (CommandUser == null || CommandUser.ToLower() == "exit") break;

                if (CommandUser == "help")
                {
                    CommandHelp();
                }
                else if (CommandUser == "profile")
                {
                    CommandProfile();
                }
                else if (CommandUser == "add")
                {
                    CommandAdd(CommandUser);
                }
                else if (CommandUser == "view")
                {
                    CommandView();
                }
                else if (CommandUser.StartsWith("done "))
                {
                    CommandDone(CommandUser);
                }
                else if (CommandUser.StartsWith("delete "))
                {
                    CommandDelete(CommandUser);
                }
                else if (CommandUser.StartsWith("update "))
                {
                    CommandUpdate(CommandUser);
                }
                else if (CommandUser.StartsWith("view"))
                {
                    CommandView();
                }
            }
        }
        static void CreateUser()
        {
            Console.Write("Введите ваше имя: ");
            UserName = Console.ReadLine();
            Console.Write("Введите вашу Фамилию: ");
            UserLastName = Console.ReadLine();

            Console.Write("Введите вашу дату рождения: ");
            string BirthYearInput = Console.ReadLine();
            int BirthYear = int.Parse(BirthYearInput);
            if (BirthYear <= 0)
            {
                Console.WriteLine("Введите реальный возраст");
                return;
            }
            int CurrentYear = DateTime.Now.Year;
            UserAge = CurrentYear - BirthYear;

            Console.WriteLine($"Добавлен пользователь {UserName} {UserLastName}, Возвраст - {UserAge}");
        }
        static void CommandHelp()
        {
            Console.WriteLine("Доступные команды:");
            Console.WriteLine("help — выводит список всех доступных команд с кратким описанием.");
            Console.WriteLine("profile - выводит данные пользователя в формате: <Имя> <Фамилия>, <Год рождения>.");
            Console.WriteLine("add — добавляет новую задачу. Формат ввода: add *Задача*");
            Console.WriteLine("view — выводит все задачи из массива (только непустые элементы).");
            Console.WriteLine("done — отмечает задачу выполненной.");
            Console.WriteLine("delete — удаляет задачу по индексу");
            Console.WriteLine("update — обновляет текст задачи");
            Console.WriteLine("exit — завершает цикл и останавливает выполнение программы.");

        }
        static void CommandProfile()
        {
            Console.WriteLine($" Пользователь:{UserName} {UserLastName}, Год рождения: {UserAge}");
        }
        static void CommandAdd(string CommandUser)
        {
            bool multiline = CommandUser.Contains("--multiline") || CommandUser.Contains("-m");
            bool urgent = CommandUser.Contains("--urgent") || CommandUser.Contains("-u");

            string task = "";
            if (multiline)
            {
                Console.WriteLine("Введите задачу (для завершения введите пустую строку):");
                string line;
                while (!string.IsNullOrWhiteSpace(line = Console.ReadLine()))
                {
                    task += line + Environment.NewLine;
                }
                task = task.TrimEnd();
            }
            else
            {
                string[] parts = CommandUser.Split('"');
                if (parts.Length >= 2)
                {
                    task = parts[1];
                }
                else
                {
                    Console.WriteLine("Ошибка: используйте формат add *текст задачи* или add --multiline");
                    return;
                }
            }
            if (taskCount >= todos.Length)
            {
                ResizeArrays();
            }
            todos[taskCount] = task;
            statuses[taskCount] = true;
            dates[taskCount] = DateTime.Now;
            if (urgent)
            {
                todos[taskCount] = "[Срочно] " + todos[taskCount];
            }
            taskCount++;
            Console.WriteLine($"Задача добавлена: {task}");
        }
    

<<<<<<< HEAD
            Console.WriteLine($"добавлен пользователь {name} {lastName}, Возвраст - {age}");
=======








            static void CommandView()
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

        static void CommandDone(string CommandUser)
        {
            string[] parts = CommandUser.Split(' ');
            if (parts.Length >= 2 && int.TryParse(parts[1], out int index))
            {
                index--;
                if (index >= 0 && index < taskCount)
                {
                    statuses[index] = true;
                    dates[index] = DateTime.Now;
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

        static void CommandDelete(string CommandUser)
        {
            string[] parts = CommandUser.Split(' ');
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

        static void CommandUpdate(string CommandUser)
        {
            int firstQuote = CommandUser.IndexOf('"');
            int secondQuote = CommandUser.LastIndexOf('"');
            if (firstQuote != -1 && secondQuote != -1 && firstQuote != secondQuote)
            {
                string indexPart = CommandUser.Substring(7, firstQuote - 7).Trim();
                if (int.TryParse(indexPart, out int index))
                {
                    index--;
                    if (index >= 0 && index < taskCount)
                    {
                        string newText = CommandUser.Substring(firstQuote + 1, secondQuote - firstQuote - 1);
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


        static void ResizeArrays()
        {
            string[] newTodos = new string[todos.Length * 2];
            bool[] newStatuses = new bool[statuses.Length * 2];
            DateTime[] newDates = new DateTime[dates.Length * 2];

            for (int i = 0; i < taskCount; i++)
            {
                newTodos[i] = todos[i];
                newStatuses[i] = statuses[i];
                newDates[i] = dates[i];
            }

            todos = newTodos;
            statuses = newStatuses;
            dates = newDates;
>>>>>>> 79c146589d33963c62f57b38073602ae94a21f6a
        }
    }
 }