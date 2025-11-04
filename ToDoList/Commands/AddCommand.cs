using System;

namespace TodoList
{
    class AddCommand : ICommand
    {
        public TodoList TodoList { get; set; }
        public bool IsMultiline { get; set; }
        public bool IsUrgent { get; set; }
        public string TaskText { get; set; }

        public void Execute()
        {
            string task = TaskText;
            if (IsMultiline)
            {
                Console.WriteLine("Введите задачу (для завершения введите пустую строку):");
                task = "";
                string line;
                while (!string.IsNullOrWhiteSpace(line = Console.ReadLine()))
                {
                    task += line + Environment.NewLine;
                }
                task = task.TrimEnd();
            }

            if (IsUrgent)
            {
                task = "[Срочно] " + task;
            }
            TodoItem newItem = new TodoItem(task);
            TodoList.Add(newItem);
            Console.WriteLine($"Задача добавлена: {task}");
        }
    }
}