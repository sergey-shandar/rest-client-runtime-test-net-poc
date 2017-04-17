using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Rest.ClientRuntime.Test.Rpc
{
    public interface IServer
    {
        Task<T> Call<T>(string method, Dictionary<string, object> @params);
    }
}
