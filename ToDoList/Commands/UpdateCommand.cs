using System;

namespace TodoList
{
    class UpdateCommand : ICommand
    {
        public TodoList TodoList { get; set; }
        public int TaskIndex { get; set; }
        public string NewText { get; set; }

        public void Execute()
        {
            try
            {
                TodoList.GetItem(TaskIndex).UpdateText(NewText);
                Console.WriteLine($"Задача {TaskIndex + 1} обновлена: {NewText}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }
}