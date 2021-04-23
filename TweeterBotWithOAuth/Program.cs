using System;
using System.Threading.Tasks;
using System.Diagnostics;
using Tweetinvi;
using System.IO;
using System.Text;

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

            
            var tweet = await userClient.Tweets.PublishTweetAsync($"Probando Tweet con Pin Auth - {DateTime.Now}");


            

        }
    }
}
