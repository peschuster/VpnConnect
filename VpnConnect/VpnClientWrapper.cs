using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace VpnConnect
{
    internal class VpnClientWrapper
    {
        private readonly string path;

        public VpnClientWrapper(string vpnClientExe)
        {
            if (!File.Exists(vpnClientExe))
                throw new ArgumentException("Specified path does not exist.", "vpnClientExe");

            this.path = vpnClientExe;
        }

        public bool Connect(string profile, string user, string password)
        {
            string standardOut;

            bool success = this.Execute(
                string.Format("connect {0} user {1} pwd {2}", profile, user, password),
                out standardOut);

            success &= standardOut.Contains("connection is secure");

            return success;
        }

        public bool IsConnected()
        {
            string standardOut;

            bool success = this.Execute("stat", out standardOut);

            return success 
                && standardOut.Contains("VPN traffic summary");
        }

        public bool IsConnected(string profile)
        {
            string standardOut;

            bool success = this.Execute("stat", out standardOut);

            return success 
                && standardOut.Contains("Connection Entry: " + profile);
        }

        public bool Disconnect()
        {
            string standardOut;

            bool success = this.Execute("disconnect", out standardOut);

            return success
                && standardOut.Contains("connection has been terminated");
        }

        private bool Execute(string arguments, out string result, int maxMilliseconds = 30000)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = this.path,
                Arguments = arguments,

                RedirectStandardOutput = true,

                UseShellExecute = false,
                CreateNoWindow = true,
            };

            StringBuilder standardOut = new StringBuilder();

            Process p = Process.Start(startInfo);

            p.OutputDataReceived += (object s, DataReceivedEventArgs d) => standardOut.AppendLine(d.Data);
            p.BeginOutputReadLine();

            bool success = p.WaitForExit(maxMilliseconds);

            result = standardOut.ToString();

            return success;
        }
    }
}
