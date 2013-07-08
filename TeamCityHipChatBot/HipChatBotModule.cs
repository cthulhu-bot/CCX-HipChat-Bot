using Ninject.Modules;

namespace HipChatBots
{
	public class HipChatBotModule : NinjectModule
	{
		public override void Load()
		{
			Kernel.Bind<HipChatBot>()
				  .ToSelf()
				  .WithConstructorArgument("hipchatAuthToken", "1b16612f462a4488e243f4cdd49136")
				  .WithConstructorArgument("hipchatApiId", "228638")
				  .WithConstructorArgument("jid", "55332_ccx_general@conf.hipchat.com");

			Kernel.Bind<TeamCityBot>()
				  .ToSelf()
				  .WithConstructorArgument("roomId", "232822");
		}
	}
}