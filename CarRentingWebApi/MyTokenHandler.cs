using Microsoft.Extensions.Caching.Distributed;

namespace CarRentingWebApi;

public class MyTokenHandler
{
    public void RevokeToken(int userId, string oldTokenId, IDistributedCache _cache)
    {
        // Use IDistributedCache to store revoked tokens
        var key = $"{userId}_revoked_tokens";
        var cachedTokens = _cache.GetString(key);

        if (string.IsNullOrEmpty(cachedTokens))
        {
            cachedTokens = oldTokenId;
        }
        else
        {
            cachedTokens += ";" + oldTokenId;
        }

        _cache.SetString(key, cachedTokens);
    }

    public bool IsTokenRevoked(int userId, string currentTokenId, IDistributedCache _cache)
    {
        var key = $"{userId}_revoked_tokens";
        var cachedTokens = _cache.GetString(key);
        if (string.IsNullOrEmpty(cachedTokens))
        {
            return false; // No tokens revoked for this user
        }

        var revokedTokens = cachedTokens.Split(';');
        return revokedTokens.Contains(currentTokenId);
    }
}
