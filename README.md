# C# Tools
C# Tools for .NET Core is an open-source project offering a suite of utilities to enhance C# development for .NET Core. It includes libraries for common tasks, code snippets, and performance optimizations, helping developers improve productivity, code quality, and simplify complex tasks.

## Install Package Manager

```bash
PM> Install-Package jff_csharp-tools
```

## Install .NET CLI

```bash
> dotnet add package jff_csharp-tools
```

## Install Paket CLI

```bash
> paket add jff_csharp-tools
```

## Example Usage

```bash
using jff_client_oidc_csharp;

namespace ExampleConnectOIDC
{
    public class ConnectOIDC
    {
        private readonly ClientCredentials client;
        public ConnectOIDC(){}
            client = new ClientCredentials("{urlAuthority}", "{clientId}", "{clientSecret}", new string[] { "openid" });
        }

        public async Task<string> GetToken(){
            return await client.GetToken();
        }

        public async Task<dynamic> GetApiRest(string url){
            return await client.Get<dynamic>(url);
        }

        public async Task<dynamic> PostApiRest(string url, dynamic objSend){
            return await client.Post<dynamic, dynamic>(url, objSend);
        }

        public async Task<dynamic> PutApiRest(string url, dynamic objSend){
            return await client.Put<dynamic, dynamic>(url, objSend);
        }

        public async Task<dynamic> DeleteApiRest(string url){
            return await client.Delete<dynamic>(url);
        }
    }
 }
```