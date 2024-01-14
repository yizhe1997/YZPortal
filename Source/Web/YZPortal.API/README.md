## 3. Configurations
To ensure a smooth startup and runtime experience, it's crucial to configure the settings before hosting. 
You can set up the backend API by defining configurations through either environment variables on the hosting server or user secrets 
on the local host. Presently, the backend API supports the following configurations:

|Configurations|Description|
|
|Jwt|Configure JWT options for securely transmitting information between parties as a JSON object.|
|ConnectionStrings|Configure database connection strings.|
|BasicAuthentication|Configure basic authentication options.|
|AzureADB2CApi|Configure Azure AD B2C options for API.|
|AzureADB2CSwagger|Configure Azure AD B2C options for Swagger.|
|AzureAdB2CManagement|Configure Azure AD B2C graph manager options.|
|Graph|Configure Graph API options.|
|AzureStorage|Configure Azure storage options.|
|Mail|Configure Mail options.|

### 3.1 Configuration options
#### 3.1.1 JWT
|Options|Description|
|
|Jwt__ValidForDays|Set the JWT token lifespan in days. The default value is "14".|
|Jwt__SecretKey|The secret key used to create signing credentials.|

#### 3.1.2 ConnectionStrings
|Options|Description|
|
|ConnectionStrings__Primary|The DB connection string for SQL server.|

#### 3.1.3 BasicAuthentication
|Options|Description|
|
|BasicAuthentication__UserName|Arbitrary user name.|
|BasicAuthentication__Password|Arbitrary password.|

#### 3.1.4 AzureADB2CApi
|Options|Description|
|
|AzureADB2CApi__ClientId|Application ID (clientId) of the application copied from the Azure portal.|
|AzureADB2CApi__Instance|Tenant name copied from the Azure portal.|
|AzureADB2CApi__SignUpSignInPolicyId|Sign up and sign in policy or user flow created in Azure portal.|
|AzureADB2CApi__Domain|Azure AD tenant name.|

#### 3.1.5 AzureADB2CSwagger
|Options|Description|
|
|AzureADB2CSwagger__ClientId|Application ID (clientId) of the application copied from the Azure portal.|
|AzureADB2CSwagger__ClientSecret|The secret to prove identity when requesting token.|
|AzureADB2CSwagger__Scope|Scopes to acces data and functionality provided by backend API.|
|AzureADB2CSwagger__AppName|App name registered in Azure portal.|

#### 3.1.6 AzureAdB2CManagement
|Options|Description|
|
|AzureAdB2CManagement__ClientId|Application ID (clientId) of the application copied from the Azure portal.|
|AzureAdB2CManagement__ClientSecret|Tenant name copied from the Azure portal.|
|AzureAdB2CManagement__TenantId|Tenant Id copied from the Azure portal.|

#### 3.1.7 Graph
|Options|Description|
|
|Graph__BaseUrl|Base URL of the Graph API.|
|Graph__Scopes|Scopes required when requesting from Graph API.|

#### 3.1.8 AzureStorage
|Options|Description|
|
|AzureStorage__ConnectionString|The connection string for Azure storage.|
|AzureStorage__UserProfileImageContainer|Container name for user profile images.|

#### 3.1.9 Mail
|Options|Description|
|
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


## 4. References
### 4.1. Identity
#### 4.1.1. AZURE AD B2C
- [B2C account auto link](https://github.com/azure-ad-b2c/samples/tree/master/policies/auto-account-linking)
- [Azure Active Directory B2C logs with Application Insights](https://learn.microsoft.com/en-us/azure/active-directory-b2c/troubleshoot-with-application-insights?pivots=b2c-custom-policy)
- [Custom policy samples](https://github.com/azure-ad-b2c/samples) 
- [Custom policy add email claim](https://stackoverflow.com/questions/46778129/azure-ad-b2c-emails-claim-in-custom-policy?rq=3)
- [Unable to see email address in clims after siginin](https://learn.microsoft.com/en-us/answers/questions/738212/b2c-custom-policy-unable-to-see-email-address-in-c)

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