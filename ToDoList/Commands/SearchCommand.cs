using System;
using System.Collections.Generic;
using System.Linq;

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

			IEnumerable<TodoItem> query = TodoList;

			if (!string.IsNullOrEmpty(ContainsText))
			{
				query = query.Where(t => t.Text.Contains(ContainsText, StringComparison.OrdinalIgnoreCase));
			}

			if (!string.IsNullOrEmpty(StartsWithText))
			{
				query = query.Where(t => t.Text.StartsWith(StartsWithText, StringComparison.OrdinalIgnoreCase));
			}

			if (!string.IsNullOrEmpty(EndsWithText))
			{
				query = query.Where(t => t.Text.EndsWith(EndsWithText, StringComparison.OrdinalIgnoreCase));
			}

			if (FromDate.HasValue)
			{
				query = query.Where(t => t.LastUpdate.Date >= FromDate.Value.Date);
			}

			if (ToDate.HasValue)
			{
				query = query.Where(t => t.LastUpdate.Date <= ToDate.Value.Date);
			}

			if (!string.IsNullOrEmpty(Status))
			{
				if (Enum.TryParse<TodoStatus>(Status, true, out var statusValue))
				{
					query = query.Where(t => t.Status == statusValue);
				}
				else
				{
					Console.WriteLine($"Ошибка: неверный статус '{Status}'");
					Console.WriteLine("Допустимые статусы: not_started, in_progress, completed, postponed, failed");
					return;
				}
			}

			if (!string.IsNullOrEmpty(SortBy))
			{
				if (SortBy.ToLower() == "text")
				{
					if (Descending)
						query = query.OrderByDescending(t => t.Text).ThenByDescending(t => t.LastUpdate);
					else
						query = query.OrderBy(t => t.Text).ThenBy(t => t.LastUpdate);
				}
				else if (SortBy.ToLower() == "date")
				{
					if (Descending)
						query = query.OrderByDescending(t => t.LastUpdate).ThenByDescending(t => t.Text);
					else
						query = query.OrderBy(t => t.LastUpdate).ThenBy(t => t.Text);
				}
				else
				{
					Console.WriteLine($"Ошибка: неверный параметр сортировки '{SortBy}'");
					Console.WriteLine("Допустимые значения: text, date");
					return;
				}
			}

			if (TopCount.HasValue)
			{
				if (TopCount.Value <= 0)
				{
					Console.WriteLine("Ошибка: параметр --top должен быть больше 0");
					return;
				}
				query = query.Take(TopCount.Value);
			}

			var results = query.ToList();

			if (results.Count == 0)
			{
				Console.WriteLine("Ничего не найдено");
				return;
			}

			Console.WriteLine($"Найдено задач: {results.Count}");

			Console.WriteLine("| Index | Text                             | Status       | LastUpdate     |");
			Console.WriteLine("|-------|----------------------------------|--------------|----------------|");

			for (int i = 0; i < results.Count; i++)
			{
				string shortText = results[i].Text;
				if (shortText.Length > 30)
					shortText = shortText.Substring(0, 27) + "...";

				shortText = shortText.Replace("\n", " ").Replace("\r", "");

				string status = results[i].GetStatusText();
				string date = results[i].LastUpdate.ToString("dd.MM.yyyy HH:mm");

				Console.WriteLine($"| {i + 1,5} | {shortText,-32} | {status,-12} | {date,-14} |");
			}
		}

		public void Unexecute()
		{
		}
	}
}