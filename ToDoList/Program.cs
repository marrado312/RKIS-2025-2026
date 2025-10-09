using System.Data;

namespace TodoList
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Работу выполнили Фучаджи и Клюев");

            Console.Write("Введите ваше имя: ");
            string name = Console.ReadLine();

            Console.Write("Введите вашу Фамилию: ");
            string lastName = Console.ReadLine();

            Console.Write("Введите вашу дату рождения: ");
            string birthday = Console.ReadLine();

            int number = int.Parse(birthday);

            int nowYear = DateTime.Now.Year;

            int age = nowYear - number;

            Console.WriteLine($"Добавлен пользователь {name} {lastName}, Возвраст - {age}");



            sting[] todos = new string[2];

            Console.WriteLine("Введите *help* для вывода доступных команд");


            while (true)
            {
                Console.WriteLine("Введите команду:")
                 string input = Console.ReadLine();

                if (command = null || command = "exit") break;
            }

                switch (command);
                {
                    case (help)
                        Console.WriteLine("Доступные команды:");
                        Console.WriteLine("help — выводит список всех доступных команд с кратким описанием.");
                        Console.WriteLine("profile - выводит данные пользователя в формате: <Имя> <Фамилия>, <Год рождения>.");
                        Console.WriteLine("add — добавляет новую задачу. Формат ввода: add "текст задачи".");
                        Console.WriteLine("view — выводит все задачи из массива (только непустые элементы).");
                        Console.WriteLine("exit — завершает цикл и останавливает выполнение программы.");
                    break;



            