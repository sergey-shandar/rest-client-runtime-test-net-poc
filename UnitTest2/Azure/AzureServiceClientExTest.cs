using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Rest;
using Microsoft.Rest.ClientRuntime.Test.Azure;
using System.Threading.Tasks;
using Microsoft.Rest.Azure;
using Newtonsoft.Json;

namespace UnitTest2.Azure
{
    [TestClass]
    public class AzureServiceClientExTest
    {
        public class X : ServiceClient<X>, IAzureClient
        {
            public ServiceClientCredentials Credentials => throw new NotImplementedException();

            public int? LongRunningOperationRetryTimeout { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public JsonSerializerSettings SerializationSettings => throw new NotImplementedException();

            public JsonSerializerSettings DeserializationSettings => throw new NotImplementedException();

            public bool? GenerateClientRequestId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        }

        [TestMethod]
        public async Task TestCall()
        {
            // var x = new X();
            // await x.Call(new AzureRequest("", "", "", new AzureParam[] { }), new Tag<object>());
        }
    }
}
