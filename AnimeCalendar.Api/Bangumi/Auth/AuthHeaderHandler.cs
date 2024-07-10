using AnimeCalendar.Api.Storage;
using System.Net.Http.Headers;

namespace AnimeCalendar.Api.Bangumi.Auth;

#nullable enable
internal class AuthHeaderHandler : DelegatingHandler {
    private readonly IAuthTokenStorage? tokenStorage;

    public AuthHeaderHandler(IAuthTokenStorage? tokenStorage, HttpMessageHandler? handler = null) {
        this.tokenStorage = tokenStorage;
        InnerHandler = handler ?? new HttpClientHandler();
    }

    private async Task<string?> GetToken() {
        if (tokenStorage == null)
            return null;

        return await tokenStorage.IsExpired() 
            ? await tokenStorage.RefreshTokenAsync()
            : await tokenStorage.GetTokenAsync();
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
        if (await GetToken() is string token) {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }
}
