using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace TweeterBotWithOAuth
{
    public class Configurations
    {
        public static string consumerKey = ConfigurationManager.AppSettings["consumerKey"];
        public static string consumerSecret = ConfigurationManager.AppSettings["consumerSecret"];
        public static string accessToken = ConfigurationManager.AppSettings["accessToken"];
        public static string accessSecretToken = ConfigurationManager.AppSettings["accessSecretToken"];
        public static string BearerToken = ConfigurationManager.AppSettings["BearerToken"];
        public static string requestTokenUrl = ConfigurationManager.AppSettings["requestTokenUrl"];
        public static string authorizeUrl = ConfigurationManager.AppSettings["authorizeUrl"];
        public static string authorizeTokenUrl = ConfigurationManager.AppSettings["authorizeTokenUrl"];
        public static string callBackUrl = ConfigurationManager.AppSettings["callBackUrl"];

    }
}
