using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace VpnConnect
{
    partial class VpnConnectInstaller
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private IContainer components = null;
        private ServiceInstaller serviceInstaller;
        private ServiceProcessInstaller serviceProcessInstaller;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.serviceInstaller = new ServiceInstaller();
            this.serviceProcessInstaller = new ServiceProcessInstaller();

            this.serviceInstaller.ServiceName = "VpnConnectService";
            this.serviceInstaller.DisplayName = "VPN Connect Service (for Cisco)";
            this.serviceInstaller.Description = "Service for establishing and keeping a VPN connection using the Cisco VPN Client.";
            this.serviceInstaller.StartType = ServiceStartMode.Automatic;
            this.serviceInstaller.DelayedAutoStart = true;

            this.serviceProcessInstaller.Account = ServiceAccount.LocalSystem;
            this.serviceProcessInstaller.Username = null;
            this.serviceProcessInstaller.Password = null;

            // 
            // ServiceInstaller
            // 
            this.Installers.AddRange(new Installer[] 
            {
                this.serviceInstaller, 
                this.serviceProcessInstaller
            });
        }

        #endregion
    }
}