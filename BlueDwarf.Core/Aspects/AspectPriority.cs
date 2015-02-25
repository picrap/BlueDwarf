// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Aspects
{
    /// <summary>
    /// Priorities for aspects
    /// Because I hate hard-coded numeric values
    /// </summary>
    public static class AspectPriority
    {
        /// <summary>
        /// Notification level: the property changes, and we take good note of it
        /// </summary>
        public const int Notification = 100;
        /// <summary>
        /// Lowest level, if data is stored virtually (not in the property's backing field)
        /// </summary>
        public const int DataHolder = 10;
    }
}
