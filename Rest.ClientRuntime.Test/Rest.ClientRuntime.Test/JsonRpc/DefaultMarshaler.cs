using Microsoft.Rest.ClientRuntime.Test.Rpc;
using Microsoft.Rest.Serialization;
using Newtonsoft.Json;

namespace Microsoft.Rest.ClientRuntime.Test.JsonRpc
{
    public sealed class DefaultMarshaler : IMarshalling
    {
        private JsonSerializerSettings _serializerSettings;
        private JsonSerializerSettings _deserializerSettings;

        public DefaultMarshaler(JsonSerializerSettings serializerSettings, JsonSerializerSettings deserializerSettings)
        {
            _serializerSettings = serializerSettings;
            _deserializerSettings = deserializerSettings;
        }

        public T Deserialize<T>(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return default(T);
            }

            return SafeJsonConvert.DeserializeObject<T>(value, _deserializerSettings);
        }

        public string Serialize(object value)
        {
            return SafeJsonConvert.SerializeObject(value, _serializerSettings);
        }
    }
}
