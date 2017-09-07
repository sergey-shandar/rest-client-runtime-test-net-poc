using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Rest.ClientRuntime.Test.Rpc
{
    /// <summary>
    /// The main RPC server interface.
    /// </summary>
    public interface IServer
    {
        Task<T> Call<T, E>(string method, Dictionary<string, object> @params);
    }
}
