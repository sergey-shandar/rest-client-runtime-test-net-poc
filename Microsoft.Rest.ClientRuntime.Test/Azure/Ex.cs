namespace Microsoft.Rest.ClientRuntime.Test.Azure
{
    public static class Ex
    {
        private static class Instance<T>
            where T : new()
        {
            public static readonly T Value = new T();
        }

        private static class Array<T>
        {
            public static readonly T[] Value = new T[0];
        }

        public static T DefaultIfNull<T>(this T value)
            where T : class, new()
            => value ?? Instance<T>.Value;

        public static T[] DefaultIfNull<T>(this T[] value)
            => value ?? Array<T>.Value;
    }
}
