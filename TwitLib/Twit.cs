using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.IO;

namespace TwitLib
{
    public class TokenResponse
    {
        public string token_type;
        public string access_token;
    }

    public class TweetResponse
    {
        public List<Tweet> statuses;
    }

    public class TweetUser
    {
        public string screen_name;
        public string location;
        public string description;
        public int followers_count;
        public int friends_count;

    }

    public class Tweet
    {
        public string created_at { get; set; }
        public long id { get; set; }
        public string id_str { get; set; }
        public string text { get; set; }
        public string source { get; set; }
        public TweetUser user { get; set; }
    }

    public class Twit
    {


        string ConsumerKey = "acanWCVqDQcHpRBias9IrVjaQ";
        string ConsumerSecret = "Yl0wMIR7nylpfYMOzy6D8VC9ERD2f9NQOXyJXNdwNXXR90B64y";

        public TweetResponse GetTweets(string BearerToken, string searchTerm)
        {
            string urlToken = "https://api.twitter.com/1.1/search/tweets.json?q=" + searchTerm;

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(new Uri(urlToken));

            req.Headers.Add("Authorization", "Bearer " + BearerToken);
            req.Method = "GET";

            WebResponse response = req.GetResponse();

            string jsonResponse = "";

            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                jsonResponse = reader.ReadToEnd();
            }

            TweetResponse tweets =
            Newtonsoft.Json.JsonConvert.DeserializeObject<TweetResponse>(jsonResponse);

            return tweets;


        }

        public string GetAuthToken()
        {

            string encodedKeyAndSecret = Convert.ToBase64String(
                new System.Text.UTF8Encoding().GetBytes(
                  ConsumerKey + ":" + ConsumerSecret));

            string urlToken = "https://api.twitter.com/oauth2/token";

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(new Uri(urlToken));

            req.Headers.Add("Authorization","Basic " + encodedKeyAndSecret);
              
            req.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            req.Method = "POST";

            string requestBody = "grant_type=client_credentials";

            Stream dataStream = req.GetRequestStream();

            byte[] byteArray = new System.Text.UTF8Encoding().GetBytes(requestBody);
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse response = req.GetResponse();

            string jsonResponse = "";

            using (var reader = new StreamReader(response.GetResponseStream()))
            {
               jsonResponse = reader.ReadToEnd();
            }

            TokenResponse t = 
                Newtonsoft.Json.JsonConvert.DeserializeObject<TokenResponse>(jsonResponse);
      
            return t.access_token;

        }


    }
}
