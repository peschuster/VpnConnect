# VpnConnect

VpnConnect is a Windows Service, which establishes a VPN connection using the Cisco VPN client. This is useful, if the connection has to be established on a server with no user logged in (no gui).

Additionally it checks every 15 seconds whether the connection is still active and tries to reconnect, if the connection was closed.

## Configuration

The following configuration options are available:

- Path to `vpnclient.exe`
- VPN profile name
- Username
- Password

Configuration can be set via the command line:

    VpnConnect.exe [-path=<path>] [-profile=<profile>] [-user=<username>] [-password=<password>]

Configuration is encrypted using (DPAPI) and stored in the `VpnConnect.exe.config` file.

## Installation

Run `install.bat` as administrator (right click -> "Run as Administrator").