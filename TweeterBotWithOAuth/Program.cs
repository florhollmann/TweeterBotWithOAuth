using System;
using System.Threading.Tasks;
using System.Diagnostics;
using Tweetinvi;
using System.IO;
using System.Text;
using System.Net;
using HtmlAgilityPack;

namespace TwitterBotWithOAuth
{
    public class Program
    {

        public static void ExportToFile(string[] tokenArr)
        {
            File.WriteAllLines(Configurations.tokenPath, tokenArr);
        }

        static async Task Main(string[] args)
        {
            TwitterClient userClient = new TwitterClient(Configurations.consumerKey, Configurations.consumerSecret);

            Console.WriteLine($"<{DateTime.Now}> - Bot Started");
            //await AuthenticateClientAsync();
            if (!File.Exists(Configurations.tokenPath))
            {
                // Create a client for your app
                TwitterClient appClient = new TwitterClient(Configurations.consumerKey, Configurations.consumerSecret);

                // Start the authentication process
                var authenticationRequest = await appClient.Auth.RequestAuthenticationUrlAsync();



                // Go to the URL so that Twitter authenticates the user and gives him a PIN code.
                Process.Start(new ProcessStartInfo(authenticationRequest.AuthorizationURL)
                {
                    UseShellExecute = true
                });

                // Ask the user to enter the pin code given by Twitter
                Console.WriteLine("Please enter the code and press enter.");
                var pinCode = Console.ReadLine();


                // With this pin code it is now possible to get the credentials back from Twitter
                var userCredentials = await appClient.Auth.RequestCredentialsFromVerifierCodeAsync(pinCode, authenticationRequest);
                string[] tokarr = new string[] { userCredentials.AccessToken, userCredentials.AccessTokenSecret };

                ExportToFile(tokarr);

                // You can now save those credentials or use them as followed
                userClient = new TwitterClient(userCredentials);
                var user = await userClient.Users.GetAuthenticatedUserAsync();

                Console.WriteLine("Congratulation you have authenticated the user: " + user);
                Console.Read();
            }
            else
            {
                string[] readText = File.ReadAllLines(Configurations.tokenPath, Encoding.UTF8);
                var accessToken = readText[0];
                var accessSecretToken = readText[1];

                userClient = new TwitterClient(Configurations.consumerKey, Configurations.consumerSecret, accessToken, accessSecretToken);

            }

            string[] elFacundo = { "http://www.cervantesvirtual.com/obra-visor/vida-de-juan-facundo-quiroga--0/html/fee191b4-82b1-11df-acc7-002185ce6064_3.html#I_1_", "http://www.cervantesvirtual.com/obra-visor/vida-de-juan-facundo-quiroga--0/html/fee191b4-82b1-11df-acc7-002185ce6064_4.html#I_8_", "http://www.cervantesvirtual.com/obra-visor/vida-de-juan-facundo-quiroga--0/html/fee191b4-82b1-11df-acc7-002185ce6064_5.html#I_13_", "http://www.cervantesvirtual.com/obra-visor/vida-de-juan-facundo-quiroga--0/html/fee191b4-82b1-11df-acc7-002185ce6064_6.html#I_18_", "http://www.cervantesvirtual.com/obra-visor/vida-de-juan-facundo-quiroga--0/html/fee191b4-82b1-11df-acc7-002185ce6064_7.html#I_24_" };


            string innerBodyScrapped;
            HtmlWeb cWeb = new HtmlWeb();
            Random rand = new Random();
            int i = rand.Next(elFacundo.Length);

            HtmlDocument doc = cWeb.Load(elFacundo[i]);

            foreach (var item in doc.DocumentNode.SelectNodes("//*[@id='obra']"))
            {

                innerBodyScrapped = WebUtility.HtmlDecode(item.InnerText).Replace("\n", "")
                                                                            .Replace("\r", "")
                                                                            .Replace("\t", "")
                                                                            .Replace("[", "")
                                                                            .Replace("]", "");

                string[] sentenceArr = innerBodyScrapped.Split('.');
                int index = rand.Next(sentenceArr.Length);
                if (sentenceArr[index].Length > 1 && sentenceArr[index].Length < 280)
                {
                    var tweet = await userClient.Tweets.PublishTweetAsync(sentenceArr[index]);
                }


            };


            

        }
    }
}
