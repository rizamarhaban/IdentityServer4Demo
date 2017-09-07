# IdentityServer4Demo
[.NET Developers Community Meetup Demo](https://www.meetup.com/NET-Developers-SG/events/242852152/) on August 30, 2017

In this demo, I use [IdentityServer4 2.0.0-rc1](https://www.nuget.org/packages/IdentityServer4/2.0.0-rc1). You can use the latest preview or if already have the RTM version.

There are 4 (four) projects in the solution folder, that is:

+ IdentityServer (The ASP.NET Core 2.0 MVC AspNetIdentity using IdentityServer4)
+ Ids4AspNetIdentity project using .NET Standard 2.0 (taken from [IdentityServer4.AspNetIdentity 2.0.0-rc1](https://github.com/IdentityServer/IdentityServer4.AspNetIdentity))
+ MyApi (The ASP.NET Core 2.0 Web Api project)
+ MyWeb (The ASP.NET Core 2.0 MVC project)

## Creating and Installing the Self-Signing Certificate using PowerShell

If you don't want to create certificate when developing, you can use the ```AddDeveloperSigningCredential()``` example;

``` CSharp
services.AddIdentityServer()
	.AddDeveloperSigningCredential()
	.AddInMemoryIdentityResources(Config.GetIdentityResources())
	.AddInMemoryApiResources(Config.GetApis())
	.AddInMemoryClients(Config.GetClients())
	.AddAspNetIdentity<ApplicationUser>();
```

Otherwise, you can create a self-signing certificate with private key as follow:

``` PowerShell
$certificate = New-SelfSignedCertificate `
    -Type Custom `
    -Provider "Microsoft Strong Cryptographic Provider" `
    -Subject "CN=rizacert" `
    -DnsName localhost `
    -KeyAlgorithm RSA `
    -KeyLength 2048 `
    -KeyExportPolicy ExportableEncrypted `
    -NotBefore (Get-Date) `
    -NotAfter (Get-Date).AddYears(6) `
    -CertStoreLocation "cert:LocalMachine\My" `
    -FriendlyName "Localhost Cert IdentityServer" `
    -HashAlgorithm SHA256 `
    -KeyUsage DigitalSignature, KeyEncipherment, DataEncipherment `
    -TextExtension @("2.5.29.37={text}1.3.6.1.5.5.7.3.1")
$certificatePath = 'Cert:\LocalMachine\My\' + ($certificate.ThumbPrint)  
$pwd = ConvertTo-SecureString -String ‘pa$$Word.123’ -Force -AsPlainText
Export-PfxCertificate -cert $certificatePath -FilePath "C:\Demo\rizacert.pfx" -Password $pwd
```
Once you have the cert .pfx file, you can install it on the cert store in Windows using the MMC (Microsoft Management Console) with Certificate Snap-in or you can just double-click the file and follow the wizrd to Install. You can choose Local Machine Personal folder to store the certificate.

On the IdentityServer project Startup.cs, make sure the certificate subject name is the same as what you make on the certificate, on my example case I use "**CN=rizacert**":

``` CSharp
services.AddIdentityServer()
  .AddSigningCredential("CN=rizacert")
  .AddInMemoryIdentityResources(Config.GetIdentityResources())
  .AddInMemoryApiResources(Config.GetApis())
  .AddInMemoryClients(Config.GetClients())
  .AddAspNetIdentity<ApplicationUser>();
```

How to wire up between the MVC and the API just follow the OpenId connect conecpt. The  grant type for the Web API is cleitn credentials, you can test in Postman like this:

![Postman Example](https://www.rizamarhaban.com/wp-content/uploads/2017/09/Ids4Demo_Client_Credetials.png "Client Credentials Example")

In my case, I use hybrid for the MVC and client credentials for the Web API. You can also change the gran type of the Web API to use resoruce owner if you want to use password as the credentials for login. See the client configuration in the ```Config.cs``` file on the IdentityServer project and just change the **AllowedGrantType** to:

``` CSharp
AllowedGrantTypes = GrantTypes.ResourceOwnerPassword
```

To Test using Postman, you can specify the ```grant_type``` parameter value as ```password```, example:

![Postman Example](https://www.rizamarhaban.com/wp-content/uploads/2017/09/Ids4Demo_ResourceOwnerPassword.png "Resource Owner Password Example")

