using System;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace VpnConnect
{
    internal static class Store
    {
        private static readonly byte[] aditionalEntropy = { 2, 8, 12, 89, 3, 19 };

        private static readonly Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        
        public static string VpnClientPath 
        {
            get { return Get("vpnClientPath"); }
            set { Set("vpnClientPath", value); }
        }

        public static string Profile
        {
            get { return Decrypt(Get("profile")); }
            set { Set("profile", Encrypt(value)); }
        }

        public static string Username
        {
            get { return Decrypt(Get("user")); }
            set { Set("user", Encrypt(value)); }
        }

        public static string Password
        {
            get { return Decrypt(Get("password")); }
            set { Set("password", Encrypt(value)); }
        }

        private static string Get(string key)
        {
            if (!configuration.AppSettings.Settings.AllKeys.Contains(key))
                return null;

            return configuration.AppSettings.Settings[key].Value;
        }

        private static void Set(string key, string value)
        {
            if (configuration.AppSettings.Settings.AllKeys.Contains(key))
            {
                configuration.AppSettings.Settings[key].Value = value;
            }
            else
            {
                configuration.AppSettings.Settings.Add(key, value);
            }
        }
        
        public static void Persist()
        {
            configuration.Save(ConfigurationSaveMode.Modified);
        }

        private static string Encrypt(string data)
        {
            if (data == null)
                return null;

            byte[] encrypted = ProtectedData.Protect(
                Encoding.Default.GetBytes(data), 
                aditionalEntropy, 
                DataProtectionScope.LocalMachine);

            return Convert.ToBase64String(encrypted);
        }

        private static string Decrypt(string encrypted)
        {
            if (encrypted == null)
                return null;

            byte[] data = ProtectedData.Unprotect(
                Convert.FromBase64String(encrypted),
                aditionalEntropy,
                DataProtectionScope.LocalMachine);

            return Encoding.Default.GetString(data);
        }
    }
}
