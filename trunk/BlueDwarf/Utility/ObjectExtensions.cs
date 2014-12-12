namespace BlueDwarf.Utility
{
    public static class ObjectExtensions
    {
        public static bool SafeEquals(this object a, object b)
        {
            if (a == null || b == null)
                return (a == null) == (b == null);
            return a.Equals(b);
        }
    }
}