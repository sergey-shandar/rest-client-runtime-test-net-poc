using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Microsoft.Rest.ClientRuntime.Test.Azure
{
    public interface IAzureRequest
    {
        IAzureRequestInfo Info { get; }

        AzureBaseUri BaseUri { get; }

        IEnumerable<AzureParam> ParamList { get; }

        Dictionary<string, List<string>> CustomHeaders { get; }

        CancellationToken CancellationToken { get; }
    }

    public sealed class AzureRequest<E> : IAzureRequest
    {
        public AzureRequestInfo<E> Info { get; }

        IAzureRequestInfo IAzureRequest.Info => Info;

        public AzureBaseUri BaseUri { get; }

        public IEnumerable<AzureParam> ParamList { get; }

        public Dictionary<string, List<string>> CustomHeaders { get; }

        public CancellationToken CancellationToken { get; }

        public AzureRequest(
            AzureRequestInfo<E> info,
            AzureBaseUri baseUri,
            IEnumerable<AzureParam> paramList,
            Dictionary<string, List<string>> customHeaders,
            CancellationToken cancellationToken)
        {
            Info = info;
            BaseUri = baseUri;
            ParamList = paramList;
            CustomHeaders = customHeaders;
            CancellationToken = cancellationToken;
        }
    }

    public static class AzureRequestEx
    {
        public static IEnumerable<AzureParam> GetConstAndParamList(this IAzureRequest request)
            => request.Info.ConstList.Concat(request.ParamList);

        public static Uri GetBaseUri(this IAzureRequest request)
            => request.BaseUri.GetUri(request);
    }
}
