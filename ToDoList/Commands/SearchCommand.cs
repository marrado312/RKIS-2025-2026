using System;

namespace TodoList
{
	class SearchCommand : ICommand
	{
		public TodoList TodoList { get; set; }
		public string ContainsText { get; set; }
		public string StartsWithText { get; set; }
		public string EndsWithText { get; set; }
		public DateTime? FromDate { get; set; }
		public DateTime? ToDate { get; set; }
		public string Status { get; set; }
		public string SortBy { get; set; }
		public bool Descending { get; set; }
		public int? TopCount { get; set; }

		public void Execute()
		{
			if (TodoList == null || TodoList.Count == 0)
			{
				Console.WriteLine("Список задач пуст");
				return;
			}

			Console.WriteLine("Команда search с флагами будет реализована в следующем задании");
			Console.WriteLine("Поддерживаемые флаги: --contains, --starts-with, --ends-with, --from, --to, --status, --sort, --desc, --top");
		}

		public void Unexecute()
		{
		
		}
	}
}