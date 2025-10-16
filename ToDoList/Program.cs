using System;

namespace TodoList
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Работу выполнил Фучаджи и Клюев");

            Console.Write("Введите ваше имя: ");
            string name = Console.ReadLine();

            Console.Write("Введите вашу Фамилию: ");
            string lastName = Console.ReadLine();

            Console.Write("Введите вашу дату рождения: ");
            string birthday = Console.ReadLine();

            int birthday = int.Parse(Birthday);

            int nowYear = DateTime.Now.Year;

            int age = nowYear - Birthday;

            Console.WriteLine($"добавлен пользователь {name} {lastName}, Возвраст - {age}");
        }
    }

}