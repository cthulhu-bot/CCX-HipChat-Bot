using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Script.Serialization;

namespace HipChatBot
{
    public class TeamCityBot : HipChatBot
    {
        public static void Main(string[] args)
        {
            HipChatBot bot = new TeamCityBot();

            string testRoom = "232822";
            string previousRequest = string.Empty;

            while (true)
            {
                KeyValuePair<string, string> lastMessage = bot.getLastMessage(testRoom);
                if (lastMessage.Value.Contains("@teamcitybot") && !lastMessage.Value.Equals(previousRequest))
                {
                    Console.WriteLine("{0}:{1}",lastMessage.Key,lastMessage.Value);
                    bot.sendMessage(testRoom, "teamcitybot", string.Format("Hello {0}", lastMessage.Key));
                }

                previousRequest = lastMessage.Value;
                System.Threading.Thread.Sleep(1000);
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();
        }

    }
}
