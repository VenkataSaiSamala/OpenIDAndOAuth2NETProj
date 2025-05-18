using Microsoft.AspNetCore.Authorization;

namespace ImageGallery.Authorization;

public static class AuthorizationPolicies
{
    public static AuthorizationPolicy CanAddImage()
    {
        var policy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .RequireClaim("country", "India")
            .RequireRole("PaidUser")
            .Build();

        return policy;
    }
}
