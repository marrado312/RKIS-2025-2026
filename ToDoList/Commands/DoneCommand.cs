using System;

namespace TodoList
{
    class DoneCommand : ICommand
    {
        public TodoList TodoList { get; set; }
        public int TaskIndex { get; set; }

        public void Execute()
        {
            try
            {
                TodoList.GetItem(TaskIndex).MarkDone();
                Console.WriteLine($"Задача {TaskIndex + 1} отмечена как выполненная");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }
}