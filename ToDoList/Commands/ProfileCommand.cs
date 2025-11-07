using System;
using ToDoList.Commands;

namespace TodoList
{
    class ProfileCommand : ICommand
    {
        public Profile Profile { get; set; }

        public void Execute()
        {
            if (Profile != null)
            {
                Console.WriteLine($"Пользователь: {Profile.GetInfo()}");
            }
            else
            {
                Console.WriteLine("Профиль не создан");
            }
        }
    }
}