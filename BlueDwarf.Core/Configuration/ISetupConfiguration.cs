// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Configuration
{
    using System.Reflection;

    public interface ISetupConfiguration
    {
        /// <summary>
        /// Gets a value indicating whether this instance is installed using click once.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is click once; otherwise, <c>false</c>.
        /// </value>
        bool IsClickOnce { get; }

        /// <summary>
        /// Registers the specified assembly to be launched at startup.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="commandLine">The command line.</param>
        void RegisterStartup(Assembly assembly, string commandLine = null);

        /// <summary>
        /// Unregisters the specified assembly from being launched at startup.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        void UnregisterStartup(Assembly assembly);

        /// <summary>
        /// Sets the uninstall icon (for click once deployment).
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="path">The path.</param>
        void SetUninstallIcon(Assembly assembly, string path = null);
    }
}
