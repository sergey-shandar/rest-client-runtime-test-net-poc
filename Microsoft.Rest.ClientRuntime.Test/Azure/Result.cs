namespace Microsoft.Rest.ClientRuntime.Test.Azure
{
    public sealed class Result<T>
    {
        public int statusCode { get; set; }
        public object headers { get; set; }
        public T respose { get; set; }
    }
}
