// This is the blue dwarf
// more information at https://code.google.com/p/blue-dwarf/

namespace BlueDwarf.Configuration
{
    using System;
    using System.IO;
    using System.Reflection;
    using Annotations;
    using vbAccelerator.Components.Shell;

    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    internal class MenuStartupConfiguration : IStartupConfiguration
    {
        /// <summary>
        /// Registers the specified assembly to be launched at startup.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="commandLine">The command line.</param>
        public void Register(Assembly assembly, string commandLine = null)
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
        public void Unregister(Assembly assembly)
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
    }
}
