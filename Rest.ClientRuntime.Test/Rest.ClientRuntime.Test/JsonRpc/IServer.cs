using Microsoft.Rest.ClientRuntime.Test.Rpc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Rest.ClientRuntime.Test.JsonRpc
{
    public interface IServer
    {
        Task Call(IMarshalling marshalling, string method, Dictionary<string, object> @params);

        Task<T> Call<T>(IMarshalling marshalling, string method, Dictionary<string, object> @params);
    }
}
