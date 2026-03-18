using System;

namespace TodoList.Commands
{
	public class DeleteCommand : ICommand, IUndo
	{
		public TodoList TodoList { get; set; }
		public int TaskIndex { get; set; }
		private TodoItem deletedItem;
		private int deletedIndex;

		public void Execute()
		{
			deletedIndex = TaskIndex;
			deletedItem = TodoList[TaskIndex];
			TodoList.Delete(TaskIndex);
			Console.WriteLine("Задача удалена.");
		}

		public void Unexecute()
		{
			if (deletedItem != null)
			{
				TodoList.Insert(deletedIndex, deletedItem);
				Console.WriteLine($"Отмена: восстановлена задача '{deletedItem.Text}'");
			}
		}
	}
}