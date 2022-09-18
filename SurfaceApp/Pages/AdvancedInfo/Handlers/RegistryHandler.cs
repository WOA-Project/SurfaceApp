using System.Text;
using RegistryRT;

#nullable enable

namespace AdvancedInfo.Handlers
{
    public class RegistryHandler
    {
        private Registry regrt => SurfaceApp.App.Registry;

        public RegistryHandler()
        {
            ReleaseName = ReadRegistryStringFromDTI("PhoneReleaseVersion");
        }

        private string? ReadRegistryStringFromDTI(string Value)
        {
            return ReadRegistryString(RegistryHive.HKEY_LOCAL_MACHINE, "SYSTEM\\Platform\\DeviceTargetingInfo", Value);
        }

        private string? ReadRegistryString(RegistryHive hive, string Key, string Value)
        {
            uint regtype;
            byte[] buffer;
            bool result = regrt.QueryValue(hive, Key, Value, out regtype, out buffer);
            if (result)
            {
                return Encoding.ASCII.GetString(buffer);
            }
            return null;
        }

        public string? ReleaseName { get; internal set; }
    }
}
