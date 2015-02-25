// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Configuration
{
    using System.Reflection;

    public interface IStartupConfiguration
    {
        /// <summary>
        /// Registers the specified assembly to be launched at startup.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="commandLine">The command line.</param>
        void Register(Assembly assembly, string commandLine = null);

        /// <summary>
        /// Unregisters the specified assembly from being launched at startup.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        void Unregister(Assembly assembly);
    }
}
