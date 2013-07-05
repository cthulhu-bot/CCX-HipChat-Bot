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

            Console.WriteLine(bot.getChatHistory(testRoom));
            
            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();
        }

    }
}
