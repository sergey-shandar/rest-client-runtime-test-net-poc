using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Rest.ClientRuntime.Test.Rpc;
using Microsoft.Rest.ClientRuntime.Test.TextRpc;
using System;
using Microsoft.Rest.ClientRuntime.Test.Utf8;
using System.Diagnostics;

namespace Microsoft.Rest.ClientRuntime.Test.JsonRpc
{
    public sealed class RemoteServer : IServer
    {
        private readonly IMarshalling _Marshalling;

        private readonly Reader _Reader;

        private readonly Writer _Writer;

        public RemoteServer(Reader reader, Writer writer, IMarshalling marshalling)
        {
            _Reader = reader;
            _Writer = writer;
            _Marshalling = marshalling;
        }

        public RemoteServer(Process process, Marshalling marshalling):
            this(
                new Reader(process.StandardOutput.BaseStream),
                new Writer(process.StandardInput.BaseStream),
                marshalling)
        {
        }

        public async Task<T> Call<T>(string method, Dictionary<string, object> @params)
        {
            _Writer.WriteMessage(
                _Marshalling,
                new Request(0, method, @params));
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
