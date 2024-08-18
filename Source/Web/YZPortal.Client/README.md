## 1. Overview
This project is a Blazor WebAssembly application powered by .NET 8.0. Blazor WebAssembly revolutionizes web development by enabling the creation of dynamic and interactive web user interfaces using C# instead of JavaScript. This approach simplifies the development process and enhances maintainability, empowering developers to build robust web applications with ease.

## 3. Configurations
To ensure a smooth startup and runtime experience, it's crucial to configure the settings before hosting. 
You can set up the backend API by defining configurations through either environment variables on the hosting server or user secrets 
on the local host. Presently, the backend API supports the following configurations:

|Configurations|Description|
|-|-|
|YZPortalApi|Configure options for the main API.|
|AzureAdB2C|Configure Azure AD B2C options.|

### 3.1 Configuration options
#### 3.1.1 JWT
|Options|Description|
|-|-|
|YZPortalApi__BaseAddress|Base address of the main API.|
|YZPortalApi__Scope|The secret key used to create signing credentials.|
|YZPortalApi__HangfireDashboardUrl|The url for Hangfire Dashboard.|
|YZPortalApi__ChatRoomUrl|The url for SignalR Chat Hub.|

#### 3.1.2 ConnectionStrings
|Options|Description|
|-|-|
|AzureAdB2C__Authority|Authority endpoint for portal sign-up/sign-in policy|
|AzureAdB2C__ClientId|Application ID (clientId) of the application copied from the Azure portal.|
|AzureAdB2C__ValidateAuthority|[MSAL.NET ValidateAuthority property](https://learn.microsoft.com/en-us/dotnet/api/microsoft.authentication.webassembly.msal.msalauthenticationoptions.validateauthority?view=aspnetcore-8.0)|

## 4. References
### 4.1. Authentication/Authorization
- [JWT token authentication](https://trystanwilcock.com/2022/09/28/net-6-0-blazor-webassembly-jwt-token-authentication-from-scratch-c-sharp-wasm-tutorial/)
- [Policy based authorization](https://chrissainty.com/securing-your-blazor-apps-configuring-policy-based-authorization-with-blazor/)

### 4.2. Containerization
- [How to](https://github.com/jongio/BlazorDocker)
- [How to](https://chrissainty.com/containerising-blazor-applications-with-docker-containerising-a-blazor-webassembly-app/)
- [How to include class library reference into docker file](https://stackoverflow.com/questions/64557885/how-to-include-class-library-reference-into-docker-file/77592431#77592431)
- [Redirect to https on nginx inside Docker container](https://stackoverflow.com/questions/72748458/redirect-to-https-on-nginx-inside-docker-container)
- [Development SSL for .NET Core and NGINX in Docker](https://meikle.io/opensource/development-ssl-dotnetcore-docker.html)
- [Docker support not available for Web Assembly project](https://stackoverflow.com/questions/64829076/blazor-webassembly-app-with-docker-support-linux)
- [.Net 6 Blazor WASM in Azure Container App](https://clearmeasure.com/net-6-blazor-wasm-in-azure-container-app/)

### 4.2. Misc
- [Nav menu](https://stackoverflow.com/questions/58914389/how-to-create-navmenu-with-collapsible-submenu-in-net-core-blazor-app)

- [Disposable](https://www.meziantou.net/canceling-background-tasks-when-a-user-navigates-away-from-a-blazor-component.htm)
- [Disposable](https://www.infoworld.com/article/3649352/how-to-work-with-iasyncdisposable-in-net-6.html)

- [Blazor component lifecycle](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/lifecycle?view=aspnetcore-8.0)
- [Blazor component lifecycle](https://blazor-university.com/components/component-lifecycles/)

- [File upload](https://learn.microsoft.com/en-us/aspnet/core/blazor/file-uploads?view=aspnetcore-8.0)
- [File download](https://learn.microsoft.com/en-us/aspnet/core/blazor/file-downloads?view=aspnetcore-8.0)

- [Logging](https://learn.microsoft.com/en-us/aspnet/core/blazor/fundamentals/logging?view=aspnetcore-8.0)