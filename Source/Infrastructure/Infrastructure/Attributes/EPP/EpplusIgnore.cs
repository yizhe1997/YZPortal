namespace Infrastructure.Attributes.EPP
{
    /// <summary>
    /// This is to mock the EpplusIgnore attribute that is included in the commercial version.
    /// Current version is for non-commerical and does not include the said attribute.
    /// Ref: https://github.com/EPPlusSoftware/EPPlus/pull/258/commits/6bce68eddeef70bdb460eb1c1addef6a6d4d5580#diff-11527f16e5d6fe856900ae436f543a47dfa59f6fffe0377dc5d88d7256b616e4
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class EpplusIgnore : Attribute
    {
    }
}
