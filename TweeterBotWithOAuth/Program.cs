using System;
using System.Threading.Tasks;
using System.Diagnostics;
using Tweetinvi;

namespace TwitterBotWithOAuth
{
    public class Program
    {

        static async Task Main(string[] args)
        {
            Console.WriteLine($"<{DateTime.Now}> - Bot Started");
            //await AuthenticateClientAsync();

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

            // You can now save those credentials or use them as followed
            var userClient = new TwitterClient(userCredentials);
            var user = await userClient.Users.GetAuthenticatedUserAsync();

            Console.WriteLine("Congratulation you have authenticated the user: " + user);
            Console.Read();

            var tweet = await userClient.Tweets.PublishTweetAsync($"Probando Tweet con Pin Auth - {DateTime.Now}");
            

        }
    }
}
