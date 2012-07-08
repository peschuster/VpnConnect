using System;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Threading;

namespace VpnConnect
{
    partial class VpnConnectService : ServiceBase
    {
        private const int BaseIntervallMs = 15 * 1000;

        private VpnClientWrapper client;

        private Timer timer;

        private string profile;

        private int currentIntervall = BaseIntervallMs;

        public VpnConnectService()
        {
            this.InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            string path = Store.VpnClientPath;

            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
                path = TryGetDefaultExePath();

            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            {
                this.eventLog.WriteEntry(
                    "vpnclient.exe not set and not found in default locations. Is the Cisco VPN Client installed?", 
                    EventLogEntryType.Error);

                throw new InvalidOperationException("vpnclient.exe not found.");
            }

            this.client = new VpnClientWrapper(path);

            this.profile = Store.Profile;
            bool success = this.client.Connect(profile, Store.Username, Store.Password);

            if (!success)
            {
                this.eventLog.WriteEntry(
                    "Unable to establish vpn connection. Are profile, username and password set correctly?",
                    EventLogEntryType.Error);

                throw new InvalidOperationException("Unable to establish vpn connection.");
            }
            else
            {
                this.eventLog.WriteEntry(
                    "Established vpn connection: " + this.profile,
                    EventLogEntryType.Information);
            }

            this.timer = new Timer(this.OnTimedAction);
            this.timer.Change(0, BaseIntervallMs);
            this.currentIntervall = BaseIntervallMs;
        }

        protected override void OnStop()
        {
            this.client.Disconnect();
        }

        private void OnTimedAction(object state)
        {
            if (!this.client.IsConnected(this.profile))
            {
                if (!this.client.Connect(this.profile, Store.Username, Store.Password))
                {
                    this.eventLog.WriteEntry(
                        "Unable to establish vpn connection. Are profile, username and password set correctly?",
                        EventLogEntryType.Error);

                    this.timer.Change(0, this.currentIntervall + BaseIntervallMs);
                    this.currentIntervall += BaseIntervallMs;
                }
                else 
                {
                    this.timer.Change(0, BaseIntervallMs);
                    this.currentIntervall = BaseIntervallMs;
                }
            }
        }

        private string TryGetDefaultExePath()
        {
            string path = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                "Cisco Systems",
                "VPN Client",
                "vpnclient.exe");

            if (File.Exists(path))
                return path;

            path = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                "Cisco Systems",
                "VPN Client",
                "vpnclient.exe");

            if (File.Exists(path))
                return path;

            return null;
        }
    }
}
