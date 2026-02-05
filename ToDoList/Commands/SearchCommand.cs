using System;

namespace TodoList
{
	class SearchCommand : ICommand
	{
		public TodoList TodoList { get; set; }

		public void Execute()
		{
			if (TodoList == null || TodoList.Count == 0)
			{
				Console.WriteLine("Список задач пуст");
				return;
			}

			Console.WriteLine("Команда search добавлена.");
		}

		public void Unexecute()
		{
		}
	}
}