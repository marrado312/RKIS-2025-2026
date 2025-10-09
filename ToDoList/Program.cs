using System.Data;

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

            int number = int.Parse(birthday);

            int nowYear = DateTime.Now.Year;

            int age = nowYear - number;

            Console.WriteLine($"Добавлен пользователь {name} {lastName}, Возвраст - {age}");



            sting[] todos = new string[2];

            Console.WriteLine("Введите *help* для вывода доступных команд");


