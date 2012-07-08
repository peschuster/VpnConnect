using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;

namespace VpnConnect
{
    [RunInstaller(true)]
    public partial class VpnConnectInstaller : System.Configuration.Install.Installer
    {
        public VpnConnectInstaller()
        {
            InitializeComponent();
        }
    }
}
