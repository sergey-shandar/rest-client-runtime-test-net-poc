using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rest.ClientRuntime.Test.JsonRpc
{
    public class RemoteServer : IServer
    {
        public async Task<T> Call<T>(string method, Dictionary<string, object> @params)
        {
            return default(T);
        }
    }
}
