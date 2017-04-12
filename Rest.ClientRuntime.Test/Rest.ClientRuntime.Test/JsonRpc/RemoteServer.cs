using System.Collections.Generic;
using System.Threading.Tasks;
using Rest.ClientRuntime.Test.Rpc;
using Rest.ClientRuntime.Test.TextRpc;
using System;

namespace Rest.ClientRuntime.Test.JsonRpc
{
    public sealed class RemoteServer : IServer
    {
        private readonly IMarshalling _Marshalling;

        private readonly Utf8Reader _Reader;

        private readonly Utf8Writer _Writer;

        public RemoteServer(Utf8Reader reader, Utf8Writer writer, IMarshalling marshalling)
        {
            _Reader = reader;
            _Writer = writer;
            _Marshalling = marshalling;
        }

        public async Task<T> Call<T>(string method, Dictionary<string, object> @params)
        {
            _Writer.WriteMessage(
                _Marshalling,
                new Request(0, method, @params);
            Response<T> response;
            while (true)
            {
                response = _Reader.ReadMessage<Response<T>>(_Marshalling);
                if (response != null)
                {
                    break;
                }
            }
            if (response.error != null)
            {
                throw new Exception(response.error.message);
            }
            return response.result;
        }
    }
}
