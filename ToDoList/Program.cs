using System;
using System.IO;
using TodoList.Commands;
using ToDoList;
using TodoList.Exceptions;


namespace TodoList
{
	class Program
	{
		static Profile userProfile;
		static string dataDir = Path.Combine(Directory.GetCurrentDirectory(), "data");
		static string profileFilePath = Path.Combine(dataDir, "profile.txt");
		static string todoFilePath = Path.Combine(dataDir, "todo.csv");
		static  FileManager fileManager = new FileManager();



		static void Main(string[] args)
		{
			Console.WriteLine("Работу выполнили Фучаджи и Клюев");

			fileManager.EnsureDataDirectory(dataDir);
			LoadData();
			Console.WriteLine("Введите help для вывода доступных команд");

			while (true)
			{
				try
				{
					Console.Write("> ");
					string input = Console.ReadLine();

					if (string.IsNullOrWhiteSpace(input)) continue;

					var command = CommandParser.Parse(input);

					if (command != null)
					{
						command.Execute();

						if (command is IUndo)
						{
							AppInfo.undoStack.Push(command);
						}

						SaveData();
					}
				}
				catch (InvalidCommandException ex)
				{
					Console.WriteLine("Ошибка команды: " + ex.Message);
				}
				catch (TaskNotFoundException ex)
				{
					Console.WriteLine("Ошибка данных: " + ex.Message);
				}
				catch (AuthenticationException ex)
				{
					Console.WriteLine("Ошибка доступа: " + ex.Message);
				}
				catch (InvalidArgumentException ex)
				{
					Console.WriteLine("Ошибка аргумента: " + ex.Message);
				}
				catch (Exception ex)
				{
					Console.WriteLine("Критическая ошибка: " + ex.Message);
				}
				
			}
		}

		static void CreateUser()
		{
			if (userProfile != null)
			{
				throw new DuplicateLoginException("Профиль уже существует.");
			}

			Console.Write("Введите имя: ");
			string firstName = Console.ReadLine();
			if (string.IsNullOrWhiteSpace(firstName))
			{
				throw new InvalidArgumentException("Имя не может быть пустым.");
			}

			Console.Write("Введите фамилию: ");
			string lastName = Console.ReadLine();

			Console.Write("Введите год рождения: ");
			string yearInput = Console.ReadLine();
			if (!int.TryParse(yearInput, out int birthYear) || birthYear < 1900 || birthYear > DateTime.Now.Year)
			{
				throw new InvalidArgumentException("Некорректный год рождения. Введите число.");
			}

			userProfile = new Profile(firstName, lastName, birthYear);
			AppInfo.CurrentProfile = userProfile;
			Console.WriteLine("Профиль успешно создан.");
		}

		static void SaveData()
		{
			if (userProfile != null)
				fileManager.SaveProfile(userProfile, profileFilePath);

			fileManager.SaveTodos(AppInfo.Todos, todoFilePath);
		}

		static void LoadData()
		{
			if (AppInfo.Todos == null)
				AppInfo.Todos = new TodoList();

			fileManager.EnsureDataDirectory(dataDir);

			if (!File.Exists(profileFilePath))
				File.WriteAllText(profileFilePath, "Default User");
			if (!File.Exists(todoFilePath))
				File.WriteAllText(todoFilePath, "");

			userProfile = fileManager.LoadProfile(profileFilePath);
			AppInfo.CurrentProfile = userProfile;

			if (userProfile == null)
			{
				CreateUser();
			}
			else
			{
				Console.WriteLine($"Загружен профиль: {userProfile.GetInfo()}");
			}

			var loadedTodos = fileManager.LoadTodos(todoFilePath);
			if (loadedTodos != null && loadedTodos.Count > 0)
			{
				AppInfo.Todos = loadedTodos;
				Console.WriteLine($"Загружено задач: {AppInfo.Todos.Count}");
			}
		}
	}
}