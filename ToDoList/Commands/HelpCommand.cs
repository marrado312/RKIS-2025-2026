using System;

namespace TodoList
{
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