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
    class ViewCommand : ICommand
    {
        public TodoList TodoList { get; set; }
        public bool ShowIndex { get; set; }
        public bool ShowStatus { get; set; }
        public bool ShowDate { get; set; }
        public void Execute()
        {
            TodoList.View(ShowIndex, ShowStatus, ShowDate);
        }
    }

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

    class ReadCommand : ICommand
    {
        public TodoList TodoList { get; set; }
        public int TaskIndex { get; set; }
        public void Execute()
        {
            try
            {
                TodoList.Read(TaskIndex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }
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

    class HelpCommand : ICommand
    {
        public void Execute()
        {
            Console.WriteLine(@"Доступные команды:
        help - справка по командам
        profile - данные пользователя
        add - добавить задачу (--multiline/-m, --urgent/-u)
        view - список задач (--index/-i, --status/-s, --update-date/-d, --all/-a)
        read - полный текст задачи
        done - отметить выполненной
        delete - удалить задачу
        update - изменить текст задачи
        exit - выход из программы");
        }
    }
}