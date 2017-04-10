using Rest.ClientRuntime.Test.Rpc;
using Newtonsoft.Json;
using Microsoft.Rest.Serialization;

namespace Rest.ClientRuntime.Test.JsonRpc
{
    public sealed class Marshalling : IMarshalling
    {
        private readonly JsonSerializerSettings _SerializerSettings;

        private readonly JsonSerializerSettings _DeserializerSettings;

        public Marshalling(
            JsonSerializerSettings serializerSettings,
            JsonSerializerSettings deserializerSettings)
        {
            _SerializerSettings = serializerSettings;
            _DeserializerSettings = deserializerSettings;
        }

        public T Deserialize<T>(string value)
            => value == null ? default(T) : SafeJsonConvert.DeserializeObject<T>(value, _DeserializerSettings);

        public string Serialize(object value)
            => SafeJsonConvert.SerializeObject(value, _SerializerSettings);
    }
}
