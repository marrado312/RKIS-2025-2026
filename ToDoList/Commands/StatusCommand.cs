using System;

namespace TodoList.Commands
{
	public class StatusCommand : ICommand
	{
		public TodoList TodoList { get; set; }
		public int TaskIndex { get; set; }
		public TodoStatus NewStatus { get; set; }

		public void Execute()
		{
			if (TaskIndex < 0 || TaskIndex >= TodoList.Count)
			{
				Console.WriteLine("Ошибка: неверный индекс задачи");
				return;
			}

			var task = TodoList.GetItem(TaskIndex);
			task.SetStatus(NewStatus);
			Console.WriteLine($"Статус задачи {TaskIndex + 1} изменен на: {GetStatusText(NewStatus)}");
		}

		private string GetStatusText(TodoStatus status)
		{
			return status switch
			{
				TodoStatus.NotStarted => "Не начато",
				TodoStatus.InProgress => "В процессе",
				TodoStatus.Completed => "Выполнено",
				TodoStatus.Postponed => "Отложено",
				TodoStatus.Failed => "Провалено",
			};
		}
	}
}