using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InIWorkspace.Utils
{
    public static class MacAddressHelper
    {
        public static string FormatMacAddress(string macAddress)
        {
            if (string.IsNullOrEmpty(macAddress) || macAddress.Length != 12)
            {
                throw new ArgumentException("Invalid MAC address");
            }

            return string.Join(":", Enumerable.Range(0, 6)
                .Select(i => macAddress.Substring(i * 2, 2).ToUpper()));
        }
        public static string GetMacAddress()
        {
            string macAddresses = string.Empty;
            foreach (System.Net.NetworkInformation.NetworkInterface nic in System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == System.Net.NetworkInformation.OperationalStatus.Up)
                {
                    macAddresses += nic.GetPhysicalAddress().ToString();
                    break;
                }
            }
            return macAddresses;
        }
    }
}
