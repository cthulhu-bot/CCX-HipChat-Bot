using HipChatBots;
using Ninject;

namespace TeamChatBotApp
{
	public class TeamCityBotProgram
	{
		private static IKernel _kernel;
		private static void CreateKernel()
		{
			_kernel = new StandardKernel(new HipChatBotModule());
		}

		public static void Main(string[] args)
		{
			CreateKernel();

			var teamCityBot = _kernel.Get<TeamCityBot>();
			teamCityBot.Invoke();
		}
	}
}
