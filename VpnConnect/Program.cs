using System;
using System.Linq;
using System.ServiceProcess;

namespace VpnConnect
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            if (Environment.UserInteractive)
            {
                string vpnClienPath, profile, username, password;

                if (ParseArguments(args, out vpnClienPath, out profile, out username, out password))
                {
                    if (vpnClienPath != null)
                    {
                        Store.VpnClientPath = vpnClienPath;
                        Console.WriteLine("Set path: " + vpnClienPath);
                    }

                    if (profile != null)
                    {
                        Store.Profile = profile;
                        Console.WriteLine("Set profile: " + profile);
                    }

                    if (username != null)
                    {
                        Store.Username = username;
                        Console.WriteLine("Set username: " + username);
                    }

                    if (password != null)
                    {
                        Store.Password = password;
                        Console.WriteLine("Set password.");
                    }

                    Store.Persist();
                }
                else
                {
                    PrintUsage();
                }
            }
            else
            {
                ServiceBase.Run(new VpnConnectService());
            }
        }

        private static bool ParseArguments(string[] args, out string vpnClientPath, out string profile, out string username, out string password)
        {
            Func<string, string, bool> isParamater = (s, p) => s != null && (s.StartsWith("-" + p) || s.StartsWith("/" + p));

            Func<string, string> parse = (string p) => args
                .Where(s => isParamater(s, p))
                .Select(s => s.Split(new[] { '=', ':' }, 2).Last())
                .Select(s => UnQuote(s))
                .FirstOrDefault();

            vpnClientPath = null;
            profile = null;
            username = null;
            password = null;

            if (args == null)
                return false;

            vpnClientPath = parse("path");
            profile = parse("profile");
            username = parse("user");
            password = parse("password");

            return vpnClientPath != null 
                || profile != null 
                || username != null 
                || password != null;
        }

        private static void PrintUsage()
        {
            Console.WriteLine("Windows Service for establishing VPN connections via the Cisco VPN Client.");
            Console.WriteLine();
            Console.WriteLine("The program needs to be started as Windows Service to establish connections.");
            Console.WriteLine("Starting the program in the console is only needed for setting configuration.");
            Console.WriteLine();
            Console.WriteLine("Usage:");
            Console.WriteLine(@"VpnConnect.exe [-path=<path>] [-profile=<profile>] [-user=<username>] [-password=<password>]");
            Console.WriteLine();
        }

        private static string UnQuote(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return value;

            if ((value.StartsWith("\"") && value.EndsWith("\""))
                || (value.StartsWith("'") && value.EndsWith("'")))
            {
                return value.Substring(1, value.Length - 2);
            }

            return value;
        }
    }
}
