using System;
using TodoList.Commands;
using ToDoList;

namespace TodoList
{
	static class CommandParser
	{
		public static ICommand Parse(string inputString, TodoList todoList, Profile profile)
		{
			if (string.IsNullOrEmpty(inputString))
				return null;

			string command = inputString.Trim().ToLower();

			if (command == "help")
				return new HelpCommand();

			if (command == "profile")
				return new ProfileCommand { Profile = profile };

			if (command == "view")
				return new ViewCommand { TodoList = todoList };

			if (command.StartsWith("view "))
			{
				var viewCommand = new ViewCommand { TodoList = todoList };
				ParseViewFlags(inputString, viewCommand);
				return viewCommand;
			}

			if (command.StartsWith("add"))
			{
				var addCommand = new AddCommand { TodoList = todoList };
				ParseAddCommand(inputString, addCommand);
				return addCommand;
			}

			if (command.StartsWith("delete "))
			{
				var deleteCommand = new DeleteCommand { TodoList = todoList };
				if (ParseTaskIndex(inputString, out int index))
					deleteCommand.TaskIndex = index;
				return deleteCommand;
			}

			if (command.StartsWith("update "))
			{
				var updateCommand = new UpdateCommand { TodoList = todoList };
				if (ParseUpdateCommand(inputString, updateCommand))
					return updateCommand;
			}

			if (command.StartsWith("read "))
			{
				var readCommand = new ReadCommand { TodoList = todoList };
				if (ParseTaskIndex(inputString, out int index))
					readCommand.TaskIndex = index;
				return readCommand;
			}

			if (command.StartsWith("status "))
			{
				var statusCommand = new StatusCommand { TodoList = todoList };
				if (ParseStatusCommand(inputString, statusCommand))
					return statusCommand;
			}
			Console.WriteLine("Неизвестная команда. Введите help для справки.");
			return null;
		}

		private static void ParseViewFlags(string input, ViewCommand command)
		{
			command.ShowIndex = input.Contains("--index") || input.Contains("-i");
			command.ShowStatus = input.Contains("--status") || input.Contains("-s");
			command.ShowDate = input.Contains("--update-date") || input.Contains("-d");

			if (input.Contains("--all") || input.Contains("-a"))
			{
				command.ShowIndex = true;
				command.ShowStatus = true;
				command.ShowDate = true;
			}
		}

		private static void ParseAddCommand(string input, AddCommand command)
		{
			command.IsMultiline = input.Contains("--multiline") || input.Contains("-m");
			command.IsUrgent = input.Contains("--urgent") || input.Contains("-u");

			if (!command.IsMultiline)
			{
				string[] parts = input.Split('"');
				if (parts.Length >= 2)
				{
					command.TaskText = parts[1];
				}
			}

			if (!command.IsMultiline && string.IsNullOrEmpty(command.TaskText))
			{
				string taskText = input.Substring(3).Trim();
				if (!string.IsNullOrEmpty(taskText))
				{
					command.TaskText = taskText.Trim('"');
				}
			}
		}

		private static bool ParseTaskIndex(string input, out int index)
		{
			index = -1;
			string[] parts = input.Split(' ');
			if (parts.Length >= 2 && int.TryParse(parts[1], out int parsedIndex))
			{
				index = parsedIndex - 1;
				return true;
			}
			return false;
		}

		private static bool ParseUpdateCommand(string input, UpdateCommand command)
		{
			string[] parts = input.Split(' ', 3);
			if (parts.Length >= 3 && int.TryParse(parts[1], out int index))
			{
				command.TaskIndex = index - 1;
				command.NewText = parts[2].Trim('"');
				return true;
			}
			return false;
		}

		private static bool ParseStatusCommand(string input, StatusCommand command)
		{
			string[] parts = input.Split(' ');
			if (parts.Length >= 3 && int.TryParse(parts[1], out int index))
			{
				command.TaskIndex = index - 1;

				string statusStr = parts[2].ToLower();
				command.NewStatus = statusStr switch
				{
					"notstarted" => TodoStatus.NotStarted,
					"inprogress" => TodoStatus.InProgress,
					"completed" => TodoStatus.Completed,
					"postponed" => TodoStatus.Postponed,
					"failed" => TodoStatus.Failed,
				};
				return true;
			}
			return false;
		}
	}
}