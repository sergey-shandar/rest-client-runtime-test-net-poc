﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rest.ClientRuntime.Test.JsonRpc;

namespace Rest.ClientRuntime.Test.UnitTest
{
    [TestClass]
    public class RemoteServerUnitTest
    {
        [TestMethod]
        public void TestCall()
        {
            var server = new RemoteServer();
        }
    }
}