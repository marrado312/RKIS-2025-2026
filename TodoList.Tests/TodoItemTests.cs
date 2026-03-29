using System;
using Xunit;
using TodoList;

namespace TodoList.Tests
{
	public class TodoItemTests
	{
		[Fact]
		public void SetStatus_WhenCalled_StatusChanges()
		{
			TodoItem item = new TodoItem("Задача Ильи");

			item.SetStatus(TodoStatus.Completed);

			Assert.Equal(TodoStatus.Completed, item.Status);
		}

		[Theory]
		[InlineData("Купить молоко", "Купить молоко")]
		[InlineData("Очень длинная задача Ильи на сегодня", "Очень длинная задача Ильи на с...")]
		public void GetShortInfo_DifferentTexts_ReturnsExpectedStart(string text, string expected)
		{
			TodoItem item = new TodoItem(text);

			string result = item.GetShortInfo();

			Assert.StartsWith(expected, result);
		}
	}
}