using System.Threading.Tasks;

namespace Microsoft.Rest.ClientRuntime.Test.Utf8
{
    public interface IReader
    {
        Task<string> ReadLineAsync();

        Task<string> ReadBlockAsync(int length);
    }
}
