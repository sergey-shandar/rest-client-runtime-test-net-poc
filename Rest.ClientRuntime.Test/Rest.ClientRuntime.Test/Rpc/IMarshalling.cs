namespace Rest.ClientRuntime.Test.Rpc
{
    public interface IMarshalling
    {
        T Deserialize<T>(string value);
        string Serialize(object value);
    }
}
