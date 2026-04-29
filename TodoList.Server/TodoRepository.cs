using Microsoft.Data.Sqlite;

namespace TodoList.Server
{
	public class TodoRepository
	{
		private readonly Database _db;

		public TodoRepository(Database db) => _db = db;

		// создание таблиц
		public async Task InitializeAsync()
		{
			using var connection = _db.CreateConnection();
			await connection.OpenAsync();

			var command = connection.CreateCommand();
			command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Users (Id TEXT PRIMARY KEY, Data BLOB);
                CREATE TABLE IF NOT EXISTS Todos (UserId TEXT PRIMARY KEY, Data BLOB);";
			await command.ExecuteNonQueryAsync();

			Console.WriteLine("=== БАЗА ДАННЫХ ИНИЦИАЛИЗИРОВАНА ===");
		}
	}
}

