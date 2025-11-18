using System.Reflection;

namespace Attributes.WPF
{
    internal static class AppInfo
    {
        public static string AppName =>
            Assembly.GetExecutingAssembly().GetName().Name;

        public static string Version =>
            Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
    }
}
