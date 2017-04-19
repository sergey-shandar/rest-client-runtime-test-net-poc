using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Rest.ClientRuntime.Test.Rpc;
using Microsoft.Rest.ClientRuntime.Test.TextRpc;
using System;
using Microsoft.Rest.ClientRuntime.Test.Utf8;
using System.Diagnostics;
using System.IO;
using Microsoft.Rest.ClientRuntime.Test.Log;

namespace Microsoft.Rest.ClientRuntime.Test.JsonRpc
{
    public sealed class RemoteServer : IServer
    {
        private readonly IMarshalling _Marshalling;

        private readonly Io _Io;

        public RemoteServer(Io io, IMarshalling marshalling)
        {
            _Io = io;
            _Marshalling = marshalling;
        }

        public RemoteServer(Process process, Marshalling marshalling):
            this(process.CreateIo(), marshalling)
        {
        }

        public RemoteServer(Process process, Stream log, Marshalling marshalling) :
            this(process.CreateIo().WithLog(log), marshalling)
        {
        }

        public async Task<T> Call<T>(string method, Dictionary<string, object> @params)
        {
            _Io.Writer.WriteMessage(
                _Marshalling,
                new Request(0, method, @params));
            Response<T> response;
            while (true)
            {
                response = _Io.Reader.ReadMessage<Response<T>>(_Marshalling);
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
