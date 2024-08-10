using AnimeCalendar.Api.Storage;
using System.Net.Http.Headers;

namespace AnimeCalendar.Api.Bangumi.Auth;

internal class AuthHeaderHandler : DelegatingHandler {
    public static IAuthTokenStorage? TokenStorage { get; set; }

    public AuthHeaderHandler(HttpMessageHandler? handler = null) {
        InnerHandler = handler ?? new HttpClientHandler();
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
        if (TokenStorage != null && await TokenStorage.GetTokenAsync() is string token) {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }
}
