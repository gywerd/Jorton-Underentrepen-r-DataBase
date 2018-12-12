using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace ClassBizz
{
    public class MacAddress
    {
        static string macAddress = GetMacAddress();

        public MacAddress() { }

        /// <summary>
        /// Method, that retrieves a MacAddress
        /// </summary>
        /// <returns></returns>
        private static string GetMacAddress()
        {
            String result = NetworkInterface
                .GetAllNetworkInterfaces()
                .Where(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .Select(nic => nic.GetPhysicalAddress().ToString())
                .FirstOrDefault();
            return result;
        }



        public override string ToString() { return macAddress.ToString(); }
    }
}
