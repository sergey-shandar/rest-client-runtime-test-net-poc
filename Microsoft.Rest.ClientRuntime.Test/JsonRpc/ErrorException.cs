using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Rest.ClientRuntime.Test.JsonRpc
{
    public class ErrorException<E> : Exception
    {
        public string Method { get; }

        public string Json { get; }

        public Error<E> Error { get; }

        public ErrorException(string method, string json, Error<E> error):
            base($"JSONRPC error, method: {method}, code: {error.code}, message: '{error.message}', json: '{json}'")
        {
            Method = method;
            Json = json;
            Error = error;
        }
    }
}
