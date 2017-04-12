using Rest.ClientRuntime.Test.Rpc;
using Newtonsoft.Json;
using Microsoft.Rest.Serialization;

namespace Rest.ClientRuntime.Test.JsonRpc
{
    public sealed class Marshalling : IMarshalling
    {
        public JsonSerializerSettings SerializerSettings { get; }

        public JsonSerializerSettings DeserializerSettings { get; }

        public Marshalling(
            JsonSerializerSettings serializerSettings,
            JsonSerializerSettings deserializerSettings)
        {
            SerializerSettings = serializerSettings;
            DeserializerSettings = deserializerSettings;
        }

        public T Deserialize<T>(string value)
            => value == null 
                ? default(T) 
                : SafeJsonConvert.DeserializeObject<T>(value, DeserializerSettings);

        public string Serialize(object value)
            => SafeJsonConvert.SerializeObject(value, SerializerSettings);
    }
}
