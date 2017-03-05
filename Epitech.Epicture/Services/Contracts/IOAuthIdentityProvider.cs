using System;
using System.Threading.Tasks;

namespace Epitech.Epicture.Services.Contracts
{
    internal interface IOAuthIdentityProvider
    {
        string IdentityToken { get; }
        string RefreshToken { get; }
        string UserId { get; }
        string UserName { get; }

        Uri GetAuthorisationUrl();
        string GetAuthenticationHeader();
        Task Authorize(string pin);
        Task ReAuthorize();
    }
}
