# YZ Portal - Backend API

## 3. Configuration

To avoid unexpected errors during startup or runtime, the configuration should be configured before hosting. The backend API can be configured by either setting environment variables on the hosting server or user secrets on the local host. Currently, the following configurations are available in the backend API:

| Configurations    | Description                                                                                                                                  |
| ----------------- | -------------------------------------------------------------------------------------------------------------------------------------------- |
| ConnectionStrings | Configure connection strings of databases and Azure storage.                                                                                 |
| Seed              | Configure option to seed database and setup the dealer portal's admin credenetial.                                                           |
| FunctionApi       | Configure Annata online function application to establish communcation with FnO runtime services.                                            |
| AzureStorage      | Configure containers for the given Azure storage.                                                                                            |
| Jwt               | Configure JWT options for securely transmitting information between parties as a JSON object.                                                |
| AzureADB2CApi     | Configure Azure AD B2C options for backend API.                                                                                              |
| AzureADB2CSwagger | Configure Azure AD B2C options for client Swagger.                                                                                           |
| AzureADApi		| Configure Azure AD options for backend API.					                                                                               |
| AzureADSwagger	| Configure Azure AD options for client Swagger.				                                                                               |
| Swagger			| Configure options for Swagger.																											   |

### 3.1 Configuration options

Whether the options are left out or setup wrongly during setup, the appropriate error msg will be displayed in the hosting environment's log stream or the frontend application's dialog box when calling certain API services. The available options for the configurations are the following:

#### 3.1.1 ConnectionStrings

| Options                              | Description                                                                                                                                  |
| ------------------------------------ | -------------------------------------------------------------------------------------------------------------------------------------------- |
| ConnectionStrings__Primary           | The DB connection string for backend. Backend I/O only supports sql server.                                                                  |
| ConnectionStrings__BYOD              | The DB connection string where FnO stages its entities for backend. Backend I/O only supports sql server.                                    |
| ConnectionStrings__AzureStorage      | The Azure storage connection string where files are stored for CRUD operations.                                                              |

#### 3.1.2 Seed

Only one dealer portal admin exists. Modifying the admin email and password will only overwrite the existing admin's credential and not create another admin. 

| Options                              | Description                                                                                                                                  |
| ------------------------------------ | -------------------------------------------------------------------------------------------------------------------------------------------- |
| Seed__AdminEmail                     | The dealer portal application admin's email.                                                                                                 |
| Seed__AdminPassword                  | The dealer portal application admin's password.                                                                                              |

#### 3.1.3 FunctionApi

| Options                              | Description                                                                                                                                  |
| ------------------------------------ | -------------------------------------------------------------------------------------------------------------------------------------------- |
| FunctionApi__TimeoutSeconds          | Controls the timeout of httpclient when communicating with AO function app layer. Has a default of 200 seconds.                              |
| FunctionApi__ApiUrl                  | The URL of the AO function app layer.                                                                                                        |
| FunctionApi__ApiKey                  | The key to communicate with AO function app layer.                                                                                           |
| FunctionApi__Language                | Sets the language.                                                                                                                           |


#### 3.1.4 AzureStorage

The setup for certain container option can be ignored if the associated module is not required. Refer to the container names in the respective Azure storage and replace the option values respectively.

| Options                                              | Description                                                                                                                                                |
| ---------------------------------------------------  | ---------------------------------------------------------------------------------------------------------------------------------------------------------- |
| AzureStorage__CaseAttachmentContainerName            | Container for user to download/upload/delete case attachment.                                                                                              |
| AzureStorage__DeliveryReportAttachmentContainerName  | Container for user to download/upload/delete delivery report attachment.                                                                                   |
| AzureStorage__DeviceAttachmentContainerName          | Container for user to download/upload/delete device attachment.                                                                                            |
| AzureStorage__DeviceImageContainerName               | Container for frontend to download and display device image.                                                                                               |
| AzureStorage__DeviceInvoiceContainerName             | Container for user to download device invoice, parts invoice, and parts return order invoice.                                                              |
| AzureStorage__DeviceOrderAttachmentContainerName     | Container for user to download/upload/delete device order attachment.                                                                                      |
| AzureStorage__DeviceQuotationAttachmentContainerName | Container for user to download/upload/delete device quotation attachment.                                                                                  |
| AzureStorage__PackingSlipContainerName               | Container for user to download device packing slip and parts packing slip.                                                                                 |
| AzureStorage__PartsInvoiceContainerName              | Obsolete                                                                                                                                                   |
| AzureStorage__PartsOrderAttachmentContainerName      | Container for user to download/upload/delete parts order attachment.                                                                                       |
| AzureStorage__PartsQuotationAttachmentContainerName  | Container for user to download/upload/delete parts quotation attachment.                                                                                   |
| AzureStorage__ReturnOrderContainerName               | Container for user to download/upload/delete return order attachment.                                                                                      |
| AzureStorage__ReturnOrdeReportContainerName          | Container for user to download return order report.                                                                                                        |
| AzureStorage__ServiceCampaignContainerName           | Container for user to download service campaign attachment.                                                                                                |
| AzureStorage__WarrantyClaimAttachmentContainerName   | Container for user to download/upload/delete warranty claim attachment.                                                                                    |
| AzureStorage__WarrantyClaimInvoiceContainerName      | Container for user to download warranty claim invoice.                                                                                                     |
| AzureStorage__WarrantyClaimStatementContainerName    | Container for user to download device packing slip, parts packing slip, and warranty claim statement.                                                      |
| AzureStorage__InviteSheetXLSMContainerName           | Container for user to download the bulk invite sheet. The XLSM sheet must be manually added to this container and its latest version can be obtained from  |


#### 3.1.5 JWT

| Options                              | Description                                                                                                                                  |
| ------------------------------------ | -------------------------------------------------------------------------------------------------------------------------------------------- |
| Jwt__ValidForDays                    | The "iss" (issuer) claim identifies the principal that issued the JWT.                                                                       |
| Jwt__SecretKey                       | The secret key used to create signing credentials.                                                                                           |
| Jwt__ValidForDays                    | Set the JWT token timespan in days. The default values is "14".                                                                              |

#### 3.1.6 AzureADB2CApi

| Options                              | Description                                                                                                                                  |
| ------------------------------------ | -------------------------------------------------------------------------------------------------------------------------------------------- |
| AzureADB2CApi__ClientId              | Application ID (clientId) of the application copied from the Azure portal.                                                                   |
| AzureADB2CApi__Instance              | Tenant name copied from the Azure portal.                                                                                                    |
| AzureADB2CApi__SignUpSignInPolicyId  | Sign up and sign in policy or user flow created in Azure portal.                                                                             |
| AzureADB2CApi__Domain                | Azure AD tenant name.                                                                                                                        |

#### 3.1.7 AzureADB2CSwagger

| Options                              | Description                                                                                                                                  |
| ------------------------------------ | -------------------------------------------------------------------------------------------------------------------------------------------- |
| AzureADB2CSwagger__ClientId          | Application ID (clientId) of the application copied from the Azure portal.                                                                   |
| AzureADB2CSwagger__ClientSecret      | The secret to prove identity when requesting token.                                                                                          |
| AzureADB2CSwagger__Scope             | Scopes to acces data and functionality provided by backend API.                                                                              |
| AzureADB2CSwagger__AppName           | App name registered in Azure portal.                                                                                                         |

#### 3.1.8 AzureADApi

| Options                           | Description                                                                                                                                  |
| ----------------------------------| -------------------------------------------------------------------------------------------------------------------------------------------- |
| AzureADApi__ClientId              | Application ID (clientId) of the application copied from the Azure portal.                                                                   |
| AzureADApi__Instance              | Tenant name copied from the Azure portal.                                                                                                    |
| AzureADApi__TenantId				| Tenant Id copied from the Azure portal.																									   |
| AzureADApi__Domain                | Azure AD tenant name.                                                                                                                        |
| AzureADApi__Audience              | Reserved option in backend API. DO NOT SET THE VALUE FOR THIS OPTION!                                                                        |

#### 3.1.9 AzureADSwagger

| Options                           | Description                                                                                                                                  |
| ----------------------------------| -------------------------------------------------------------------------------------------------------------------------------------------- |
| AzureADSwagger__ClientId          | Application ID (clientId) of the application copied from the Azure portal.                                                                   |
| AzureADSwagger__ClientSecret      | The secret to prove identity when requesting token.                                                                                          |
| AzureADSwagger__Scope             | Scopes to acces data and functionality provided by backend API.                                                                              |

#### 3.1.10 Sawgger

| Options                              | Description																																														                                                        |
| ------------------------------------ | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Swagger__IsAzureAdOAuth2Provider	   | Controls the OAuth2 provider for swagger UI. Swagger UI does not support both Azure Ad and Azure Ad B2C simultaneously. Take note that authorization policy "combinedScheme" will select only one Ad based on this option as well. Default value is false. |

*disclaimer: certain configuration options are not included because they are not/rarely used or have the appropriate default values already.