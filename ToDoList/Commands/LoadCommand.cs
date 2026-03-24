using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList.Exceptions;

namespace TodoList.Commands
{
	public class LoadCommand : ICommand
	{
		private string _args;

		public LoadCommand(string args)
		{
			_args = args;
		}

		public void Execute()
		{
		}
	}
}