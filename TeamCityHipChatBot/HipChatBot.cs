using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Script.Serialization;

namespace HipChatBots
{
	public class HipChatBot
    {
		public string HipchatAuthToken { get; set; }//= "1b16612f462a4488e243f4cdd49136";

		public string HipchatApiId { get; set; }//= "228638";

		public string Jid { get; set; }// = "55332_ccx_general@conf.hipchat.com";

		public HipChatBot(string hipchatAuthToken, string hipchatApiId, string jid)
		{
			HipchatAuthToken = hipchatAuthToken;
			HipchatApiId = hipchatApiId;
			Jid = jid;
		}

	    public string sendMessage(string roomID, string from, string message)
        {
            string response = string.Empty;
            string uri = string.Format("https://api.hipchat.com/v1/rooms/message?format=json&auth_token={0}", HipchatAuthToken);
            string messageParams = string.Format("room_id={0}&from={1}&message={2}", roomID, from, message);

		    using (var webClient = new WebClient())
            {
                try
                {
                    webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    response = webClient.UploadString(uri, messageParams);
                }
                catch (WebException ex)
                {
	                var webResponse = ex.Response as HttpWebResponse;
	                if (webResponse != null)
                    {
                        switch (webResponse.StatusCode)
                        {
                            case HttpStatusCode.NotFound:
                                response = null;
                                break;

                            default:
                                throw;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

		    var javaScriptSerializer = new JavaScriptSerializer();
		    var dict = javaScriptSerializer.Deserialize<Dictionary<string, object>>(response);
            string status = dict["status"].ToString();
            return status;
        }

        /// <summary>
        /// Returns a dictionary containing the following fields parsed from the json returned by the hipchat api:
        /// date: message timestamp
        /// from: {name: username, user_id: id of user or "api" if the message was programatically generated}
        /// message: message text
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns>A dictionary indexed by the message timestamp connected to a KeyValuePair of user:message</returns>
		public SortedSet<HipChatMessage> getChatHistory(string roomId)
        {
            var history = new Dictionary<DateTime, HipChatMessage>();
            string uri =
                string.Format(
                    "https://api.hipchat.com/v1/rooms/history?room_id={0}&date={1}&timezone=MST&format=json&auth_token={2}", roomId, DateTime.Now.ToString("yyyy-MM-dd"), HipchatAuthToken);

            string webResponse = string.Empty;
            
            using (WebClient webClient = new WebClient())
            {
                try
                {
                    webResponse = webClient.DownloadString(uri);
                    while (webResponse.Contains("403") || webResponse.Equals(""))
                    {
                        webResponse = webClient.DownloadString(uri);
                    }
                }
                catch (WebException ex)
                {
                    if (ex.Response is HttpWebResponse)
                    {
                        switch (((HttpWebResponse)ex.Response).StatusCode)
                        {
                            case HttpStatusCode.NotFound:
                                history = null;
                                break;
                            case HttpStatusCode.Forbidden:
                                Console.WriteLine(ex.Message);
                                break;
                            default:
                                throw;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            var dict = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(webResponse);
            if (dict != null)
            {
                var messages = (ArrayList)dict["messages"];
                foreach (var o in messages)
                {
                    var message = (Dictionary<string, object>)o;
                    var user = (Dictionary<string, object>) message["from"];
                    string userName = user["name"].ToString();
                    string msg = message["message"].ToString();

	                var utcDateTime = DateTimeOffset.Parse(message["date"].ToString()).UtcDateTime;
	                if (!history.ContainsKey(utcDateTime))
		                history.Add(utcDateTime, new HipChatMessage(utcDateTime, userName, msg));
                }
            }

            return new SortedSet<HipChatMessage>(history.Values);
        }

        /// <summary>
        /// Retrieves the last message posted to the given room and the user who posted it
        /// </summary>
        /// <param name="roomID"></param>
        /// <returns>KeyValuePair - user:request</returns>
		public HipChatMessage getLastMessage(string roomID)
        {           
	        var history = getChatHistory(roomID);			

	        if (history.Any())
		        return history.Last();

	        return HipChatMessage.NilMessage;
        }


        public string getUserList(string roomId)
        {
            return string.Empty;
        }

        // Needs work (having problem extracting list of rooms from returned deserialized dictionary
        public List<string> getListOfRooms()
        {
            string uri = string.Format("https://api.hipchat.com/v1/rooms/list?auth_token={0}", HipchatAuthToken);
            var response = new List<string>();
            string webResponse = string.Empty;

            using (WebClient webClient = new WebClient())
            {
                try
                {
                    webResponse = webClient.DownloadString(uri);
                }
                catch (WebException ex)
                {
                    if (ex.Response is HttpWebResponse)
                    {
                        switch (((HttpWebResponse)ex.Response).StatusCode)
                        {
                            case HttpStatusCode.NotFound:
                                response = null;
                                break;

                            default:
                                throw ex;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            //Console.WriteLine(webResponse);
            var dict = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(webResponse);
            var roomList = dict.Select(k => k.Key.Equals("rooms")).ToList();

            foreach (var o in dict)
            {
                Console.WriteLine("value type: {0}", o.Value.GetType());

                List<object> objects = (List<object>)o.Value;

                foreach (var o2 in objects)
                {
                    Console.WriteLine(o2);
                }
                
                //string room_id = o.Key.Equals("room_id") ? o.Value.ToString() : string.Empty;
                //string room_name = o.Key.Equals("room_key") ? o.Value.ToString() : string.Empty;
                //if (!room_id.Equals(string.Empty) && !room_name.Equals(string.Empty))
                //{
                //    response.Add(string.Format("{0}:{1}",));
                //}
            }

            return response;
        }
    }

}
