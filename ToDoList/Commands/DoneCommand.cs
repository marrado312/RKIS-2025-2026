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
				TodoList.GetItem(TaskIndex).SetStatus(TodoStatus.Completed);
				Console.WriteLine($"Задача {TaskIndex + 1} отмечена как выполненная (статус: Выполнено)");
			}
			catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }
}