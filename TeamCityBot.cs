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

            //while (true)
            //{
                string lastMessage = bot.getLastMessage(testRoom);
                Console.WriteLine(lastMessage);

                System.Threading.Thread.Sleep(1000);
            //}

            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();
        }

    }
}
