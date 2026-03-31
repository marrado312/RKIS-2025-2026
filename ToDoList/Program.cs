using System;
using System.IO;
using TodoList.Commands;
using TodoList.Exceptions;
using TodoList.Interfaces;
using ToDoList;


namespace TodoList
{
	class Program
	{
		static Profile userProfile;
		static IDataStorage storage;
		static string dataDir = Path.Combine(Directory.GetCurrentDirectory(), "data");
		static string profileFilePath = Path.Combine(dataDir, "profile.txt");
		static string todoFilePath = Path.Combine(dataDir, "todo.csv");





		static void Main(string[] args)
		{
			Console.WriteLine("Работу выполнили Фучаджи");

			string key = "12345678901234567890123456789012";
			string iv = "1234567890123456";
			storage = new FileManager(key, iv);


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
			SaveData();
			Console.WriteLine("Профиль успешно создан.");
		}

		static void SaveData()
		{
			if (userProfile != null)
				storage.SaveProfiles(new List<Profile> { userProfile });
			storage.SaveProfiles(new List<Profile> { userProfile });

			storage.SaveTodos(userProfile.Id, AppInfo.Todos);
		}

		static void LoadData()
		{
			if (AppInfo.Todos == null)
				AppInfo.Todos = new TodoList();

			var profiles = storage.LoadProfiles();

			var profilesnum = storage.LoadProfiles();
			Console.WriteLine($"Счетчик: Найдено профилей в файле: {profiles.Count()}");

			userProfile = profiles.FirstOrDefault();

			if (userProfile == null)
			{
				CreateUser();
			}
			else
			{
				AppInfo.CurrentProfile = userProfile;
				Console.WriteLine($"Загружен профиль: {userProfile.GetInfo()}");


				var loadedTodos = storage.LoadTodos(userProfile.Id);
				foreach (var todo in loadedTodos)
				{
					AppInfo.Todos.Add(todo);
					Console.WriteLine($"Загружено задач: {AppInfo.Todos.Count}");
				}
			}
		}
	}
}