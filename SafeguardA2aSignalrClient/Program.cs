using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using OneIdentity.SafeguardDotNet;

namespace SafeguardA2aSignalrClient
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Usage: SafeguardSignalrClient [URL] [THUMBPRINT] [API_KEY1,API_KEY2,...]");
                Environment.Exit(1);
            }

            //extract URL, thumbprint, and comma-delimted API keys from command line arguments
            string url = args[0];
            string thumbprint = args[1];
            List<SecureString> apiKeys = args[2].Split(',').Select(k => k.ToSecureString()).ToList();

            //connect to Safeguard with provided thumbprint
            var context = Safeguard.A2A.GetContext(url, thumbprint, 3, true);

            //create A2A listener for the provided API keys, and assign the handler function defined below to execute when an event is received
            var listener = context.GetA2AEventListener(apiKeys, Handler);

            //start listener and wait for any input, then stop.
            listener.Start();
            Console.WriteLine($"Listening for AssetAccountPasswordUpdated events...\npress any key to exit");
            Console.ReadKey();
            listener.Stop();
        }

        private static void Handler(string eventname, string eventbody)
        {
            Console.WriteLine(eventname);
            Console.WriteLine(eventbody);
        }

    }
}
