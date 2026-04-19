using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TodoList.Server
{
	class Program
	{
		private const string Url = "http://localhost:5000/";

		static async Task Main(string[] args)
		{
			using var listener = new HttpListener();
			listener.Prefixes.Add(Url);
			listener.Start();

			Console.WriteLine("=== СЕРВЕР ЗАПУЩЕН ===");
			Console.WriteLine($"Ожидание на {Url}");

			while (true)
			{
				var context = await listener.GetContextAsync();
				_ = Task.Run(() => HandleRequest(context));
			}
		}

		static void HandleRequest(HttpListenerContext context)
		{
			var request = context.Request;
			var response = context.Response;
			string path = request.Url.AbsolutePath.Trim('/');

			try
			{
				string fileName = path == "profiles"
					? "server_profiles.dat"
					: $"server_todos_{path.Replace("todos/", "")}.dat";

				if (request.HttpMethod == "POST")
				{
					using (var fileStream = File.Create(fileName))
					{
						request.InputStream.CopyTo(fileStream);
					}
					response.StatusCode = (int)HttpStatusCode.OK;
				}
				else if (request.HttpMethod == "GET")
				{
					if (File.Exists(fileName))
					{
						byte[] buffer = File.ReadAllBytes(fileName);
						response.ContentLength64 = buffer.Length;
						response.OutputStream.Write(buffer, 0, buffer.Length);
					}
					else
					{
						response.StatusCode = (int)HttpStatusCode.NotFound;
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Ошибка: {ex.Message}");
				response.StatusCode = (int)HttpStatusCode.InternalServerError;
			}
			finally
			{
				response.Close();
			}
		}
	}
}