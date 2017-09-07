using System.Threading.Tasks;

namespace Microsoft.Rest.ClientRuntime.Test.Utf8
{
    public interface IWriter
    {
        Task WriteAsync(string value);

        Task FlushAsync();
    }
}
