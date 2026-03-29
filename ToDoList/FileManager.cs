using System;
using System.Collections.Generic;
using System.IO;
using ToDoList;

namespace TodoList
{
	public class FileManager
	{
		private readonly IFileSystem _fileSystem;

		public FileManager(IFileSystem fileSystem = null)
		{
			_fileSystem = fileSystem ?? new SystemFileSystem();
		}

		public void EnsureDataDirectory(string dirPath)
		{
			if (!_fileSystem.DirectoryExists(dirPath))
			{
				_fileSystem.CreateDirectory(dirPath);
			}
		}

		public void SaveProfile(Profile profile, string filePath)
		{
			string data = $"{profile.FirstName};{profile.LastName};{profile.BirthYear}";
			_fileSystem.WriteAllText(filePath, data);
		}

		public Profile LoadProfile(string filePath)
		{
			if (!_fileSystem.Exists(filePath)) return null;
			string content = _fileSystem.ReadAllText(filePath);
			if (string.IsNullOrEmpty(content)) return null;
			string[] parts = content.Split(';');
			if (parts.Length < 3) return null;
			return new Profile(parts[0], parts[1], int.Parse(parts[2]));
		}

		public void SaveTodos(TodoList todos, string filePath)
		{
			string[] lines = new string[todos.Count];
			for (int i = 0; i < todos.Count; i++)
			{
				var todo = todos[i];
				string text = EscapeCsv(todo.Text);
				lines[i] = $"{i};{text};{todo.Status};{todo.LastUpdate:yyyy-MM-ddTHH:mm:ss}";
			}
			_fileSystem.WriteAllLines(filePath, lines);
		}

		public TodoList LoadTodos(string filePath)
		{
			var todoList = new TodoList();
			if (!_fileSystem.Exists(filePath)) return todoList;
			string[] lines = _fileSystem.ReadAllLines(filePath);
			foreach (var line in lines)
			{
				string[] parts = line.Split(';');
				if (parts.Length < 4) continue;
				string text = UnescapeCsv(parts[1]);
				TodoStatus status = (TodoStatus)Enum.Parse(typeof(TodoStatus), parts[2]);
				var todo = new TodoItem(text);
				todo.SetStatus(status);
				todoList.AddTodoFromFile(todo);
			}
			return todoList;
		}

		private string EscapeCsv(string text)
		{
			if (text == null) return "\"\"";
			return "\"" + text.Replace("\"", "\"\"").Replace("\n", "\\n") + "\"";
		}

		private string UnescapeCsv(string text)
		{
			if (text == null) return "";
			return text.Trim('"').Replace("\\n", "\n").Replace("\"\"", "\"");
		}
	}
}