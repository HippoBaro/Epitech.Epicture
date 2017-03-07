namespace Epitech.Epicture.Services.Contracts
{
    public interface IBaseClient
    {
        /// <summary>
        /// The Identity Provider for this service provider.
        /// </summary>
        IOAuthIdentityProvider IdentityProvider { get; }
    }
}
