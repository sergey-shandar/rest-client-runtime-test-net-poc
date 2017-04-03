using Microsoft.Rest.ClientRuntime.Test.Rpc;
using Microsoft.Rest.ClientRuntime.Test.TextRpc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Microsoft.Rest.ClientRuntime.Test.JsonRpc
{
    public class RemoteServer : IServer
    {
        private StreamWriter Writer { get; }

        private StreamReader Reader { get; }

        public RemoteServer(
            StreamWriter writer,
            StreamReader reader)
        {
            Writer = writer;
            Reader = reader;
        }

        public Task Call(IMarshalling marshalling, string method, Dictionary<string, object> @params)
        {
            Writer.WriteMessage(marshalling, Request.Create(method, @params));
            Response response = null;
            while (response == null)
            {
                response = Reader.ReadMessage<Response>(marshalling);
            }
            if (response.error != null)
            {
                throw new Exception(response.error.message);
            }
            return Task.FromResult(0);
        }

        public Task<T> Call<T>(IMarshalling marshalling, string method, Dictionary<string, object> @params)
        {
            Writer.WriteMessage(marshalling, Request.Create(method, @params));
            Response<T> response = null;
            while (response == null)
            {
                response = Reader.ReadMessage<Response<T>>(marshalling);
            }
            if (response.error != null)
            {
                throw new Exception(response.error.message);
            }
            return Task.FromResult(response.result);
        }
    }
}
