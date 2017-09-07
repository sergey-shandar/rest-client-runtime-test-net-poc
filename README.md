# REST API Client RunTime for .Net Test (Proof of concept)

[![Build status](https://ci.appveyor.com/api/projects/status/g9dia8i8hjgldk5b?svg=true)](https://ci.appveyor.com/project/sergey-shandar/rest-client-runtime-test-net-poc)

[NuGet Feed](https://ci.appveyor.com/nuget/rest-client-runtime-test-net-p-lft6230b45rt).

## 1. JSON-RPC API

- [JSON-RPC 2.0 Specification](http://www.jsonrpc.org/specification).
- [Language Server Protocol](https://github.com/Microsoft/language-server-protocol/blob/master/protocol.md#base-protocol).
- Namespace `JsonRpc`

```csharp
class RemoteServer
{
        public RemoteServer(Process process, Marshalling marshalling);
        public Task<T> Call<T>(string method, Dictionary<string, object> @params);
}
```

For example, the following code

```csharp
var remoteServer = new RemoteServer(process, new Marshalling(...));

remoteServer.Call<string>("MyMethod", new Dictionary<string, object> { { "a", "value" } });
```

will produce this JSON-RPC message

```json
{
    "jsonrpc": "2.0",
    "id": "0",
    "method": "MyMethod",
    "params": {
        "a": "value"
    }
}
```

The JSON-RPC message will be send to the process using the
**Language Service Protocol**

```
Content-Length:...

{
    "jsonrpc": "2.0",
    "id": "0",
    "method": "MyMethod",
    "params": {
        "a": "value"
    }
}
```

The service should response with a message with the same id, for example

```
Content-Length:...

{
    "jsonrpc": "2.0",
    "id": "0",
    "result": "result"
}
```

## 2. Azure SDK Test Framework Protocol

The Azure SDK Test Framework Protocol using JSON-RPC to call Azure SDK functions.

### 2.1. Operation Mapping

|Title|Swagger Operation Id|JSON-RPC method|
|-----|--------------------|---------------|
|`A`  |`B_C`               |`A.B_C`        |

### 2.2. Reserved Parameters

An additional parameter `__reserved` is added to each JSON-RPC call.

```json
{
    "__reserved": {
        "credentials": {
            "tenantId": "...",
            "clientId": "...",
            "secret": "..."
        }
    }
    ...
}
```

### 2.3. Result

The result structure should contain three fields
- **statusCode**
- **headers**
- **response**

For example

```
Content-Length:...

{
    "jsonrpc": "2.0",
    "id": "0",
    "result": {
        "statusCode": 200,
        "headers": { },
        "response": "somevalue"
    }
}
```

## 3. Implementing Azure SDK Test Service

Eeach swagger operation may have several corresponded API functions. A service developer should decide which type of API functions should be implemented.

## 4. For Developers

### 4.1 Setup

1. Install [Visual Studio 2017](https://www.visualstudio.com/)
2. Install [.Net Core 1.1.1](https://github.com/dotnet/core/blob/master/release-notes/download-archives/1.1.1-download.md).
