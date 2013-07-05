using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace HipChatBot
{
    public abstract class HipChatBot
    {
        protected static string hipchatAuthToken = "1b16612f462a4488e243f4cdd49136";
        protected static string hipchatAPIID = "228638";
        protected static string jid = "55332_ccx_general@conf.hipchat.com";

        public string sendMessage(string roomID, string from, string message)
        {
            string response = string.Empty;
            string uri = string.Format("https://api.hipchat.com/v1/rooms/message?format=json&auth_token={0}",
                                        hipchatAuthToken);
            string messageParams = string.Format("room_id={0}&from={1}&message={2}", roomID, from, message);

            using (WebClient webClient = new WebClient())
            {
                try
                {
                    webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    response = webClient.UploadString(uri, messageParams);
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

            var dict = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(response);
            string status = dict["status"].ToString();
            return status;
        }

        public string getChatHistory(string roomId)
        {
            string history = string.Empty;
            string uri =
                string.Format(
                    "https://api.hipchat.com/v1/rooms/history?room_id={0}&date={1}&timezone=MST&format=json&auth_token={2}", roomId, DateTime.Now.ToString("yyyy-MM-dd"), hipchatAuthToken);

            string webResponse = string.Empty;
            
            using (WebClient webClient = new WebClient())
            {
                try
                {
                    webResponse = webClient.DownloadString(uri);
                    Console.WriteLine(webResponse);
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

            Console.WriteLine(webResponse);

            return history;
        }

        public string getUserList(string roomId)
        {
            return string.Empty;
        }

        // Needs work (having problem extracting list of rooms from returned deserialized dictionary
        public List<string> getListOfRooms()
        {
            string uri = string.Format("https://api.hipchat.com/v1/rooms/list?auth_token={0}", hipchatAuthToken);
            List<string> response = new List<string>();
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
