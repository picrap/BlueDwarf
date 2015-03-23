// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Configuration
{
    using System;
    using System.Deployment.Application;
    using System.IO;
    using System.Reflection;
    using Annotations;
    using Microsoft.Win32;
    using Utility;
    using vbAccelerator.Components.Shell;

    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    internal class SetupConfiguration : ISetupConfiguration
    {
        /// <summary>
        /// Registers the specified assembly to be launched at startup.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="commandLine">The command line.</param>
        public void RegisterStartup(Assembly assembly, string commandLine = null)
        {
            using (var shortcut = new ShellLink())
            {
                var location = assembly.Location;
                shortcut.Target = location;
                shortcut.WorkingDirectory = Path.GetDirectoryName(location);
                shortcut.DisplayMode = ShellLink.LinkDisplayMode.edmNormal;
                shortcut.IconPath = location;
                shortcut.IconIndex = 0;
                if (commandLine != null)
                    shortcut.Arguments = commandLine;
                shortcut.Save(GetShortcutPath(assembly));
            }
        }

        /// <summary>
        /// Unregisters the specified assembly from being launched at startup.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        public void UnregisterStartup(Assembly assembly)
        {
            File.Delete(GetShortcutPath(assembly));
        }

        /// <summary>
        /// Gets the shortcut path in startup menu.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns></returns>
        private static string GetShortcutPath(Assembly assembly)
        {
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            var name = Path.GetFileNameWithoutExtension(assembly.Location) + ".lnk";
            return Path.Combine(folder, name);
        }

        private static RegistryKey GetUninstallKey(Assembly assembly = null)
        {
            assembly = assembly ?? Assembly.GetEntryAssembly();
            var assemblyProduct = assembly.GetCustomAttribute<AssemblyProductAttribute>();
            // I am totally aware that the two won't necessarily match.
            // But I found no other easy way.
            // If you copy this code, now you know too and you won't complain.
            var displayName = assemblyProduct.Product;
            using (var uninstallKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Uninstall"))
            {
                if (uninstallKey == null)
                    return null;
                var subKeyNames = uninstallKey.GetSubKeyNames();
                foreach (var subKeyName in subKeyNames)
                {
                    var subKey = uninstallKey.OpenSubKey(subKeyName, true);
                    var appDisplayName = (string)subKey.GetValue("DisplayName");
                    if (displayName == appDisplayName)
                        return subKey;
                    subKey.Dispose();
                }
            }
            return null;
        }

        /// <summary>
        /// Sets the uninstall icon (for click once deployment).
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="path">The path.</param>
        public void SetUninstallIcon(Assembly assembly, string path)
        {
            if (!ApplicationDeployment.IsNetworkDeployed)
                return;
            path = path ?? assembly.Location;
            using (var uninstallKey = GetUninstallKey(assembly))
            {
                if (uninstallKey == null)
                    return;

                uninstallKey.SetValue("DisplayIcon", path);
            }
        }
    }
}
