using System;
using System.Threading.Tasks;

namespace Epitech.Epicture.Services.Contracts
{
    internal interface IOAuthIdentityProvider
    {
        string IdentityToken { get; }
        string UserId { get; }
        string UserName { get; }
        bool NeedUserInput { get; }

        Task<Uri> GetAuthorisationUrl();
        string GetAuthenticationHeader();
        Task Authorize(string pin);
        Task ReAuthorize();
    }
}
