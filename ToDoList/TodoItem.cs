using System;

namespace TodoList
{
	public enum TodoStatus
	{
		NotStarted, // не начато
		InProgress, // в процессе
		Completed, // выполнено
		Postponed, // отложено
		Failed // провалено
	}
	public class TodoItem
    {
        public string Text { get; private set; }
		public TodoStatus Status { get; private set; }
		public DateTime LastUpdate { get; private set; }

        public TodoItem(string text)
        {
            this.Text = text;
			this.Status = TodoStatus.NotStarted;
			this.LastUpdate = DateTime.Now;
        }
        public void MarkDone()
        {
            this.IsDone = true;
            this.LastUpdate = DateTime.Now;
        }

        public void UpdateText(string newText)
        {
            this.Text = newText;
            this.LastUpdate = DateTime.Now;
        }

        public string GetShortInfo()
        {
            string shortText = Text.Length > 30 ? Text.Substring(0, 30) + "..." : Text;
            string status = IsDone ? "Сделано" : "Не сделано";
            string date = LastUpdate.ToString("dd.MM.yyyy HH:mm");
            return $"{shortText} | {status} | {date}";
        }
        public string GetFullInfo()
        {
            string statusText = IsDone ? "выполнена" : "не выполнена";
            string dateText = LastUpdate.ToString("dd.MM.yyyy HH:mm");
            return $"Текст: {Text}\nСтатус: {statusText}\nДата изменения: {dateText}";
        }
    }
}