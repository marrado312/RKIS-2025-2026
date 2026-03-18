using System;
using System.Collections.Generic;
using TodoList.Commands;
using TodoList.Exceptions;

namespace TodoList
{
	public static class CommandParser
	{
		private static Dictionary<string, Func<string, ICommand>> commandHandlers =
			new Dictionary<string, Func<string, ICommand>>(StringComparer.OrdinalIgnoreCase);

		static CommandParser()
		{
			RegisterCommand("help", input => new HelpCommand());
			RegisterCommand("undo", input => new UndoCommand());
			RegisterCommand("redo", input => new RedoCommand());
			RegisterCommand("profile", input => new ProfileCommand { Profile = AppInfo.CurrentProfile });
			RegisterCommand("view", ParseViewCommand);
			RegisterCommand("add", ParseAddCommand);
			RegisterCommand("delete", ParseDeleteCommand);
			RegisterCommand("update", ParseUpdateCommand);
			RegisterCommand("read", ParseReadCommand);
			RegisterCommand("status", ParseStatusCommand);
			RegisterCommand("search", ParseSearchCommand);
		}

		public static void RegisterCommand(string commandName, Func<string, ICommand> handler)
		{
			commandHandlers[commandName.ToLower()] = handler;
		}

		public static ICommand Parse(string inputString)
		{
			if (string.IsNullOrEmpty(inputString))
				return null;

			string[] parts = inputString.Split(' ', 2);
			string commandName = parts[0].ToLower();
			string arguments = parts.Length > 1 ? parts[1] : "";

			if (commandHandlers.TryGetValue(commandName, out var handler))
			{
				try
				{
					return handler(arguments);
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Ошибка выполнения команды: {ex.Message}");
					return null;
				}
			}
			else
			{
				Console.WriteLine($"Неизвестная команда: {commandName}");
				return null;
			}
		}

		private static ICommand ParseViewCommand(string args)
		{
			var command = new ViewCommand { TodoList = AppInfo.Todos };

			command.ShowIndex = args.Contains("--index") || args.Contains("-i");
			command.ShowStatus = args.Contains("--status") || args.Contains("-s");
			command.ShowDate = args.Contains("--update-date") || args.Contains("-d");

			if (args.Contains("--all") || args.Contains("-a"))
			{
				command.ShowIndex = true;
				command.ShowStatus = true;
				command.ShowDate = true;
			}

			return command;
		}

		private static ICommand ParseAddCommand(string args)
		{
			var command = new AddCommand();

			command.IsMultiline = args.Contains("--multiline") || args.Contains("-m");
			command.IsUrgent = args.Contains("--urgent") || args.Contains("-u");

			if (!command.IsMultiline)
			{
				string[] parts = args.Split('"');
				if (parts.Length >= 2)
				{
					command.TaskText = parts[1];
				}
			}

			if (!command.IsMultiline && string.IsNullOrEmpty(command.TaskText))
			{
				string taskText = args.Trim();
				if (!string.IsNullOrEmpty(taskText))
				{
					command.TaskText = taskText.Trim('"');
				}
			}

			return command;
		}

		private static ICommand ParseDeleteCommand(string args)
		{
			var command = new DeleteCommand { TodoList = AppInfo.Todos };

			if (ParseTaskIndex(args, out int index))
				command.TaskIndex = index;

			return command;
		}

		private static ICommand ParseUpdateCommand(string args)
		{
			string[] parts = args.Split(' ', 3);
			if (parts.Length >= 3 && int.TryParse(parts[0], out int index))
			{
				return new UpdateCommand
				{
					TodoList = AppInfo.Todos,
					TaskIndex = index - 1,
					NewText = parts[2].Trim('"')
				};
			}
			return null;
		}

		private static ICommand ParseReadCommand(string args)
		{
			if (string.IsNullOrWhiteSpace(args))
			{
				throw new InvalidCommandException("Номер задачи не может быть пустым.");
			}

			if (int.TryParse(args.Trim(), out int index))
			{
				if (index < 1 || index > AppInfo.Todos.Count)
				{
					throw new TaskNotFoundException("Ошибка: Задача №" + index + " не найдена.");
				}

				return new ReadCommand { TodoList = AppInfo.Todos, TaskIndex = index - 1 };
			}

			throw new InvalidCommandException("Для чтения задачи введите её номер числом.");
		}

		private static ICommand ParseStatusCommand(string args)
		{
			string[] parts = args.Split(' ');
			if (parts.Length >= 2 && int.TryParse(parts[0], out int index))
			{
				string statusStr = parts[1].ToLower();
				TodoStatus newStatus = statusStr switch
				{
					"not_started" => TodoStatus.NotStarted,
					"in_progress" => TodoStatus.InProgress,
					"completed" => TodoStatus.Completed,
					"postponed" => TodoStatus.Postponed,
					"failed" => TodoStatus.Failed,
					_ => throw new ArgumentException($"Неверный статус: {statusStr}")
				};

				return new StatusCommand
				{
					TodoList = AppInfo.Todos,
					TaskIndex = index - 1,
					NewStatus = newStatus
				};
			}
			return null;
		}

		private static ICommand ParseSearchCommand(string args)
		{
			var command = new SearchCommand { TodoList = AppInfo.Todos };
			var parts = args.Split(' ', StringSplitOptions.RemoveEmptyEntries);

			for (int i = 0; i < parts.Length; i++)
			{
				string arg = parts[i];

				if (arg == "--contains" && i + 1 < parts.Length)
				{
					command.ContainsText = parts[++i].Trim('"');
				}
				else if (arg == "--starts-with" && i + 1 < parts.Length)
				{
					command.StartsWithText = parts[++i].Trim('"');
				}
				else if (arg == "--ends-with" && i + 1 < parts.Length)
				{
					command.EndsWithText = parts[++i].Trim('"');
				}
				else if (arg == "--status" && i + 1 < parts.Length)
				{
					command.Status = parts[++i];
				}
				else if (arg == "--from")
				{
					if (i + 1 < parts.Length && DateTime.TryParse(parts[++i], out var date))
					{
						command.FromDate = date;
					}
					else
					{
						throw new InvalidCommandException("Неверный формат даты --from. Используйте ГГГГ-ММ-ДД");
					}
				}
				else if (arg == "--to")
				{
					if (i + 1 < parts.Length && DateTime.TryParse(parts[++i], out var date))
					{
						command.ToDate = date;
					}
					else
					{
						throw new InvalidCommandException("Неверный формат даты --to. Используйте ГГГГ-ММ-ДД");
					}
				}
				else if (arg == "--top")
				{
					if (i + 1 < parts.Length && int.TryParse(parts[++i], out int top))
					{
						command.TopCount = top;
					}
					else
					{
						throw new InvalidCommandException("Параметр --top должен быть числом.");
					}
				}
				else if (arg == "--desc")
				{
					command.Descending = true;
				}
			}

			return command;
		}

		private static bool ParseTaskIndex(string args, out int index)
		{
			index = -1;
			if (int.TryParse(args.Trim(), out int parsedIndex))
			{
				index = parsedIndex - 1;
				return true;
			}
			return false;
		}
	}
}