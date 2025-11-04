using System;

namespace TodoList
{
    class DeleteCommand : ICommand
    {
        public TodoList TodoList { get; set; }
        public int TaskIndex { get; set; }

        public void Execute()
        {
            try
            {
                TodoList.Delete(TaskIndex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }
}