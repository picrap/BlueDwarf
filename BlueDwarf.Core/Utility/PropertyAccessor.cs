#region SignReferences
// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
#endregion

namespace BlueDwarf.Utility
{
    using System;

    /// <summary>
    /// Property accessor, a simple version
    /// </summary>
    /// <typeparam name="TProperty">The type of the property.</typeparam>
    public class PropertyAccessor<TProperty>
    {
        private readonly Func<TProperty> _get;
        private readonly Action<TProperty> _set;

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public TProperty Value
        {
            get { return _get(); }
            set { _set(value); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyAccessor{TProperty}"/> class.
        /// </summary>
        /// <param name="get">The get.</param>
        /// <param name="set">The set.</param>
        public PropertyAccessor(Func<TProperty> get, Action<TProperty> set)
        {
            _get = get;
            _set = set;
        }
    }
}
