namespace YZPortal.API.Controllers.Graph.GraphGroups
{
    public class GraphGroupViewModel
	{
        public string? Id { get; set; }
        public object? DeletedDateTime { get; set; }
        public object? Classification { get; set; }
        public object? CreatedDateTime { get; set; }
        public object[]? CreationOptions { get; set; }
        public string? Description { get; set; }
        public string? DisplayName { get; set; }
        public object? ExpirationDateTime { get; set; }
        public object[]? GroupTypes { get; set; }
        public object? IsAssignableToRole { get; set; }
        public object? Mail { get; set; }
        public bool MailEnabled { get; set; }
        public string? MailNickname { get; set; }
        public object? MembershipRule { get; set; }
        public object? MembershipRuleProcessingState { get; set; }
        public object? OnPremisesDomainName { get; set; }
        public object? OnPremisesLastSyncDateTime { get; set; }
        public object? OnPremisesNetBiosName { get; set; }
        public object? OnPremisesSamAccountName { get; set; }
        public object? OnPremisesSecurityIdentifier { get; set; }
        public object? OnPremisesSyncEnabled { get; set; }
        public object? PreferredDataLocation { get; set; }
        public object? PreferredLanguage { get; set; }
        public object[]? ProxyAddresses { get; set; }
        public object? RenewedDateTime { get; set; }
        public object[]? ResourceBehaviorOptions { get; set; }
        public object[]? ResourceProvisioningOptions { get; set; }
        public bool SecurityEnabled { get; set; }
        public string? SecurityIdentifier { get; set; }
        public object? Theme { get; set; }
        public object? Visibility { get; set; }
        public object[]? OnPremisesProvisioningErrors { get; set; }
    }
}
