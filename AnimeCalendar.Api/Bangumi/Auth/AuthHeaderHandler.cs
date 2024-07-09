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

    private async Task<string?> GetToken(CancellationToken? cancellation) {
        if (tokenStorage == null)
            return null;

        if (await tokenStorage.IsExpired(cancellation)) {
            return await tokenStorage.RefreshTokenAsync(cancellation);
        }

        return await tokenStorage.GetTokenAsync(cancellation);
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
        var token = await GetToken(cancellationToken);

        if (token != null) {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }
}
