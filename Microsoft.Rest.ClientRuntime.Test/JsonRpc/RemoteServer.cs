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

        public async Task<T> Call<T, E>(string method, Dictionary<string, object> @params)
        {
            var request = new Request(i.ToString(), method, @params);
            await _Io.Writer.WriteMessageAsync(_Marshalling, request);
            ++i;
            Response<T, E> response;
            while (true)
            {
                response = await _Io.Reader.ReadMessageAsync<Response<T, E>>(_Marshalling);
                if (response != null)
                {
                    break;
                }
            }
            if (response.error != null)
            {
                request.@params.Remove("__reserved");
                throw new ErrorException<E>(method, _Marshalling.Serialize(request), response.error);
            }
            return response.result;
        }
    }
}
