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

        private long i = 0;

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
            var request = new Request(i.ToString(), method, @params);
            _Io.Writer.WriteMessage(_Marshalling, request);
            ++i;
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
                throw new ErrorException(method, _Marshalling.Serialize(request), response.error);
            }
            return response.result;
        }
    }
}
