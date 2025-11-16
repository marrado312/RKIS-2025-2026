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

			var task = TodoList[TaskIndex];
			task.SetStatus(NewStatus);
			Console.WriteLine($"Статус задачи {TaskIndex + 1} изменен на: {task.GetStatusText()}");
		}
	}
}