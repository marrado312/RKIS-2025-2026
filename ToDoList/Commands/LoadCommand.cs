using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList.Exceptions;

namespace TodoList.Commands
{
	public class LoadCommand : ICommand
	{
		private readonly int _downloadsCount;
		private readonly int _totalSize;

		public LoadCommand(string args)
		{
			string[] parts = args.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

			if (parts.Length < 2)
				throw new InvalidCommandException("Используйте: load <количество> <размер>");

			if (!int.TryParse(parts[0], out _downloadsCount) || !int.TryParse(parts[1], out _totalSize))
				throw new InvalidCommandException("Аргументы должны быть числами.");

			if (_downloadsCount <= 0 || _totalSize <= 0)
				throw new InvalidCommandException("Значения должны быть больше 0.");
		}

		public void Execute()
		{
			RunAsync().Wait();
		}

		private async Task RunAsync()
		{
			int startRow = Console.CursorTop;

			for (int i = 0; i < _downloadsCount; i++)
			{
				Console.WriteLine();
			}

			await Task.CompletedTask;
		}
	}
}