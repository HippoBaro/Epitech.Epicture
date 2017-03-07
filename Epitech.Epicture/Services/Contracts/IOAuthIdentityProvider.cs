using System;
using System.Threading.Tasks;

namespace Epitech.Epicture.Services.Contracts
{
    public interface IOAuthIdentityProvider
    {
        /// <summary>
        /// The identity token future requests for this service will be signed with.
        /// </summary>
        string IdentityToken { get; }

        /// <summary>
        /// The service provider specific Id for the currently authenticated user
        /// </summary>
        string UserId { get; }

        /// <summary>
        /// The currently authenticated user's username.
        /// </summary>
        string UserName { get; }

        /// <summary>
        /// Should be true if the authentication process need a user input. Ex. Ingur need the user to enter a service provided PIN in order to complete the authentication process.
        /// </summary>
        bool NeedUserInput { get; }

        /// <summary>
        /// Get the url the user will be redirected to to authenticated.
        /// </summary>
        /// <returns></returns>
        Task<Uri> GetAuthorisationUrl();

        /// <summary>
        /// Get the full authenticating string that will be used to sign request.
        /// </summary>
        /// <returns>Ex "Bearer [TOKEN]"</returns>
        string GetAuthenticationHeader();

        /// <summary>
        /// Complete the authentication process whith the accessory user provided input. 
        /// </summary>
        /// <param name="pin"></param>
        /// <returns></returns>
        Task Authorize(string pin);

        /// <summary>
        /// Chance for the service to re-authenticate the user using, for example, a refresh token.
        /// </summary>
        /// <returns></returns>
        Task ReAuthorize();
    }
}
