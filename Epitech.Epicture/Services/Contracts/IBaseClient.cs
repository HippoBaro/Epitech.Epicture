namespace Epitech.Epicture.Services.Contracts
{
    internal interface IBaseClient
    {
        IOAuthIdentityProvider IdentityProvider { get; }
    }
}
