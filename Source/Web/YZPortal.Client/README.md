## 3. Configurations
To ensure a smooth startup and runtime experience, it's crucial to configure the settings before hosting. 
You can set up the backend API by defining configurations through either environment variables on the hosting server or user secrets 
on the local host. Presently, the backend API supports the following configurations:

|Configurations|Description|
|
|YZPortalApi|Configure options for the main API.|
|AzureAdB2C|Configure Azure AD B2C options.|

### 3.1 Configuration options
#### 3.1.1 JWT
|Options|Description|
|
|YZPortalApi__BaseAddress|Base address of the main API.|
|YZPortalApi__Scope|The secret key used to create signing credentials.|

#### 3.1.2 ConnectionStrings
|Options|Description|
|
|AzureAdB2C__Authority|Authority endpoint for portal sign-up/sign-in policy|
|AzureAdB2C__ClientId|Application ID (clientId) of the application copied from the Azure portal.|
|AzureAdB2C__ValidateAuthority|[MSAL.NET ValidateAuthority property](AzureAdB2C__Authority)|

## 4. References

- [Nav menu](https://stackoverflow.com/questions/58914389/how-to-create-navmenu-with-collapsible-submenu-in-net-core-blazor-app)

- [JWT token authentication](https://trystanwilcock.com/2022/09/28/net-6-0-blazor-webassembly-jwt-token-authentication-from-scratch-c-sharp-wasm-tutorial/)

- [Policy based authorization](https://chrissainty.com/securing-your-blazor-apps-configuring-policy-based-authorization-with-blazor/)

- [Razor component lifecycle](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/lifecycle?view=aspnetcore-8.0)