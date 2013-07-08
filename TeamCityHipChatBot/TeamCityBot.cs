using System;
using System.Threading;
using Ninject;

namespace HipChatBots
{
	public class TeamCityBot 
	{
		public TeamCityBot(HipChatBot bot, string roomId)			
		{
			Bot = bot;
			RoomId = roomId;
		}

		public HipChatBot Bot { get; set; }
		public string RoomId { get; set; }
			
		public void Invoke()
		{
			var previousRequest = HipChatMessage.NilMessage;
			while (true)
			{
				var lastMessage = Bot.getLastMessage(RoomId);
				if (lastMessage.From.Contains("@teamcitybot") && !lastMessage.Equals(previousRequest))
				{
					Console.WriteLine("{0}:{1}", lastMessage.From, lastMessage.Message);
					Bot.sendMessage(RoomId, "teamcitybot", string.Format("Hello {0}", lastMessage.From));
				}

				previousRequest = lastMessage;
				Thread.Sleep(1000);
			}
		}
	}
}