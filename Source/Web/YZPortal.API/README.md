## 3. Configurations
To ensure a smooth startup and runtime experience, it's crucial to configure the settings before hosting. 
You can set up the backend API by defining configurations through either environment variables on the hosting server or user secrets 
on the local host. Presently, the backend API supports the following configurations:

|Configurations|Description|
|-|-|
|Jwt|Configure JWT options for securely transmitting information between parties as a JSON object.|
|ConnectionStrings|Configure database connection strings.|
|BasicAuthentication|Configure basic authentication options.|
|AzureADB2CApi|Configure Azure AD B2C options for API.|
|AzureADB2CSwagger|Configure Azure AD B2C options for Swagger.|
|AzureAdB2CManagement|Configure Azure AD B2C graph manager options.|
|Graph|Configure Graph API options.|
|AzureStorage|Configure Azure storage options.|
|Mail|Configure Mail options.|
|CORS|Configure CORS options.|
|Cache|Configure Cache options.|
|SignalR|Configure SignalR options.|

### 3.1 Configuration options
#### 3.1.1 JWT
|Options|Description|
|-|-|
|Jwt__ValidForDays|Set the JWT token lifespan in days. The default value is "14".|
|Jwt__SecretKey|The secret key used to create signing credentials.|

#### 3.1.2 ConnectionStrings
|Options|Description|
|-|-|
|ConnectionStrings__Primary|The DB connection string for SQL server.|

#### 3.1.3 BasicAuthentication
|Options|Description|
|-|-|
|BasicAuthentication__UserName|Arbitrary user name.|
|BasicAuthentication__Password|Arbitrary password.|

#### 3.1.4 AzureADB2CApi
|Options|Description|
|-|-|
|AzureADB2CApi__ClientId|Application ID (clientId) of the application copied from the Azure portal.|
|AzureADB2CApi__Instance|Tenant name copied from the Azure portal.|
|AzureADB2CApi__SignUpSignInPolicyId|Sign up and sign in policy or user flow created in Azure portal.|
|AzureADB2CApi__Domain|Azure AD tenant name.|

#### 3.1.5 AzureADB2CSwagger
|Options|Description|
|-|-|
|AzureADB2CSwagger__ClientId|Application ID (clientId) of the application copied from the Azure portal.|
|AzureADB2CSwagger__ClientSecret|The secret to prove identity when requesting token.|
|AzureADB2CSwagger__Scope|Scopes to acces data and functionality provided by backend API.|
|AzureADB2CSwagger__AppName|App name registered in Azure portal.|

#### 3.1.6 AzureAdB2CManagement
|Options|Description|
|-|-|
|AzureAdB2CManagement__ClientId|Application ID (clientId) of the application copied from the Azure portal.|
|AzureAdB2CManagement__ClientSecret|Tenant name copied from the Azure portal.|
|AzureAdB2CManagement__TenantId|Tenant Id copied from the Azure portal.|

#### 3.1.7 Graph
|Options|Description|
|-|-|
|Graph__BaseUrl|Base URL of the Graph API.|
|Graph__Scopes|Scopes required when requesting from Graph API.|

#### 3.1.8 AzureStorage
|Options|Description|
|-|-|
|AzureStorage__ConnectionString|The connection string for Azure storage.|
|AzureStorage__UserProfileImageContainer|Container name for user profile images.|

#### 3.1.9 Mail
|Options|Description|
|-|-|
|Mail__SendGridSMTP__FromName|The display name used as the sender when sending emails via SendGrid.|
|Mail__SendGridSMTP__FromEmail|The email address used as the sender when sending emails via SendGrid.|
|Mail__SendGridSMTP__ApiKey|The API key used for authentication when sending emails via SendGrid.|
|Mail__GoogleSMTP__FromName|The display name used as the sender when sending emails via Google SMTP.|
|Mail__GoogleSMTP__FromEmail|The email address used as the sender when sending emails via Google SMTP.|
|Mail__GoogleSMTP__NetworkCredUsername|The username used for network credentials when connecting to Google SMTP.|
|Mail__GoogleSMTP__HostName|The host name or IP address of the mail server used for sending emails via Google SMTP.|
|Mail__GoogleSMTP__NetworkCredPassword|The password used for network credentials when connecting to Google SMTP.|
|Mail__GoogleSMTP__Port|The port number used for connecting to the mail server via Google SMTP.|
|Mail__GoogleSMTP__EnableSSL|A boolean value indicating whether SSL (Secure Sockets Layer) should be enabled when connecting to the mail server via Google SMTP.|

#### 3.1.10 CORS
|Options|Description|
|-|-|
|CORS__Blazor|The client Urls for Blazor app.|

#### 3.1.11 Cache
|Options|Description|
|-|-|
|Cache__DistributedCacheType|Type of distributed cache e.g. Redis, SqlServer, InMemory. Will default to in-memory cache if set to null or empty.|
|Cache__DefaultAbsoluteExpirationSeconds|Sets absolute time a cache entry can be inactive before it is removed from the cache.|
|Cache__DefaultSlidingExpirationSeconds|Sets how long a cache entry can be inactive before it is removed from the cache.|
|Cache__Redis__RedisURL|URL for redis cache server.|

#### 3.1.12 SignalR
|Options|Description|
|-|-|
|SignalR__SignalRType|Type of SignalR e.g. Azure. Will default to asp.net built in SignalR.|
|SignalR__Azure__ConnectionString|The connection string for Azure SignalR.|

## 4. References
### 4.1. Identity
#### 4.1.1. AZURE AD B2C
- [B2C account auto link](https://github.com/azure-ad-b2c/samples/tree/master/policies/auto-account-linking)
- [Azure Active Directory B2C logs with Application Insights](https://learn.microsoft.com/en-us/azure/active-directory-b2c/troubleshoot-with-application-insights?pivots=b2c-custom-policy)
- [Custom policy samples](https://github.com/azure-ad-b2c/samples) 
- [Custom policy add email claim](https://stackoverflow.com/questions/46778129/azure-ad-b2c-emails-claim-in-custom-policy?rq=3)
- [Unable to see email address in clims after siginin](https://learn.microsoft.com/en-us/answers/questions/738212/b2c-custom-policy-unable-to-see-email-address-in-c)
- [B2C example for MVC](https://github.com/Azure-Samples/active-directory-aspnetcore-webapp-openidconnect-v2/tree/master/1-WebApp-OIDC/1-5-B2C)

### 4.2. SMTP
- [FromForm and lists are not serialized correctly from swagger](https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/1107)

#### 4.2.1. Gmail SMTP 
- [How to](https://kinsta.com/blog/gmail-smtp-server/)
- [How to](https://www.hostinger.my/tutorials/how-to-use-free-google-smtp-server)
- [Troubleshoiting](https://stackoverflow.com/questions/18503333/the-smtp-server-requires-a-secure-connection-or-the-client-was-not-authenticated/66169647#66169647)
 
#### 4.2.2. Azure mail
- [How to](https://mailtrap.io/blog/azure-send-email/)

#### 4.2.3. Mailkit
- [How to](https://blog.christian-schou.dk/send-emails-with-asp-net-core-with-mailkit/)

#### 4.2.4. twillio 
- [How to](https://docs.sendgrid.com/api-reference/mail-send/mail-send)
- [How to](https://github.com/sendgrid/sendgrid-csharp)

### 4.3. Background Jobs
- [Official Site](https://www.hangfire.io/)
- [Setup](https://docs.hangfire.io/en/latest/getting-started/aspnet-core-applications.html)
- [Allow the Antiforgery cookie to be sent with cross-origin requests for Dashboard UI iframe](https://stackoverflow.com/questions/52669145/antiforgery-token-cookie-not-appearing-in-request-headers-only-in-when-embeded-i/52709829#52709829)
- [Integrate Azure AD B2C Authen cookie](https://medium.com/we-code/integrate-hangfire-dashboard-with-angular-using-azure-ad-and-azure-ad-b2c-tokens-a13449e0bbd4)

### 4.4. Security
#### 4.4.1 CORS
- [Middleware](https://learn.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-8.0) 

- #### 4.4.2 Storage Protection
- [SAS URLs in Microsoft Azure Blob Storage](https://tejaksha-k.medium.com/understanding-sas-urls-in-microsoft-azure-blob-storage-aa358d7ca8bf#:~:text=SAS%20URLs%20provide%20a%20secure,to%20a%20third%2Dparty%20user.)

### 4.5. Caching
#### 4.5.1 General
- [Distributed cache](https://code-maze.com/aspnetcore-distributed-caching/) 
- [In-Memory cache](https://code-maze.com/aspnetcore-in-memory-caching/) 
- [Redis cache](https://www.c-sharpcorner.com/article/easily-use-redis-cache-in-asp-net-6-0-web-api/)

### 4.6. Real-time Web Functionality
#### 4.6.1 SignalR
- [Example](https://code-maze.com/creating-blazor-webassembly-signalr-charts/)
- [Microsoft documentation](https://learn.microsoft.com/en-us/aspnet/core/signalr/hubs?view=aspnetcore-8.0)
- [Authentication](https://learn.microsoft.com/en-us/azure/azure-signalr/signalr-concept-authenticate-oauth)
- [Advance Example](https://www.dotnetcurry.com/aspnet-core/realtime-app-using-blazor-webassembly-signalr-csharp9)
- [Example](https://github.com/aspnet/AzureSignalR-samples/tree/main/samples/ChatRoomLocal)
- [Example](https://learn.microsoft.com/en-us/azure/azure-signalr/signalr-quickstart-dotnet-core)
- [Scaling](https://learn.microsoft.com/en-us/aspnet/core/signalr/scale?view=aspnetcore-8.0#azure-signalr-service)

#### 4.6.2 Websockets
- [How to](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/websockets?view=aspnetcore-3.1)

### 4.7 CICD
#### 4.7.1 Github Actions
- [Manually Trigger a GitHub Action with “workflow_dispatch”](https://medium.com/@irfankaraman/manually-trigger-a-github-action-with-workflow-dispatch-124708e26afe)
- [Azure AD B2C IEF deployment](https://learn.microsoft.com/en-us/azure/active-directory-b2c/deploy-custom-policies-github-action)

### 4.8 Security
#### 4.8.1 Refresh token
- [OAuth 2.0 refresh tokens with Azure AD B2C](https://melmanm.github.io/misc/2023/01/29/article2-oauth-refresh-tokens-with-azure-b2c.html#refreshing-tokens-by-the-book)
- [OAuth 2.0 authorization code flow in Azure Active Directory B2C](https://learn.microsoft.com/en-us/azure/active-directory-b2c/authorization-code-flow#1-get-an-authorization-code)

### 4.9 Instrumentation
#### 4.9.1 Logging
- [Logging in Azure with Application Insights and Serilog](https://hackernoon.com/logging-in-azure-with-application-insights-and-serilog)
- [Logging in .NET Core and ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-6.0)
- [Azure App Service Logs](https://stackify.com/azure-app-service-log-files/)
- [Azure App Service Logging](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/azure-app-service-logging-how-to-monitor-your-web-apps-in-real/ba-p/3800390)

