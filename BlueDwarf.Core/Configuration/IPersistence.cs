// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Configuration
{
    /// <summary>
    /// Abstraction for persistent configuration
    /// </summary>
    public interface IPersistence
    {
        /// <summary>
        /// Gets a value, or default value.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        object GetValue(string name, object defaultValue);

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <param name="writeNow">if set to <c>true</c> persists the value immediately.</param>
        void SetValue(string name, object value, bool writeNow);

        /// <summary>
        /// Writes all changes down to persistence.
        /// </summary>
        void Write();
    }
}
